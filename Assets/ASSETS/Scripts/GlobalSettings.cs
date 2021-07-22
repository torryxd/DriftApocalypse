using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public bool mobileCam = false;
    public bool disablePixelEffect = false;
    public bool muteAll = false;
    public bool muteMusic = false;
    public bool muteScreenshake = false;
    public int hiScore = 100;

    private static GlobalSettings instanceExists = null;
    public static GlobalSettings Instance{
        get {return instanceExists ; }
    }
    void Awake() {
        if (instanceExists == null) { 
            instanceExists = this;
            DontDestroyOnLoad(this);
        }else{
            Destroy(this.gameObject);
        }

        LoadPlayerPrefs();

        Application.targetFrameRate = 60;
    }

    public void SaveScore(int score){
        hiScore = score;
        PlayerPrefs.SetInt("hiScore", hiScore);
    }
    public void SavePlayerPrefs(){
        PlayerPrefs.SetInt("mobileCam", (mobileCam ? 1 : 0));
        PlayerPrefs.SetInt("pixelEffect", (disablePixelEffect ? 1 : 0));
        PlayerPrefs.SetInt("muteAll", (muteAll ? 1 : 0));
        PlayerPrefs.SetInt("muteMusic", (muteMusic ? 1 : 0));
        PlayerPrefs.SetInt("muteScreenshake", (muteScreenshake ? 1 : 0));
    }
    public void LoadPlayerPrefs(){
        mobileCam = PlayerPrefs.GetInt("mobileCam") == 0 ? false : true;
        disablePixelEffect = PlayerPrefs.GetInt("pixelEffect") == 0 ? false : true;
        muteAll = PlayerPrefs.GetInt("muteAll") == 0 ? false : true;
        muteMusic = PlayerPrefs.GetInt("muteMusic") == 0 ? false : true;
        muteScreenshake = PlayerPrefs.GetInt("muteScreenshake") == 0 ? false : true;
        hiScore = PlayerPrefs.GetInt("hiScore");
    }
}
