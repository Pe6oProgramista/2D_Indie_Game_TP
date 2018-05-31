using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControler : MonoBehaviour {

    public AudioClip[] audios;
    AudioSource audioSource;

    int currClipIndex = 0;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audios[currClipIndex];
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if(currClipIndex + 1 == audios.Length)
            {
                currClipIndex = 0;
            }
            else
            {
                currClipIndex++;
            }
            audioSource.clip = audios[currClipIndex];
            audioSource.Play();
        }
    }
}
