using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Dialogue
{
    public string charactername;
    [TextArea(5, 10)]
    public string[] characterDialogue;
    public string mc_CharacterResponse;
    public bool playerResponse;
    public int queueCount;
    public Image[] charcterSprite; 




}
