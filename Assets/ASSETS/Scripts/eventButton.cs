using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class eventButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler 
{
    public bool ispressed = false;

    public void OnPointerDown(PointerEventData eventData) {
        ispressed = true;
    }
    public void OnPointerUp(PointerEventData eventData) {
        ispressed = false;
    }
}
