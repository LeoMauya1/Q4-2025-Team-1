using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class interactionEnabler : MonoBehaviour
{
    public MeshRenderer player;
    public TextMeshProUGUI interactText;
    public static bool canInteract;
    public Transform OgCamPos;
    public Image box;




    private void OnTriggerEnter(Collider other)
    {
        interactText.gameObject.SetActive(true);

        canInteract = true;
       

    }


    private void OnTriggerExit(Collider other)
    {
        box.enabled = false;
        player.enabled = true;
        canInteract = false;
        interactText.gameObject.SetActive(false);
        Camera.main.transform.position = OgCamPos.position;
        Camera.main.transform.rotation = OgCamPos.rotation;
    }
}
