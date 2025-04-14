using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //public Animator textBoxAnimation;
    //public TextMeshProUGUI characterName;
    //public TextMeshProUGUI dialogueText;
    // public Image txtBox;

    private bool transitionComplete;
    public static bool canInteractAgain = false;
    public Vector3 camOffset;
    public GameObject mainCharacter;
    public bool OBJECTION;
    [Header("CAMERAS")]
    public Transform characterCam;
    public Camera mainCharacterCam;
    private Vector3 oGposition;
    public int debug;
    private Queue<string> dialoguePiece;
    public static int sentenceTracker;
    public bool yayaya;

    private void Start()
    {
        dialoguePiece = new Queue<string>();
        oGposition = Camera.main.transform.position;

        //Debug.Log(mainCharacterCam.transform.position);
        //Debug.Log(Camera.main.transform.position);


    }

    private void Awake()
    {

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


        StartCoroutine(DialogueStart(dialogue));
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
                StartCoroutine(FollowUpDialogue(runnerUpInteractionDialogue));




            }
            else
            {
                //var runnerUpDialogue = StaticVariables.runnerUpInteraction.GetComponent<Idialogue>();
                Debug.Log("nobody cared to further extrapolate");
                StartCoroutine(DialogueEnd());
            }




            StartCoroutine(DialogueEnd());
            return;
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


    IEnumerator DialogueStart(Dialogue dialogue)
    {

        StaticVariables.initialInteraction = true;
        StaticVariables.isConversing = true;
        StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("interactionEnabled", true);
        yield return new WaitForSeconds(0.5f);
        StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("interactionEnabled", false);
        StaticVariables.currentInteraction.GetComponent<Interactions>().textBox.SetActive(true);
        canInteractAgain = false;
        StaticVariables.currentInteraction.GetComponent<Interactions>().subjectName.text = dialogue.charactername;
        dialoguePiece.Clear();
        //yield return new WaitUntil(() => StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        transitionComplete = true;
        //Camtransition();
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
            Camera.main.transform.position = oGposition;
            Camera.main.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-camOffset);
            StaticVariables.isConversing = false;

        }


    }


    private void Camtransition()
    {
        if (transitionComplete)
        {

            //mainCharacter.GetComponent<MeshRenderer>().enabled = false;
            //Camera.main.transform.position = characterCam.position;
            // Camera.main.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(camOffset);
        }

    }







    IEnumerator FollowUpDialogue(Dialogue dialogue)
    {
        if(StaticVariables.runnerUpInteraction != null)
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
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.SetBool("interactionEnabled", true);
        yield return new WaitForSeconds(0.5f);
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.SetBool("interactionEnabled", false);
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(true);
        canInteractAgain = false;
        Debug.Log(dialogue.charactername);
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().subjectName.text = dialogue.charactername;
        StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(true);
        dialoguePiece.Clear();

        //yield return new WaitUntil(() => StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        transitionComplete = true;

        foreach (string dialogues in dialogue.characterDialogue)
        {


            dialoguePiece.Enqueue(dialogues);
        }



        nextFollowUpSentence();
    }





    IEnumerator FollowUpDialogueEnd()
    {
   
        if (StaticVariables.isConversing)
        {
   
            StaticVariables.nextInteraction = false;
            StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.SetActive(false);
            StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.SetBool("InteractionEnded", true);
            StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.SetBool("conversationEnded", true);
            //StaticVariables.isConversing = false;
            yield return new WaitForSeconds(1);
            StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.SetBool("InteractionEnded", false);
            StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.SetBool("conversationEnded", false);
            //yield return new WaitUntil(() => StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(false);
            //mainCharacter.GetComponent<MeshRenderer>().enabled = true;
            //StaticVariables.isConversing = false;
            Debug.Log("UIYYYYYYYYYYYYYYYYYYYYYY");
            Camera.main.transform.position = oGposition;
            Camera.main.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-camOffset);
            StaticVariables.isConversing =  false;
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
                StartCoroutine(FollowUpDialogue(followUpDialogue));
                return;



            }
            else
            {
                StartCoroutine(FollowUpDialogueEnd());
                return;
            }


        }
        else
        {

            sentenceTracker = dialoguePiece.Count;
            string dialogue = dialoguePiece.Dequeue();
            StopCoroutine(LetterByLetterFollowUp(dialogue));
            StartCoroutine(LetterByLetterFollowUp(dialogue));
        }




















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

























}




















