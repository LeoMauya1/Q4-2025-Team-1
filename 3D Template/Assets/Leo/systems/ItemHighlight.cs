using UnityEngine;

public class ItemHighlight : MonoBehaviour
{

    private GameObject previouslyHighlightedItem;
    private float checktime = 0.1f;
    private float nextCheckTime = 0;



    private void Update()
    {
     if(Time.time >= nextCheckTime)
        {
            ItemHighlightHandler();
            nextCheckTime = Time.time + checktime;
        }
    }





    private void ItemHighlightHandler()
    {
        var ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));


        if (Physics.Raycast(ray, out RaycastHit hitItem, 3f) || StaticVariables.promptInterogation)
        {
            GameObject hitItemInfo = hitItem.collider?.gameObject;

            if (hitItem.collider != null &&
                (hitItem.collider.CompareTag("interactable") || hitItem.collider.CompareTag("QuestionableEvidence")))
            {

                if (hitItemInfo != previouslyHighlightedItem)
                {

                    if (previouslyHighlightedItem != null)
                    {
                        hitItemInfo.GetComponent<EvidencePlaceholder>().ReturnItemMaterial();
                    }

                    if(hitItemInfo.GetComponent<EvidencePlaceholder>() != null)
                    {
                     hitItemInfo.GetComponent<EvidencePlaceholder>().HighLightItemMaterial();
                     previouslyHighlightedItem = hitItemInfo;

                    }
                }

                StaticVariables.currentInteraction = hitItemInfo;
                return;
            }
        }


        if (previouslyHighlightedItem != null)
        {
            previouslyHighlightedItem.GetComponent<EvidencePlaceholder>().ReturnItemMaterial();
            previouslyHighlightedItem = null;
        }
    }
}
