using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip songThisLevel;

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = collectibleData.MusicVolumeLevel;
        audioSource.clip = songThisLevel;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = collectibleData.MusicVolumeLevel;
    }
}
