using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateMask : MonoBehaviour
{
    public float speed = 35f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate (Vector3.forward * Time.deltaTime * speed);
    }
}
