using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class interactionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float scaleIncrease = 1.15f;
    
    private Vector3 originalScale;


    void Start(){
        originalScale = transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData) {
        transform.localScale = originalScale * scaleIncrease;
    }
    public void OnPointerUp(PointerEventData eventData) {
        transform.localScale = originalScale;
    }
}
