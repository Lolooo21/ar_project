using UnityEngine;

[CreateAssetMenu(fileName = "NourritureConf", menuName = "Scriptable Objects/NourritureConf")]
public class NourritureConf : ScriptableObject
{
    [Header("Empoisonnement")]
    [Range(0f, 1f)]
    public float probabiliteEmpoisonnee = 0.2f;

    [Header("Explosion")]
    public float rayonExplosion = 2f;
    public float forceExplosion = 8f;
    public float upwardsModifier = 0.5f;
    public GameObject effetExplosionPrefab;
}
