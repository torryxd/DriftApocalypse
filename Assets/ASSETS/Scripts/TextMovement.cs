using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMovement : MonoBehaviour
{
	private Vector3 originalSize;
	public float rotMove = 2.2f;
	public float yMove = 0.0025f;
	public float scaleRecoveryTime = 2.5f;

	private float offSetTime;

    // Start is called before the first frame update
    void Start()
    {
		originalSize = transform.localScale;
		//transform.localScale *= 0.1f;
		offSetTime = Random.Range(0.01f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0){
			float wibbleRot = Mathf.Cos((Time.time+offSetTime)*2.5f)*rotMove;
			//if(wibbleRot < 0) wibbleRot = 360-wibbleRot;
			transform.localEulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y,
			0 + wibbleRot);
			float wibblePos = Mathf.Cos((Time.time+offSetTime)*1.85f)*yMove;
			transform.position = new Vector3(transform.position.x,
				transform.position.y + wibblePos, transform.position.z);
			transform.localScale = Vector3.Lerp(transform.localScale, originalSize, Time.deltaTime * scaleRecoveryTime);
		}
    }
}
