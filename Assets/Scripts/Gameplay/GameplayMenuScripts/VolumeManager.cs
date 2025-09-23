using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeManager : MonoBehaviour
{
    [SerializeField] CollectibleData collectibleData;
    [SerializeField] SaveManager saveManager;
    [SerializeField] AudioSource[] AllAudioObjects;
    [SerializeField] Player player;

    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider SFXSlider;
    // Start is called before the first frame update
    void Start(){
        masterSlider.onValueChanged.AddListener(delegate {masterSliderVolumeCheck();});
        SFXSlider.onValueChanged.AddListener(delegate {SFXSliderVolumeCheck();});
        musicSlider.onValueChanged.AddListener(delegate {musicSliderVolumeCheck();});

    }
    // Update is called once per frame
    void FixedUpdate(){
        AllAudioObjects = QueryAllAudioObjectsInScene();
        //if(AllAudioObjects.Length != 0){
            //SetMaxAudioLevels();
        //}
    }
    void masterSliderVolumeCheck(){
        SFXSliderVolumeCheck();
        musicSliderVolumeCheck();
        collectibleData.MasterVolumeLevel = masterSlider.value;
    }
    void SFXSliderVolumeCheck(){

        if(SFXSlider.value > masterSlider.value){
            SFXSlider.value = masterSlider.value;
        }
        collectibleData.SFXVolumeLevel = SFXSlider.value;
    }
    void musicSliderVolumeCheck(){
        if(musicSlider.value > masterSlider.value){
            musicSlider.value = masterSlider.value;
        }
        collectibleData.MusicVolumeLevel = musicSlider.value;
    }

    AudioSource[] QueryAllAudioObjectsInScene(){
        return GameObject.FindObjectsOfType<AudioSource>();
    }
        

    void SetMaxAudioLevels(){
        //first make sure that all sliders and values are within acceptable ranges based on the master volume.
        foreach (AudioSource obj in AllAudioObjects){
            if(obj.gameObject.tag == "MusicManager"){
                obj.volume = collectibleData.MusicVolumeLevel;
            } else {
                obj.volume = collectibleData.SFXVolumeLevel;
            }
        }
    }

    public void SetMasterVolume(float amount){
        collectibleData.MasterVolumeLevel = amount;
        if(collectibleData.SFXVolumeLevel > amount){
            collectibleData.SFXVolumeLevel = amount;
        }
        if(collectibleData.MusicVolumeLevel > amount){
            collectibleData.MusicVolumeLevel = amount;
        }
        player.sfx.volume = amount;
    }
    public void SetSFXVolume(float amount){
        collectibleData.SFXVolumeLevel = amount;
    }
    public void SetMusicVolume(float amount){
        collectibleData.MusicVolumeLevel = amount;
    }
}
