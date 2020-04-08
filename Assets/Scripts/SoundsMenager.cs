using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsMenager : MonoBehaviour
{
    public static SoundsMenager S;
    public AudioClip clickButton;
    public AudioClip zombieDeath;
    public AudioClip itemPickedup;
    public AudioClip spikesAttack; 

    AudioSource audioSource;
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
        audioSource.volume = 1f;
        audioSource.clip = clickButton;
        Play();
    }

    public void PlayZombieDeathSound()
    {
        audioSource.volume = 0.2f;
        audioSource.clip = zombieDeath;
        Play();
    }

    public void PlayItemPickedup()
    {
        audioSource.volume = 1f;
        audioSource.clip = itemPickedup;
        Play();
    }

    public void PlaySpikesAttack()
    {
        audioSource.volume = 1f;
        audioSource.clip = spikesAttack;
        Play();
    }

    private void Play()
    {
        if (!audioSource.isPlaying)
            audioSource.Play();
    }
}
