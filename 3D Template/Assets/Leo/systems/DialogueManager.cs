using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public Animator textBoxAnimation;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueText;
    public Image txtBox;
    public static bool isConversing = false;
    private bool transitionComplete;
    public static bool canInteractAgain = false;
    public Vector3 camOffset;
    public GameObject mainCharacter;

    [Header("CAMERAS")]
    public Transform characterCam;
    public Camera mainCharacterCam;
    private Vector3 oGposition;

    private Queue<string> dialoguePiece;
    public static int sentenceTracker;

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
        StartCoroutine(dialogueStart(dialogue));

    }
    private void Update()
    {
        Debug.Log(isConversing);
    }

    public void nextSentence()
    {

        if (dialoguePiece.Count == 0)
        {
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
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        StartCoroutine(DialogueEnd());
    }


    IEnumerator dialogueStart(Dialogue dialogue)
    {

        textBoxAnimation.SetBool("IsInteracted", true);
        yield return new WaitForSeconds(0.5f);
        textBoxAnimation.SetBool("IsInteracted", false);
        txtBox.gameObject.SetActive(true);
        canInteractAgain = false;
        characterName.text = dialogue.charactername;
        txtBox.enabled = true;
        dialoguePiece.Clear();
        yield return new WaitUntil(() => textBoxAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
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
        if (isConversing)
        {

            textBoxAnimation.SetBool("conversationEnded", true);
            isConversing = false;
            yield return new WaitForSeconds(1);
            textBoxAnimation.SetBool("conversationEnded", false);
            yield return new WaitUntil(() => textBoxAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            txtBox.gameObject.SetActive(false);
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
            Debug.Log("nanana");
            Camera.main.transform.position = oGposition;
            Camera.main.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(-camOffset);


        }


    }


    private void aniTimer()
    {
        if (transitionComplete)
        {

            mainCharacter.GetComponent<MeshRenderer>().enabled = false;
            Camera.main.transform.position = characterCam.position;
            Camera.main.transform.rotation = Camera.main.transform.rotation * Quaternion.Euler(camOffset);
        }

    }

    private IEnumerator EndofDilogue()
    {
        FindObjectOfType<Interactions>().dialogueInteraction();
        isConversing = true;
        yield return new WaitForSeconds(6);
        isConversing = false;
    }

   
    // change this bru this is wack 
    
    // private void FixedUpdate()
   // {


       // if (moveMent.toggledInteraction)
       // {
        //    StartCoroutine(EndofDilogue());
      //  }
   //}
}
