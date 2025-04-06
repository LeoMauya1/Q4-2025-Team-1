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

        if(other.CompareTag("QuestionableEvidence"))
        {
            EvidencePlaceholder evidencePlaceholder = other.GetComponent<EvidencePlaceholder>();


            Debug.Log(evidencePlaceholder);
            StaticVariables.questionablEvidence.Add(evidencePlaceholder.evidence);
            
        }

        StaticVariables.currentInteraction = other.gameObject;


    }


    private void OnTriggerExit(Collider other)
    {
        // box.enabled = false;
        // player.enabled = true;
        StaticVariables.canInteract = false;
        StaticVariables.currentInteraction = null;
        // interactText.gameObject.SetActive(false);
        //   Camera.main.transform.position = OgCamPos.position;
        //   Camera.main.transform.rotation = OgCamPos.rotation;
    }
}
