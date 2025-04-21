using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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

            foreach (var item in p_Inventory)
            {
             
   
                
             
                var ItemSpace = Instantiate(itemSpace);
                itemSpace.GetComponent<Image>().sprite = item.itemImage;
                ItemSpace.transform.SetParent(gameObject.transform, false);
            }
        }
    }
}
