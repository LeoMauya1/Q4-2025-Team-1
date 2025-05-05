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
    public bool doubleDown;
     public GameObject FollowUpInteraction;
    public Transform interactionCamera;

    public List<Dialogue> itemDialogues;


    public Dialogue dialogue;
    public Dialogue[] monologues;
 


    public void dialogueInteraction()
    {
        FindObjectOfType<DialogueManager>().BeginConversation(dialogue);

      
       
    }

    public void MonologueInteraction()
    {
        FindObjectOfType<Monologue>().BeginConversation(monologues[0]);
       
    }
    public void nextStreamOfConscious()
    {
        FindObjectOfType<Monologue>().BeginConversation(monologues[1]);
       
    }

    public void nextStreamOfConscious2()
    {
        FindObjectOfType<Monologue>().BeginConversation(monologues[2]);
        

    }




    public void ItemInteraction(int itemID)
    {
        FindAnyObjectByType<DialogueManager>().BeginItemInteraction(itemID);
    }

    

}
