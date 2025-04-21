using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class interactionEnabler : MonoBehaviour
{
    //public MeshRenderer player;
    //public TextMeshProUGUI interactText;
   // public Transform OgCamPos;
   // public Image box;




    private void OnTriggerEnter(Collider other)
    {
        //interactText.gameObject.SetActive(true);

        StaticVariables.canInteract = true;

       


    }


    private void OnTriggerExit(Collider other)
    {
    
    
    }
}
