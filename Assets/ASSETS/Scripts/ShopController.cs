using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ShopController : MonoBehaviour
{
    private GlobalSettings gs;
    private CarStorage cs;
    public MainMenu mainmenu;
    public SpriteRenderer sprCar;
    public SpriteShadow shadowCar;
    public ScreenShakeController cam;
    public TextMeshProUGUI PopUpTxt;
    public TextMeshProUGUI MoneyTxt;
    public GameObject CoinImg;
    public GameObject popUpBuy;
    private float timeActive = 101;


    // Start is called before the first frame update
    void Start()
    {
        gs = FindObjectOfType<GlobalSettings>();
        cs = FindObjectOfType<CarStorage>();

        MoneyTxt.text = gs.driftocoins.ToString();
        MoneyTxt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gs.driftocoins.ToString();

        gs.unlockedCars[0] = true;
        if(!gs.unlockedCars[gs.selectedCar])
            gs.selectedCar = 0;

        LoadCar();
        popUpBuy.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(timeActive < 100){
            if(timeActive < 0){
                timeActive = 100;
                StartCoroutine(flickerPopUp());
            }else{
                popUpBuy.SetActive(true);
                timeActive -= Time.unscaledDeltaTime;
            }
        }
    }

    public void LoadCar(){
        PopUpTxt.text = cs.carNames[gs.selectedCar] + "\n";

        if (gs.unlockedCars[gs.selectedCar]) {
            CoinImg.SetActive(false);
            PopUpTxt.text += cs.descriptions[gs.selectedCar];
            popUpBuy.GetComponent<Image>().raycastTarget = false;
            
            if(timeActive == 100)
                timeActive = 2.25f;
        }
        else {
            CoinImg.SetActive(true);
            PopUpTxt.text += "[     " + cs.carPrices[gs.selectedCar] + " ] BUY";
            popUpBuy.GetComponent<Image>().raycastTarget = true;

            timeActive = 100;
        }


        sprCar.sprite = cs.sprsArray[gs.selectedCar];
        shadowCar.enabled = false; shadowCar.enabled = true;

        cam.Shakes(0.06f, 0.125f, 0);
        gs.SavePlayerPrefs();
    }

    public void increaseCar(int increase) {
        if (increase == +1) {
            gs.selectedCar ++;
            if(gs.selectedCar >= cs.sprsArray.Length)
                gs.selectedCar = 0;
        }
        else if (increase == -1) {
            gs.selectedCar --;
            if(gs.selectedCar < 0)
                gs.selectedCar = cs.sprsArray.Length-1;
        }
        else {
            gs.selectedCar = 0;
        }

        if(gs.unlockedCars[gs.selectedCar])
            timeActive = 3;
        popUpBuy.SetActive(true);
        LoadCar();
    }

    IEnumerator flickerPopUp(){
        for(int i = 0; i < 3; i++){
            yield return new WaitForSecondsRealtime(0.1f);
            popUpBuy.SetActive(true);
            yield return new WaitForSecondsRealtime(0.1f);
            popUpBuy.SetActive(false);
        }
        for(int i = 0; i < 2; i++){
            yield return new WaitForSecondsRealtime(0.05f);
            popUpBuy.SetActive(true);
            yield return new WaitForSecondsRealtime(0.05f);
            popUpBuy.SetActive(false);
        }
    }

    public void buyCar() {
        if(!gs.unlockedCars[gs.selectedCar] && gs.driftocoins >= cs.carPrices[gs.selectedCar]){
            gs.driftocoins -= cs.carPrices[gs.selectedCar];
            gs.unlockedCars[gs.selectedCar] = true;

            
            MoneyTxt.text = gs.driftocoins.ToString();
            MoneyTxt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gs.driftocoins.ToString();
        } else {
            //Muestra pantalla compra
            mainmenu.updateScreens(3);
            mainmenu.changeOptions(1);
        }
        LoadCar();
    }
}
