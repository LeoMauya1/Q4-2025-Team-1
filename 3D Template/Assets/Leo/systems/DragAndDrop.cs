using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{


    private RectTransform m_rectTransform;
    private void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }
    public void OnBeginDrag (PointerEventData evedata)
    {

    }
    public void OnEndDrag(PointerEventData evedata)
    {

    }
    public void OnDrag(PointerEventData eventdata)
    {
        m_rectTransform.anchoredPosition += eventdata.delta;


        // Find a the player and create a gameobject in the real world that will move in relativity to the itme in the canvas. 


        //FindAnyObjectByType<playerInventory>().gameObject
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("mouse click down");
    }
 
    public void OnDrop(PointerEventData eventData)
    {

    }
}
