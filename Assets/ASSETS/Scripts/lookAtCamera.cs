using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAtCamera : MonoBehaviour
{
    private Transform camTrans;

    // Start is called before the first frame update
    void Start() {
        camTrans = FindObjectOfType<CamController>().gameObject.transform;
        this.transform.rotation = camTrans.transform.rotation;
        
        if(!FindObjectOfType<GlobalSettings>().mobileCam)
            Destroy(this);
    }

    // Update is called once per frame
    void Update(){        
        this.transform.rotation = camTrans.transform.rotation;
    }
}