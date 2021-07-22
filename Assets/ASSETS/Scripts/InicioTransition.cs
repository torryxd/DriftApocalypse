using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InicioTransition : MonoBehaviour
{
    public bool empieza = false;
    public GameObject [] gosToMove;
    public Camera cam;

    public GameObject cactusObj;
    private float timeToSpawnAnotherCactus = 0f;


    // Update is called once per frame
    void Update()
    {
        timeToSpawnAnotherCactus -= Time.unscaledDeltaTime;
        if(timeToSpawnAnotherCactus < 0){
            timeToSpawnAnotherCactus = 0.9f;
            float rndPosCactus = 2;
            while(rndPosCactus > 1.5f && rndPosCactus < 2.5f)
                rndPosCactus = Random.Range(-3.5f, 3.5f);
            
            GameObject cac = Instantiate(cactusObj, new Vector2(rndPosCactus, 2.5f), cactusObj.transform.rotation);
            Rigidbody2D rb2d = cac.AddComponent(typeof (Rigidbody2D)) as Rigidbody2D;
            rb2d.AddForce(Vector2.down * 6.5f);
        }

        if(empieza) {
            Vector2 camLerp = Vector2.Lerp(cam.transform.position, transform.position, Time.unscaledDeltaTime * 1.5f);
            cam.transform.position = new Vector3(camLerp.x, camLerp.y, cam.transform.position.z);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 3, Time.unscaledDeltaTime * 1.25f);

            for(int i = 0; i < gosToMove.Length; i++){
                gosToMove[i].transform.Translate(Vector2.down * Time.deltaTime * 8);
            }

            if(gosToMove[0].transform.position.y < 0){
                Time.timeScale = 1;
                SceneManager.LoadScene("GameScene");
            }
        }
    }

    public void OnCollisionEnter2D(Collision2D col) {
        Destroy(col.gameObject);
    }
}
