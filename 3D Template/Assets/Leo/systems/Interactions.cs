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

  

    public void MonologueHandler(int index)
    {
        if(index >= 0 &&  index < monologues.Length)
        {
            FindAnyObjectByType<Monologue>().BeginConversation(monologues[index]);
        }


    }


    public void MonologueInteraction()
    {
        FindAnyObjectByType<Monologue>().BeginConversation(monologues[0]);
    }
    public void nextStreamOfConscious()
    {
        FindAnyObjectByType<Monologue>().BeginConversation(monologues[1]);
    }
    public void nextStreamOfConscious2()
    {
        FindAnyObjectByType<Monologue>().BeginConversation(monologues[2]);
    }
    public void ItemInteraction(int itemID)
    {
        FindAnyObjectByType<DialogueManager>().BeginItemInteraction(itemID);
    }

    

}
