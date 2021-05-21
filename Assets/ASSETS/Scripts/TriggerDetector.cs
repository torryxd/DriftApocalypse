using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDetector : MonoBehaviour
{
    private CarController car;

    void Start (){
        car = transform.parent.GetComponent<CarController>();
    }

    void OnTriggerEnter2D(Collider2D col){
        car.OnTriggerEnterChilds(col, this.transform.name);
    }
}
