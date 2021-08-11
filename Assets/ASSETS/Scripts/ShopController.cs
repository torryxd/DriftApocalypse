using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Purchasing;

public class ShopController : MonoBehaviour
{
    private GlobalSettings gs;
    private CarStorage cs;
    public MainMenu mainmenu;
    public SpriteRenderer sprCar;
    public SpriteShadow shadowCar;
    public ScreenShakeController cam;
    public TextMeshProUGUI PopUpTxt;
    private TextMeshProUGUI MoneyTxt;
    public GameObject CoinImg;
    public GameObject popUpBuy;
    public GameObject premiumBTN;
    private float timeActive = 101;


    // Start is called before the first frame update
    void Start()
    {
        gs = FindObjectOfType<GlobalSettings>();
        cs = FindObjectOfType<CarStorage>();

        updateDriftocoins();

        gs.unlockedCars[0] = true;
        if(!gs.PREMIUM && !gs.unlockedCars[gs.selectedCar])
            gs.selectedCar = 0;

        LoadCar();
        popUpBuy.SetActive(false);

        if(gs.PREMIUM)
            premiumBTN.transform.position *= 1000;
    }

    // Update is called once per frame
    void Update()
    {
        if(timeActive < 100){
            if(timeActive < 0){
                timeActive = 100;
                StartCoroutine(flickerPopUp(false));
            }else{
                popUpBuy.SetActive(true);
                timeActive -= Time.unscaledDeltaTime;
            }
        }
    }

    public void LoadCar(){
        PopUpTxt.text = cs.cars[gs.selectedCar].name + "\n";

        if (timeActive != 101)
            StartCoroutine(flickerPopUp(true));

        if (gs.PREMIUM || gs.unlockedCars[gs.selectedCar]) {
            CoinImg.SetActive(false);
            PopUpTxt.text += cs.cars[gs.selectedCar].description;
            popUpBuy.GetComponent<Image>().raycastTarget = false;

            if (timeActive != 101)
                timeActive = 2.25f;
            else
                popUpBuy.SetActive(false);

        }
        else {
            CoinImg.SetActive(true);
            PopUpTxt.text += "[     " + cs.cars[gs.selectedCar].price + " ] BUY";
            popUpBuy.GetComponent<Image>().raycastTarget = true;

            timeActive = 100;
        }


        sprCar.sprite = cs.cars[gs.selectedCar].sprite;
        shadowCar.enabled = false; shadowCar.enabled = true;

        cam.Shakes(0.06f, 0.12f, 0);
        gs.SavePlayerPrefs();
    }

    public void increaseCar(int increase) {
        if (increase == +1) {
            gs.selectedCar ++;
            if(gs.selectedCar >= cs.cars.Length)
                gs.selectedCar = 0;
        }
        else if (increase == -1) {
            gs.selectedCar --;
            if(gs.selectedCar < 0)
                gs.selectedCar = cs.cars.Length-1;
        }
        else {
            gs.selectedCar = 0;
        }

        popUpBuy.SetActive(true);
        LoadCar();
    }

    IEnumerator flickerPopUp(bool onOff){
        if (!onOff) {
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
                yield return new WaitForSecondsRealtime(0.05f);
            }
            popUpBuy.SetActive(false);
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.5f);
            popUpBuy.SetActive(true);
        }
    }

    public void updateDriftocoins()
    {
        MoneyTxt = GameObject.Find("txtNumCoins").GetComponent<TextMeshProUGUI>();
        MoneyTxt.text = gs.driftocoins.ToString();
        MoneyTxt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gs.driftocoins.ToString();
    }
    public void buyCar() {
        if(!gs.unlockedCars[gs.selectedCar] && gs.driftocoins >= cs.cars[gs.selectedCar].price) {
            gs.driftocoins -= cs.cars[gs.selectedCar].price;
            gs.unlockedCars[gs.selectedCar] = true;

            updateDriftocoins();
        } else {
            //Muestra pantalla compra
            mainmenu.updateScreens(3);
            mainmenu.changeOptions(1);
        }
        LoadCar();
    }

    void onRewardSuccess() {
        gs.driftocoins += 3;
        gs.SavePlayerPrefs();
        updateDriftocoins();
        Debug.Log(gs.driftocoins);
    }
    public void watchAddEarnCoins() {
        FindObjectOfType<AdsManager>().PlayRewardedAd(onRewardSuccess);
    }

    private string premiumID = "com.torrydev.driftapocalypse.premium";
    public void OnPurchaseComplete(Product product){
        if(product.definition.id == premiumID){
            gs.PREMIUM = true;
            gs.SavePlayerPrefs();
            premiumBTN.transform.position *= 1000;
            Debug.Log("PREMMIUM SUCCESSFULLY BOUGHT");
        }
    }
    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason){
        Debug.Log("Purchase of " + product.definition.id + " failed due to " + reason);
    }
}
