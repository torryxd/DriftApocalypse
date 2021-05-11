using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool paused = true;
    public bool firstTimePause = true;
    public GameObject PauseButton;
    public GameObject pauseLayer;

    public GameObject LeftArrow;
    private Vector3 leftArrowScale;
    public GameObject RightArrow;
    private Vector3 rightArrowScale;
    public GameObject textHold;

    [Header("Components")]
    public CarController carController;
    public CamController cam;

    // Start is called before the first frame update
    void Start()
    {
        leftArrowScale = LeftArrow.transform.localScale;
        rightArrowScale = RightArrow.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Pause();

        if(firstTimePause){
            if(carController.steeringInput < -0.1f){
                LeftArrow.transform.localScale = leftArrowScale * 1.15f;
            }else{
                LeftArrow.transform.localScale = leftArrowScale * (0.95f + 0.05f*Mathf.Sin(Time.time*3));
            }
            if(carController.steeringInput > 0.1f){
                RightArrow.transform.localScale = rightArrowScale * 1.15f;
            }else{
                RightArrow.transform.localScale = rightArrowScale * (0.95f + 0.05f*Mathf.Sin(Time.time*3));
            }
            FindObjectOfType<Camera>().orthographicSize = Mathf.Lerp(FindObjectOfType<Camera>().orthographicSize, 0.5f + 2.5f * Mathf.Abs(Mathf.Abs(carController.steeringInput)-1), Time.deltaTime * 5);
        }
    }
    public void unPauseFirstPause(){
        firstTimePause = false;
        RightArrow.SetActive(false);
        LeftArrow.SetActive(false);
        textHold.SetActive(false);
    }

    public void Pause(){
        if(!firstTimePause){
            cam.Shake(0.1f, 0.125f);
            paused = !paused;
            pauseLayer.SetActive(paused);
            Time.timeScale = (paused ? 0 : 1);
        }
    }

    public void loadScene(string sceneName){
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}
