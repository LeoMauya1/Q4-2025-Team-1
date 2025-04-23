using UnityEngine;
using UnityEngine.UI;


[CreateAssetMenu(fileName = "new Item", menuName = "ItemCreation")]
public class PhysicalEvidence : ScriptableObject
{
 
    public string ItemName;
    [TextArea(5, 10)]
    public string[] ItemDisc;
    public Sprite itemImage;
    public int IDnumber;
  
   




    [Header("IF SAID ITEM HAS ANY ASSOCIATED DIALOGUE WITH A CHARACTER/ MONOLOGUE")]
    public Dialogue itemDialogue;
 


   
    

}
