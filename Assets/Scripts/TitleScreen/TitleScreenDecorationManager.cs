using UnityEngine;
using System.Collections.Generic;

public class TitleScreenDecorationManager : MonoBehaviour
{
    //Randomizes the  decorations for the title menu
    //Trying something different with capitalizing and pluralizing array names. Readability experiment.
    [SerializeField] GameObject Bubba;
    [SerializeField] GameObject[] ENVS;
    [System.NonSerialized] GameObject env;
    [SerializeField] Material[] SKYBOXES;
    [System.NonSerialized] Material skybox;
    [SerializeField] Camera cam;
    [SerializeField] GameObject[] PLATFORMS;
    [System.NonSerialized] GameObject platform;
    [SerializeField] RuntimeAnimatorController[] ANIMS;
    [SerializeField] List<RuntimeAnimatorController> ToyEnabledAnims;
    [System.NonSerialized] RuntimeAnimatorController anim;
    [SerializeField] GameObject toy;
    private CollectibleData collectibleData => SaveManager.Instance.collectibleData;
    [SerializeField] GameObject[] skinObjs;

    private void Start()
    {
        env = Helper.getRandomItemFromArray(ENVS);
        skybox = Helper.getRandomItemFromArray(SKYBOXES);
        platform = Helper.getRandomItemFromArray(PLATFORMS);
        anim = Helper.getRandomItemFromArray(ANIMS);

        //Ensure that the toy is only visible for the play with toy animation.
        if (toy != null)
        {
            toy.SetActive(ToyEnabledAnims.Contains(anim));
        }

        //Assign the correct skin.
        skinObjs[collectibleData.CurrentSkin].SetActive(true);

        //Randomize the puppy
        Bubba.GetComponent<Animator>().runtimeAnimatorController = anim;
        Bubba.GetComponent<Animator>().Play("Base Layer.Entry", 0);

        //Randomize the ENV
        ShowRandomENV();
        ShowRandomPlatform();

        //Randomize the Skybox
        RenderSettings.skybox = skybox;
    }

    private void ShowRandomENV()
    {
        foreach (GameObject e in ENVS)
        {
            e.SetActive(false);
        }
        env.SetActive(true);
    }

    private void ShowRandomPlatform()
    {
        foreach (GameObject p in PLATFORMS)
        {
            p.SetActive(false);
        }
        platform.SetActive(true);
    }
}
