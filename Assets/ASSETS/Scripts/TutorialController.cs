using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialController : MonoBehaviour
{
    public int FASE; //0 no ha empezado, 1 timer gira der o izq, 2 timer boost, 3 drift over cactus, 4 zombies y zombie gordo (poisoning gas) (aparecen 1, 2, 2 y un gordo), 5 score shows you how close from your best, keep driftin to mantain combo, 6 good luck 
    public CarController car;
    public CamController cam;
    public PauseMenu pause;
    public TextMeshProUGUI txtTutorial;
    public GameObject cacti;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    private GlobalSettings gs;
    private float timerLeft, timerRight, timerBoost;
    private float timesToBeat = 0;


    // Start is called before the first frame update
    void Start()
    {
        gs = FindObjectOfType<GlobalSettings>();
        FASE = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (FASE == 1){
            changeText("Hold LEFT or RIGHT to turn" + "\n(" + timesToBeat + "/2)");

            if(car.LEFT) {
                timerLeft += Time.deltaTime;
            }else if(car.RIGHT){
                timerRight += Time.deltaTime;
            }
            if(timerRight > 2.5f && timerLeft > 2.5f){
                FASE ++;
                timerLeft = 0;
                timerRight = 0;
                cam.Shake(0.1f, 0.125f, 150);
                timesToBeat = 0;
            }else if(timerRight > 2 || timerLeft > 2){
                timesToBeat = 1;
            }
        }else if(FASE == 2){
            changeText("Hold LEFT and RIGHT together to boost" + "\n(" + timesToBeat + "/1)");

            if(car.LEFT && car.RIGHT){
                timerBoost += Time.deltaTime;
            }
            if(timerBoost > 3){
                FASE ++;
                cam.Shake(0.1f, 0.125f, 150);
            }
        }else if(FASE == 3){
            cacti.SetActive(true);
            int num = 0;
            foreach(GameObject cac in GameObject.FindGameObjectsWithTag("Cacti")){
                if(cac.GetComponent<CircleCollider2D>().enabled)
                    num++;
            }
            
            changeText("DRIFT over cactus to DESTROY them" + "\n(" + (3-num) + "/3)");
            if(num <= 0){
                FASE ++;
                cam.Shake(0.1f, 0.125f, 150);
            }

        }else if(FASE == 4){
            changeText("Be careful with the FRONT of the car" + "\nYour engine is FRAGILE!");

            if(car.LEFT) {
                timerLeft += Time.deltaTime;
            }else if(car.RIGHT){
                timerRight += Time.deltaTime;
            }
            if(timerRight > 2 && timerLeft > 2){
                FASE ++;
                timerLeft = 0;
                timerRight = 0;
                cam.Shake(0.1f, 0.125f, 150);
            }
        }else if(FASE == 5){
            enemy1.SetActive(true);
            int num = GameObject.FindGameObjectsWithTag("Enemy").Length;

            changeText("DRIFT over zombies to FINISH them" + "\n(" + (2-num) + "/5)");

            if(num <= 0){
                FASE ++;
                cam.Shake(0.1f, 0.125f, 150);
            }
        }else if(FASE == 6){
            enemy2.SetActive(true);
            int num = GameObject.FindGameObjectsWithTag("Enemy").Length;

            changeText("DRIFT over zombies to FINISH them" + "\n(" + (5-num) + "/5)");
            
            if(num <= 0){
                FASE ++;
                cam.Shake(0.1f, 0.125f, 150);
            }
        }else if(FASE == 7){
            enemy3.SetActive(true);
            int num = GameObject.FindGameObjectsWithTag("Enemy").Length;

            changeText("Watch out, big ones will leave POISONING GAS" + "\n(" + (1-num) + "/1)");

            if(num <= 0){
                FASE ++;
                cam.Shake(0.1f, 0.125f, 150);
            }
        }else if(FASE == 8){
            changeText("SCORE shows you how close you are from" + "\nyour BEST. Keep DRIFTING to mantain COMBO");

            if(car.LEFT) {
                timerLeft += Time.deltaTime;
            }else if(car.RIGHT){
                timerRight += Time.deltaTime;
            }
            if(timerRight > 2 && timerLeft > 2){
                FASE ++;
                timerLeft = 0;
                timerRight = 0;
                cam.Shake(0.1f, 0.125f, 150);
            }
        }else if(FASE == 9){
            changeText("Congratulations you FINISHED your TRAINING\nGood luck");

            if(car.LEFT) {
                timerLeft += Time.deltaTime;
            }else if(car.RIGHT){
                timerRight += Time.deltaTime;
            }
            if(timerRight > 2 && timerLeft > 2){
                gs.tutorialCompleted = true;
                gs.SavePlayerPrefs();
                pause.loadScene("MainMenu");
            }
        }
    }

    void changeText(string txt){
        txtTutorial.text = txt;
        txtTutorial.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = txt;
    }
}
