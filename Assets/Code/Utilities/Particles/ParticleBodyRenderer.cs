using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class ParticleBodyRenderer : ParticleModule
{
    public override void Start(ParticleUtilities utility)
    {
        base.Start(utility);
    }

    public void SetupBodyRendererEmission()
    {
        Actor actor = particleUtility.transform.GetComponentInParent<Actor>();
    }

    public void RemoveRendererEmissions()
    {
        foreach (ParticleSystem ps in particleUtility.allParticleSystems)
        {
            if (ps.shape.shapeType == ParticleSystemShapeType.SkinnedMeshRenderer)
            {
                var newShape = ps.shape;
                newShape.skinnedMeshRenderer = null;
            }
        }
    }
}
