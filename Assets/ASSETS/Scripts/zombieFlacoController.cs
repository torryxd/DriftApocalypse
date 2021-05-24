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
    private Vector3 originalScale;
    private Rigidbody2D rb2d;
    private PauseMenu pauseMenu;
    private GameManager gameManager;
    private CamController cam;
    public GameObject squishEffect;
    public SpriteRenderer spr;

    // Start is called before the first frame update
    void Start()
    {
        car = FindObjectOfType<CarController>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        gameManager = FindObjectOfType<GameManager>();
        cam = FindObjectOfType<CamController>();
        rb2d = GetComponent<Rigidbody2D>();

        originalScale = this.transform.localScale;
        defaultMaxSpeed = Random.Range(defaultMaxSpeed*0.8f, defaultMaxSpeed*1.2f);
    }

    void Update(){

        float dot = Vector2.Dot(car.transform.position - this.transform.position, cam.transform.right);
            this.transform.localScale = new Vector3(originalScale.x*(dot < 0 ? -1 : 1), originalScale.y, originalScale.z);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!pauseMenu.paused){
            Vector2 v2 = car.transform.position - this.transform.position;
            float carDist = Vector2.Distance(transform.position, car.transform.position);
            
            if(carDist > 2f){
                rb2d.AddForce(v2.normalized * moveForce);
            }else{
                rb2d.AddForce(v2.normalized * moveForce/5f);
            }
        
            maxSpeed = defaultMaxSpeed + carDist * 0.125f;
            if (rb2d.velocity.magnitude > maxSpeed) {
                rb2d.velocity = Vector2.ClampMagnitude(rb2d.velocity, maxSpeed);
            }
        }
    }

    public void die(){
        gameManager.IncreaseRate();

        GameObject squish = Instantiate(squishEffect, transform.position, squishEffect.transform.rotation);
        squish.GetComponent<AudioSource>().pitch *= 1f + (0.75f * (car.COMBO/10f));
        squish.transform.eulerAngles = new Vector3(0,0, Vector2.Angle(transform.position - car.transform.Find("BACK").transform.position, cam.transform.up));
        
        Destroy(this.gameObject);
    }
}
