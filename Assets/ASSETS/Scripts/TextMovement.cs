using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMovement : MonoBehaviour
{
	private Vector3 originalSize;
	public float rotMove = 2.2f;
	public float yMove = 0.0025f;

    // Start is called before the first frame update
    void Start()
    {
		originalSize = transform.localScale;
		//transform.localScale *= 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0){
			float wibbleRot = Mathf.Cos(Time.time*2.5f)*rotMove;
			//if(wibbleRot < 0) wibbleRot = 360-wibbleRot;
			transform.localEulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,
			0 + wibbleRot);
			float wibblePos = Mathf.Cos(Time.time*1.85f)*yMove;
			transform.position = new Vector3(transform.position.x,
				transform.position.y + wibblePos, transform.position.z);
			//transform.localScale = Vector3.Lerp(transform.localScale, originalSize, Time.deltaTime * 2.5f);
		}
    }
}
