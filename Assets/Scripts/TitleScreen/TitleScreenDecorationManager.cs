using UnityEngine;

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
    [System.NonSerialized] RuntimeAnimatorController anim;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        env = Helper.getRandomItemFromArray(ENVS);
        skybox = Helper.getRandomItemFromArray(SKYBOXES);
        platform = Helper.getRandomItemFromArray(PLATFORMS);
        anim = Helper.getRandomItemFromArray(ANIMS);

        //TODO: actually enable the correct items.

        //Randomize the puppy
        Bubba.GetComponent<Animator>().runtimeAnimatorController = anim;
        Bubba.GetComponent<Animator>().Play("Base Layer.Entry", 0);

        //Randomize the ENV
        ShowRandomENV();
        ShowRandomPlatform();

        //Randomize the Skybox
        RenderSettings.skybox = skybox;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ShowRandomENV()
    {
        foreach (GameObject e in ENVS)
        {
            e.SetActive(false);
        }
        env.SetActive(true);
    }
    void ShowRandomPlatform()
    {
        foreach (GameObject p in PLATFORMS)
        {
            p.SetActive(false);
        }
        platform.SetActive(true);
    }
}
