using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Car", menuName = "Car")]
public class Car : ScriptableObject
{
    //public bool unlocked = false;
    public string name;
    public Sprite sprite;
    public string description;
    public int price;

    [Header("STATS")]
    public bool boostMultiplierOrSet = true; //Boost is a multiplier or set that value
    public float boostAcceleration = 1; //Potencia extra que gana al hacer boost
    public float accelerationForce = 1; //Potencia del coche para acelerar
    public float size = 1; //Tamaño del coche
    public float healthRegenSpeed = 1; //Velocidad a la que se regenera el coche
    public float vidaPerdidaImpacto = 1; //Vida (menor = mas vida)
    public float fuerzaParaContarHit = 1; //Angulo necesario para contar un hit (menor = mas debil)
    public float maxSteeringSpeed = 1; //Velocidad maxima de giro
    public float steeringWheelTurnSpeed = 1; //Velocidad a la que gira el volante
    public float resetWheelSteeringFactor = 1; //Velocidad a la que vuelve el volante al centro
    public float driftFactor = 1; //Factor para corregir el derrape y añadir agarre (menor = mas derrape)
    public float comboDecreaseFactor = 1; //Velocidad a la que baja el combo en general (menor = mejor)
    public bool hasTyres = false; //Bool para activar las ruedas

}
