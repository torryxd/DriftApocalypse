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
    private ParticleSystem.MainModule leftEffMain;
    private ParticleSystem.MainModule rightEffMain;
    private TrailRenderer[] trailRenderers;
    
    public eventButton ButtonLeft;
    public eventButton ButtonRight;
    public float steeringInput = 0;
    private float rotationAngle;
    private Vector2 rightVelocity;

    [Header("Components")]
    private Rigidbody2D carRigidbody2D;
    public PauseMenu pauseMenu;
    
    void Start() {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        leftEffMain = LeftEffect.GetComponent<ParticleSystem>().main;
        rightEffMain = RightEffect.GetComponent<ParticleSystem>().main;
        trailRenderers = GetComponentsInChildren<TrailRenderer>();

        defaultAccelerationFactor = accelerationFactor;
    }

    void Update() {
        
        //steeringInput = Input.GetAxis("Horizontal");
        bool LEFT = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || ButtonLeft.ispressed;
        bool RIGHT = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || ButtonRight.ispressed;

        if(LEFT){
            steeringInput -= Time.deltaTime * (wheelSteeringFactor + wheelAdaptativeSteeringFactor*Mathf.Abs((steeringInput + 1)/2));
            leftEffMain.startLifetime = smokeTime;
        }else{
            leftEffMain.startLifetime = 0f;
        } 
        if(RIGHT){
            steeringInput += Time.deltaTime * (wheelSteeringFactor + wheelAdaptativeSteeringFactor*Mathf.Abs((steeringInput - 1)/2));
            rightEffMain.startLifetime = smokeTime;
        }else{
            rightEffMain.startLifetime = 0f;
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
        
        if(pauseMenu.paused && Mathf.Abs(steeringInput) >= 1){ //unpause
            pauseMenu.paused = false;
            pauseMenu.unPauseFirstPause();
        }

        Debug.Log(rotationAngle);
        if(rightVelocity.magnitude/actualMaxSpeed > 0.8f || Mathf.Abs(steeringInput) > 0.35f){ //drift factor / Trail renderers
            trailRenderers[0].emitting = true;
            trailRenderers[1].emitting = true;
        }else{
            trailRenderers[0].emitting = false;
            trailRenderers[1].emitting = false;
        }
    }

    void FixedUpdate() {
        if(pauseMenu.paused)
            return;
        
        //Create a force for the engine & limit max speed
        if (carRigidbody2D.velocity.magnitude <= actualMaxSpeed) {
            float speedIncrement = (actualMaxSpeed - carRigidbody2D.velocity.magnitude)/actualMaxSpeed;
            Vector2 engineForceVector = transform.up * accelerationFactor * Time.fixedDeltaTime * (1+speedIncrement);
            carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);
        }
        
        //Kill Orthogonal Velocity
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);
        //Debug.Log(rightVelocity.magnitude/actualMaxSpeed);
        carRigidbody2D.velocity = forwardVelocity + rightVelocity * ((1-driftFactor) + driftFactor*(rightVelocity.magnitude/actualMaxSpeed));
        
        //Clamp Circle
        if (transform.position.magnitude > 8) { 
            carRigidbody2D.AddForce(-transform.position.normalized * (transform.position.magnitude - 8)*5, ForceMode2D.Force);
        }

        //Apply rotation
        rotationAngle -= steeringInput * steeringSpeed * Time.fixedDeltaTime;
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    /*
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
    */
}
