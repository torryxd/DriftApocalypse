using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieFlacoController : MonoBehaviour
{
    public float moveForce = 1;
    public float defaultMaxSpeed = 1;
    private float originalMaxSpeed = 1;
    private float maxSpeed = 1;
    private Vector2 flankVec;
    public int comportamiento = 0; //1 Directo, 2 diagonal, 3 flanqueador, 4 cobarde

    private CarController car;
    private Vector3 originalScale;
    private bool isHeFlipped;
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
        if(comportamiento == 0)
            comportamiento = Random.Range(1, 5);

        originalMaxSpeed = defaultMaxSpeed;
        defaultMaxSpeed = Random.Range(defaultMaxSpeed*0.75f, defaultMaxSpeed*1.25f);
        if(comportamiento == 1){ // NORMAL
            defaultMaxSpeed *= 1f;
        }else if(comportamiento == 2){ // DIAGONAL
            defaultMaxSpeed *= 1.175f;
        }else if(comportamiento == 3){ // FLANQUEADOR
            defaultMaxSpeed *= 1.4f;
        }else if(comportamiento == 4){ // COBARDE
            defaultMaxSpeed *= 0.45f;
        }
        
        //Dificultad relativa a la score
        defaultMaxSpeed *= 1f + (Mathf.Sqrt(car.SCORE) / 30f);
        moveForce *= 1f + (Mathf.Sqrt(car.SCORE) / 78f);
    }

    void Update(){

        float dot = Vector2.Dot(rb2d.velocity, cam.transform.right);
        if(Mathf.Abs(dot) > 0.3f)
            isHeFlipped = (dot < 0);
        GetComponent<SpriteRenderer>().flipX = isHeFlipped;

        this.transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * 2f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!pauseMenu.paused){
            Vector2 v2 = car.transform.position - this.transform.position;
            float carDist = Vector2.Distance(transform.position, car.transform.position);

            if(comportamiento == 1){ // NORMAL
                if(carDist > 3f){
                    rb2d.AddForce(v2.normalized * moveForce * 0.1f);
                    v2 = (car.transform.position + car.transform.right*20*(defaultMaxSpeed > originalMaxSpeed ? 1 : -1)) - this.transform.position;
                    rb2d.AddForce(v2.normalized * moveForce);
                }else{
                    rb2d.AddForce(v2.normalized * moveForce);
                }
                
            }else if(comportamiento == 2){ // DIAGONAL
                rb2d.AddForce(v2.normalized * moveForce);
                v2 = (car.transform.position + car.transform.right*4*(defaultMaxSpeed > originalMaxSpeed ? 1 : -1)) - this.transform.position;
                rb2d.AddForce(v2.normalized * moveForce);
            }else if(comportamiento == 3){ // FLANQUEADOR
                if(carDist > 6f){
                    flankVec = v2;
                }
                rb2d.AddForce(flankVec.normalized * moveForce);
            }else if(comportamiento == 4){ // COBARDE
                if(carDist > 4f){
                    rb2d.AddForce(v2.normalized * moveForce * 0.1f);
                    v2 = (car.transform.position + car.transform.right*10*(defaultMaxSpeed > originalMaxSpeed ? 1 : -1)) - this.transform.position;
                    rb2d.AddForce(v2.normalized * moveForce);
                }else{
                    rb2d.AddForce(v2.normalized * -moveForce / 7f);
                }
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
        squish.GetComponent<AudioSource>().pitch *= 1f + (0.9f * (car.COMBO/10f));
        squish.transform.eulerAngles = new Vector3(0,0, Vector2.Angle(transform.position - car.transform.GetChild(0).Find("BACK").transform.position, cam.transform.up));
        
        if(this.gameObject.GetComponent<zombieGordoController>() != null)
            squish.transform.localScale *= 1.5f;

        Destroy(this.gameObject);
    }
}
