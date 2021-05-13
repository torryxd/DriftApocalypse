using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieController : MonoBehaviour
{
    public string mobName = "Flaco";
    public float moveForce = 1;
    public float maxSpeed = 1;

    private CarController car;
    private Rigidbody2D rb2d;
    private PauseMenu pauseMenu;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        car = FindObjectOfType<CarController>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        gameManager = FindObjectOfType<GameManager>();
        rb2d = GetComponent<Rigidbody2D>();

        maxSpeed = Random.Range(maxSpeed*0.9f, maxSpeed*1.1f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!pauseMenu.paused){
            Vector2 v2 = car.transform.position - this.transform.position;

            rb2d.AddForce(v2.normalized * moveForce);

            if (rb2d.velocity.magnitude > maxSpeed) {
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed + v2.magnitude*0.1f);
            }
            
        }
    }

    public void die(){
        gameManager.IncreaseRate(mobName);
        Destroy(this.gameObject);
    }
}
