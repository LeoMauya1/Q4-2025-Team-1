using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FollowUpInteraction : MonoBehaviour
{
    public Dialogue dialogue1;
    public Dialogue dialogue2;
    public Animator dialogueAnimation;
     public GameObject textBox;
     public TextMeshProUGUI subjectName;
     public TextMeshProUGUI subjectText;
    public Transform FollowUpcharacterCam;


    


    public void DialogueInteraction(Dialogue dialogue)
    {
        FindAnyObjectByType<DialogueManager>().BeginConversation(dialogue);



    }


    


}
