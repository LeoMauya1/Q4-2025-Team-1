using UnityEngine;
using TMPro;

public interface ICameraTransition
{
    public Animator dialogueAnimator { get; }
    public GameObject textBox { get; }
    public TextMeshProUGUI subjectText { get; } 

    public TextMeshProUGUI subjectName { get; }


}