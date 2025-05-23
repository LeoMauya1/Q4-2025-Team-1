using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class MC_interactions : MonoBehaviour
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
    public bool mcDialogue;






    public void dialogueInteraction(string characterName)
    {


        foreach(var dialogueOption in dialogueOptions)
        {
            if(characterName == dialogueOption.mc_CharacterResponse)
            {
            


               for (int i = 0; i < dialogueOptions.Count; i++)
               {
                if( i == StaticVariables.eventValue)
                {
                        mcDialogue = true;
                        FindObjectOfType<DialogueManager>().BeginConversation(dialogueOptions[i]);
                        return;
                }
               }
            }
            return;
        }


    }
    public void Interactions()
    {
        mcDialogue = true;
        FindObjectOfType<DialogueManager>().BeginConversation(dialogue);
    }
    



    public void ItemInteraction(int itemID)
    {
        FindAnyObjectByType<DialogueManager>().BeginItemInteraction(itemID);
    }


}
