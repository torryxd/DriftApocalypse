using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieGordoController : MonoBehaviour
{
    public GameObject humo;
    public float tiempoEntreHumo = 5;
    private Vector3 originalScale;

    
    void Start()
    {
        originalScale = this.transform.localScale;
        StartCoroutine(squishAndSmoke());
        //this.transform.localScale = originalScale;
    }

    IEnumerator squishAndSmoke(){
        this.transform.localScale = new Vector3(originalScale.x*1.5f, originalScale.y*0.8f, originalScale.z);
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        yield return new WaitForSeconds(0.1f);
        Instantiate(humo, transform.position, humo.transform.rotation);

        yield return new WaitForSeconds(tiempoEntreHumo);
        StartCoroutine(squishAndSmoke());
    }
}
