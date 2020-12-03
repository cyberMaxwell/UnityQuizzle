using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChangeVolume : MonoBehaviour
{
    private AudioSource audioSource;
    private float volume = 1f;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        volume = PlayerPrefs.GetFloat("volume");
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volume;
    }

    public void SetVolume(float vol)
    {
        volume = vol;
        PlayerPrefs.SetFloat("volume", volume);
        PlayerPrefs.Save();
    }

}
