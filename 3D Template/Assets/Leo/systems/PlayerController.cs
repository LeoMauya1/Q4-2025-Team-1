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
       


        //used to visually show the user whats being contained statically.
        StaticVariables.hasQuestionableEvidence = toggle;
        p_currentInteraction = StaticVariables.currentInteraction;
        evidenceList = StaticVariables.questionablEvidence;
        Debug.Log(StaticVariables.isConversing);

        if (movement.IsPressed())
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
            StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueInteraction();
            
        }
     
   

        
        //start dialogue from whatevers currently in the static "current interaction." 
    }


    private void Click(InputAction.CallbackContext context)
    {
        if(StaticVariables.isConversing && StaticVariables.initialInteraction)
        {
            FindAnyObjectByType<DialogueManager>().nextSentence();
        }

        if (StaticVariables.isConversing && StaticVariables.nextInteraction)
        {
            FindAnyObjectByType<DialogueManager>().nextFollowUpSentence();
        }
    }



    //private IEnumerator EndofDilogue()
    //{
      
   // }




}
