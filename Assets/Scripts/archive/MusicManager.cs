using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip songThisLevel;
    [SerializeField] AudioClip powerupSong;

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;

    private void Start()
    {
        audioSource.volume = collectibleData.MusicVolumeLevel;
        audioSource.clip = songThisLevel;
        audioSource.velocityUpdateMode = AudioVelocityUpdateMode.Dynamic;
        audioSource.Play();
    }

    private float levelSongPlaybackTime = 0f;

    private void Update()
    {
        audioSource.volume = collectibleData.MusicVolumeLevel;
    }

    // TODO Drake: Unused, delete
    public float PlaybackSpeed
    {
        get
        {
            return audioSource.pitch;
        }
        set
        {
            audioSource.pitch = value;
        }
    }

    public void StartPowerupSong()
    {
        if (audioSource.clip != powerupSong)
        {
            levelSongPlaybackTime = audioSource.time;
            audioSource.clip = powerupSong;
            audioSource.Play();
        }
    }

    public void StopPowerupSong()
    {
        if (audioSource.clip == powerupSong)
        {
            audioSource.clip = songThisLevel;
            audioSource.time = levelSongPlaybackTime;
            audioSource.Play();
        }
    }
}
