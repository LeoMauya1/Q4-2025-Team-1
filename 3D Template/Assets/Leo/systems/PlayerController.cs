using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices;
using UnityEditor.Rendering;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UI;
using Unity.Collections;

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
    private bool isLooking;

    [Header("static Containers")]
    public GameObject P_followUpInteraction;
    public GameObject p_currentInteraction;
  
    public List<PhysicalEvidence> evidenceList = new List<PhysicalEvidence>();


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

       
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitInfo, 3f))
        {
          
          
            Debug.Log(hitInfo);

            if(hitInfo.collider.gameObject.tag == "interactable" || hitInfo.collider.gameObject.tag == "QuestionableEvidence")
            {
                StaticVariables.currentInteraction = hitInfo.collider.gameObject;
                isLooking = true;
            }
        }
        else
        {
            StaticVariables.currentInteraction = null;
           isLooking = false;
            Debug.Log("not hit");
        }


        //used to visually show the user whats being contained statically.
        StaticVariables.hasQuestionableEvidence = toggle;
        p_currentInteraction = StaticVariables.currentInteraction;
        evidenceList = StaticVariables.questionablEvidence;
        P_followUpInteraction = StaticVariables.runnerUpInteraction;
 
   

        if (movement.IsPressed() && StaticVariables.isConversing == false)
        {
            direction = movement.ReadValue<Vector2>();
            moveDirection = transform.right * direction.x + transform.forward * direction.y;
            characterController.Move(moveDirection * speed * Time.deltaTime);


        


        }

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
                StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueInteraction();
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



   




}
