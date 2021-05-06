using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject main;
    public GameObject options;

    public GameObject CameraCheck;
    public GameObject PixelCheck;
    private GlobalSettings gs;

    void Start(){
        gs = FindObjectOfType<GlobalSettings>();
        CameraCheck.SetActive(gs.mobileCam);
        PixelCheck.SetActive(gs.pixelEffect);
    }

    public void loadScene(string sceneName){
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void changeOptions(){
        options.SetActive(!options.activeSelf);
        main.SetActive(!main.activeSelf);
    }
    
    public void CameraChange(){
        gs.mobileCam = !gs.mobileCam;
        CameraCheck.SetActive(gs.mobileCam);
        gs.SavePlayerPrefs();
    }
    public void PixelChange(){
        gs.pixelEffect = !gs.pixelEffect;
        PixelCheck.SetActive(gs.pixelEffect);
        FindObjectOfType<Camera>().gameObject.GetComponent<PixelCamera>().enabled = gs.pixelEffect;
        gs.SavePlayerPrefs();
    }
}
