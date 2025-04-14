using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Interactions : MonoBehaviour
{

    public Animator dialogueAnimation;
      public GameObject textBox;
       public TextMeshProUGUI subjectName;
      public TextMeshProUGUI subjectText;
     public bool hasFollowUp;
     public GameObject FollowUpInteraction;


 


    public Dialogue dialogue;
    public void dialogueInteraction()
    {
        FindObjectOfType<DialogueManager>().BeginConversation(dialogue);

      
       
    }

    

}
