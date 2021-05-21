using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieFlacoController : MonoBehaviour
{
    public float moveForce = 1;
    public float defaultMaxSpeed = 1;
    private float maxSpeed = 1;
    private int comportamiento = 0; //1 Directo, 2 Esquivando, 3  4 Cobarde 

    private CarController car;
    private Rigidbody2D rb2d;
    private PauseMenu pauseMenu;
    private GameManager gameManager;
    public GameObject squishEffect;

    // Start is called before the first frame update
    void Start()
    {
        car = FindObjectOfType<CarController>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        gameManager = FindObjectOfType<GameManager>();
        rb2d = GetComponent<Rigidbody2D>();

        defaultMaxSpeed = Random.Range(defaultMaxSpeed*0.8f, defaultMaxSpeed*1.2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!pauseMenu.paused){
            Vector2 v2 = car.transform.position - this.transform.position;
            float carDist = Vector2.Distance(transform.position, car.transform.position);
            
            maxSpeed = defaultMaxSpeed + carDist * 0.125f;
            if(carDist > 2f){
                rb2d.AddForce(v2.normalized * moveForce);
            }else{
                rb2d.AddForce(v2.normalized * moveForce/5f);
            }
        
            if (rb2d.velocity.magnitude > maxSpeed) {
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
            }
        }
    }

    public void die(){
        gameManager.IncreaseRate();
        Instantiate(squishEffect, transform.position, squishEffect.transform.rotation);
        Destroy(this.gameObject);
    }
}
