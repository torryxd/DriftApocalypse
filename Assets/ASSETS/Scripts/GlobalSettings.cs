using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public bool mobileCam = false;
    public bool pixelEffect = true;
    public int hiScore = 0;

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
        PlayerPrefs.SetInt("pixelEffect", (pixelEffect ? 1 : 0));
    }
    public void LoadPlayerPrefs(){
        mobileCam = PlayerPrefs.GetInt("mobileCam") == 0 ? false : true;
        pixelEffect = PlayerPrefs.GetInt("pixelEffect") == 0 ? false : true;
        hiScore = PlayerPrefs.GetInt("hiScore");
    }
}
