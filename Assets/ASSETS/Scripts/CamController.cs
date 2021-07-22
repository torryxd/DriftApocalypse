using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CamController : MonoBehaviour
{
    private Camera cam;
    private PixelCamera pixelCam;
    private GlobalSettings gs;
    private float defaultCamSize;

    public float smoothOffsetSpeed = 5f;
    public float smoothRotationSpeed = 3f;
    public float smoothZoomSpeed = 5f;

    private bool isShaking = false;
	private float freezeMS = 50f;
	private bool freezeCancel = false;

    private float timeIsFrozen;

    float fpsDeltaTime = 0.0f;

    [Header("Components")]
    public CarController car;
    public PauseMenu pauseMenu;
    public ScreenShakeController screenshake;

    void Start() {
        gs = FindObjectOfType<GlobalSettings>();
        cam = GetComponent<Camera>();
        pixelCam = GetComponent<PixelCamera>();

        defaultCamSize = GetComponent<Camera>().orthographicSize;
    }

    void Update() {
        
        if(pauseMenu.paused || pauseMenu.isGamingOver)
            return;

        Vector3 v3speed = new Vector3(car.GetComponent<Rigidbody2D>().velocity.x, car.GetComponent<Rigidbody2D>().velocity.y, 0);
        
        //camera offset
        bool boosting = car.accelerationFactor != car.defaultAccelerationFactor;
        Vector3 desiredPosition = new Vector3(
            car.transform.position.x + (v3speed.x * (boosting ? 0.525f : 0.5f)) + (car.transform.up.normalized.x * 0.4f),
            car.transform.position.y + (v3speed.y * (boosting ? 0.525f : 0.5f)) + (car.transform.up.normalized.y * 0.4f),
            this.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothOffsetSpeed * Time.unscaledDeltaTime);
        
        //camera zoom
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, defaultCamSize - defaultCamSize*0.275f * ((v3speed.magnitude - car.MinMaxSpeed.y)/-car.MinMaxSpeed.y), smoothZoomSpeed * Time.unscaledDeltaTime);
        
        //camera rotation
        if(gs.mobileCam){
            Vector3 v3Between = (v3speed + car.transform.up).normalized;
            float speedAngle = Mathf.Atan2(-v3Between.x, v3Between.y) * Mathf.Rad2Deg;
            Vector3 desiredRotation = new Vector3(
                transform.eulerAngles.x,
                transform.eulerAngles.y,
                Mathf.LerpAngle(transform.eulerAngles.z, speedAngle, Time.unscaledDeltaTime * smoothRotationSpeed)
                );
            transform.eulerAngles  = desiredRotation;
            
        }

        //FPS
        fpsDeltaTime += (Time.unscaledDeltaTime - fpsDeltaTime) * 0.1f;


        //Try screenshake
        if(Input.GetKeyDown(KeyCode.S))
            Shake(0.2f, 0.5f, 60);
    }

    public void Shake(float magnitude, float duration, int freezeTime = 0){
		if(!gs.muteScreenshake){
			screenshake.Shakes(magnitude, duration, freezeTime);
		}
	}

	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;
 
		GUIStyle style = new GUIStyle();
 
		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
		float msec = fpsDeltaTime * 1000.0f;
		float fps = 1.0f / fpsDeltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);
	} 
}
