using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler
{

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("mouse click down");
    }
 
}
