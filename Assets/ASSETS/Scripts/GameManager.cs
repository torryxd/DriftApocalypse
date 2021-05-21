using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float spawnRadius = 10;
    [SerializeField]
    private float spawnQueue = 1;
    public float incrementSpawnQueue = 1f; //Valor con el que empieza el Increment
    public float difficultyFactor = 0.99f; //Valor por el que se multiplica el increment por cada zombie matado
    public float gordoChance = 0.2f;
    public GameObject zombieFlaco;
    public GameObject zombieGordo;

    private float ang = 0;

    private float rnd;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 10; i++){
            SpawningZombie();
            ang = (ang+10)%360;
        }
    }

    void Update()
    {
        ang = (ang+(Time.deltaTime*30))%360;
    }

    // Update is called once per frame
    void SpawningZombie(){
        int queue = Mathf.FloorToInt(spawnQueue);
        spawnQueue = (spawnQueue - queue);

        for(int i = 0; i < (queue + 1); i++){
            rnd = Random.Range(0f, 1f);
            if(rnd < gordoChance){
                Instantiate(zombieGordo, RandomCircle(), zombieGordo.transform.rotation);
            }else{
                Instantiate(zombieFlaco, RandomCircle(), zombieFlaco.transform.rotation);
            }
            ang = (ang+3)%360;
        }
    }

    Vector2 RandomCircle (){
        Vector2 pos;
        pos.x = 0 + spawnRadius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = 0 + spawnRadius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }

    public void IncreaseRate(){
        spawnQueue += incrementSpawnQueue;
        incrementSpawnQueue *= difficultyFactor;
        SpawningZombie();
    }
}
