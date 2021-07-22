using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private GlobalSettings gs;
    public AudioClip[] songs;
    private AudioSource audioSource;
    private float lastIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        gs = FindObjectOfType<GlobalSettings>();
        audioSource = this.gameObject.GetComponent<AudioSource>();

        audioSource.enabled = !gs.muteMusic;
    }

    private AudioClip GetRandomClip(){
        int index;

        do {
            index = Random.Range(0, songs.Length);
        } while (songs.Length > 1 && index == lastIndex);
        lastIndex = index;

        return songs[index];
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying) {
            audioSource.clip = GetRandomClip();
            audioSource.Play();
        }
    }
}
