using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

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
    private Vector3 direction;
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.3f;
    private Quaternion targetRotation;
    public string sentences;

    private GameObject runnerUpinteractions;

    private Coroutine currentTypingCoroutine;

    private Interactions mc_interactionR;


   //Do stuff with the next sentence function...... its the key.












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

        //var interactioncamera = StaticVariables.currentInteraction.GetComponent<Interactions>().interactionCamera;
        Debug.Log(StaticVariables.currentInteraction);
        var interactionCamera = GetcCurrentInteractionComponent<Interactions>();
      
        runnerUpinteractions = interactionCamera.FollowUpInteraction;

        Debug.Log(runnerUpinteractions);
        StartCoroutine(DialogueStart(dialogue, interactionCamera.interactionCamera));



    }

    public void BeginItemInteraction(int id)
    {
           var currentInteraction = GetcCurrentInteractionComponent<Interactions>().itemDialogues;
        
        if(id >= 0 && id < currentInteraction.Count)
        {
            Debug.Log("yah yha");  
            var interactionCamera = GetcCurrentInteractionComponent<Interactions>().interactionCamera;
            StaticVariables.itemInteraction = true;
            var selectedDialogue = currentInteraction[id];
            
            StartCoroutine(DialogueStart(selectedDialogue, interactionCamera));


        }
           
        
        
       
     
    }
    private void Update()
    {
        //mc_interactionR = GetcCurrentInteractionComponent<Interactions>();
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
        var currentInteraction = GetcCurrentInteractionComponent<Interactions>();


        if (dialoguePiece.Count == 0)

        {
            Debug.Log("CHECK");
            //CHECKS IF SOMEONE ELSE HAS SOMETHING TO SAY.
            if (currentInteraction.hasFollowUp && StaticVariables.itemInteraction == false)

            {
                //var runnerUpINteraction = 
                Debug.Log($"{StaticVariables.currentInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().dialogue1.charactername} has something to say!");
                var runnerUpInteractionDialogue = GetFollowUpInteractionComponent<FollowUpInteraction>().dialogue1;
                var characterCam = GetFollowUpInteractionComponent<FollowUpInteraction>().FollowUpcharacterCam;
                StartCoroutine(FollowUptransition(runnerUpInteractionDialogue, characterCam));
               
                return;



            }
            
            else
            {
                //var runnerUpDialogue = StaticVariables.runnerUpInteraction.GetComponent<Idialogue>();
                Debug.Log("nobody cared to further extrapolate");
               
                StartCoroutine(DialogueEnd());
            
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
       
        
        var currentInteraction = GetcCurrentInteractionComponent<Interactions>();
        if(currentInteraction.dialogue.playerResponse && mainCharacter.GetComponent<MC_interactions>().mcDialogue)
        {

            mainCharacter.GetComponent<MC_interactions>().subjectText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                if (letter == '.')
                {

                    mainCharacter.GetComponent<MC_interactions>().subjectText.text += letter;
                    StaticVariables.sentenceCompletion = false;
                    yield return new WaitForSeconds(0.1f);
                }

                mainCharacter.GetComponent<MC_interactions>().subjectText.text += letter;
                StaticVariables.sentenceCompletion = false;
                yield return new WaitForSeconds(0.03f);
            }
            StaticVariables.sentenceCompletion = true;
        }
        else
        {
            currentInteraction.subjectText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                if (letter == '.')
                {

                    currentInteraction.subjectText.text += letter;
                    StaticVariables.sentenceCompletion = false;
                    yield return new WaitForSeconds(0.1f);
                }

                currentInteraction.subjectText.text += letter;
                StaticVariables.sentenceCompletion = false;
                yield return new WaitForSeconds(0.03f);
            }
            StaticVariables.sentenceCompletion = true;
        }
        
       

    }

    void EndDialogue()
    {
        //StartCoroutine(DialogueEnd());
    }


    IEnumerator DialogueStart(Dialogue dialogue, Transform targetCamera)
    {
        var currentInteraction = GetcCurrentInteractionComponent<Interactions>();
        
        
        
        
        
        if(currentInteraction.dialogue.playerResponse && mainCharacter.GetComponent<MC_interactions>().mcDialogue && mainCharacter.GetComponent<MC_interactions>().mcDialogue)
        {
            currentInteraction.textBox.SetActive(false);
            StaticVariables.initialInteraction = true;
            StaticVariables.isConversing = true;
            mainCharacter.GetComponent<MC_interactions>().textBox.SetActive(true);
            mainCharacter.GetComponent<MC_interactions>().subjectName.text = dialogue.charactername;
            dialoguePiece.Clear();
            initialConversationStarted = true;
            foreach (string dialogues in dialogue.characterDialogue)
            {


                dialoguePiece.Enqueue(dialogues);
            }



            nextSentence();
            yield break;
        }




        oGposition = Camera.main.transform.position;
        StaticVariables.initialInteraction = true;
        StaticVariables.isConversing = true;

        currentInteraction.dialogueAnimation.SetBool("interactionEnabled", true);
       
        yield return new WaitForSeconds(0.3f);
        currentInteraction.dialogueAnimation.SetBool("interactionEnabled", false);
        currentInteraction.GetComponent<Interactions>().textBox.SetActive(true);
        currentInteraction.GetComponent<Interactions>().subjectName.text = dialogue.charactername;
        dialoguePiece.Clear();
        //yield return new WaitUntil(() => StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        initialConversationStarted = true;
        var parent = currentInteraction.transform;
        Camtransition(targetCamera, parent);
        foreach (string dialogues in dialogue.characterDialogue)
        {


            dialoguePiece.Enqueue(dialogues);
        }



        nextSentence();

    }

    IEnumerator DialogueEnd()
    {
        var currentInteraction = GetcCurrentInteractionComponent<Interactions>();


        if (StaticVariables.itemInteraction == true)
        {
            StaticVariables.promptInterogation = false;
            StaticVariables.promptInterogation = false;
            StaticVariables.initialInteraction = false;
            currentInteraction.textBox.gameObject.SetActive(false);
            currentInteraction.dialogueAnimation.SetBool("InteractionEnded", true);
            yield return new WaitForSeconds(0.3f);
         
            StaticVariables.itemInteraction = false;
            Debug.Log(StaticVariables.dragAndDropInteraction);
            StaticVariables.isConversing = false;
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
            StaticVariables.currentInteraction = null;
            mainCamera.transform.position = oGposition;
            yield break;


        }
        if(mainCharacter.GetComponent<MC_interactions>().mcDialogue == true)
        {
            StaticVariables.promptInterogation = false;
            StaticVariables.promptInterogation = false;
            StaticVariables.initialInteraction = false;
            yield return new WaitForSeconds(0.3f);

            mainCharacter.GetComponent<MC_interactions>().textBox.SetActive(false);
            mainCharacter.GetComponent<MC_interactions>().mcDialogue = false;
            StaticVariables.itemInteraction = false;
            StaticVariables.isConversing = false;
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
            yield break;
        }



        if (currentInteraction.dialogue.playerResponse && mainCharacter.GetComponent<MC_interactions>().mcDialogue == false)
        {
          
            StaticVariables.promptInterogation = false;
            StaticVariables.promptInterogation = false;
            StaticVariables.initialInteraction = false;
            currentInteraction.textBox.gameObject.SetActive(false);
            StaticVariables.itemInteraction = false;
            StaticVariables.isConversing = false;
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;

            Debug.Log(dialoguePiece.Count);
            mainCharacter.GetComponent<MC_interactions>().dialogueInteraction(currentInteraction.dialogue.charactername);


            
            yield break;
        }


        if (StaticVariables.isConversing && StaticVariables.currentInteraction.GetComponent<Interactions>().hasFollowUp == false)
        {
            StaticVariables.promptInterogation = false;

            StaticVariables.initialInteraction = false;
            currentInteraction.textBox.gameObject.SetActive(false);
            currentInteraction.dialogueAnimation.SetBool("InteractionEnded", true);
            // textBoxAnimation.SetBool("conversationEnded", true);
            yield return new WaitForSeconds(0.3f);
            currentInteraction.dialogueAnimation.SetBool("InteractionEnded", false);
            currentInteraction.dialogueAnimation.SetBool("conversationEnded", false);
            //yield return new WaitUntil(() => StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            currentInteraction.textBox.gameObject.SetActive(false);
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
            StaticVariables.currentInteraction = null;
            mainCamera.transform.position = oGposition;
            initialConversationStarted = false;
            StaticVariables.isConversing = false;
        }

        if (StaticVariables.isConversing && StaticVariables.currentInteraction.GetComponent<Interactions>().hasFollowUp)
        {
            StaticVariables.promptInterogation = false;
            StaticVariables.itemInteraction =false;
            Debug.Log("thats all folks!");
            StaticVariables.initialInteraction = false;
            currentInteraction.textBox.gameObject.SetActive(false);
            currentInteraction.dialogueAnimation.SetBool("InteractionEnded", true);
            // textBoxAnimation.SetBool("conversationEnded", true);
            yield return new WaitForSeconds(0.3f);
            currentInteraction.dialogueAnimation.SetBool("InteractionEnded", false);
            currentInteraction.dialogueAnimation.SetBool("conversationEnded", false);
            //yield return new WaitUntil(() => StaticVariables.currentInteraction.GetComponent<Interactions>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            currentInteraction.textBox.gameObject.SetActive(false);
            initialConversationStarted = false;
            StaticVariables.itemInteraction = false;
          
     
        }

    }


    private void Camtransition(Transform playerCam, Transform parent)
    {
        Debug.Log("Camtransition called with itemInteraction = " + StaticVariables.itemInteraction);
        mainCharacter.GetComponent<MeshRenderer>().enabled = false;

        startPosition = mainCamera.transform.position;
        startRotation = mainCamera.transform.rotation;

        targetPosition = playerCam.position;
        targetRotation = Quaternion.LookRotation(-parent.forward, Vector3.up); // Always rotate toward the current speaker

        transitionTimer = 0f;
        isTransitioning = true;









    }







    IEnumerator FollowUpDialogue(Dialogue dialogue, Transform characterCamera)
    {

        var runnerUpinteraction = GetFollowUpInteractionComponent<Interactions>();
        var runnerUpinteractionF = GetFollowUpInteractionComponent<FollowUpInteraction>();
        if (runnerUpinteractions != null && runnerUpinteraction.FollowUpInteraction != null)
        {
            StaticVariables.runnerUpInteraction = runnerUpinteraction.FollowUpInteraction;
        } 
        else
        {
   
            StaticVariables.runnerUpInteraction = runnerUpinteraction.FollowUpInteraction;
        }

       
        StaticVariables.isConversing = true;
        StaticVariables.nextInteraction = true;

        yield return new WaitForSeconds(0.3f);
        runnerUpinteractionF.textBox.gameObject.SetActive(true);

        runnerUpinteractionF.subjectName.text = dialogue.charactername;
        runnerUpinteractionF.textBox.gameObject.SetActive(true);
        dialoguePiece.Clear();

        //yield return new WaitUntil(() => StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        transitionComplete = true;
        followUpConversattionStarted = true;
        var parent = runnerUpinteractions.transform;
        Camtransition(characterCamera, parent);
        foreach (string dialogues in dialogue.characterDialogue)
        {


            dialoguePiece.Enqueue(dialogues);
        }



        nextFollowUpSentence();
    }





    IEnumerator FollowUpDialogueEnd(Transform characterCam)
    {
        var runnerUp = GetFollowUpInteractionComponent<Interactions>();
        var runnerUpF = GetFollowUpInteractionComponent<FollowUpInteraction>();
        if (runnerUp.doubleDown == false && runnerUp.FollowUpInteraction == null)
        {
            StaticVariables.nextInteraction = false;
            runnerUpF.textBox.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            //yield return new WaitUntil(() => StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            runnerUpF.textBox.gameObject.SetActive(false);



            StaticVariables.isConversing = false;

            followUpConversattionStarted = false;
            StaticVariables.runnerUpInteraction = null;
            StaticVariables.currentInteraction = null;
            mainCamera.transform.position = oGposition;
            mainCharacter.GetComponent<MeshRenderer>().enabled = true;
            yield break;

        }

        if (StaticVariables.isConversing && runnerUp.doubleDown == true)
        {

            StaticVariables.nextInteraction = false;
            runnerUpF.textBox.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            //yield return new WaitUntil(() => StaticVariables.runnerUpInteraction.GetComponent<FollowUpInteraction>().dialogueAnimation.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
            runnerUpF.textBox.gameObject.SetActive(false);



            StaticVariables.isConversing = false;

            followUpConversattionStarted = false;


            var parent = runnerUpinteractions.transform;
               Camtransition(characterCam, parent);
        }





    }



    public void nextFollowUpSentence()
    {

        var runnerUp  = GetFollowUpInteractionComponent<Interactions>();
        var runnerUpF = GetFollowUpInteractionComponent<FollowUpInteraction>();

        if (dialoguePiece.Count == 0)
        {
            //CHECKS IF SOMEONE ELSE HAS SOMETHING TO SAY.
            if (runnerUp.doubleDown && runnerUp.FollowUpInteraction != null   )

            {

                //Debug.Log($"{StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().dialogue2.charactername} has something to say!");
                var followUpDialogue = StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().dialogue2;
                var followUpCharacterCam = StaticVariables.runnerUpInteraction.GetComponent<Interactions>().FollowUpInteraction.GetComponent<FollowUpInteraction>().FollowUpcharacterCam;

                StartCoroutine(Conversationextendor(followUpDialogue, followUpCharacterCam));
                return;



            }
            else
            {
                var runnerUpcam = runnerUpF.FollowUpcharacterCam;
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
        var runnerUp = GetFollowUpInteractionComponent<FollowUpInteraction>();
        runnerUp.subjectText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {


            runnerUp.subjectText.text += letter;
            StaticVariables.sentenceCompletion = false;
            yield return new WaitForSeconds(0.03f);
        }
        StaticVariables.sentenceCompletion = true;
    }





    private IEnumerator FollowUptransition(Dialogue runnerUpInteractionDialogue, Transform characterCam)
    {
        StaticVariables.itemInteraction = false;
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





    private T GetcCurrentInteractionComponent<T>() where T : MonoBehaviour
    {
        var c_components = StaticVariables.currentInteraction.GetComponents<T>();
  
        foreach(var interaction in c_components)
        {
            if(interaction.enabled)
            {
                return interaction;
            }
        }
        return null;
    }
    private T GetFollowUpInteractionComponent<T>() where T : MonoBehaviour
    {
        var c_components = runnerUpinteractions.GetComponents<T>();

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




















