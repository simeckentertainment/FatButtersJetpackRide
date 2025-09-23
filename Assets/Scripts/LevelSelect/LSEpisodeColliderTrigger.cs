using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSEpisodeColliderTrigger : MonoBehaviour
{
    public Episode episode;
    [SerializeField] LevelSelectLightManager lm;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other){
        if(other.gameObject.tag == "Player"){
            switch(episode){
                case Episode.E1:
                    lm.Episode1 = true;
                    break;
                case Episode.E2:
                    lm.Episode2 = true;
                    break;
                case Episode.E3:
                    lm.Episode3 = true;
                    break;
                case Episode.E4:
                    lm.Episode4 = true;
                break;
                case Episode.E5:
                    lm.Episode5 = true;
                break;
            }
        }
    }
    void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Player"){
            switch(episode){
                case Episode.E1:
                    lm.Episode1 = true;
                    lm.Episode2 = false;
                    lm.Episode3 = false;
                    lm.Episode4 = false;
                    lm.Episode5 = false;
                    break;
                case Episode.E2:
                    lm.Episode1 = false;
                    lm.Episode2 = true;
                    lm.Episode3 = false;
                    lm.Episode4 = false;
                    lm.Episode5 = false;
                    break;
                case Episode.E3:
                    lm.Episode1 = false;
                    lm.Episode2 = false;
                    lm.Episode3 = true;
                    lm.Episode4 = false;
                    lm.Episode5 = false;
                    break;
                case Episode.E4:
                    lm.Episode1 = false;
                    lm.Episode2 = false;
                    lm.Episode3 = false;
                    lm.Episode4 = true;
                    lm.Episode5 = false;
                break;
                case Episode.E5:
                    lm.Episode1 = false;
                    lm.Episode2 = false;
                    lm.Episode3 = false;
                    lm.Episode4 = false;
                    lm.Episode5 = true;
                break;
            }
        }
    }
    void OnTriggerExit(Collider other){
        if(other.gameObject.tag == "Player"){
            switch(episode){
                case Episode.E1:
                    lm.Episode1 = false;
                    break;
                case Episode.E2:
                    lm.Episode2 = false;
                    break;
                case Episode.E3:
                    lm.Episode3 = false;
                    break;
                case Episode.E4:
                    lm.Episode4 = false;
                break;
                case Episode.E5:
                    lm.Episode5 = false;
                break;
            }
        }
    }

    public enum Episode {E1, E2, E3,E4,E5}
}
