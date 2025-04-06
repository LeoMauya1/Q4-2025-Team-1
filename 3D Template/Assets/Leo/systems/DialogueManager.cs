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
       if(StaticVariables.hasQuestionableEvidence)
        {
            foreach(PhysicalEvidence physicalevidence in StaticVariables.questionablEvidence)
            {
                if(StaticVariables.currentInteraction != null && physicalevidence.ItemName == StaticVariables.currentInteraction.tag)
                {
                    BeginConversation(physicalevidence.itemDialogue);
                    return;
                }

            }
        }
        
       
       

        
        StartCoroutine(dialogueStart(dialogue));

    }
    private void Update()
    {
        if(yayaya)
        {
            Debug.Log("shoulder");
        }
    }

    public void nextSentence()
    {

        Debug.Log("someones keeping watch");

        if (dialoguePiece.Count == 0)

        {
            
            if (StaticVariables.currentInteraction.GetComponent<Interactions>().hasFollowUp)

            {

                
                StaticVariables.currentInteraction = StaticVariables.currentInteraction.GetComponent<Interactions>().followUpInteraction;
                Debug.Log($"{StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().dialogue.characterDialogue} has something to say!");
                StartCoroutine(FollowUpDialogue(StaticVariables.currentInteraction.GetComponent<Interactions>().dialogue));
               

            }
            else
            {
                Debug.Log("nobody cared to further extrapolate");
            }
           

            EndDialogue();
            return;
        }
        sentenceTracker = dialoguePiece.Count;
        string dialogue = dialoguePiece.Dequeue();
        StopAllCoroutines();
        StartCoroutine(LetterByLetter(dialogue));

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
        StartCoroutine(DialogueEnd());
    }


    IEnumerator dialogueStart(Dialogue dialogue)
    {
        StaticVariables.isConversing = true;
        StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("interactionEnabled", true);
        yield return new WaitForSeconds(0.5f);
        StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.SetBool("interactionEnabled", false);
        StaticVariables.currentInteraction.GetComponent<Interactions>().textBox.gameObject.SetActive(true);
        canInteractAgain = false;
        StaticVariables.currentInteraction.GetComponent<Interactions>().subjectName.text = dialogue.charactername;
        StaticVariables.currentInteraction.GetComponent<Interactions>().textBox.enabled = true;
        dialoguePiece.Clear();
        //yield return new WaitUntil(() => textBoxAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        transitionComplete = true;
        aniTimer();
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
            StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(false);
            StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.SetBool("InteractionEnded", true);
            // textBoxAnimation.SetBool("conversationEnded", true);
            StaticVariables.isConversing = false;
            yield return new WaitForSeconds(1);
            StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.SetBool("InteractionEnded", false);
            StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.SetBool("conversationEnded", false);
            yield return new WaitUntil(() => StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(false);
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
            StaticVariables.isConversing = false;
            Debug.Log("nanana");
            Camera.main.transform.position = oGposition;
            Camera.main.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-camOffset);


        }


    }


    private void aniTimer()
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
       
        //StaticVariables.currentInteraction = StaticVariables.currentInteraction.GetComponent<Interactions>().followUpInteraction;
        //Debug.Log("MOnkey");
        StaticVariables.isConversing = true;
        StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.SetBool("interactionEnabled", true);
        yield return new WaitForSeconds(0.5f);
        StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.SetBool("interactionEnabled", false);
        StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().textBox.gameObject.SetActive(true);
        canInteractAgain = false;
        StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().subjectName.text = dialogue.charactername;
        StaticVariables.currentInteraction.GetComponent<FollowUpInteraction>().textBox.enabled = true;
        dialoguePiece.Clear();
        OBJECTION = true;
        //yield return new WaitUntil(() => textBoxAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        transitionComplete = true;
        aniTimer();
        foreach (string dialogues in dialogue.characterDialogue)
        {


            dialoguePiece.Enqueue(dialogues);
        }



        nextSentence();

    }





















}
