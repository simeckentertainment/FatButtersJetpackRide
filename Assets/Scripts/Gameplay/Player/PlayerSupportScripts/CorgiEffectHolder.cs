using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.Services.CloudSave.Models.Data.Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]
public class CorgiEffectHolder : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] Animator anim;
    [Header("Audiovisual FX statics")]
    public float startVolume;
    public float maxVolume;
    public float volumeSpeed;
    public float RocketLightMult;

    [Header("Skins")]
    [SerializeField] public PlayerSkin[] PlayerSkins;
    [SerializeField] public GameObject[] Costumes;


    [Header("All Skin AV data holders. \nThis data will get overridden during Gameplay Start.")]
    [SerializeField] public GameObject PrimaryPuppyObj;
    [SerializeField] public GameObject jetpackHolsterHolder;
    [SerializeField] public GameObject leftJetpackParentObj;
    [SerializeField] public GameObject leftJetpackModelHolder;
    [SerializeField] public GameObject rightJetpackParentObj;
    [SerializeField] public GameObject rightJetpackModelHolder;
    [SerializeField] public GameObject combinedJetpackParentObj;
    [SerializeField] public GameObject combinedJetpackModelHolder;
    [SerializeField] public GameObject[] MainThrusterParticleHolders;
    [SerializeField] public GameObject[] PlusThrusterParticleHolders;
    [SerializeField] public GameObject[] MinusThrusterParticleHolders;
    [SerializeField] public GameObject[] ThrusterLightHolders;
    [SerializeField] public GameObject[] ThrusterSoundHolders;
    [SerializeField] public AudioSource[] ThrusterSources;
    [SerializeField] public RuntimeAnimatorController animatorController;
    [SerializeField] public ParticleSystem successParticles;
    [SerializeField] public AudioClip successSound;
    [SerializeField] public ParticleSystem deathParticles;
    [SerializeField] public AudioClip deathSound;

    [SerializeField] public PlayerSkin chosenSkin;
    [SerializeField] public SkinnedMeshRenderer[] butterySkinnedMeshRenderers;
    [SerializeField] public AudioClip[] borks;

    [SerializeField] ParticleSystem leftPlus;
    [SerializeField] ParticleSystem rightPlus;
    [SerializeField] ParticleSystem leftMinus;
    [SerializeField] ParticleSystem rightMinus;

    [Header("Null Objects to prevent the system from being a quitter.")]
    [SerializeField] ParticleSystem nullParticleSystem;

    [Header("The default skin object, for non-default skins.")]
    GameObject defaultSkinObj;
    GameObject premiumSkinObj;

    void Awake()
    {
        //Run this before it makes sense to just in case the preset assets get uppity.
        StopRocketSounds();
        TurnOffThrusterLights();
    }

    void Start()
    {
        ThrusterSources = new AudioSource[2] { ThrusterSoundHolders[0].GetComponent<AudioSource>(), ThrusterSoundHolders[1].GetComponent<AudioSource>() };
        //We need to set the player's sound to an appropriate volume...
        
    }

    public PlayerSkin GetIndividualSkinAsset(int skindex)
    {
        return PlayerSkins[skindex];
    }

    public void ApplySkin(int skindex) //Applies to most skins.
    {
        defaultSkinObj = GameObject.Find("DefaultGameplayCorgiAnims");
        chosenSkin = new PlayerSkin();
        chosenSkin = PlayerSkins[skindex];
        if (!chosenSkin.isPremiumSkin)
        {
            ApplyNonPremiumSkin(skindex);
            return;
        }
        if (chosenSkin.isPremiumSkin & !chosenSkin.is2DSkin) //should only apply to the 3D premium skins.
        {
            DoPremiumSkinStuff(chosenSkin.PremiumSkinColliderCoords, chosenSkin.PremiumSkinColliderRadii, chosenSkin.PremiumSkinColliderHeights);
            return;
        }
        if (chosenSkin.isPremiumSkin & chosenSkin.is2DSkin) //Should only apply to 8 bit corg.
        {
            //ApplyPremium2DSkin();
            //return;
        }

    }
    void Apply8BitSkin()
    {
        Vector3[] centers = new Vector3[] {
            Vector3.zero,
            new Vector3(-0.08f, 0f,0f),
            new Vector3(0.16f,0f,0f),
            Vector3.zero,
            Vector3.zero,
            new Vector3(-0.16f,0f,0f),
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            new Vector3(-0.3f,-0.12f,0f),
            Vector3.zero,
            new Vector3(-0.1f,0f,0f),
            Vector3.zero,
            Vector3.zero,
            new Vector3(-0.32f,0f,0f)
         };
        float[] radii = new float[] {
            0.38f,
            0.39f,
            0.15f,
            0.14f,
            0.14f,
            0.15f,
            0.05f,
            0.05f,
            0.05f,
            0.05f,
            0.52f,
            0.35f,
            0.5f,
            0.28f,
            0.28f,
            0.64f
         };
        float[] heights = new float[] {
            1f,
            1.17f,
            0.7f,
            0.74f,
            0.74f,
            0.7f,
            0.05f,
            0.42f,
            0.42f,
            0.42f,
            0f,
            1f,
            1.17f,
            0.7f,
            0.7f,
            1.19f
        };
        DoPremiumSkinStuff(centers, radii, heights);
    }
    void ApplyButterStickSkin()
    {
        //DoPremiumSkinStuff();
    }
    void ApplyTripppSkin()
    {
        Vector3[] centers = new Vector3[] {
            Vector3.zero,
            new Vector3(-0.08f, 0f,0f),
            new Vector3(0.16f,0f,0f),
            Vector3.zero,
            Vector3.zero,
            new Vector3(-0.16f,0f,0f),
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            Vector3.zero,
            new Vector3(-0.3f,-0.12f,0f),
            Vector3.zero,
            new Vector3(-0.1f,0f,0f),
            Vector3.zero,
            Vector3.zero,
            new Vector3(-0.32f,0f,0f)
         };
        float[] radii = new float[] {
            0.38f,
            0.39f,
            0.15f,
            0.14f,
            0.14f,
            0.15f,
            0.05f,
            0.05f,
            0.05f,
            0.05f,
            0.52f,
            0.35f,
            0.5f,
            0.28f,
            0.28f,
            0.64f
         };
        float[] heights = new float[] {
            1f,
            1.17f,
            0.7f,
            0.74f,
            0.74f,
            0.7f,
            0.05f,
            0.42f,
            0.42f,
            0.42f,
            0f,
            1f,
            1.17f,
            0.7f,
            0.7f,
            1.19f
        };
        DoPremiumSkinStuff(centers, radii, heights);
        //Don't forget to shrink the back right knee to 0.
        //Don't forget to reapply the correct material.
    }
    private void DoPremiumSkinStuff(Vector3[] centers, float[] radii, float[] heights)
    {
        //Determining the workspace.
        GameObject spawnParent = defaultSkinObj.transform.parent.gameObject;
        //Doing the main replacement.
        DestroyDefaultCorgi();
        premiumSkinObj = Instantiate(chosenSkin.CharacterOverride, spawnParent.transform);
        //nab the important joints
        GameObject[] importantJoints = premiumSkinObj.GetComponent<PremiumSkinJointList>().colliderJoints;
        //Reattach the reverse rotation and ensure correct compensation axis.

        BodyReverseRot brr = GameObject.FindAnyObjectByType<BodyReverseRot>();
        brr.affectAxis = BodyReverseRot.AffectAxis.X;
        if (player.saveManager.collectibleData.CurrentSkin == 24) //ferret is different.
        {
            brr.offset = -90f;
        } else {
            brr.offset = 90f;
        }
        //Reassign and activate the parent constraint for the new dog.
            Helper.ReassignParentConstraint(premiumSkinObj, brr.gameObject);

         //Memorial skin stuff as applicable.
        DoMemorialSkinStuff();


        GameObject[] colliderJoints = premiumSkinObj.GetComponent<PremiumSkinJointList>().colliderJoints;
        //This is moving the jetpack into place.
        Helper.ReassignPositionConstraint(leftJetpackParentObj, colliderJoints[12]);
        Helper.ReassignPositionConstraint(rightJetpackParentObj, colliderJoints[12]);
        //This is moving all of the colliders into place.
        ReattachCollidersToPremiumSkin(player.CollidersAndTriggers,colliderJoints);
        ResizeCollidersAndTriggers(player.CollidersAndTriggers, centers, radii, heights);

        //Adding in the normal skin stuff.
        DoNormalSkinStuff();
        player.anim = premiumSkinObj.GetComponent<Animator>();


    }
    void ApplyNonPremiumSkin(int skindex)
    {
        //Holster Mesh
        if (chosenSkin.JetpackHolsterMesh != null)
        {
            jetpackHolsterHolder = InstantiateSkinItem(chosenSkin.JetpackHolsterMesh, jetpackHolsterHolder.transform);
        }

        //Activate the skin.
        if (Costumes[skindex] != null)
        {
            Costumes[skindex].SetActive(true);
        }

        DoNormalSkinStuff();

    }
    private void DoMemorialSkinStuff()
    {
        if (!chosenSkin.isMemorialSkin) { return; }
        //Create the halo. Wings are handled in DoNormalSkinStuff.
        GameObject HaloObj = InstantiateSkinItem(chosenSkin.memorialHaloModel, player.CollidersAndTriggers[10].transform);
        HaloObj.transform.localPosition = new Vector3(-.5f, 0.5f, 0f);
    }

    private void DoNormalSkinStuff()
    {

        //Left jetpack mesh
        if (chosenSkin.LeftJetpackMesh != null & !chosenSkin.combinedJetpack){
            MainThrusterParticleHolders[0] = InstantiateSkinItem(chosenSkin.LeftJetpackMesh, leftJetpackParentObj.transform);
        }
        //Right jetpack mesh
        if (chosenSkin.RightJetpackMesh != null & !chosenSkin.combinedJetpack){
            rightJetpackModelHolder = InstantiateSkinItem(chosenSkin.RightJetpackMesh, rightJetpackParentObj.transform);
        }
        //Combined jetpack mesh, if applicable
        if (chosenSkin.CombinedJetpackMeshOverride != null & chosenSkin.combinedJetpack){
            combinedJetpackModelHolder = InstantiateSkinItem(chosenSkin.CombinedJetpackMeshOverride, combinedJetpackParentObj.transform);
        }

        //Thruster particles
        if (chosenSkin.MainThrusterParticles[0] != null)
        {
            try
            {
                MainThrusterParticleHolders[0] = InstantiateSkinItem(chosenSkin.MainThrusterParticles[0], MainThrusterParticleHolders[0].transform);
            }
            catch { }
        }
        if (chosenSkin.MainThrusterParticles[1] != null){
            try{
                MainThrusterParticleHolders[1] = InstantiateSkinItem(chosenSkin.MainThrusterParticles[1], MainThrusterParticleHolders[1].transform);
            }
            catch { }
        }

        // plus thruster particles
        if (chosenSkin.PlusThrusterParticles[0] != null){
            PlusThrusterParticleHolders[0] = InstantiateSkinItem(chosenSkin.PlusThrusterParticles[0], PlusThrusterParticleHolders[0].transform);
        }
        if (chosenSkin.PlusThrusterParticles[1] != null){
            PlusThrusterParticleHolders[1] = InstantiateSkinItem(chosenSkin.PlusThrusterParticles[1], PlusThrusterParticleHolders[1].transform);
        }
        // minus thruster particles
        if (chosenSkin.MinusThrusterParticles[0] != null){
            MinusThrusterParticleHolders[0] = InstantiateSkinItem(chosenSkin.MinusThrusterParticles[0], MinusThrusterParticleHolders[0].transform);
        }
        if (chosenSkin.MinusThrusterParticles[1] != null){
            MinusThrusterParticleHolders[1] = InstantiateSkinItem(chosenSkin.MinusThrusterParticles[1], MinusThrusterParticleHolders[1].transform);
        }

        //thruster lights
        if (chosenSkin.ThrusterLights[0] != null){
            ThrusterLightHolders[0] = InstantiateSkinItem(chosenSkin.ThrusterLights[0].gameObject, ThrusterLightHolders[0].transform);
        }
        if (chosenSkin.ThrusterLights[1] != null){
            ThrusterLightHolders[1] = InstantiateSkinItem(chosenSkin.ThrusterLights[1].gameObject, ThrusterLightHolders[1].transform);
        }

        //thruster sounds
        if (chosenSkin.ThrusterSounds[0] != null){
            ThrusterSoundHolders[0].GetComponent<AudioSource>().clip = chosenSkin.ThrusterSounds[0];
        }
        if (chosenSkin.ThrusterSounds[1] != null)
{
            ThrusterSoundHolders[1].GetComponent<AudioSource>().clip = chosenSkin.ThrusterSounds[1];
        }
        StopRocketSounds();
        //explosion sound
        if (chosenSkin.explosionSound != null)
        {
            deathSound = chosenSkin.explosionSound;
        }
        //success sound
        if (chosenSkin.successSound != null)
        {
            successSound = chosenSkin.successSound;
        }
        //anim override
        if (chosenSkin.AnimOverride != null)
        {
            animatorController = chosenSkin.AnimOverride;
        }


        //success particles
        if (chosenSkin.successParticles != null)
        {
            successParticles = chosenSkin.successParticles;
        }
        //explosion particles
        if (chosenSkin.explosionParticles != null)
        {
            deathParticles = chosenSkin.explosionParticles;
        }
        //Collecting all skinned Mesh Renderers for Ball Mode.
        NabAllSkinnedMeshRenderers();

        //Runtime Animation Controller
        if (chosenSkin.AnimOverride != null)
        {
            player.anim.runtimeAnimatorController = chosenSkin.AnimOverride;
        }

        //Plug in the borks
        if (chosenSkin.barkSounds.Length > 0)
        {
            borks = chosenSkin.barkSounds;
        }

        return; //Testing this one level at a time.
                //explosion light
    }

    void DestroyDefaultCorgi()
    {
        Destroy(GameObject.Find("DefaultGameplayCorgiAnims"));
    }

    void ReattachCollidersToPremiumSkin(GameObject[] inputGOA, GameObject[] newAttachmentObjs)
    {
        //It's a 1:1 attachment up until we get to the damage objs. Then it becomes tricky. Buckle in.
        Helper.ReassignParentConstraint(inputGOA[0], newAttachmentObjs[0]);
        Helper.ReassignParentConstraint(inputGOA[1], newAttachmentObjs[1]);
        Helper.ReassignParentConstraint(inputGOA[2], newAttachmentObjs[2]);
        Helper.ReassignParentConstraint(inputGOA[3], newAttachmentObjs[3]);
        Helper.ReassignParentConstraint(inputGOA[4], newAttachmentObjs[4]);
        Helper.ReassignParentConstraint(inputGOA[5], newAttachmentObjs[5]);
        Helper.ReassignParentConstraint(inputGOA[6], newAttachmentObjs[6]);
        Helper.ReassignParentConstraint(inputGOA[7], newAttachmentObjs[7]);
        Helper.ReassignParentConstraint(inputGOA[8], newAttachmentObjs[8]);
        Helper.ReassignParentConstraint(inputGOA[9], newAttachmentObjs[9]);
        Helper.ReassignParentConstraint(inputGOA[10], newAttachmentObjs[10]);
        //Starting on the damage triggers.
        Helper.ReassignParentConstraint(inputGOA[11], newAttachmentObjs[11]);
        Helper.ReassignParentConstraint(inputGOA[12], newAttachmentObjs[12]);
        Helper.ReassignParentConstraint(inputGOA[13], newAttachmentObjs[13], newAttachmentObjs[14]);
        Helper.ReassignParentConstraint(inputGOA[14], newAttachmentObjs[15], newAttachmentObjs[16]);
        Helper.ReassignParentConstraint(inputGOA[15], newAttachmentObjs[17]);
    }
    void ResizeCollidersAndTriggers(GameObject[] colliders, Vector3[] centers, float[] radii, float[] heights)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            CapsuleCollider activeCollider = colliders[i].GetComponent<CapsuleCollider>();
            activeCollider.center = centers[i];
            activeCollider.radius = radii[i];
            activeCollider.height = heights[i];
        }
    }

    GameObject InstantiateSkinItem(GameObject inputObj, Transform parentTransform)
    { //has a return because it replaces the placeholder object.
        GameObject tempObj = new GameObject();
        tempObj = Instantiate(inputObj);
        tempObj.transform.parent = parentTransform;
        tempObj.transform.localPosition = inputObj.transform.localPosition;

        return tempObj;
    }

    public void StartRocketSounds()
    {
        if (!ThrusterSoundHolders[0].GetComponent<AudioSource>().isPlaying){
            for (int i = 0; i < ThrusterSoundHolders.Length; i++){
                ThrusterSoundHolders[i].GetComponent<AudioSource>().Play();
            }
        }
    }
    public void StopRocketSounds()
    {
        ThrusterSoundHolders[0].GetComponent<AudioSource>().Stop();
        ThrusterSoundHolders[1].GetComponent<AudioSource>().Stop();
    }
    public float GetThrusterVolume()
    {
        return ThrusterSources[0].volume;
    }
    public void SetThrusterVolume(float vol)
    {
        ThrusterSources[0].volume = vol;
        ThrusterSources[1].volume = vol;
    }
    public bool GetThrusterAudioStatus()
    {
        return ThrusterSources[0].isPlaying;
    }
    public void TurnOffThrusterLights()
    {
        for (int a = 0; a < ThrusterLightHolders.Length; a++)
        {
            ThrusterLightHolders[a].GetComponent<Light>().intensity = 0.0f;
        }
    }
    public void SetThrusterLightIntensity(float intensity)
    {
        for (int a = 0; a < ThrusterLightHolders.Length; a++)
        {
            ThrusterLightHolders[a].GetComponent<Light>().intensity = intensity;
        }
    }
    public float GetThrusterLightIntensity()
    {
        return ThrusterLightHolders[0].GetComponent<Light>().intensity;
    }

    public void StartPlusRotParticles()
    {
        if (leftPlus == null | rightPlus == null)
        {
            leftPlus = PlusThrusterParticleHolders[0].GetComponent<ParticleSystem>();
            rightPlus = PlusThrusterParticleHolders[1].GetComponent<ParticleSystem>();
        }
        if (!leftPlus.isEmitting | !rightPlus.isEmitting)
        {
            leftPlus.Play();
            rightPlus.Play();
        }
    }
    public void StartMinusRotParticles()
    {
        if (leftMinus == null | leftPlus == null)
        {
            leftMinus = MinusThrusterParticleHolders[0].GetComponent<ParticleSystem>();
            rightMinus = MinusThrusterParticleHolders[1].GetComponent<ParticleSystem>();
        }
        if (!leftMinus.isEmitting | !rightMinus.isEmitting)
        {
            leftMinus.Play();
            rightMinus.Play();
        }
    }
    public void StopAllRotParticles()
    {
        if (leftPlus == null | rightPlus == null)
        {
            leftPlus = PlusThrusterParticleHolders[0].GetComponent<ParticleSystem>();
            rightPlus = PlusThrusterParticleHolders[1].GetComponent<ParticleSystem>();
        }
        if (leftMinus == null | leftPlus == null)
        {
            leftMinus = MinusThrusterParticleHolders[0].GetComponent<ParticleSystem>();
            rightMinus = MinusThrusterParticleHolders[1].GetComponent<ParticleSystem>();
        }
        leftPlus.Stop();
        leftMinus.Stop();
        rightPlus.Stop();
        rightMinus.Stop();
    }
    public void StartPrimaryThrusters()
    {
        if (!MainThrusterParticleHolders[0].GetComponent<ParticleSystem>().isEmitting | !MainThrusterParticleHolders[1].GetComponent<ParticleSystem>().isEmitting)
        {
            MainThrusterParticleHolders[0].GetComponent<ParticleSystem>().Play();
            MainThrusterParticleHolders[1].GetComponent<ParticleSystem>().Play();
            SetThrusterLightIntensity(2.5f);
        }
    }
    public void StopPrimaryThrusters()
    {
        MainThrusterParticleHolders[0].GetComponent<ParticleSystem>().Stop();
        MainThrusterParticleHolders[1].GetComponent<ParticleSystem>().Stop();
        TurnOffThrusterLights();
    }
    public void HideCorgi()
    {
        //for(int a=0;a<PrimaryMeshes.Count;a++){
        //for(int b=0;b<PrimaryMeshes[a].Length;b++){
        //PrimaryMeshes[a][b].enabled = false;
        // }
        //}
        // for(int a=0;a<SecondaryMeshes.Count;a++){
        //     for(int b=0;b<SecondaryMeshes[a].Length;b++){
        //        SecondaryMeshes[a][b].enabled = false;
        //    }
        // }

    }
    void NabAllSkinnedMeshRenderers()
    {
        butterySkinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    public void BallEffectRunner()
    {
        Color color = new Color (Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
        foreach(SkinnedMeshRenderer pm in butterySkinnedMeshRenderers){
            pm.material.SetColor("_EmissionColor",color);
        }
    }
    public void BallEffectCanceler()
    {
        foreach(SkinnedMeshRenderer pm in butterySkinnedMeshRenderers){
            try
            {
                pm.material.SetColor("_EmissionColor", Color.black);
            }
            catch
            {
                
            }
        }
    }
}
