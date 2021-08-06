using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarStorage : MonoBehaviour
{
    private GlobalSettings gs;
    private CarController car;
    public Sprite[] sprsArray;
    public string[] carNames;
    public string[] descriptions;
    public int[] carPrices;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartStats(){ //Multipliers to base attributes
        gs = FindObjectOfType<GlobalSettings>();
        car = FindObjectOfType<CarController>();

        GameObject spr = car.transform.GetChild(0).gameObject;
        spr.GetComponent<SpriteRenderer>().sprite = sprsArray[gs.selectedCar];
        spr.GetComponent<SpriteShadow>().enabled = false;
        spr.GetComponent<SpriteShadow>().enabled = true;

        if(gs.selectedCar == 1){ //CAT
            car.steeringSpeed *= 1.05f;
            car.driftFactor *= 2.5f;
            car.fuerzaParaContarHit *= 1 / 1.5f;
            car.transform.localScale *= 1/ 1.3f;
        }
        else if(gs.selectedCar == 2){ //CELICA
            car.defaultAccelerationFactor *= 1 / 1.5f;
            car.boostAcceleration *= 1.7f;
        }
        else if(gs.selectedCar == 3){ //MISSING
            car.hasTyres = false;
            car.tyreLeft.SetActive(false);
            car.tyreRight.SetActive(false);
            car.vidaPerdidaImpacto *= 5f;
        }
        else if(gs.selectedCar == 4){ //TANK
            car.hasTyres = false;
            car.tyreLeft.SetActive(false);
            car.tyreRight.SetActive(false);
            car.defaultAccelerationFactor *= 1 / 3f;
            car.boostAcceleration *= 1.5f;
            car.wheelSteeringFactor *= 1 / 1.1f;
            car.steeringSpeed *= 1 / 1.5f;
            car.vidaPerdidaImpacto *= 1 / 1.5f;
            car.HealthRegen *= 1 / 1.2f;
            car.transform.localScale *= 1.15f;
        }
        else if(gs.selectedCar == 5){ //OCTANE
            car.hasTyres = false;
            car.tyreLeft.SetActive(false);
            car.tyreRight.SetActive(false);
            car.defaultAccelerationFactor *= 1.3f;
            car.HealthRegen *= 1 / 1.5f;
            car.fuerzaParaContarHit *= 1.05f;
        }
        else if(gs.selectedCar == 6){ //TAXI
            car.defaultAccelerationFactor *= 1 / 1.5f;
            car.wheelSteeringFactor *= 1 / 1.3f;
            car.comboDecreaseFactor *= 1 / 2.5f;
            car.fuerzaParaContarHit *= 1.1f;
        }
        else if(gs.selectedCar == 7){ //CAMION
            car.hasTyres = false;
            car.tyreLeft.SetActive(false);
            car.tyreRight.SetActive(false);
            car.steeringSpeed *= 1 / 1.5f;
            car.boostAcceleration = 0.2f;
            car.transform.localScale *= 1.6f;
        }
        else if(gs.selectedCar == 8){ //PROTOTYPE
            car.defaultAccelerationFactor *= 2f;
            car.driftFactor *= 1.5f;
            car.boostAcceleration = -0.6f;
        }
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
