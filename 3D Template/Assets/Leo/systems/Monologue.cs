using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

public class Monologue : MonoBehaviour
{

    private Queue<string> dialoguePiece;
    public Interactions mainCharacter;
    public AudioClip clips;
    public AudioSource audioclips;
    public bool nextThought;
    public bool secondIteration;
     private int randomInt;
    public bool sentenceSkipped;
    public bool complete { get; set; }
    public Dialogue currentMonologue { get; set; }



    private void Start()
    {
        dialoguePiece = new Queue<string>();
    }
    public void BeginConversation(Dialogue dialogue)
    {

        currentMonologue = dialogue;
        StartCoroutine(DialogueStart(dialogue));



    }



    IEnumerator DialogueStart(Dialogue dialogue)
    {










        Debug.Log(dialogue.charactername);
        StaticVariables.initialInteraction = true;
        StaticVariables.isConversing = true;
        mainCharacter.textBox.SetActive(true);
        mainCharacter.subjectName.text = dialogue.charactername;
        dialoguePiece.Clear();
        foreach (string dialogues in dialogue.characterDialogue)
        {


            dialoguePiece.Enqueue(dialogues);


        }



        nextSentence();
        yield break;

    }



    public void nextSentence()
    {
        







        if (dialoguePiece.Count == 0)

        {
           Debug.Log("nobody cared to further extrapolate");

           StartCoroutine(DialogueEnd());

            



        }
        else
        {



         
            string dialogue = dialoguePiece.Dequeue();
            StopCoroutine(LetterByLetter(dialogue));
            StartCoroutine(LetterByLetter(dialogue));
        }





    }
    IEnumerator LetterByLetter(string sentence)
    {
              int letterCount = 0;
            mainCharacter.subjectText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
             mainCharacter.subjectText.text += letter;
            StaticVariables.dialogueTextprogressing = true;

            if (sentenceSkipped)
            {
                mainCharacter.subjectText.text = sentence;
                sentenceSkipped = false;
                StaticVariables.sentenceCompletion = true;
                yield break;
            }

            if (letter == '.' || letterCount % 2 == 0)
                {
 
                 audioclips.PlayOneShot(clips);
   
                }
        
                StaticVariables.sentenceCompletion = false;
                letterCount++;
                yield return new WaitForSeconds(letter == '.'? 0.1f : 0.03f);
            }
            StaticVariables.sentenceCompletion = true;
            StaticVariables.dialogueTextprogressing = false;




    }
    IEnumerator DialogueEnd()
    {


          
            StaticVariables.promptInterogation = false;
            StaticVariables.promptInterogation = false;
            StaticVariables.initialInteraction = false;
            yield return new WaitForSeconds(0.3f);
            mainCharacter.textBox.SetActive(false);
            StaticVariables.itemInteraction = false;
            StaticVariables.isConversing = false;
             nextThought = true;
             

        




    }










}
