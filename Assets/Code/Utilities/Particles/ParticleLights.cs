using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class ParticleLights : ParticleModule
{

    public List<ParticleLight> allLights;

    public override void Start(ParticleUtilities utility)
    {
        base.Start(utility);

        var lights = particleUtility.GetComponentsInChildren<Light>();
        foreach (Light light in lights)
        {
            allLights.Add(new ParticleLight(light));
        }
    }

    public IEnumerator StopLightsRoutine(float endTime)
    {
        //set all the intensities
        foreach (ParticleLight light in allLights)
        {
            light.startIntensity = light.light.intensity;
        }

        float ElapsedTime = 0.0f;
        while (ElapsedTime < endTime)
        {
            foreach (ParticleLight light in allLights)
            {
                light.light.intensity = Mathf.Lerp(light.startIntensity, 0, ElapsedTime / endTime);
            }
            ElapsedTime += Time.deltaTime;
            yield return null;
        }

        if (particleUtility.particlePooledObject)
        {
            particleUtility.particlePooledObject.ResetParticles();
        }
        else
        {
            GameObject.Destroy(particleUtility.rootParticleSystem.gameObject);
        }
    }
}

[Serializable]
public class ParticleLight
{
    public ParticleLight(Light pLight)
    {
        light = pLight;
        startIntensity = pLight.intensity;
    }

    public Light light;
    public float startIntensity;
}


