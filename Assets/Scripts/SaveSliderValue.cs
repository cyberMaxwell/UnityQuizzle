using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SaveSliderValue : MonoBehaviour
{

    private Slider volumeSlider;
    void Start()
    {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.value = PlayerPrefs.GetFloat("volumeValue");
    }

    

 

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat("volumeValue", volumeSlider.value);
        PlayerPrefs.Save();

    }
}
