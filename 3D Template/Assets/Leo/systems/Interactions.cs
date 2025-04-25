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
    public List<Dialogue> dialogueOptions;
    
  
    public void dialogueInteraction()
    {
        FindObjectOfType<DialogueManager>().BeginConversation(dialogueOptions[StaticVariables.storyArc]);

      
       
    }
    public void ItemInteraction(int itemID)
    {
        FindAnyObjectByType<DialogueManager>().BeginItemInteraction(itemID);
    }

    

}
