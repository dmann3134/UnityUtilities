using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class ParticleTrailRenderers : ParticleModule
{ 
    public List<ParticleTrailRenderer> allTrailRenderers = new List<ParticleTrailRenderer>();

    public override void Start(ParticleUtilities utility)
    {
        base.Start(utility);

        var trails = particleUtility.GetComponentsInChildren<TrailRenderer>(true);
        foreach (TrailRenderer trail in trails)
        {
            allTrailRenderers.Add(new ParticleTrailRenderer(trail));
        }
    }

    public IEnumerator DisableTrailRendererRoutine()
    {
        foreach (ParticleTrailRenderer trailRenderer in allTrailRenderers)
        {
            //unparent
            trailRenderer.trailRenderer.transform.parent = null;

            //wait until the newest one has faded out
            yield return new WaitForSeconds(trailRenderer.trailRenderer.time);

            //call method for cleanup
            trailRenderer.Recycle();
        }
    }
}

public class ParticleTrailRenderer
{
    public Transform CachedParent;
    public Vector3 CachedOffset;
    public TrailRenderer trailRenderer;
    public Coroutine DisableRoutine;
    public bool isActive = false;

    public ParticleTrailRenderer(TrailRenderer trail)
    {
        trailRenderer = trail;
        CachedParent = trailRenderer.transform.parent;
        CachedOffset = trailRenderer.transform.localPosition;
    }

    public void Recycle()
    {
        //disable
        trailRenderer.enabled = false;

        //set up to be used
        isActive = false;

        trailRenderer.Clear();
        //reparent with cached offset
        trailRenderer.transform.parent = CachedParent;
        trailRenderer.transform.localPosition = CachedOffset;
        trailRenderer.Clear();
    }

    public void Enable()
    {
        Recycle();
        trailRenderer.enabled = true;
        isActive = true;
    }
}