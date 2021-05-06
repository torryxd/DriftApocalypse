using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour
{
    private Transform camTrans;

    // Start is called before the first frame update
    void Start() {
        camTrans = FindObjectOfType<Camera>().gameObject.transform;
    }

    // Update is called once per frame
    void Update(){        
        this.transform.rotation = camTrans.transform.rotation;
    }
}