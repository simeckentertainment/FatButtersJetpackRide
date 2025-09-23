using UnityEngine;
using System.Diagnostics; // For stack trace

[RequireComponent(typeof(AudioSource))]
public class AudioVolumeDebugger : MonoBehaviour
{
    private AudioSource _audioSource;
    public float Volume
    {
        get => _audioSource.volume;
        set
        {
            // Ignore if value hasn't changed
            if (Mathf.Approximately(_audioSource.volume, value)) return;

            // Log the change and its origin
            StackTrace stackTrace = new StackTrace();
            UnityEngine.Debug.Log($"Volume set to: {value}\n" +
                                 $"Called by: {stackTrace.GetFrame(1).GetMethod().Name}\n" +
                                 $"Stack trace:\n{stackTrace}", this);
            
            _audioSource.volume = value;
        }
    }

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    // Update inspector value for debugging
    void Update()
    {
        // Force-update if bypassed (optional)
        if (!Mathf.Approximately(Volume, _audioSource.volume))
        {
            UnityEngine.Debug.LogWarning($"Volume bypassed! Current: {_audioSource.volume}", this);
            Volume = _audioSource.volume; // Sync and log
        }
    }
}