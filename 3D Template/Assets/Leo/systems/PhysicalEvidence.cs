using UnityEngine;


[CreateAssetMenu(fileName = "new Item", menuName = "ItemCreation")]
public class PhysicalEvidence : ScriptableObject
{
 
    public string ItemName;
    [TextArea(5, 10)]
    public string[] ItemDisc;
   




    [Header("IF SAID ITEM HAS ANY ASSOCIATED DIALOGUE WITH A CHARACTER/ MONOLOGUE")]
    public Dialogue itemDialogue;
 


   
    

}
