using UnityEngine;
[System.Serializable]
public class Dialogue
{
    public string charactername;
    [TextArea(5, 10)]
    public string[] characterDialogue;
    public int eventValue;
    public bool hasFollowUp;
}
