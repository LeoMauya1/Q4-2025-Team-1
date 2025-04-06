using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Interactions : MonoBehaviour
{

 
    public Dialogue dialogue;
    public Image textBox;
    public Animator dialogueAnimation;
    public TextMeshProUGUI subjectName;
    public TextMeshProUGUI subjectText;
    public bool hasFollowUp;
    public GameObject followUpInteraction;


    public void dialogueInteraction()
    {
        FindObjectOfType<DialogueManager>().BeginConversation(dialogue);

      
       
    }

}
