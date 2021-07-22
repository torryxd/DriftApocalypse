using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScreenShakeController : MonoBehaviour
{
    private float magnitudeG;
    private float durationG;
    private float freezeTimesG;

    private bool isFrozen = false;
    private float originalTS;
    private float timeIsFrozen = 0;


    public void Update(){
        if(timeIsFrozen > 0){
            timeIsFrozen -= Time.unscaledDeltaTime;
        }else if(isFrozen){
            isFrozen = false;
            if(freezeTimesG > 0)
                Time.timeScale = originalTS;
            StartCoroutine(justShake());
        }
    }

    public void Shakes(float magnitudes, float durations, int freezeTimes = 0)
    {
		isFrozen = true;
        magnitudeG = magnitudes;
        durationG = durations;
        freezeTimesG = freezeTimes;

        if(freezeTimes > 0){
            if(timeIsFrozen <= 0)
                originalTS = Time.timeScale;

            Time.timeScale = 0;
            float timeToFreeze = (freezeTimes / 1000f);
            if(timeToFreeze > timeIsFrozen)
                timeIsFrozen = timeToFreeze;
            
        }
    }

    public IEnumerator justShake()
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < durationG)
        {
			float rnd = Random.Range(0f, 1f);
			float x = Random.Range(-rnd, rnd) * magnitudeG;
			float y = Random.Range(-(1-rnd), (1-rnd)) * magnitudeG;

			transform.position = originalPosition + new Vector3(x, y, 0);
			elapsed += Time.unscaledDeltaTime;
			yield return 0;
        }
        transform.position = originalPosition;
    }
}
