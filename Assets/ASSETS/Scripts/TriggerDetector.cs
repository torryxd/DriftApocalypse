using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    public CarController car;

    void OnTriggerEnter2D(Collider2D col){
        car.OnTriggerEnterChilds(col, this.transform.name);
    }
}
