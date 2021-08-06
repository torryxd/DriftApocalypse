using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public AudioClip[] sounds;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource aso = this.gameObject.GetComponent<AudioSource>();
        aso.clip = GetRandomClip();
        aso.Play();
    }

    private AudioClip GetRandomClip(){
        int index = Random.Range(0, sounds.Length);
        return sounds[index];
    }
}
