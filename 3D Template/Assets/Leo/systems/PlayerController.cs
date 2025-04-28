using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Unity.Collections;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{

    private ActionMaps inputActions;
    public InputAction movement;
    public InputAction interact;
    public InputAction click;
    public bool toggle;
    public float speed;
    private Vector3 direction;
    public float mouseSensitivity = 100f;
    private Vector3 moveDirection;
    public CharacterController characterController;
    private bool wasLooking;
    private bool islooking;
    [Header("static Containers")]
    public GameObject P_followUpInteraction;
    public GameObject p_currentInteraction;
    private GameObject yeaaass;
    public List<PhysicalEvidence> evidenceList = new List<PhysicalEvidence>();
    private GameObject previousMaterial;
    private RaycastHit hitItem;
    public int eventValue;
    public UnityEvent highLightItemMaterial;
    public UnityEvent returnItemMatieral;
    GameObject previouslyHighlightedItem = null;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Awake()
    {
        inputActions = new ActionMaps();

    }
    private void OnEnable()
    {
        movement = inputActions.PlayerActions.Movement;
        interact = inputActions.PlayerActions.Interact;
        click = inputActions.PlayerActions.Click;
        movement.Enable();
        interact.Enable();
        click.Enable();

        interact.performed += Interact;
        click.performed += Click;



    }

    private void OnDisable()
    {
        movement.Disable();
        interact.Disable();

        interact.performed -= Interact;
        click.performed -= Click;
        
    }



    private void Update()
    
    {
        StaticVariables.eventValue = eventValue;


        if (movement.IsPressed() && StaticVariables.isConversing == false)
        {
            direction = movement.ReadValue<Vector2>();
            moveDirection = transform.right * direction.x + transform.forward * direction.y;
            characterController.Move(moveDirection * speed * Time.deltaTime);





        }





        Debug.Log(StaticVariables.currentInteraction);

        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));


        if (Physics.Raycast(ray, out RaycastHit hitItem, 3f) || StaticVariables.promptInterogation)
        {
            GameObject hitItemInfo = hitItem.collider?.gameObject;

            if (hitItem.collider != null &&
                (hitItem.collider.CompareTag("interactable") || hitItem.collider.CompareTag("QuestionableEvidence")))
            {
                StaticVariables.currentInteraction = hitItemInfo;
                wasLooking = true;
                return;

            }
            
      
        }


        //used to visually show the user whats being contained statically.
      
        p_currentInteraction = StaticVariables.currentInteraction;
        evidenceList = StaticVariables.questionablEvidence;
        P_followUpInteraction = StaticVariables.runnerUpInteraction;
      
   

        

        //Debug.Log(StaticVariables.hasQuestionableEvidence);
      //  Debug.Log(StaticVariables.canInteract);
        //Debug.Log(StaticVariables.questionablEvidence);
      
    }












    private void Interact(InputAction.CallbackContext context)
    {
        // FindObjectOfType<Interactions>().dialogueInteraction(); starts interactions.

        if(StaticVariables.currentInteraction != null)
        {
            if(StaticVariables.currentInteraction.GetComponent<Interactions>() != null)
            {
                var currentInteraction = GetcCurrentInteractionComponent<Interactions>();
                wasLooking = false;

                currentInteraction.dialogueInteraction();
            }


 
            if(StaticVariables.currentInteraction.GetComponent<EvidencePlaceholder>() != null)
            {
                if(FindAnyObjectByType<playerInventory>().playerEvidenceList.Contains(StaticVariables.currentInteraction.GetComponent<EvidencePlaceholder>().evidence))
                {
                    Debug.Log("already in your inventory!");
                    return;
                }

                FindAnyObjectByType<playerInventory>().playerEvidenceList.Add(StaticVariables.currentInteraction.GetComponent<EvidencePlaceholder>().evidence);
                FindAnyObjectByType<playerInventory>().inventoryUpdate.Invoke();

            }
            
        }
     
   

        
        //start dialogue from whatevers currently in the static "current interaction." 
    }


    private void Click(InputAction.CallbackContext context)
    {
        if(StaticVariables.isConversing && StaticVariables.initialInteraction && StaticVariables.sentenceCompletion == true)
        {
            FindAnyObjectByType<DialogueManager>().nextSentence();
        }

        if (StaticVariables.isConversing && StaticVariables.nextInteraction && StaticVariables.sentenceCompletion == true)
        {
            FindAnyObjectByType<DialogueManager>().nextFollowUpSentence();
        }
    }



   private void ReturnItemMaterial (GameObject hitItem)
    {
        hitItem.GetComponent<EvidencePlaceholder>().ReturnItemMaterial();
    }
    private void HighlightItem(GameObject hitItem)
    {
        hitItem.GetComponent<EvidencePlaceholder>().HighLightItemMaterial();
    }


    private T GetcCurrentInteractionComponent<T>() where T : MonoBehaviour
    {
        var c_components = StaticVariables.currentInteraction.GetComponents<T>();

        foreach (var interaction in c_components)
        {
            if (interaction.enabled)
            {
                return interaction;
            }
        }
        return null;
    }

}
