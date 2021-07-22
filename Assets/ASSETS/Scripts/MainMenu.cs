using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject main;

    private GlobalSettings gs;
    public GameObject CameraCheck;
    public GameObject PixelCheck;
    public GameObject MuteAllCheck;
    public GameObject MuteMusicCheck;
    public GameObject MuteScreenShakeCheck;

    public TextMeshProUGUI ScoreCheck;
    public AudioSource musicSource;
    public GameObject FadeDown;
    public GameObject Settings;
    public GameObject Credits;
    private Vector3 FadeOriginalTrans;
    private int fadeUp = 0;

    public Vector2 maxMinCreditsAlt;
    public GameObject creditsText;

    public GameObject Fade;
    private bool isFaded = false;
    private Vector3 FadePos;

    void Start(){
        gs = FindObjectOfType<GlobalSettings>();
        CameraCheck.SetActive(gs.mobileCam);
        PixelCheck.SetActive(gs.disablePixelEffect);
        MuteAllCheck.SetActive(gs.muteAll);
        MuteMusicCheck.SetActive(gs.muteMusic);
        MuteScreenShakeCheck.SetActive(gs.muteScreenshake);

        ScoreCheck.text = "BEST SCORE: " + gs.hiScore;
        ScoreCheck.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "BEST SCORE: " + gs.hiScore;
    
        if(gs.muteAll)
            AudioListener.volume = 0;
        else
            AudioListener.volume = 1;

        FadeOriginalTrans = FadeDown.transform.position;
        
        FadePos = Fade.transform.position;
        Fade.transform.localPosition = Vector3.zero;
    }

    void Update(){
        if(!isFaded){
            if(Vector3.Distance(Fade.transform.localPosition, new Vector3(0,-10,0)) > 1)
                Fade.transform.localPosition = Vector2.Lerp(Fade.transform.localPosition, new Vector3(0,-10,0), Time.fixedDeltaTime * 2);
            else{
                isFaded = true;
                Fade.transform.localPosition = FadePos;
                Debug.Log("Loaded");
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape)){
            if(FadeDown.transform.position.y == FadeOriginalTrans.y){
                Debug.Log("Exit game");
                QuitGame();
            }else {
                changeOptions(-1);
            }
        }

        if(fadeUp == +1){
            FadeDown.transform.Translate(Vector2.up * Time.deltaTime * 15);
            if(FadeDown.transform.position.y > 0){
                FadeDown.transform.position = Vector2.zero;
                fadeUp = 0;
            }
        }else if(fadeUp == -1){
            FadeDown.transform.Translate(Vector2.down * Time.deltaTime * 15);
            if(FadeDown.transform.position.y < FadeOriginalTrans.y){
                FadeDown.transform.position = FadeOriginalTrans;
                fadeUp = 0;
            }
        }

        creditsText.transform.Translate(Vector2.up * Time.deltaTime * 3 * UpDown);
        if(creditsText.transform.localPosition.y < maxMinCreditsAlt.x){
            creditsText.transform.localPosition = new Vector3(creditsText.transform.localPosition.x, maxMinCreditsAlt.x, creditsText.transform.localPosition.z);
        }else if(creditsText.transform.localPosition.y > maxMinCreditsAlt.y){
            creditsText.transform.localPosition = new Vector3(creditsText.transform.localPosition.x, maxMinCreditsAlt.y, creditsText.transform.localPosition.z);
        }

    }
    public void QuitGame(){
        Application.Quit();
    }

    public void loadScene(string sceneName){
        if(isFaded){
            Time.timeScale = 1;
            SceneManager.LoadScene(sceneName);
        }
    }

    public void startGame(){
        if(isFaded){
            FindObjectOfType<InicioTransition>().empieza = true;
        }
    }

    public void changeCredits(){
        Settings.SetActive(!Settings.activeSelf);
        Credits.SetActive(!Credits.activeSelf);
    }
    private int UpDown = 0;
    public void moveCredits(int UpOrDown){
        UpDown = UpOrDown;
    }

    public void changeOptions(int upDown){
        if(isFaded){
            //options.SetActive(!options.activeSelf);
            //main.SetActive(!main.activeSelf);
            if(fadeUp == 0){
                fadeUp = upDown;
                if(fadeUp == 1){ //Reset Credits
                    creditsText.transform.localPosition = new Vector3(creditsText.transform.localPosition.x, maxMinCreditsAlt.x, creditsText.transform.localPosition.z);
                    Settings.SetActive(true);
                    Credits.SetActive(false);
                }
            }
        }
    }
    
    public void CameraChange(){
        gs.mobileCam = !gs.mobileCam;
        CameraCheck.SetActive(gs.mobileCam);
        gs.SavePlayerPrefs();
    }
    public void ScreenshakeChange(){
        gs.muteScreenshake = !gs.muteScreenshake;
        MuteScreenShakeCheck.SetActive(gs.muteScreenshake);
        gs.SavePlayerPrefs();
    }
    public void MuteMusicChange(){
        gs.muteMusic = !gs.muteMusic;
        MuteMusicCheck.SetActive(gs.muteMusic);
        gs.SavePlayerPrefs();

        musicSource.enabled = !gs.muteMusic;
    }
    public void MuteAllChange(){
        gs.muteAll = !gs.muteAll;
        MuteAllCheck.SetActive(gs.muteAll);
        gs.SavePlayerPrefs();

        if(gs.muteAll)
            AudioListener.volume = 0;
        else
            AudioListener.volume = 1;
    }
    public void PixelChange(){
        gs.disablePixelEffect = !gs.disablePixelEffect;
        PixelCheck.SetActive(gs.disablePixelEffect);
        FindObjectOfType<Camera>().gameObject.GetComponent<PixelCamera>().enabled = !gs.disablePixelEffect;
        gs.SavePlayerPrefs();
    }
}
