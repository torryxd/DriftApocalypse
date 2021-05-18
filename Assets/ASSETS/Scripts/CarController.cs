using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CarController : MonoBehaviour
{
    public float SCORE = 0;
    public float COMBO = 1;
    public float HEALTH = 100;

    [Header("Car engine")]
    public float accelerationFactor = 200f;
    public float defaultAccelerationFactor;
    public float actualMaxSpeed = 2.5f;
    public Vector2 MinMaxSpeed;
    public float boostAcceleration = 1.35f;
    public Vector3 originalScale;
    private float tailSpeed = 1;
    private Vector3 previousTailPosition;

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
    public GameObject EngineEffect;
    private ParticleSystem.MainModule leftEffMain;
    private ParticleSystem.MainModule rightEffMain;
    private ParticleSystem engineEffectMain;
    private TrailRenderer[] trailRenderers;
    
    public eventButton ButtonLeft;
    public eventButton ButtonRight;
    public float steeringInput = 0;
    private float rotationAngle;
    private Vector2 rightVelocity;
    private bool isDrifting;

    [Header("Components")]
    private Rigidbody2D carRigidbody2D;
    public CamController cam;
    public PauseMenu pauseMenu;
    public GameObject EndPoint;
    public Collider2D FrontCollider;
    public Collider2D BackCollider;
    public AudioSource EngineSound;
    public AudioSource GravelSound;
    private float GravelSoundVolume;
    public AudioSource SmokeSound;
    private float SmokeSoundVolume;
    public TextMeshProUGUI txtScore;
    public TextMeshProUGUI txtCombo;
    
    void Start() {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        leftEffMain = LeftEffect.GetComponent<ParticleSystem>().main;
        rightEffMain = RightEffect.GetComponent<ParticleSystem>().main;
        engineEffectMain = EngineEffect.GetComponent<ParticleSystem>();
        engineEffectMain.emissionRate = 0;
        trailRenderers = GetComponentsInChildren<TrailRenderer>();

        defaultAccelerationFactor = accelerationFactor;
        GravelSoundVolume = GravelSound.volume;
        SmokeSoundVolume = SmokeSound.volume;
        originalScale = this.transform.localScale;
        
    }

    bool firstLEFT, firstRIGHT, firstLEFTandRIGHT;
    void Update() {
        //steeringInput = Input.GetAxis("Horizontal");
        bool LEFT = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || ButtonLeft.ispressed;
        bool RIGHT = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || ButtonRight.ispressed;
        
        if(RIGHT && LEFT) {
            actualMaxSpeed = Mathf.Lerp(actualMaxSpeed, MinMaxSpeed.y, Time.deltaTime);

            if(firstLEFTandRIGHT){
                this.transform.localScale = new Vector3(originalScale.x*0.875f, originalScale.y*1.125f, 1);
                SmokeSound.pitch = 1f;
                accelerationFactor = defaultAccelerationFactor * boostAcceleration;
                SmokeSound.volume = SmokeSoundVolume*1.75F;
                firstLEFTandRIGHT = false;
            }
        }else{
            actualMaxSpeed = Mathf.Lerp(actualMaxSpeed, MinMaxSpeed.x, Time.deltaTime*3);

            if(!firstLEFTandRIGHT){
                accelerationFactor = defaultAccelerationFactor;
                firstLEFTandRIGHT = true;
            }
        }

        if(LEFT) {
            steeringInput -= Time.deltaTime * (wheelSteeringFactor + wheelAdaptativeSteeringFactor*Mathf.Abs((steeringInput + 1)/2));
            SmokeSound.volume = Mathf.Lerp(SmokeSound.volume, Mathf.Cos(Time.time*10f) * 0.01f + (SmokeSoundVolume*0.5f), Time.deltaTime*2.5f);

            if(firstLEFT){
                leftEffMain.startLifetime = smokeTime;
                SmokeSound.pitch = 1.075f;
                if(SmokeSound.volume < SmokeSoundVolume)
                    SmokeSound.volume = SmokeSoundVolume;
                firstLEFT = false;
            }
        }else{
            if(!firstLEFT){
                leftEffMain.startLifetime = 0.01f;
                firstLEFT = true;
            }
        }

        if(RIGHT) {
            steeringInput += Time.deltaTime * (wheelSteeringFactor + wheelAdaptativeSteeringFactor*Mathf.Abs((steeringInput - 1)/2));
            SmokeSound.volume = Mathf.Lerp(SmokeSound.volume, Mathf.Cos(Time.time*10f) * 0.01f + (SmokeSoundVolume*0.5f), Time.deltaTime*2.5f);

            if(firstRIGHT){
                rightEffMain.startLifetime = smokeTime;
                SmokeSound.pitch = 0.925f;
                if(SmokeSound.volume < SmokeSoundVolume)
                    SmokeSound.volume = SmokeSoundVolume;
                firstRIGHT = false;
            }
        }else{
            if(!firstRIGHT){
                rightEffMain.startLifetime = 0.01f;
                firstRIGHT = true;
            }
        }

        if(!RIGHT && !LEFT) {
            steeringInput -= Time.deltaTime * Mathf.Sign(steeringInput) * resetWheelSteeringFactor;
            SmokeSound.volume = Mathf.Lerp(SmokeSound.volume, SmokeSoundVolume * 0f, Time.deltaTime * 6);
        }

        steeringInput = Mathf.Clamp(steeringInput, -1, 1);
        this.transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * 2f);
        

        //Girar ruedas
        Vector3 wheelRotation = new Vector3(0,0,steeringInput * -30);
        tyreLeft.transform.localEulerAngles = wheelRotation;
        tyreRight.transform.localEulerAngles = wheelRotation;

        if(pauseMenu.paused && Mathf.Abs(steeringInput) >= 1){ //unpause
            pauseMenu.paused = false;
            pauseMenu.unPauseFirstPause();
        }
        if(pauseMenu.paused)
            return;

        //Velocidad de cola
        Vector3 v3TailSpeed = BackCollider.transform.position - previousTailPosition;
        tailSpeed = v3TailSpeed.magnitude*50;
        previousTailPosition = BackCollider.transform.position;
        //Sonido motor
        EngineSound.pitch = 1 + ((tailSpeed / MinMaxSpeed.y) * (1f + Mathf.Cos(Time.time*10) * 0.075f));

        //DERRAPANDO
        isDrifting = rightVelocity.magnitude/actualMaxSpeed > 0.4f || Mathf.Abs(steeringInput) > 0.8f;
        if(isDrifting){ 
            trailRenderers[0].emitting = true;
            trailRenderers[1].emitting = true;
            GravelSound.volume = GravelSoundVolume * ((rightVelocity.magnitude/actualMaxSpeed + Mathf.Abs(steeringInput))/2);
            GravelSound.pitch = 0.7f + Mathf.Cos(Time.time*5)*0.05f;
        }else{
            trailRenderers[0].emitting = false;
            trailRenderers[1].emitting = false;
            GravelSound.volume = Mathf.Lerp(GravelSound.volume, GravelSoundVolume*0.2f, Time.deltaTime*8);
        }

        //Combo
        if(COMBO > 1f)
            COMBO -= Time.deltaTime * (isDrifting ? 0.1f : 2f); //Decrease derrapando
        if(COMBO > 0.75f){
            txtCombo.text = "x" + Mathf.CeilToInt(COMBO); txtCombo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + Mathf.CeilToInt(COMBO);
        }

        //Health
        if(HEALTH < 100){
            engineEffectMain.emissionRate = Mathf.RoundToInt(100 - (HEALTH))/5;
            HEALTH += Time.deltaTime * 7.5f;
            Debug.Log(HEALTH);
        }else{
            HEALTH = 100;
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

            //Clamp Circle
            if (transform.position.magnitude > 8) { 
                carRigidbody2D.AddForce(-transform.position.normalized * (transform.position.magnitude - 8)*5, ForceMode2D.Force);
            }
        }
        
        //Kill Orthogonal Velocity
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);
        //Debug.Log(rightVelocity.magnitude/actualMaxSpeed);
        carRigidbody2D.velocity = forwardVelocity + rightVelocity * ((1-driftFactor) + driftFactor*(rightVelocity.magnitude/actualMaxSpeed));

        //Apply rotation
        rotationAngle -= steeringInput * steeringSpeed * Time.fixedDeltaTime;
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    public void OnTriggerEnterChilds(Collider2D col, string colChild) {
        if(col.transform.CompareTag("Enemy") && isDrifting)
        {
            if(colChild == "BACK")
            {
                col.gameObject.GetComponent<zombieController>().die();
                if(carRigidbody2D.velocity.magnitude < MinMaxSpeed.x)
                    carRigidbody2D.velocity += rightVelocity.normalized * 0.125f;
                cam.Shake(0.1f, 0.1f, 30);

                SCORE += 1 * Mathf.Ceil(COMBO);
                if(txtScore.gameObject.transform.localScale.magnitude < 4f)
                    txtScore.gameObject.transform.localScale *= 1.2f;
                txtScore.text = SCORE.ToString(); txtScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SCORE.ToString();

                if(COMBO < 9)
                    COMBO += 1;
                if(txtCombo.gameObject.transform.localScale.magnitude < 2.5f)
                    txtCombo.gameObject.transform.localScale *= 1.3f;
            }
        }

        if(colChild == "FRONT")
        {
            HEALTH -= 45;
            if(HEALTH <= 0){ //DEAD
                cam.Shake(0.1f, 0.3f, 1000);
                death();
            }else{ //LOOSE HP
                cam.Shake(0.1f, 0.3f, 100);

            }
        }
    }

    public void death(){
            SCORE = 0;
            txtScore.text = "GAME OVER"; txtScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "GAME OVER";
            
            pauseMenu.Pause();
    }
}
