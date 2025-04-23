using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class UI_Inventory : MonoBehaviour
{

    public  List<PhysicalEvidence> p_Inventory;
    public GameObject itemSpace;



    private void Awake()
    {
      
 
  
    }

    private void Start()
    {
        
    }
    void Update()
    {
     




    }



    public void UIupdate()
    {

        p_Inventory = FindAnyObjectByType<playerInventory>().playerEvidenceList;

        if (p_Inventory != null)
        {
           
            Debug.Log("updating UI");

             
   

            var ItemSpace = Instantiate(itemSpace);
            ItemSpace.GetComponent<Image>().sprite = StaticVariables.currentInteraction.GetComponent<EvidencePlaceholder>().evidence.itemImage;
            ItemSpace.GetComponent<EvidencePlaceholder>().evidence = StaticVariables.currentInteraction.GetComponent<EvidencePlaceholder>().evidence;
            ItemSpace.transform.SetParent(gameObject.transform, false);
            
             
            
            
        }
    }
}
