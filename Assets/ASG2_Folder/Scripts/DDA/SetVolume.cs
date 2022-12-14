/*
 * Author: Melvyn Hoo
 * Date: 20 Nov 2022
 * Description: Simple volume slider to control game audio
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SetVolume : MonoBehaviour
{
    public bool isOn = true;
    public GameObject volumeConsole;
    
    public AudioMixer mixer;

    /// <summary>
    /// Controls game audio using slider
    /// </summary>
    /// <param name="sliderValue"></param>
    public void SetLevel (float sliderValue)
    {
        mixer.SetFloat("MusicVol", Mathf.Log10(sliderValue) * 20);

    }

    public void OpenVolumeConsole()
    {
        
    }

}
