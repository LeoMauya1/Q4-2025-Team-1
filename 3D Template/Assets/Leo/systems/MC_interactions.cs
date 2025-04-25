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


    public void dialogueInteraction()
    {
        FindObjectOfType<DialogueManager>().BeginConversation(dialogueOptions[StaticVariables.storyArc]);



    }
    public void ItemInteraction(int itemID)
    {
        FindAnyObjectByType<DialogueManager>().BeginItemInteraction(itemID);
    }


}
