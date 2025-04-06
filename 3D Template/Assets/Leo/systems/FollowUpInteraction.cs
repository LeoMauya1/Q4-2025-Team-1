using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FollowUpInteraction : MonoBehaviour
{
    public Dialogue dialogue;
    public Image textBox;
    public Animator dialogueAnimation;
    public TextMeshProUGUI subjectName;
    public TextMeshProUGUI subjectText;


    public void DialogueInteraction(Dialogue dialogue)
    {
        FindAnyObjectByType<DialogueManager>().BeginConversation(dialogue);



    }


}
