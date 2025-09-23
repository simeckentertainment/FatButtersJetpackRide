using UnityEngine;

[CreateAssetMenu(fileName = "New Player Skin", menuName = "Fatbutters/New Player Skin")]
public class PlayerSkin : ScriptableObject
{
    [SerializeField] public bool isPremiumSkin;
    [SerializeField] public bool isMemorialSkin;
    [SerializeField] public Vector3 memorialWingsCoords;
    [SerializeField] public bool is2DSkin;
    [SerializeField] public GameObject memorialHaloModel;
    [SerializeField] public GameObject CharacterOverride;
    [SerializeField] public GameObject JetpackHolsterMesh;
    [SerializeField] public bool combinedJetpack;
    [SerializeField] public GameObject LeftJetpackMesh;
    [SerializeField] public GameObject RightJetpackMesh;
    [SerializeField] public GameObject CombinedJetpackMeshOverride;
    [SerializeField] public GameObject[] MainThrusterParticles;
    [SerializeField] public GameObject[] PlusThrusterParticles;
    [SerializeField] public GameObject[] MinusThrusterParticles;
    [SerializeField] public Light[] ThrusterLights;
    [SerializeField] public AudioClip[] ThrusterSounds;
    [SerializeField] public RuntimeAnimatorController AnimOverride;
    [SerializeField] public AudioClip explosionSound;
    [SerializeField] public AudioClip successSound;
    [SerializeField] public ParticleSystem successParticles;
    [SerializeField] public ParticleSystem explosionParticles;
    [SerializeField] public Light explosionLight;
    [SerializeField] public AudioClip[] barkSounds;
    [SerializeField] public Vector3[] PremiumSkinColliderCoords;
    [SerializeField] public float[] PremiumSkinColliderRadii;
    [SerializeField] public float[] PremiumSkinColliderHeights;
}
