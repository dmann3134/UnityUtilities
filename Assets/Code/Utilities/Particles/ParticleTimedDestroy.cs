using UnityEngine;
using System.Collections;

[System.Serializable]
public class ParticleTimedDestroy : ParticleModule
{
    public override void Start(ParticleUtilities utility)
    {
        base.Start(utility);
    }

    public IEnumerator DestroyParticles(float time)
    {
        yield return new WaitForSeconds(time);
        ParticlePooledObject obj = particleUtility.rootParticleSystem.GetComponent<ParticlePooledObject>();
        if (obj)
        {
            obj.ResetParticles();
        }
        else
        {
            GameObject.Destroy(particleUtility.rootParticleSystem.gameObject);
        }
    }
}
