using UnityEngine;

public class Nourriture : MonoBehaviour
{
    public NourritureConf nourritureConf;

    private void OnDestroy()
    {
        if (nourritureConf == null)
        {
            return;
        }

        if (Random.value > nourritureConf.probabiliteEmpoisonnee)
        {
            return;
        }

        if (nourritureConf.effetExplosionPrefab != null)
        {
            Instantiate(nourritureConf.effetExplosionPrefab, transform.position, Quaternion.identity);
        }

        var colliders = Physics.OverlapSphere(transform.position, nourritureConf.rayonExplosion);
        for (int i = 0; i < colliders.Length; i++)
        {
            var col = colliders[i];
            var rb = col.attachedRigidbody != null ? col.attachedRigidbody : col.GetComponentInParent<Rigidbody>();
            if (rb == null)
            {
                continue;
            }
            print("explosion !!!!!!");
            rb.AddExplosionForce(
                nourritureConf.forceExplosion,
                transform.position,
                nourritureConf.rayonExplosion,
                nourritureConf.upwardsModifier,
                ForceMode.Impulse);
        }
    }
}
