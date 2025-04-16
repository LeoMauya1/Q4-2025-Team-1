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

    private void Start()
    {
        dialoguePiece = new Queue<string>();
        oGposition = Camera.main.transform.position;

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
        Debug.Log("Skib");

    }
    private void Update()
    {

    }

    public void nextSentence()
    {



        if (dialoguePiece.Count == 0)

        {
            //CHECKS IF SOMEONE ELSE HAS SOMETHING TO SAY.
            if (StaticVariables.currentInteraction.GetComponent<Interactions>().hasFollowUp)

            {

                Debug.Log($"{StaticVariables.currentInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().dialogue.charactername} has something to say!");
                var runnerUpInteractionDialogue = StaticVariables.currentInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().dialogue;
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

        StaticVariables.initialInteraction = true;
        StaticVariables.isConversing = true;
  
        StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("interactionEnabled", true);
       
        yield return new WaitForSeconds(0.5f);
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
        if (StaticVariables.isConversing)
        {
            StaticVariables.initialInteraction = false;
            StaticVariables.currentInteraction.GetComponent<Interactions>().textBox.gameObject.SetActive(false);
            StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("InteractionEnded", true);
            // textBoxAnimation.SetBool("conversationEnded", true);
            yield return new WaitForSeconds(1);
            StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("InteractionEnded", false);
            StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("conversationEnded", false);
            //yield return new WaitUntil(() => StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            StaticVariables.currentInteraction.GetComponent<Interactions>().textBox.gameObject.SetActive(false);
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
            Debug.Log("nanana");
            //Camtransition
            StaticVariables.isConversing = false;

        }


    }


    private void Camtransition(Transform playerCam)
    {
        if (initialConversationStarted)
        {
            Debug.Log("start");
            mainCharacter.GetComponent<MeshRenderer>().enabled = false;

            mainCamera.transform.position = playerCam.position;
            mainCamera.transform.rotation = mainCamera.transform.rotation * Quaternion.Euler(camOffset);
        }
        else
        {
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
        }


        if(followUpConversattionStarted)
        {
            //lerping mechanics here. 

            Debug.Log("next person transition");
            mainCharacter.GetComponent<MeshRenderer>().enabled = false;

            mainCamera.transform.position = playerCam.position;
            mainCamera.transform.rotation = mainCamera.transform.rotation * Quaternion.Euler(camOffset);


        }
        else
        {
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
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
            Debug.Log("else");
            StaticVariables.runnerUpInteraction = StaticVariables.currentInteraction.GetComponent<Interactions>().FollowUpInteraction;
        }

       
        StaticVariables.isConversing = true;
        StaticVariables.nextInteraction = true;

        yield return new WaitForSeconds(0.5f);
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(true);
        Debug.Log(dialogue.charactername);
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().subjectName.text = dialogue.charactername;
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(true);
        dialoguePiece.Clear();

        //yield return new WaitUntil(() => StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        transitionComplete = true;
        Camtransition(characterCamera);
        foreach (string dialogues in dialogue.characterDialogue)
        {


            dialoguePiece.Enqueue(dialogues);
        }



        nextFollowUpSentence();
    }





    IEnumerator FollowUpDialogueEnd()
    {
         
            if(StaticVariables.isConversing)
        {
            StaticVariables.nextInteraction = false;
            StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.SetActive(false);
            yield return new WaitForSeconds(1);
            //yield return new WaitUntil(() => StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(false);

            Debug.Log("UIYYYYYYYYYYYYYYYYYYYYYY");

            StaticVariables.isConversing = false;
        }
            
        


    }



    public void nextFollowUpSentence()
    {



        if (dialoguePiece.Count == 0)
        {
            //CHECKS IF SOMEONE ELSE HAS SOMETHING TO SAY.
            if (StaticVariables.runnerUpInteraction.GetComponent<Interactions>().hasFollowUp)

            {

                Debug.Log($"{StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().dialogue.charactername} has something to say!");
                var followUpDialogue = StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().dialogue;
                var followUpCharacterCam = StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().FollowUpcharacterCam;
                StartCoroutine(Conversationextendor(followUpDialogue, followUpCharacterCam));
                return;



            }
            else
            {
                StartCoroutine(FollowUpDialogueEnd());
                return;
            }
        }

           
              
        //if (dialoguePiece.Count > 0 && StaticVariables.nextInteraction == false)
       // {
            Debug.Log("you still got more to say?!");
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
        yield return new WaitForSeconds(1.3f);
        StartCoroutine(FollowUpDialogue(runnerUpInteractionDialogue, characterCam));
    }

    private IEnumerator Conversationextendor(Dialogue runnerUpInteractionDialogue, Transform characterCam)
    {
        StartCoroutine(FollowUpDialogueEnd());
        yield return new WaitForSeconds(1.3f);
        StartCoroutine(FollowUpDialogue(runnerUpInteractionDialogue, characterCam));

    }
















}




















