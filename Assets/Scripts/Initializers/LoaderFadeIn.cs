using UnityEngine;
using UnityEngine.UI;

public class LoaderFadeIn : MonoBehaviour
{

    [SerializeField] GameObject darkener;
    Image darkenerImg;
    [SerializeField] AnalyticsSceneScript analyticsSceneScript;
    float fadeInTimer;
    float fadeInTimerThreshold = 30;
    bool done;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        done = false;
        fadeInTimer = 0;
        darkenerImg = darkener.GetComponent<Image>();
        darkenerImg.color = new UnityEngine.Color(0,0,0,1);
    }

    // Update is called once per frame
    void Update()
    {
        if(done){return;}
        fadeInTimer++;
        darkenerImg.color = new UnityEngine.Color(0,0,0,((fadeInTimerThreshold-fadeInTimer)/fadeInTimerThreshold));
        
    }
}
