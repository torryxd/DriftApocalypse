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
    [SerializeField] private int comportamiento = 0; //1 Directo, 2 diagonal, 3 flanqueador, 4 cobarde

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
        comportamiento = Random.Range(1, 5);

        originalMaxSpeed = defaultMaxSpeed;
        defaultMaxSpeed = Random.Range(defaultMaxSpeed*0.75f, defaultMaxSpeed*1.25f);
        if(comportamiento == 1){ // NORMAL
            defaultMaxSpeed *= 1f;
        }else if(comportamiento == 2){ // DIAGONAL
            defaultMaxSpeed *= 1.25f;
        }else if(comportamiento == 3){ // FLANQUEADOR
            defaultMaxSpeed *= 1.5f;
        }else if(comportamiento == 4){ // COBARDE
            defaultMaxSpeed *= 0.4f;
        }
    }

    void Update(){

        float dot = Vector2.Dot(car.transform.position - this.transform.position, cam.transform.right);
        originalScale = new Vector3(Mathf.Abs(originalScale.x)*(dot < 0 ? -1 : 1), originalScale.y, originalScale.z);
        
        Vector3 newScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * 2f);
        newScale = new Vector3(Mathf.Abs(newScale.x)*(dot < 0 ? -1 : 1), newScale.y, newScale.z);
            this.transform.localScale = newScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!pauseMenu.paused){
            Vector2 v2 = car.transform.position - this.transform.position;
            float carDist = Vector2.Distance(transform.position, car.transform.position);

            if(comportamiento == 1){ // NORMAL
                if(carDist > 2f){
                    rb2d.AddForce(v2.normalized * moveForce);
                }else{
                    rb2d.AddForce(v2.normalized * moveForce/5f);
                }
            }else if(comportamiento == 2){ // DIAGONAL
                rb2d.AddForce(v2.normalized * moveForce);
                v2 = (car.transform.position + car.transform.right*10*(defaultMaxSpeed > originalMaxSpeed ? 1 : -1)) - this.transform.position;
                rb2d.AddForce(v2.normalized * moveForce);
            }else if(comportamiento == 3){ // FLANQUEADOR
                if(carDist > 5f){
                    flankVec = v2;
                }
                rb2d.AddForce(flankVec.normalized * moveForce);
            }else if(comportamiento == 4){ // COBARDE
                if(carDist > 3f){
                    rb2d.AddForce(v2.normalized * moveForce / 2f);
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
        squish.GetComponent<AudioSource>().pitch *= 1f + (0.75f * (car.COMBO/10f));
        squish.transform.eulerAngles = new Vector3(0,0, Vector2.Angle(transform.position - car.transform.Find("BACK").transform.position, cam.transform.up));
        
        Destroy(this.gameObject);
    }
}
