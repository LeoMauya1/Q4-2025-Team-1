using UnityEngine;
using System.Collections.Generic;

public class UI_Inventory : MonoBehaviour
{

    private List<PhysicalEvidence> p_Inventory;
    public GameObject itemSpace;


    private void Awake()
    {
        p_Inventory = new List<PhysicalEvidence>();
        p_Inventory = FindAnyObjectByType<playerInventory>().playerEvidenceList;

    }

    private void Start()
    {
        
    }
    void Update()
    {
      


    }



    public void UIupdate()
    {
        if (p_Inventory != null)
        {
            foreach (var item in p_Inventory)
            {
                var ItemSpace = Instantiate(itemSpace);
                ItemSpace.transform.SetParent(gameObject.transform, false);
            }
        }
    }
}
