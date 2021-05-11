using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieGenerator : MonoBehaviour
{
    [SerializeField]
    private float spawnQueue = 1;
    private float incrementSpawnQueue = 1;
    public float defaultSpawnRate = 1;
    private float spawnRate = 1;
    public float spawnRadius = 10;
    public GameObject zombieGordo;
    public GameObject zombieFlaco;


    // Start is called before the first frame update
    void Start()
    {
        spawnRate = defaultSpawnRate;
        StartCoroutine(WaitForWave());
    }

    // Update is called once per frame
    IEnumerator WaitForWave(){
        int queue = Mathf.FloorToInt(spawnQueue);
        spawnQueue -= (spawnQueue - queue);

        for(int i = 0; i < spawnQueue; i++){
            Vector2 pos = RandomCircle(Vector2.zero, spawnRadius);
            Instantiate(zombieFlaco, pos, zombieFlaco.transform.rotation);
        }
        
        spawnQueue += 0.05f;
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(WaitForWave());
    }

    Vector3 RandomCircle (Vector2 center, float radius){
         float ang = Random.value * 360;
         Vector2 pos;
         pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
         pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
         return pos;
     }
}
