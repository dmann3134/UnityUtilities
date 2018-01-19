using UnityEngine;
using System.Collections;

[System.Serializable]
public class ParticleAutoDestroy : ParticleModule
{
    private ParticleSystem longestLastingSystem;

    public override void Start(ParticleUtilities utility)
    {
        base.Start(utility);

        float longestDuration = -1; // there better not be a duration of negative time
        for (int i = 0; i < particleUtility.allParticleSystems.Count; ++i)
        { // look through all the particle system in children
            if (particleUtility.allParticleSystems[i].main.loop == false && particleUtility.allParticleSystems[i].main.duration > longestDuration)
            { // if a system is not looping and has a duration longer than the current one then set that as longest
                longestDuration = particleUtility.allParticleSystems[i].main.duration;
                longestLastingSystem = particleUtility.allParticleSystems[i];
            }
        }
    }

    public void Update()
    {
        if (particleUtility.rootParticleSystem && !particleUtility.rootParticleSystem.IsAlive(true))
        {
            if (particleUtility.particlePooledObject)
            {
                particleUtility.particlePooledObject.ReturnToPool();
            }
            else
            {
                GameObject.Destroy(particleUtility.rootParticleSystem.gameObject);
            }
        }
    }
}