using UnityEngine;

public class Interactions : MonoBehaviour
{


    public Dialogue dialogue;

    public void dialogueInteraction()
    {
        FindObjectOfType<DialogueManager>().BeginConversation(dialogue);

    }

}
