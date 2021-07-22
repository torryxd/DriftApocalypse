using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upwardNumber : MonoBehaviour
{
    private GameObject target;
	public bool moveOnPause = false;

	private float offSetTime;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("txtScore");
        this.transform.parent = GameObject.Find("Canvas").transform;
        this.transform.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeScale > 0 || moveOnPause){
            if(Vector2.Distance(transform.position, target.transform.position) < 0.6f){
                transform.localScale *= 0.95f;
                if(transform.localScale.x < 0.05f)
                    Destroy(this.gameObject);
            }
		    transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.unscaledDeltaTime * 0.75f);
		}
    }
}
