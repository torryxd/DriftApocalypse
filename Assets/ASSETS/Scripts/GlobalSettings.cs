using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSettings : MonoBehaviour
{
    public int hiScore = 0;
    public bool mobileCam = false;
    public bool disablePixelEffect = false;
    public int volumeAll = 0;
    public bool muteMusic = false;
    public bool muteScreenshake = false;
    public bool tutorialCompleted = false;
    public int driftocoins = 0;
    public bool[] unlockedCars = new bool[5];
    public int selectedCar = 0;
    public bool PREMIUM = false;
    public int pointsToReachACoin = 0;

    private static GlobalSettings instanceExists = null;
    public static GlobalSettings Instance{
        get {return instanceExists ; }
    }
    void Awake() {
        //Eliminar GS repetido
        if (instanceExists == null) { 
            instanceExists = this;
            DontDestroyOnLoad(this);
        }else{
            Destroy(this.gameObject);
        }

        //Cargar datos del guardado al objeto
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
        PlayerPrefs.SetInt("muteAll", volumeAll);
        PlayerPrefs.SetInt("muteMusic", (muteMusic ? 1 : 0));
        PlayerPrefs.SetInt("muteScreenshake", (muteScreenshake ? 1 : 0));
        PlayerPrefs.SetInt("tutorialCompleted", (tutorialCompleted ? 1 : 0));

        PlayerPrefs.SetInt("driftocoins", driftocoins);
        for (int i = 0; i < unlockedCars.Length; i++) {
            PlayerPrefs.SetInt("unlockedCar" + i, ((unlockedCars[i])) ? 1 : 0);
        }
        PlayerPrefs.SetInt("selectedCar", selectedCar);
        PlayerPrefs.SetInt("PREMIUM", (PREMIUM ? 1 : 0));
        PlayerPrefs.SetInt("pointsToReachACoin", pointsToReachACoin);
    }
    public void LoadPlayerPrefs(){
        hiScore = PlayerPrefs.GetInt("hiScore");
        mobileCam = PlayerPrefs.GetInt("mobileCam") == 0 ? false : true;
        disablePixelEffect = PlayerPrefs.GetInt("pixelEffect") == 0 ? false : true;
        volumeAll = PlayerPrefs.GetInt("muteAll");
        muteMusic = PlayerPrefs.GetInt("muteMusic") == 0 ? false : true;
        muteScreenshake = PlayerPrefs.GetInt("muteScreenshake") == 0 ? false : true;
        tutorialCompleted = PlayerPrefs.GetInt("tutorialCompleted") == 0 ? false : true;
        
        driftocoins = PlayerPrefs.GetInt("driftocoins");
        for (int i = 0; i < unlockedCars.Length; i++) {
            unlockedCars[i] = PlayerPrefs.GetInt("unlockedCar"+i) == 0 ? false : true;
        }
        selectedCar = PlayerPrefs.GetInt("selectedCar");
        PREMIUM = PlayerPrefs.GetInt("PREMIUM") == 0 ? false : true;
        pointsToReachACoin = PlayerPrefs.GetInt("pointsToReachACoin");
    }
}
