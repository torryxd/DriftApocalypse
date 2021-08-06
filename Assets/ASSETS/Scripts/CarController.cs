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
    public float HEALTH = 1; //MaxSpeed

    [Header("Car engine")]
    public float accelerationFactor = 200f;
    public float defaultAccelerationFactor;
    public Vector2 MinMaxSpeed;
    public float boostAcceleration = 1.35f;
    public Vector3 originalScale;
    private float tailSpeed = 1;
    private Vector3 previousTailPosition;
    private float justHitted = 0;
    public float HealthRegen = 0.2f;
    public float fuerzaParaContarHit = 0.7f;
    public float vidaPerdidaImpacto = 3.25f;

    [Header("Car steering")]
    public float steeringSpeed = 250f;
    public float wheelSteeringFactor = 1f;
    public float wheelAdaptativeSteeringFactor = 5f;
    public float resetWheelSteeringFactor = 1f;
    [Range(0,1)] public float driftFactor = 0.025f;
    public float driftAmountToDrift = 0.6f;
    public bool hasTyres = true;
    public GameObject tyreLeft;
    public GameObject tyreRight;
    public float smokeTime = 2f;
    public GameObject LeftEffect;
    public GameObject RightEffect;
    public GameObject EngineEffect;
    public AudioSource EngineFailureSound;
    public GameObject EngineRepairedEff;
    private bool engineFullHP = true;
    private ParticleSystem.MainModule leftEffMain;
    private ParticleSystem.MainModule rightEffMain;
    private ParticleSystem engineEffectMain;
    private TrailRenderer[] trailRenderers;
    public GameObject scoreUpwardsNumber;
    
    public eventButton ButtonLeft;
    public eventButton ButtonRight;
    public float steeringInput = 0;
    private float rotationAngle;
    private Vector2 rightVelocity;
    private bool isDrifting;
    private float driftCoyote;
    public bool immortal = false;
    
    [SerializeField]private bool comboPowerUp = false;
    public float comboDecreaseFactor = 1;
    public GameObject invencibilityEffect;
    public GameObject splashInvencible;
    public Color invencibilityColor;
    private float colorGoWhite = 0;
    private float comboCooldown = 0;

    public GameObject crashEffect;

    [Header("Components")]
    private Rigidbody2D carRigidbody2D;
    private GlobalSettings gs;
    public CamController cam;
    public PauseMenu pauseMenu;
    public GameObject EndPoint;
    public Collider2D BackCollider;
    public AudioSource EngineSound;
    public AudioSource GravelSound;
    private float GravelSoundVolume;
    public AudioSource SmokeSound;
    private float SmokeSoundVolume;
    public TextMeshProUGUI txtScore;
    public TextMeshProUGUI txtCombo;
    public Slider sliderCombo;
    public GameObject carSprite;
    
    void Start() {
        carRigidbody2D = GetComponent<Rigidbody2D>();
        carRigidbody2D.velocity = Vector2.zero;
        leftEffMain = LeftEffect.GetComponent<ParticleSystem>().main;
        rightEffMain = RightEffect.GetComponent<ParticleSystem>().main;
        engineEffectMain = EngineEffect.GetComponent<ParticleSystem>();
        engineEffectMain.emissionRate = 0;
        trailRenderers = GetComponentsInChildren<TrailRenderer>();
        gs = FindObjectOfType<GlobalSettings>();

        defaultAccelerationFactor = accelerationFactor;
        GravelSoundVolume = GravelSound.volume;
        SmokeSoundVolume = SmokeSound.volume;
        SmokeSoundVolume = SmokeSound.volume;
        originalScale = carSprite.transform.localScale;
        txtScore.text = gs.hiScore.ToString(); txtScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gs.hiScore.ToString();
    }

    public bool LEFT, RIGHT, firstLEFT, firstRIGHT, firstLEFTandRIGHT;
    void Update() {
        //steeringInput = Input.GetAxis("Horizontal");
        LEFT = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || ButtonLeft.ispressed;
        RIGHT = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || ButtonRight.ispressed;

        if(RIGHT && LEFT) {
            SmokeSound.pitch = 1f;
            SmokeSound.panStereo = 0f;
            if(HEALTH >= MinMaxSpeed.x)
                HEALTH = Mathf.Lerp(HEALTH, MinMaxSpeed.y, Time.deltaTime);

            if(firstLEFTandRIGHT && !pauseMenu.isGamingOver){
                if(boostAcceleration >= 1){
                    carSprite.transform.localScale = new Vector3(originalScale.x*0.87f, originalScale.y*1.12f, 1);
                }else{
                    carSprite.transform.localScale = new Vector3(originalScale.x*1.15f, originalScale.y*0.9f, 1);
                }
                accelerationFactor = defaultAccelerationFactor * boostAcceleration;
                SmokeSound.volume = SmokeSoundVolume*1.75F;
                firstLEFTandRIGHT = false;
            }
        }else{
            if(HEALTH >= MinMaxSpeed.x)
                HEALTH = Mathf.Lerp(HEALTH, MinMaxSpeed.x, Time.deltaTime*3);

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
                SmokeSound.pitch = 1.025f;
                SmokeSound.panStereo = -0.8f;
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
                SmokeSound.pitch = 0.975f;
                SmokeSound.panStereo = +0.8f;
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
        carSprite.transform.localScale = Vector3.Lerp(carSprite.transform.localScale, originalScale, Time.deltaTime * 2f);
        

        //GIRAR RUEDAS
        if(hasTyres){
            Vector3 wheelRotation = new Vector3(0,0,steeringInput * -30);
            tyreLeft.transform.localEulerAngles = wheelRotation;
            tyreRight.transform.localEulerAngles = wheelRotation;
        }

        //EMPEZAR JUEGO
        if(pauseMenu.firstTimePause && (RIGHT || LEFT)) { //Mathf.Abs(steeringInput) >= 1){
            pauseMenu.paused = false;
            pauseMenu.unPauseFirstPause();
        }
        if(pauseMenu.paused)
            return;

        //VELOCIDAD DE COLA (Depende de los frames)
        //Vector3 v3TailSpeed = BackCollider.transform.position - previousTailPosition;
        //tailSpeed = v3TailSpeed.magnitude * 50;
        //previousTailPosition = BackCollider.transform.position;
        //SONIDO MOTOR
        EngineSound.pitch = 1 + ((carRigidbody2D.velocity.magnitude / MinMaxSpeed.y) * (1f + Mathf.Cos(Time.time*10) * 0.075f));

        //DERRAPANDO
        float driftAmount = (rightVelocity.magnitude/carRigidbody2D.velocity.magnitude);
        if(driftAmount > driftAmountToDrift){
            driftCoyote = driftAmount; 
        }else if(driftAmount > 0){
            driftCoyote -= Time.deltaTime * 3f; //Decrease coyote drift
        }
        isDrifting = driftCoyote > 0;
        if(isDrifting){
            trailRenderers[0].emitting = true;
            trailRenderers[1].emitting = true;
            GravelSound.volume = GravelSoundVolume * (rightVelocity.magnitude/carRigidbody2D.velocity.magnitude);
            GravelSound.pitch = 0.7f + Mathf.Cos(Time.time*5)*0.05f;
        }else{
            trailRenderers[0].emitting = false;
            trailRenderers[1].emitting = false;
            GravelSound.volume = Mathf.Lerp(GravelSound.volume, GravelSoundVolume*0.2f, Time.deltaTime*8);
        }

        //COMBO
        if(COMBO > 1f){
            float decreaseRatio = 1f; //Decrease ratio / Tiempo que esta activo el combo
            if(!comboPowerUp){
                if(isDrifting)
                    decreaseRatio = 0.2f;
                else
                    decreaseRatio = 3f;
            }else{
                sliderCombo.value = ((COMBO-1) / 9f);
            }
            COMBO -= Time.deltaTime * (decreaseRatio * comboDecreaseFactor);
        }else if (comboPowerUp){
            comboPowerUp = false;
            Instantiate(splashInvencible, invencibilityEffect.transform.position, invencibilityEffect.transform.rotation);
            cam.Shake(0.1f, 0.1f, 175);
            GetComponent<CapsuleCollider2D>().isTrigger = false;
            colorGoWhite = 0;
            invencibilityEffect.SetActive(false);
            sliderCombo.gameObject.SetActive(false);
        }
        
        if(!comboPowerUp && COMBO > 0.75f){
            txtCombo.text = "x" + Mathf.CeilToInt(COMBO); txtCombo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "x" + Mathf.CeilToInt(COMBO);
        }
        
        if(comboCooldown > 0)
            comboCooldown -= Time.deltaTime;
        
        Color clr = carSprite.GetComponent<SpriteRenderer>().color;
        Color newClr;
        if (colorGoWhite < 0) {
            newClr = invencibilityColor;
            colorGoWhite -= Time.deltaTime;
            if(colorGoWhite < -1)
                colorGoWhite = 0.65f;
        } else {
            newClr = Color.white;
            if(colorGoWhite > 0){
                colorGoWhite += Time.deltaTime;
                if(colorGoWhite > 1)
                    colorGoWhite = -0.65f;
            }
        }
        carSprite.GetComponent<SpriteRenderer>().color = Color.Lerp(clr, newClr, Time.deltaTime * 5);

        //HEALTH REGEN
        if(HEALTH < MinMaxSpeed.x){
            engineEffectMain.emissionRate = Mathf.RoundToInt((MinMaxSpeed.x - (HEALTH))*20);

            EngineFailureSound.enabled = true;
            EngineFailureSound.pitch = 1 + ((MinMaxSpeed.y - HEALTH)-1);
            
            HEALTH += (HealthRegen + ((MinMaxSpeed.x - HEALTH)/MinMaxSpeed.x) * (0.875f*HealthRegen)) * Time.deltaTime;  // Health regen rate
            engineFullHP = false;
        }else{
            if(!engineFullHP){
                Instantiate(EngineRepairedEff, transform.position, transform.rotation);
                EngineFailureSound.enabled = false;
                engineFullHP = true;
            }
        }

        if(justHitted > 0)
            justHitted -= Time.deltaTime;
    }

    void FixedUpdate() {
        if(pauseMenu.paused)
            return;
        
        //Create a force for the engine & limit max speed
        if (carRigidbody2D.velocity.magnitude <= (HEALTH > 1.75f ? HEALTH : 1.75f)) {
            float speedIncrement = (HEALTH - carRigidbody2D.velocity.magnitude)/HEALTH; //Increment inertia if going fast
            Vector2 engineForceVector = transform.up * accelerationFactor * Time.fixedDeltaTime * (1+speedIncrement);
            carRigidbody2D.AddForce(engineForceVector, ForceMode2D.Force);

            //Clamp Circle
            if (transform.position.magnitude > 8) { 
                carRigidbody2D.AddForce(-transform.position.normalized * (transform.position.magnitude - 8)*5, ForceMode2D.Force);
            }
        }else{
            carRigidbody2D.velocity = carRigidbody2D.velocity.normalized * HEALTH;
        }
        
        //Kill Orthogonal Velocity
        Vector2 forwardVelocity = transform.up * Vector2.Dot(carRigidbody2D.velocity, transform.up);
        rightVelocity = transform.right * Vector2.Dot(carRigidbody2D.velocity, transform.right);
        //Debug.Log(rightVelocity.magnitude/carRigidbody2D.velocity.magnitude);
        carRigidbody2D.velocity = forwardVelocity + rightVelocity * ((1-driftFactor) + driftFactor*(rightVelocity.magnitude/MinMaxSpeed.y));

        //Apply rotation
        rotationAngle -= steeringInput * steeringSpeed * Time.fixedDeltaTime;
        carRigidbody2D.MoveRotation(rotationAngle);
    }

    public void OnTriggerEnterChilds(Collider2D col, string colChild) { //COLLIDER COLA
        if(!col.transform.CompareTag("Smoke") && transform.position.magnitude < 8 && (isDrifting || comboPowerUp)) // colChild == "BACK" && 
        {
            if(col.transform.CompareTag("Enemy")) {
                bool isGordo = col.GetComponent<zombieGordoController>() != null;
                if(isGordo)
                    cam.Shake(0.15f, 0.12f, 75);
                else
                    cam.Shake(0.125f, 0.115f, 45);

                if(carRigidbody2D.velocity.magnitude < MinMaxSpeed.x && HEALTH < MinMaxSpeed.x && !comboPowerUp) //Impulse on kill
                    carRigidbody2D.velocity += (rightVelocity.normalized * 0.6f + carRigidbody2D.velocity.normalized * 0.15f);

                col.gameObject.GetComponent<zombieFlacoController>().die();

                writeScore(col.gameObject, isGordo, true);
            }else if(col.transform.CompareTag("Cacti")){
                cam.Shake(0.075f, 0.075f, 30);
                
                if(carRigidbody2D.velocity.magnitude < MinMaxSpeed.x && HEALTH < MinMaxSpeed.x && !comboPowerUp) //Impulse on kill
                    carRigidbody2D.velocity += (rightVelocity.normalized * 0.3f + carRigidbody2D.velocity.normalized * 0.075f);
                    
                col.gameObject.GetComponent<CactusController>().cut();
            }
        }
    }
    
    public void OnCollisionEnter2D(Collision2D col) { //COLLIDER CABEZA
        if(comboPowerUp || col.contacts[0].otherCollider.transform.gameObject.name != transform.gameObject.name){
            return;
        }

        float colForce = Vector2.Dot((transform.position - col.transform.position).normalized, -transform.up);

        if(colForce > fuerzaParaContarHit && justHitted <= 0){
    	    
            HEALTH -= (0.4f + ((colForce - 0.5f)/2f)) * vidaPerdidaImpacto; //
            justHitted = 2;

            Instantiate(crashEffect, transform.position, crashEffect.transform.rotation);

            if(HEALTH <= 0){ //DEAD
                HEALTH = 0;

                if(!immortal){
                    pauseMenu.showGameOver();
                    Time.timeScale = 0;
                    cam.Shake(0.2f, 0.5f);
                }
            }else{ //LOOSE HP
                cam.Shake(0.15f, 0.35f, 150);
            }
            
            if(col.transform.CompareTag("Enemy")){
                col.gameObject.GetComponent<zombieFlacoController>().die();
                bool isGordo = col.gameObject.GetComponent<zombieGordoController>() != null;
                writeScore(col.gameObject, isGordo, false);
            }else if(col.transform.CompareTag("Cacti")){
                col.gameObject.GetComponent<CactusController>().cut();
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D col){
        if(comboPowerUp)
            OnTriggerEnterChilds(col, "HEAD");
    }

    public void OnTriggerStay2D(Collider2D col){
        if(col.transform.CompareTag("Smoke") && !comboPowerUp){
            if(HEALTH > 0.5f){
                HEALTH -= Time.deltaTime * 0.3f;
                //cam.Shake(0.01f, 0.01f);
            }
        }
    }

    void writeScore(GameObject enem, bool gordo, bool sumaCombo) {
        string scoreStr;
        float scoreIncrease = (gordo ? 3 : 1) * (comboPowerUp ? 1 : Mathf.Ceil(COMBO));
        SCORE += scoreIncrease;

        GameObject upNum = Instantiate(scoreUpwardsNumber, enem.transform.localPosition, cam.transform.rotation);
        upNum.GetComponent<TextMeshProUGUI>().text = "+" + scoreIncrease.ToString();

        if(txtScore.gameObject.transform.localScale.magnitude < 4f)
            txtScore.gameObject.transform.localScale *= 1.225f;

        if(SCORE < gs.hiScore){
            scoreStr = (Mathf.Abs(SCORE - gs.hiScore)).ToString();
        }else{
            scoreStr = "+ " + SCORE.ToString() + " +";
            gs.SaveScore((int)SCORE);
        }
        txtScore.text = scoreStr; txtScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = scoreStr;

        if(sumaCombo && comboCooldown <= 0){
            if(COMBO < 8 && !comboPowerUp){ //HASTA EL 10
                COMBO = Mathf.Ceil(COMBO) + 1; //SUMA COMBO
                
                if(txtCombo.gameObject.transform.localScale.magnitude < 2.75f)
                    txtCombo.gameObject.transform.localScale *= 1.325f;
            }
            else if(!comboPowerUp){ //CUANDO LLEGA A 10
                comboPowerUp = true;
                cam.Shake(0.1f, 0.1f, 175);
                GetComponent<CapsuleCollider2D>().isTrigger = true;
                Instantiate(splashInvencible, invencibilityEffect.transform.position, invencibilityEffect.transform.rotation);
                colorGoWhite = -1;
                invencibilityEffect.SetActive(true);
                txtCombo.text = "MAX"; txtCombo.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "MAX";
                sliderCombo.gameObject.SetActive(true);

                HEALTH = MinMaxSpeed.x; //Curar full vida
                EngineFailureSound.enabled = false;
                engineFullHP = true;
                engineEffectMain.emissionRate = Mathf.RoundToInt((MinMaxSpeed.x - (HEALTH))*20);
            }
            comboCooldown = 0.5f; //tiempo para sumar combos
        }
    }
}
