using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballController : MonoBehaviour
{
    public float speed = 4;

    // Update is called once per frame
    void Update()
    {
        this.transform.transform.Translate(Vector2.down * speed * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D col) {
        if(col.transform.CompareTag("Enemy"))
            col.gameObject.GetComponent<zombieFlacoController>().die();

        //Animación destruirse
        Destroy(this.gameObject);
    }
}
