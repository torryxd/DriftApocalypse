using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusController : MonoBehaviour
{
    public Sprite[] sprites; 

    void Start() {
        this.transform.GetComponentInChildren<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length*2)/2];

        //check if needs to rotate
        //this.GetComponent<lookAtCamera>().enabled = FindObjectOfType<CamController>().cameraView;
    }
}
