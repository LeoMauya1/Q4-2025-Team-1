using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Testimonies", menuName = "Testimonies")]
public class Testimonies : ScriptableObject
{
    public string characterTestimony;
    [TextArea(5, 10)]
    public string[] testimony;
    public Image characterSprite;
}
