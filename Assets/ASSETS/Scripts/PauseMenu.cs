using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool paused = true;
    public bool firstTimePause = true;
    public GameObject PauseButton;
    public GameObject pauseLayer;
    public GameObject gameOverLayer;

    public GameObject LeftArrow;
    private Vector3 leftArrowScale;
    public GameObject RightArrow;
    private Vector3 rightArrowScale;
    public GameObject textHold;

    public GameObject FadeScreen;
    public GameObject popSound;
    public GameObject GameOverScreen;
    public TextMeshProUGUI txtLastScore;
    public TextMeshProUGUI txtBestScore;
    public Slider sliderScore;
    private int bestScore;

    public bool isGamingOver = false;
    private bool isGamedOver = false;
    private float isFading = 1;
    private bool loadingScene = false;
    private string sceneToLoad = "";

    [Header("Components")]
    public CarController carController;
    private GlobalSettings gs;
    public CamController cam;
    public AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        FadeScreen.SetActive(true);
        GameOverScreen.SetActive(true);
        leftArrowScale = LeftArrow.transform.localScale;
        rightArrowScale = RightArrow.transform.localScale;
        gs = FindObjectOfType<GlobalSettings>();
        bestScore = gs.hiScore;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            Pause();
        
        if(firstTimePause){ //IMGS positions
            RightArrow.transform.localPosition = Vector2.Lerp(RightArrow.transform.localPosition, new Vector2(500, 0), Time.fixedDeltaTime * 2.25f);
            LeftArrow.transform.localPosition = Vector2.Lerp(LeftArrow.transform.localPosition, new Vector2(-500, 0), Time.fixedDeltaTime * 2.5f);
            textHold.transform.localPosition = Vector2.Lerp(textHold.transform.localPosition, new Vector2(0, 100), Time.fixedDeltaTime * 3);

            cam.GetComponent<Camera>().orthographicSize = Mathf.Lerp(cam.GetComponent<Camera>().orthographicSize, 0.5f + 2.5f * Mathf.Abs(Mathf.Abs(carController.steeringInput)-1), Time.deltaTime * 5);
        }else if(FadeScreen.activeSelf){
            FadeScreen.transform.Translate(Vector2.down * Time.deltaTime * 10f);
            RightArrow.transform.Translate(Vector2.down * Time.deltaTime * 12f);
            LeftArrow.transform.Translate(Vector2.down * Time.deltaTime * 14f);
            textHold.transform.Translate(Vector2.down * Time.deltaTime * 13f);

            if(FadeScreen.transform.localPosition.y < -2000){
                FadeScreen.SetActive(false);
                RightArrow.SetActive(false);
                LeftArrow.SetActive(false);
                textHold.SetActive(false);
            }
        }


        if(isGamingOver){
            if(!isGamedOver){
                if (GameOverScreen.transform.localPosition.y < 0) {
                    isGamedOver = true;
                    Debug.Log("SHOW ANUNCIO");

                    gameOverLayer.SetActive(true);
                    audioMixer.SetFloat("Effects", -20);
                    audioMixer.SetFloat("Feedback", -20);

                    Vector3 carPos = carController.transform.position;
                    cam.transform.position = new Vector3(carPos.x + 2.25f, carPos.y, cam.transform.position.z);

                    foreach(GameObject del in GameObject.FindGameObjectsWithTag("CleanScreen")) {
                        Destroy(del);
                    }   

                    if(carController.SCORE < bestScore){
                        sliderScore.value = carController.SCORE / bestScore;
                        txtBestScore.text = "Best: " + bestScore; txtBestScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Best: " + bestScore;
                    }else{
                        sliderScore.value = 1;
                        txtBestScore.text = "NEW HIGH SCORE!"; txtBestScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "NEW HIGH SCORE!";
                    }
                    txtLastScore.text = "Last: " + carController.SCORE; txtLastScore.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Last: " + carController.SCORE;

                }else{
                    GameOverScreen.transform.localPosition = Vector2.Lerp(GameOverScreen.transform.localPosition, new Vector2(0, -50), Time.fixedDeltaTime * 2);
                }
            }

            if(isGamedOver && isFading > 0){ //&& anuncio visto
                Color clr = GameOverScreen.GetComponent<Image>().color;
                GameOverScreen.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, isFading);

                isFading -= Time.unscaledDeltaTime * 3;
                if (isFading < 0) {
                    GameOverScreen.transform.localPosition = new Vector3(0, 2000, GameOverScreen.transform.localPosition.z);
                    GameOverScreen.GetComponent<Image>().color = new Color(clr.r, clr.g, clr.b, 1);
                }
            }
        }
        
        if(loadingScene){
            if (GameOverScreen.transform.localPosition.y < 0) {
                loadingScene = false;

                Time.timeScale = 1;
                audioMixer.ClearFloat("Effects");
                audioMixer.ClearFloat("Feedback");
                SceneManager.LoadScene(sceneToLoad);
            }else{
                GameOverScreen.transform.localPosition = Vector2.Lerp(GameOverScreen.transform.localPosition, new Vector2(0, -50), Time.fixedDeltaTime * 3);
            }
        }
    }
    public void unPauseFirstPause(){
        Instantiate(popSound, transform.position, transform.rotation);
        firstTimePause = false;
    }

    public void Pause(){
        if(!firstTimePause && !isGamingOver){
            cam.Shake(0.1f, 0.125f);
            paused = !paused;
            pauseLayer.SetActive(paused);
            Time.timeScale = (paused ? 0 : 1);
        }
    }

    public void loadScene(string sceneName){
        loadingScene = true;
        sceneToLoad = sceneName;
    }

    public void showGameOver(){
        Time.timeScale = 0;
        isGamingOver = true;
    }
}
