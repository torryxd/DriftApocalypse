using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieController : MonoBehaviour
{
    public string mobName = "Flaco";
    public float moveForce = 1;
    public float defaultMaxSpeed = 1;
    private float maxSpeed = 1;

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

        defaultMaxSpeed = Random.Range(defaultMaxSpeed*0.8f, defaultMaxSpeed*1.2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!pauseMenu.paused){
            Vector2 v2 = car.transform.position - this.transform.position;
            float carDist = Vector2.Distance(transform.position, car.transform.position);

            maxSpeed = defaultMaxSpeed + carDist * 0.125f;
            if(carDist > 1.25f){
                rb2d.AddForce(v2.normalized * moveForce);
            }else{
                rb2d.AddForce(v2.normalized * -moveForce/5);
            }
        
            if (rb2d.velocity.magnitude > maxSpeed) {
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
            }
        }
    }

    public void die(){
        gameManager.IncreaseRate(mobName);
        Destroy(this.gameObject);
    }
}
