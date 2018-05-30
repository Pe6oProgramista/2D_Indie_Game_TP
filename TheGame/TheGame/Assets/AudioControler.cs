using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControler : MonoBehaviour {

    public AudioClip[] audios;

	void Start () {
        DontDestroyOnLoad(gameObject);
        
    }
}
