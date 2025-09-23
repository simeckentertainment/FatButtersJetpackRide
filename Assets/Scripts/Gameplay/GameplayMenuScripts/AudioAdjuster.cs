using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AudioAdjuster : MonoBehaviour
{

    [SerializeField] public Slider slider;
    [SerializeField] public CollectibleData collectibleData;

    public void AdjustMusicVolume(){
        collectibleData.MusicVolumeLevel = slider.value*0.9f;
    }
    public void AdjustMasterVolume(){
        collectibleData.MasterVolumeLevel = slider.value*0.9f;
    }
    public void AdjustSFXVolume(){
        collectibleData.SFXVolumeLevel = slider.value*0.9f;
    }
}
