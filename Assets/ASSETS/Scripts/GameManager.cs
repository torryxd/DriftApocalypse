using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool RESPAWN = true;
    public float spawnRadius = 10;
    [SerializeField]
    private float spawnQueue = 1;
    public float incrementSpawnQueue = 1f; //Valor con el que empieza el Increment
    public float decrementBySpawn = 0.98f; //Valor con el que empieza el Increment
    public float gordoChance = 0.2f;
    public GameObject zombieFlaco;
    public GameObject zombieGordo;

    private float ang = 0;

    private float rnd;


    // Update is called once per frame
    void SpawningZombie(){
        int queue = Mathf.FloorToInt(spawnQueue);
        spawnQueue = (spawnQueue - queue);

        for(int i = 0; i < (queue + 1); i++){
            rnd = Random.Range(0f, 1f);
            ang = Random.Range(0f, 360f);

            if(rnd < gordoChance){
                GameObject zombi = Instantiate(zombieGordo, RandomCircle(), zombieGordo.transform.rotation);
                zombi.GetComponent<zombieFlacoController>().comportamiento = 3; //FLANQUEADOR
            }else{
                Instantiate(zombieFlaco, RandomCircle(), zombieFlaco.transform.rotation);
            }
        }
    }

    Vector2 RandomCircle (){
        Vector2 pos;
        pos.x = 0 + spawnRadius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = 0 + spawnRadius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    public void IncreaseRate(){
        if(RESPAWN){
            spawnQueue += incrementSpawnQueue;
            incrementSpawnQueue *= decrementBySpawn;

            SpawningZombie();
        }
    }
}
