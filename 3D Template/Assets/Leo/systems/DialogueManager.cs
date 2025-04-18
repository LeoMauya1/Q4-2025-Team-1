using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    //public Animator textBoxAnimation;
    //public TextMeshProUGUI characterName;
    //public TextMeshProUGUI dialogueText;
    // public Image txtBox;

    private bool transitionComplete;
    public Vector3 camOffset;
    public GameObject mainCharacter;
    private Vector3 oGposition;
    public int debug;
    private Queue<string> dialoguePiece;
    public static int sentenceTracker;
    private Camera mainCamera;
    private bool initialConversationStarted;
    private bool followUpConversattionStarted;




    private bool isTransitioning = false;
    private float transitionDuration = 0.3f;
    private float transitionTimer = 1f;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private Vector3 targetPosition;
    
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.3f;
    private Quaternion targetRotation;






    // VERY IMPORTAMT BUG, follow ups have some loop holes, like if yoy have a back to back conversation it requires each runner up to have their follow up bool selected. But if you were talk to that person in specific the it starts the follow up regardless.












    private void Start()
    {
        dialoguePiece = new Queue<string>();
        

        //Debug.Log(mainCharacterCam.transform.position);
        //Debug.Log(Camera.main.transform.position);


    }

    private void Awake()
    {
        mainCamera = Camera.main;
    } 



    public void BeginConversation(Dialogue dialogue)
    {
        if (StaticVariables.hasQuestionableEvidence)
        {
            foreach (PhysicalEvidence physicalevidence in StaticVariables.questionablEvidence)
            {
                if (StaticVariables.currentInteraction != null && physicalevidence.ItemName == StaticVariables.currentInteraction.tag)
                {
                    BeginConversation(physicalevidence.itemDialogue);
                    return;
                }

            }
        }

        var interactionCamera = StaticVariables.currentInteraction.GetComponent<Interactions>().interactionCamera;
        StartCoroutine(DialogueStart(dialogue, interactionCamera));


    }
    private void Update()
    {
        if (isTransitioning)
        {
            mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetPosition, ref velocity, smoothTime);
            mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, targetRotation, Time.deltaTime * 5f);

  
            if (Vector3.Distance(mainCamera.transform.position, targetPosition) < 0.05f && Quaternion.Angle(mainCamera.transform.rotation, targetRotation) < 1f)
            {
                isTransitioning = false;
            }
        }
    }

    public void nextSentence()
    {



        if (dialoguePiece.Count == 0)

        {
            //CHECKS IF SOMEONE ELSE HAS SOMETHING TO SAY.
            if (StaticVariables.currentInteraction.GetComponent<Interactions>().hasFollowUp)

            {

                Debug.Log($"{StaticVariables.currentInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().dialogue1.charactername} has something to say!");
                var runnerUpInteractionDialogue = StaticVariables.currentInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().dialogue1;
                var characterCam = StaticVariables.currentInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().FollowUpcharacterCam;
                StartCoroutine(FollowUptransition(runnerUpInteractionDialogue, characterCam));
               
                return;



            }
            
            else
            {
                //var runnerUpDialogue = StaticVariables.runnerUpInteraction.GetComponent<Idialogue>();
                Debug.Log("nobody cared to further extrapolate");
                StartCoroutine(DialogueEnd());
                return;
            }


       
        }else
        {
             sentenceTracker = dialoguePiece.Count;
        string dialogue = dialoguePiece.Dequeue();
        StopCoroutine(LetterByLetter(dialogue));
        StartCoroutine(LetterByLetter(dialogue));
        }
        
       

       

    }


    IEnumerator LetterByLetter(string sentence)
    {
        StaticVariables.currentInteraction.GetComponent<Interactions>().subjectText.text = "";
        
        foreach (char letter in sentence.ToCharArray())
        {
            StaticVariables.currentInteraction.GetComponent<Interactions>().subjectText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        //StartCoroutine(DialogueEnd());
    }


    IEnumerator DialogueStart(Dialogue dialogue, Transform targetCamera)
    {
        oGposition = Camera.main.transform.position;
        StaticVariables.initialInteraction = true;
        StaticVariables.isConversing = true;
  
        StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("interactionEnabled", true);
       
        yield return new WaitForSeconds(0.3f);
        StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("interactionEnabled", false);
        StaticVariables.currentInteraction.GetComponent<Interactions>().textBox.SetActive(true);
        StaticVariables.currentInteraction.GetComponent<Interactions>().subjectName.text = dialogue.charactername;
        dialoguePiece.Clear();
        //yield return new WaitUntil(() => StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        initialConversationStarted = true;
        Camtransition(targetCamera);
        foreach (string dialogues in dialogue.characterDialogue)
        {


            dialoguePiece.Enqueue(dialogues);
        }



        nextSentence();

    }

    IEnumerator DialogueEnd()
    {
        if (StaticVariables.isConversing && StaticVariables.currentInteraction.GetComponent<Interactions>().hasFollowUp)
        {
            StaticVariables.initialInteraction = false;
            StaticVariables.currentInteraction.GetComponent<Interactions>().textBox.gameObject.SetActive(false);
            StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("InteractionEnded", true);
            // textBoxAnimation.SetBool("conversationEnded", true);
            yield return new WaitForSeconds(0.3f);
            StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("InteractionEnded", false);
            StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("conversationEnded", false);
            //yield return new WaitUntil(() => StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            StaticVariables.currentInteraction.GetComponent<Interactions>().textBox.gameObject.SetActive(false);
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
            initialConversationStarted = false;
     
            //Camtransition
            StaticVariables.isConversing = false;

        }
        if(StaticVariables.isConversing && StaticVariables.currentInteraction.GetComponent<Interactions>().hasFollowUp == false)
        {
            StaticVariables.initialInteraction = false;
            StaticVariables.currentInteraction.GetComponent<Interactions>().textBox.gameObject.SetActive(false);
            StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("InteractionEnded", true);
            // textBoxAnimation.SetBool("conversationEnded", true);
            yield return new WaitForSeconds(0.3f);
            StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("InteractionEnded", false);
            StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("conversationEnded", false);
            //yield return new WaitUntil(() => StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            StaticVariables.currentInteraction.GetComponent<Interactions>().textBox.gameObject.SetActive(false);
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
      
            mainCamera.transform.position = oGposition;
            initialConversationStarted = false;
            StaticVariables.isConversing = false;
        }

    }


    private void Camtransition(Transform playerCam)
    {
        if (initialConversationStarted)
        {
     
            mainCharacter.GetComponent<MeshRenderer>().enabled = false;

            startPosition = mainCamera.transform.position;
            startRotation = mainCamera.transform.rotation;
            targetPosition = playerCam.position;
            targetRotation = Quaternion.Euler(camOffset) * startRotation;
            transitionTimer = 0f;
            isTransitioning = true;
            return;
        }



        if (followUpConversattionStarted)
        {
            //lerping mechanics here. 

             mainCharacter.GetComponent<MeshRenderer>().enabled = false;

             startPosition = mainCamera.transform.position;
             startRotation = mainCamera.transform.rotation;
             targetPosition = playerCam.position;
             targetRotation = Quaternion.Euler(camOffset) * startRotation; 
             transitionTimer = 0f;
             isTransitioning = true;






        }
 
        
             
            
          
            

        





    }







    IEnumerator FollowUpDialogue(Dialogue dialogue, Transform characterCamera)
    {
        if(StaticVariables.runnerUpInteraction != null && StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction != null)
        {
            StaticVariables.runnerUpInteraction = StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction;
        } 
        else
        {
   
            StaticVariables.runnerUpInteraction = StaticVariables.currentInteraction.GetComponent<Interactions>().FollowUpInteraction;
        }

       
        StaticVariables.isConversing = true;
        StaticVariables.nextInteraction = true;

        yield return new WaitForSeconds(0.3f);
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(true);
  
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().subjectName.text = dialogue.charactername;
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(true);
        dialoguePiece.Clear();

        //yield return new WaitUntil(() => StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        transitionComplete = true;
        followUpConversattionStarted = true;
        Camtransition(characterCamera);
        foreach (string dialogues in dialogue.characterDialogue)
        {


            dialoguePiece.Enqueue(dialogues);
        }



        nextFollowUpSentence();
    }





    IEnumerator FollowUpDialogueEnd(Transform characterCam)
    {
         
            if(StaticVariables.isConversing)
        {

            StaticVariables.nextInteraction = false;
            StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            //yield return new WaitUntil(() => StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(false);



            StaticVariables.isConversing = false;
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
            followUpConversattionStarted = false;
            Debug.Log(StaticVariables.isConversing);
            Debug.Log(StaticVariables.runnerUpInteraction.GetComponent<Interactions>().hasFollowUp);
            if ( StaticVariables.runnerUpInteraction.GetComponent<Interactions>().doubleDown == false)
            {


                mainCamera.transform.position = oGposition;
            }

               Camtransition(characterCam);
        }

       



    }



    public void nextFollowUpSentence()
    {



        if (dialoguePiece.Count == 0)
        {
            //CHECKS IF SOMEONE ELSE HAS SOMETHING TO SAY.
            if (StaticVariables.currentInteraction.GetComponent<Interactions>().doubleDown && StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction != null   )

            {

                //Debug.Log($"{StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().dialogue2.charactername} has something to say!");
                var followUpDialogue = StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().dialogue2;
                var followUpCharacterCam = StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().FollowUpcharacterCam;

                StartCoroutine(Conversationextendor(followUpDialogue, followUpCharacterCam));
                return;



            }
            else
            {
                var runnerUpcam = StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().FollowUpcharacterCam;
                StartCoroutine(FollowUpDialogueEnd(runnerUpcam));
                return;
            }
        }

           
              
        //if (dialoguePiece.Count > 0 && StaticVariables.nextInteraction == false)
       // {
       
            sentenceTracker = dialoguePiece.Count;
            string dialogue = dialoguePiece.Dequeue();
            StopAllCoroutines();
            StartCoroutine(LetterByLetterFollowUp(dialogue));
     //   }




















    }





    IEnumerator LetterByLetterFollowUp(string sentence)
    {
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().subjectText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().subjectText.text += letter;
            yield return null;
        }
    }





    private IEnumerator FollowUptransition(Dialogue runnerUpInteractionDialogue, Transform characterCam)
    {
        StartCoroutine(DialogueEnd());
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(FollowUpDialogue(runnerUpInteractionDialogue, characterCam));
    }

    private IEnumerator Conversationextendor(Dialogue runnerUpInteractionDialogue, Transform characterCam)
    {
        StartCoroutine(FollowUpDialogueEnd(characterCam));
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(FollowUpDialogue(runnerUpInteractionDialogue, characterCam));

    }
















}




















