using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarStorage : MonoBehaviour
{
    private GlobalSettings gs;
    private CarController car;
    public Car[] cars;

    // Start is called before the first frame update
    void Start()
    {
        /*
        //Reset All Cars (only for updates)
        if(!gs.updated1) {
            gs.updated1 = true;
            resetAllCars();
        }
        */
    }

    public void resetAllCars(){
        gs = FindObjectOfType<GlobalSettings>();

        for(int i = 1; i < cars.Length; i++){
            if(gs.unlockedCars[i]) {
                gs.unlockedCars[i] = false;
                gs.driftocoins += cars[i].price;
            }
        }
        gs.SavePlayerPrefs();
    }

    public void StartStats(){ //Multipliers to base attributes
        gs = FindObjectOfType<GlobalSettings>();
        car = FindObjectOfType<CarController>();

        GameObject spr = car.transform.GetChild(0).gameObject;
        spr.GetComponent<SpriteRenderer>().sprite = cars[gs.selectedCar].sprite;
        spr.GetComponent<SpriteShadow>().enabled = false;
        spr.GetComponent<SpriteShadow>().enabled = true;


        if(cars[gs.selectedCar].boostMultiplierOrSet)
            car.boostAcceleration *= cars[gs.selectedCar].boostAcceleration;
        else
            car.boostAcceleration = cars[gs.selectedCar].boostAcceleration;

        car.hasTyres = cars[gs.selectedCar].hasTyres;
        car.tyreLeft.SetActive(cars[gs.selectedCar].hasTyres);
        car.tyreRight.SetActive(cars[gs.selectedCar].hasTyres);

        car.defaultAccelerationFactor *= cars[gs.selectedCar].accelerationForce;
        car.transform.localScale *= cars[gs.selectedCar].size;
        car.HealthRegen *= cars[gs.selectedCar].healthRegenSpeed;
        car.vidaPerdidaImpacto *= cars[gs.selectedCar].vidaPerdidaImpacto;
        car.fuerzaParaContarHit *= cars[gs.selectedCar].fuerzaParaContarHit;
        car.steeringSpeed *= cars[gs.selectedCar].maxSteeringSpeed;
        car.wheelSteeringFactor *= cars[gs.selectedCar].steeringWheelTurnSpeed;
        car.resetWheelSteeringFactor *= cars[gs.selectedCar].resetWheelSteeringFactor;
        car.driftFactor *= cars[gs.selectedCar].driftFactor;
        car.comboDecreaseFactor *= cars[gs.selectedCar].comboDecreaseFactor;
    }

    /* PARAMETERS TO CHANGE
        Potencia del coche para acelerar // defaultAccelerationFactor
        Tamaño del coche // car.transform.localScale
        Potencia extra que gana al hacer boost // boostAcceleration
        Velocidad a la que se regenera el coche // HealthRegen
        Vida (menor = mas vida) // vidaPerdidaImpacto
        Angulo necesario para contar un hit (menor = mas debil) // fuerzaParaContarHit
        Velocidad maxima de giro // steeringSpeed
        Velocidad a la que gira el volante // wheelSteeringFactor
        Velocidad a la que vuelve el volante al centro // resetWheelSteeringFactor
        Factor para corregir el derrape y añadir agarre (menor = mas derrape) // driftFactor
        Velocidad a la que baja el combo en general (menor = mejor)// comboDecreaseFactor
        Bool para activar las ruedas // hastyres 
            car.tyreLeft.SetActive(false);
            car.tyreRight.SetActive(false);
    */
}
