using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MusicManager : MonoBehaviour
{
    SaveManager sm;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip songThisLevel;


    // Start is called before the first frame update
    void Start()
    {
        sm = Helper.NabSaveData().GetComponent<SaveManager>();
        audioSource.volume = sm.collectibleData.MusicVolumeLevel;
        audioSource.clip = songThisLevel;
        audioSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        audioSource.volume = sm.collectibleData.MusicVolumeLevel;
    }
}
