using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ParticlePause : ParticleModule
{
    [Range(0, 1)]
    public float PauseNormalizedTime = .97f;
    private float startTime = 0;
    public float endTime = 0;
    public bool isPaused = false;

    public List<ParticleSystem> ParticlesToPauseOverride = new List<ParticleSystem>();

    public override void Start(ParticleUtilities utility)
    {
        base.Start(utility);

        endTime = (particleUtility.rootParticleSystem.main.startLifetimeMultiplier * PauseNormalizedTime) + particleUtility.rootParticleSystem.main.startDelayMultiplier;
        startTime = Time.time;
    }

    public void Update()
    {
        if (Time.time >= startTime + endTime)
        {
            if (ParticlesToPauseOverride.Count > 0)
            {
                foreach (ParticleSystem particle in ParticlesToPauseOverride)
                {
                    particle.Pause(false);
                }
            }
            else
            {
                particleUtility.rootParticleSystem.Pause(true);
            }
            isPaused = true;
        }
    }
}
