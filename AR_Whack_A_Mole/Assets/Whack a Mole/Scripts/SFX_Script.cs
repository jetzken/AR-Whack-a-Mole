using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Script : MonoBehaviour {

    public AudioClip hitOne;
    public AudioClip hitTwo;

    private AudioSource source;
    private float lowPitch = 0.75f;
    private float highPitch = 1.25f;
    private float lowVol = 0.5f;
    private float highVol = 1.0f;

	// Use this for initialization
	void Awake () {
        source = GetComponent<AudioSource>();
	}

    public void PlayMoleHit()
    {
        // randomise hit sound
        source.pitch = Random.Range(lowPitch, highPitch);
        source.volume = Random.Range(lowVol, highVol);
        if (Random.Range(1, 50) / 2 == 2)
            source.PlayOneShot(hitOne);
        else
            source.PlayOneShot(hitTwo);
    }
}
