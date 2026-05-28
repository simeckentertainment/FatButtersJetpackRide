using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelectLighting : MonoBehaviour
{
    [SerializeField] private Light lightObject;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private List<EpisodeLighting> episodeLighting;
    [SerializeField] private LightingEffect disabledLighting;

    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;

    private void Start()
    {
        episodeLighting = episodeLighting.OrderBy(x => x.FirstLevelSelectButton.transform.position.x).ToList();
    }

    private void Update()
    {
        UpdateLighting();
    }

    private void UpdateLighting()
    {
        var cameraPosition = cameraTransform.transform.position.x;
        var currentIndex = 0;

        while (currentIndex < episodeLighting.Count - 1 &&
            cameraPosition > episodeLighting[currentIndex].LastLevelSelectButton.transform.position.x)
        {
            currentIndex++;
        }

        var currentLighting = episodeLighting[currentIndex];
        var previousLighting = GetPreviousLighting(currentIndex);

        var endPosition = currentLighting.FirstLevelSelectButton.transform.position.x;
        var startPosition = previousLighting.LastLevelSelectButton.transform.position.x;

        var transitionPercent = GetTransitionPercent(cameraPosition, startPosition, endPosition);

        SetLighting(previousLighting, currentLighting, transitionPercent);
    }

    private EpisodeLighting GetPreviousLighting(int currentIndex)
    {
        if (currentIndex > 0)
        {
            return episodeLighting[currentIndex - 1];
        }
        else
        {
            var previousLighting = episodeLighting[currentIndex];
            previousLighting.LastLevelSelectButton = episodeLighting[currentIndex].FirstLevelSelectButton;
            return previousLighting;
        }
    }

    private float GetTransitionPercent(float cameraPosition, float startPosition, float endPosition)
    {
        var transitionPercent = (cameraPosition - startPosition) / (endPosition - startPosition);
        if (transitionPercent < 0)
        {
            transitionPercent = 0;
        }
        if (transitionPercent > 1)
        {
            transitionPercent = 1;
        }

        return transitionPercent;
    }

    private void SetLighting(EpisodeLighting previous, EpisodeLighting next, float transitionPercent)
    {
        if (!collectibleData.LevelBeaten[previous.LastLevelSelectButton.levelID])
        {
            next.Lighting = disabledLighting;
        }
        if (!collectibleData.LevelBeaten[previous.FirstLevelSelectButton.levelID])
        {
            previous.Lighting = disabledLighting;
        }

        lightObject.intensity = ((next.Lighting.Intensity - previous.Lighting.Intensity) * transitionPercent) + previous.Lighting.Intensity;
        lightObject.color = ((next.Lighting.Color - previous.Lighting.Color) * transitionPercent) + previous.Lighting.Color;
    }
}

[Serializable]
public struct EpisodeLighting
{
    public LevelButtonIDHolder FirstLevelSelectButton;
    public LevelButtonIDHolder LastLevelSelectButton;
    public LightingEffect Lighting;
}

[Serializable]
public struct LightingEffect
{
    public float Intensity;
    public Color Color;
}
