using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class playerInventory : MonoBehaviour
{
    public List<PhysicalEvidence> playerEvidenceList;
    public List<Testimonies> playerTestimonyList;
    public UnityEvent inventoryUpdate;


    private void Awake()
    {
        playerEvidenceList = new List<PhysicalEvidence>();
        playerTestimonyList = new List<Testimonies>();
    }

    

}
