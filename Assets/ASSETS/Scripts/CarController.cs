using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    [Header("Car engine")]
    public float accelerationFactor = 200f;
    private float defaultAccelerationFactor;
    public float actualMaxSpeed = 2.5f;
    public Vector2 MinMaxSpeed;
    public float boostAcceleration = 1.35f;

    [Header("Car steering")]
    public float steeringSpeed = 250f;
    public float wheelSteeringFactor = 1f;
    public float wheelAdaptativeSteeringFactor = 5f;
    public float resetWheelSteeringFactor = 1f;
    [Range(0,1)] public float driftFactor = 0.025f;
    public GameObject tyreLeft;
    public GameObject tyreRight;
    public float smokeTime = 2f;
    public GameObject LeftEffect;
    public GameObject RightEffect;
    
    public eventButton ButtonLeft;
    public eventButton ButtonRight;
    public float steeringInput = 0;
    private float rotationAngle;

    //Components
    private Rigidbody2D carRigidbody2D;
    private EscenarioController escenario;
    private PauseMenu pauseMenu;
    
    void Start() {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        escenario = FindObjectOfType<EscenarioController>();
        pauseMenu = FindObjectOfType<PauseMenu>();
        defaultAccelerationFactor = accelerationFactor;
    }

    void Update() {
        
        //steeringInput = Input.GetAxis("Horizontal");
        bool LEFT = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || ButtonLeft.ispressed;
        bool RIGHT = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || ButtonRight.ispressed;
        
        if(LEFT){
            steeringInput -= Time.deltaTime * (wheelSteeringFactor + wheelAdaptativeSteeringFactor*Mathf.Abs((steeringInput + 1)/2));
            LeftEffect.GetComponent<ParticleSystem>().startLifetime = smokeTime;
        }else{
            LeftEffect.GetComponent<ParticleSystem>().startLifetime = 0f;
        } 
        if(RIGHT){
            steeringInput += Time.deltaTime * (wheelSteeringFactor + wheelAdaptativeSteeringFactor*Mathf.Abs((steeringInput - 1)/2));
            RightEffect.GetComponent<ParticleSystem>().startLifetime = smokeTime;
        }else{
            RightEffect.GetComponent<ParticleSystem>().startLifetime = 0f;
        }
        if(RIGHT && LEFT){
            actualMaxSpeed = Mathf.Lerp(actualMaxSpeed, MinMaxSpeed.y, Time.deltaTime);
            if(accelerationFactor == defaultAccelerationFactor){
                this.transform.localScale = new Vector3(0.875f, 1.125f, 1);
            }
            accelerationFactor = defaultAccelerationFactor * boostAcceleration;
        }else{
            actualMaxSpeed = Mathf.Lerp(actualMaxSpeed, MinMaxSpeed.x, Time.deltaTime*3);
            accelerationFactor = defaultAccelerationFactor;
        }
        if(!RIGHT && !LEFT){
            steeringInput -= Time.deltaTime * Mathf.Sign(steeringInput) * resetWheelSteeringFactor;
        }
        steeringInput = Mathf.Clamp(steeringInput, -1, 1);
        this.transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, Time.deltaTime * 2f);
        
        Vector3 wheelRotation = new Vector3(0,0,steeringInput * -30);
        tyreLeft.transform.localEulerAngles = wheelRotation;
        tyreRight.transform.localEulerAngles = wheelRotation;
        
        if(pauseMenu.paused && Mathf.Abs(steeringInput) >= 1){
            pauseMenu.paused = false;
            pauseMenu.unPauseFirstPause();
        }
    }

    void FixedUpdate() {
        
        if(pauseMenu.paused)
            return;
        //Create a force for the engine
        float speedIncrement = (actualMaxSpeed - carRigidbody2D.velocity.magnitude)/actualMaxSpeed;
        Vector2 engineForceVector = transform.up * accelerationFactor * Time.fixedDeltaTime * (1+speedIncrement);
        carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
        
        //Limit max speed
        if (carRigidbody2D.velocity.magnitude > actualMaxSpeed) {
            carRigidbody2D.velocity = Vector2.ClampMagnitude(carRigidbody2D.velocity, actualMaxSpeed);
        }
        
        //Kill Orthogonal Velocity
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);
        //Debug.Log(rightVelocity.magnitude/maxSpeed);
        carRigidbody2D.velocity = forwardVelocity + rightVelocity * ((1-driftFactor) + driftFactor*(rightVelocity.magnitude/actualMaxSpeed));
        
        //Clamp Circle //da error
        if (transform.position.magnitude > 10) { 
            //float angle = ((Mathf.Atan2(transform.position.y-0, transform.position.x-0)*180 / Mathf.PI)+90)%360;
            //transform.eulerAngles = new Vector3(0,0, Mathf.LerpAngle(transform.eulerAngles.z, angle, Time.fixedDeltaTime * 2));
            //rotationAngle = transform.eulerAngles.z;
            carRigidbody2D.AddForce(-transform.position.normalized * (transform.position.magnitude - 10)*5, ForceMode2D.Force);
        }
        rotationAngle -= steeringInput * steeringSpeed * Time.fixedDeltaTime;
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.tag == "Enemy"){
            Debug.Log("DEAD");
        }
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Enemy"){
            col.gameObject.GetComponent<zombieController>().die();
        }
    }
}
