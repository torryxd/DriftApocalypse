using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieController : MonoBehaviour
{
    public float moveForce = 1;
    public float maxSpeed = 1;

    private CarController car;
    private Rigidbody2D rb2d;
    private PauseMenu pauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        car = FindObjectOfType<CarController>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        rb2d = GetComponent<Rigidbody2D>();

        maxSpeed -= Random.Range(0f, 0.5f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!pauseMenu.paused){
            Vector2 v2 = car.transform.position - this.transform.position;
            rb2d.AddForce(v2.normalized * moveForce);

            if (rb2d.velocity.magnitude > maxSpeed) {
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
            }
        }
    }

    public void die(){
        Destroy(this.gameObject);
    }
}
