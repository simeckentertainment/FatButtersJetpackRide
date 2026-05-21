using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelectLighting : MonoBehaviour
{
    [SerializeField] private Light lightObject;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private List<EpisodeLighting> episodeLighting;

    private void Start()
    {
        episodeLighting = episodeLighting.OrderBy(x => x.FirstLevelSelectButton.position.x).ToList();
    }

    private void Update()
    {
        var cameraPosition = cameraTransform.transform.position.x;
        var currentIndex = 0;

        while (currentIndex < episodeLighting.Count - 1 && 
            cameraPosition > episodeLighting[currentIndex].LastLevelSelectButton.transform.position.x)
        {
            currentIndex++;
        }

        var currentLighting = episodeLighting[currentIndex];
        var previousLighting = currentIndex > 0 ? episodeLighting[currentIndex - 1] : currentLighting;

        var endPosition = currentLighting.FirstLevelSelectButton.transform.position.x;
        var startPosition = previousLighting.LastLevelSelectButton.transform.position.x;

        var transitionPercent = (cameraPosition - startPosition) / (endPosition - startPosition);
        if (transitionPercent < 0)
        {
            transitionPercent = 0;
        }
        if (transitionPercent > 1)
        {
            transitionPercent = 1;
        }

        SetLighting(previousLighting, currentLighting, transitionPercent);
    }

    private void SetLighting(EpisodeLighting previous, EpisodeLighting next, float transitionPercent)
    {
        lightObject.intensity = ((next.Intensity - previous.Intensity) * transitionPercent) + previous.Intensity;
        lightObject.color = ((next.Color - previous.Color) * transitionPercent) + previous.Color;
    }
}

[Serializable]
public struct EpisodeLighting
{
    public Transform FirstLevelSelectButton;
    public Transform LastLevelSelectButton;
    public float Intensity;
    public Color Color;
}