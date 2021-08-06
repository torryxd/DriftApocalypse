using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscenarioController : MonoBehaviour
{
    public Vector2 escenarioPosition;
    public float escenarioRadius = 20f;
    private Vector2 escenarioSizes;
    public float escenarioSpacing = 0.5f;

    public float perlinScale = 10f;
    public Vector2 perlinOffSet;
    
    public GameObject cacti;
    public GameObject path;

    public bool noGenerarCactus = false;

    void Start(){
        escenarioSizes.x = escenarioRadius*2f;
        escenarioSizes.y = escenarioRadius*2f;

        perlinOffSet = new Vector2(Random.Range(0,10000), Random.Range(0,10000));

        for(float x = 0f; x < escenarioSizes.x; x += escenarioSpacing){
            for (float y = 0f; y < escenarioSizes.y; y += escenarioSpacing){
                Vector2 calcCirc = new Vector2(x-escenarioRadius, y-escenarioRadius);
                if(calcCirc.magnitude <= escenarioRadius-0.5f){
                    if(calcCirc.magnitude > 2){ //Dejar centro
                        float sample = Mathf.PerlinNoise((x + perlinOffSet.x) * perlinScale, (y + perlinOffSet.y) * perlinScale);
                        if(sample > 0.75f && sample < 0.85f){ //Probabilidad de cactus
                            if(!noGenerarCactus)
                                generarObjeto(x, y, cacti, 0.3f);
                        }else if(sample < 0.415f){ //Probabilidad de camino
                            generarObjeto(x, y, path, 0.3f);  
                        }
                    }
                }else if(calcCirc.magnitude <= escenarioRadius){
                    generarObjeto(x, y, path, 0.25f);
                }
            }
        }

        //Reload script lo load car data
        CarStorage carStorage = FindObjectOfType<CarStorage>();
        carStorage.StartStats();

        Destroy(this);
    }

    void generarObjeto(float x, float y, GameObject obj, float randomPos){
        GameObject scenarioObj;
        Vector2 rndPos = Random.insideUnitCircle.normalized * escenarioSpacing*randomPos; //Random de la posicion
        scenarioObj = Instantiate(obj, new Vector2(x + escenarioPosition.x + rndPos.x, y + escenarioPosition.y + rndPos.y), obj.transform.localRotation);
        scenarioObj.transform.parent = this.transform;
        scenarioObj.transform.eulerAngles = new Vector3(0,0, Random.rotation.eulerAngles.z);   
    }

}
