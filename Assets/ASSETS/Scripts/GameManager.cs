using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float spawnRadius = 10;
    [Header("Flaco")][SerializeField]
    private float FlacoSpawnQueue = 1;
    public GameObject zombieFlaco;
    public float FlacoIncrementSpawnQueue = 1;

    
    [Header("Gordo")][SerializeField]
    private float GordoSpawnQueue = 1;
    public GameObject zombieGordo;
    public float GordoIncrementSpawnQueue = 1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void SpawningFlaco(){
        int queue = Mathf.FloorToInt(FlacoSpawnQueue);
        FlacoSpawnQueue -= (FlacoSpawnQueue - queue);

        for(int i = 0; i < queue; i++){
            Vector2 pos = RandomCircle(Vector2.zero, spawnRadius);
            Instantiate(zombieFlaco, pos, zombieFlaco.transform.rotation);
        }
    }

    void SpawningGordo(){
        int queue = Mathf.FloorToInt(FlacoSpawnQueue);
        GordoSpawnQueue -= (GordoSpawnQueue - queue);

        for(int i = 0; i < queue; i++){
            Vector2 pos = RandomCircle(Vector2.zero, spawnRadius);
            Instantiate(zombieGordo, pos, zombieGordo.transform.rotation);
        }
    }

    Vector3 RandomCircle (Vector2 center, float radius){
        float ang = Random.value * 360;
        Vector2 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    public void IncreaseRate(string mobName){
        if(mobName == "Flaco"){
            FlacoSpawnQueue *= FlacoIncrementSpawnQueue;
            SpawningFlaco();
        }else if(mobName == "Gordo"){
            GordoSpawnQueue *= GordoIncrementSpawnQueue;
            SpawningGordo();
        }
    }
}
