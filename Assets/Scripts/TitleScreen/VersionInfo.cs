using UnityEngine;
using TMPro;

public class VersionInfo : MonoBehaviour
{
    [SerializeField] TMP_Text versionText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        versionText.text = "Fat Butters' Jetpack Ride " + Application.version + " Alpha." + "\nThanks for testing!";
    }
}


