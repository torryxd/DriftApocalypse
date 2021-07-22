using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomPitch : MonoBehaviour
{
    public float rndRegion = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().pitch *= Random.Range(1-rndRegion, 1+rndRegion);
    }
}
