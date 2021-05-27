using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class humoGordo : MonoBehaviour
{
    public float timeToDespawn = 10;
    private Vector3 originalSize;
    private float counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = timeToDespawn;
        originalSize = transform.GetChild(0).transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;
        if(counter <= 0){
            Destroy(this.gameObject);
        }else if(counter < 1){
            transform.GetChild(0).transform.localScale = originalSize * counter;
            transform.GetChild(1).transform.localScale = originalSize * counter;
        }
    }
}
