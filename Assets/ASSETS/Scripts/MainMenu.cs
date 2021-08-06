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
    public GameObject[] VolumeAllSpeakers;
    public GameObject MuteMusicCheck;
    public GameObject MuteScreenShakeCheck;

    public TextMeshProUGUI ScoreCheck;
    public AudioSource musicSource;
    public GameObject FadeDown;
    public GameObject Settings;
    public GameObject Credits;
    public GameObject Money;
    private Vector3 FadeOriginalTrans;
    private int fadeUp = 0;
    private int screenToShow = 1; //Settings, Credits, Money

    public Vector2 maxMinCreditsAlt;
    public GameObject creditsText;

    public GameObject Fade;
    private bool isFaded = false;
    private Vector3 FadePos;

    void Start() {
        gs = FindObjectOfType<GlobalSettings>();
        CameraCheck.SetActive(gs.mobileCam);
        PixelCheck.SetActive(gs.disablePixelEffect);
        MuteMusicCheck.SetActive(gs.muteMusic);
        MuteScreenShakeCheck.SetActive(gs.muteScreenshake);

        ScoreCheck.text = "BEST SCORE: " + gs.hiScore;
        ScoreCheck.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "BEST SCORE: " + gs.hiScore;
    
        if(gs.volumeAll == 0)
            gs.volumeAll = (SystemInfo.deviceType == DeviceType.Desktop ? 2 : 3);
        VolumeAllApply();

        FadeOriginalTrans = FadeDown.transform.localPosition;
        
        FadePos = Fade.transform.position;
        Fade.transform.localPosition = Vector3.zero;
    }

    void Update() {
        if(!isFaded){
            if(Vector3.Distance(Fade.transform.localPosition, new Vector3(0,-10,0)) > 1)
                Fade.transform.localPosition = Vector2.Lerp(Fade.transform.localPosition, new Vector3(0,-10,0), Time.fixedDeltaTime * 2);
            else{
                isFaded = true;
                Fade.transform.localPosition = FadePos;
                Debug.Log("Loaded");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if(FadeDown.transform.localPosition.y == FadeOriginalTrans.y){
                Debug.Log("Exit game");
                QuitGame();
            }else {
                changeOptions(-1);
            }
        }

        if(fadeUp == +1){
            FadeDown.transform.Translate(Vector3.up * Time.deltaTime * 15);
            if(FadeDown.transform.localPosition.y > 0){
                FadeDown.transform.localPosition = Vector3.zero;
                fadeUp = 0;
            }
        }else if(fadeUp == -1){
            FadeDown.transform.Translate(Vector3.down * Time.deltaTime * 15);
            if(FadeDown.transform.localPosition.y < FadeOriginalTrans.y){
                FadeDown.transform.localPosition = FadeOriginalTrans;
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
            if(gs.unlockedCars[gs.selectedCar]){
                Time.timeScale = 1;
                //SceneManager.LoadScene(sceneName);
                InicioTransition it = FindObjectOfType<InicioTransition>();
                it.sceneToLoad = (gs.tutorialCompleted ? sceneName : "Tutorial");
                it.empieza = true;
            }else{
                gs.selectedCar = 0;
                FindObjectOfType<ShopController>().LoadCar();
            }
        }
    }

    public void changeCredits(){
        if(screenToShow == 1){
            screenToShow = 2;
        }else if(screenToShow == 2){
            screenToShow = 1;
        }
        updateScreens(0);
    }
    public void updateScreens(int n){
        if(n == 0)
            n = screenToShow;

        if(n == 1){
            Settings.SetActive(true);
            Credits.SetActive(false);
            Money.SetActive(false);
        }else if(n == 2){
            Settings.SetActive(false);
            Credits.SetActive(true);
            Money.SetActive(false);
        }else if(n == 3){
            Settings.SetActive(false);
            Credits.SetActive(false);
            Money.SetActive(true);
        }
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
    public void VolumeAllChange(){
        if(gs.volumeAll == 1){
            gs.volumeAll = 3;
        }
        else if(gs.volumeAll == 2){
            gs.volumeAll = 1;
        }
        else if(gs.volumeAll == 3){
            gs.volumeAll = 2;
        }
        gs.SavePlayerPrefs();
        VolumeAllApply();
    }
    public void VolumeAllApply(){
        if(gs.volumeAll == 1){
            AudioListener.volume = 0f;
            VolumeAllSpeakers[1].SetActive(true);
            VolumeAllSpeakers[2].SetActive(false);
            VolumeAllSpeakers[3].SetActive(false);
        }
        else if(gs.volumeAll == 2){
            AudioListener.volume = 0.3f;
            VolumeAllSpeakers[1].SetActive(false);
            VolumeAllSpeakers[2].SetActive(true);
            VolumeAllSpeakers[3].SetActive(false);
        }
        else if(gs.volumeAll == 3){
            AudioListener.volume = 1;
            VolumeAllSpeakers[1].SetActive(false);
            VolumeAllSpeakers[2].SetActive(false);
            VolumeAllSpeakers[3].SetActive(true);
        }
    }
    public void PixelChange(){
        gs.disablePixelEffect = !gs.disablePixelEffect;
        PixelCheck.SetActive(gs.disablePixelEffect);
        FindObjectOfType<Camera>().gameObject.GetComponent<PixelCamera>().enabled = !gs.disablePixelEffect;
        gs.SavePlayerPrefs();
    }
}
