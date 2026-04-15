using System.Collections;
using UnityEngine;

public class HolsterTrigger : MonoBehaviour
{
    [SerializeField] GameObject holster;
    [SerializeField] GameObject jetpackPanel;
    [SerializeField] EventTriggeers eventTriggeers;
    [SerializeField] Player player;
    [SerializeField] private EditorLocalTransform jetPackArrowTransform;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            
           transform.GetChild(0).gameObject.SetActive(false);
          
           GetComponent<CapsuleCollider>().enabled = false;
           
            //player.UI.ShowInfoText("Jet PackEnabled",jetPackArrowTransform ,true);
           StartCoroutine(OffJetPack());
        }
    }
    // jetpack on info
    IEnumerator OffJetPack()
    {
  
         yield return new WaitForSecondsRealtime(0.7f);
        
        StartCoroutine(ScaleOverTime(2,1,Vector3.zero));
        eventTriggeers.killThrust = false;
        yield return new WaitForSecondsRealtime(4);
        StartCoroutine(ScaleOverTime(1,0,Vector3.one));
        
    }
    // Scale effects lerping 
    private IEnumerator ScaleOverTime(float duration, float scale,Vector3 startScale) {
   
    var endScale = Vector3.one * scale;
    var elapsed = 0f;

    while (elapsed < duration) {
        var t = elapsed / duration;
        jetpackPanel.transform.localScale = Vector3.Lerp(startScale, endScale, t);
        elapsed += Time.deltaTime;
        yield return null;
    }
    holster.SetActive(true);
    jetpackPanel.transform.localScale = endScale;
}
}
