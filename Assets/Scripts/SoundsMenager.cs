using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsMenager : MonoBehaviour
{
    public static SoundsMenager S;
    public AudioClip clickButton;
    public AudioClip zombieDeath;

    AudioSource audioSource;
    bool isPlaying = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        S = this;
    }

    public void PlayButtonSound()
    {
        audioSource.clip = clickButton;
        if(!audioSource.isPlaying)
            audioSource.Play();
    }

    public void PlayZombieDeathSound()
    {
        audioSource.clip = zombieDeath;
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
