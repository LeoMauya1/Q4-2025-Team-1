using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEditor.Overlays;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{


    private RectTransform m_rectTransform;
    private Camera mainCamera;
    private bool canInteract;
    public PhysicalEvidence currentItem;
    private int passedID;
    private Vector2 previosPos;
    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }
    public void OnBeginDrag (PointerEventData evedata)
    {

        previosPos = m_rectTransform.anchoredPosition;


    }
    public void OnEndDrag(PointerEventData evedata)
    {
        

        
    }
    public void OnDrag(PointerEventData eventdata)
    {
        m_rectTransform.anchoredPosition += eventdata.delta;

        Ray canvasRay = mainCamera.ScreenPointToRay(eventdata.position);
        if(Physics.Raycast(canvasRay, out RaycastHit hitInfo ))
        {

            if(hitInfo.collider.tag == "interactable")
            {
                Debug.Log(hitInfo.collider);

                if(hitInfo.collider.gameObject.GetComponent<Interactions>().itemDialogue != null)
                {
                    StaticVariables.currentInteraction = hitInfo.collider.gameObject;
                    Debug.Log(hitInfo.collider.gameObject);
                    canInteract = true;
                    StaticVariables.promptInterogation = true;
                    
                }
               
            }
          
        }
        else
        {
            Debug.Log("not on anything.");
        }

        // Find a the player and create a gameobject in the real world that will move in relativity to the itme in the canvas. 


        //FindAnyObjectByType<playerInventory>().gameObject
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      

    }

    public void OnDrop(PointerEventData eventData)
    {
        if(canInteract)
        {
            Debug.Log("commence investigation!");
            FindAnyObjectByType<PlayerTabUI>().tabOutInteraction();
            passedID = gameObject.GetComponent<EvidencePlaceholder>().evidence.IDnumber;
          
            StaticVariables.currentInteraction.GetComponent<Interactions>().ItemInteraction(passedID);
        }
         else
        {
            Debug.Log(previosPos);
            m_rectTransform.anchoredPosition = previosPos;
        }
    }
}
