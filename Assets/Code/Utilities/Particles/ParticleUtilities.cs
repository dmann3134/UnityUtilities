using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class ParticleUtilities : MonoBehaviour
{
    [HideInInspector]
    public ParticleSystem rootParticleSystem;
    public List<ParticleSystem> allParticleSystems = new List<ParticleSystem>();

    public ParticlePooledObject particlePooledObject;

    private List<Coroutine> CoroutinesToDisable = new List<Coroutine>();

    private bool RunUpdate = false;

    //auto destroy settings
    [Space(5f)]
    [Header("Auto Destroy")]
    [Tooltip("This module will auto destroy the particles when they are not active or if they are ")]
    public bool useAutoDestroy;
    public ParticleAutoDestroy autoDestroy;

    //Time Destory Settings
    [Space(5f)]
    [Header("Timed Destroy")]
    [Tooltip("This module will Destroy the particle system after a delay")]
    public bool useTimedDestroy;
    public ParticleTimedDestroy timedDestroy;


    //pause settings
    [Space(5f)]
    [Header("Particle Pausing")]
    [Tooltip("This module will pause the particle system at a normalized time based off of the root particle systems start lifetime")]
    public bool useParticlePause;
    public ParticlePause pause;

    //RendererEmission
    [Space(5f)]
    [Header("Renderer Emission")]
    [Tooltip("This module will set up skinned mesh renderer emission for the parented target")]
    public bool useBodyRendererEmission;
    public ParticleBodyRenderer bodyRenderer;

    [Space(5f)]
    [Header("Timed Stop")]
    [Tooltip("This module will pause the particle system at a normalized time based off of the root particle systems start lifetime")]
    public bool useTimedStop;
    public int timeToStop;

    [Space(5f)]
    [Header("Timed Unparent")]
    [Tooltip("This module will pause the particle system at a normalized time based off of the root particle systems start lifetime")]
    public bool useTimedUnparent;
    public int timeToUnparent;


    //Lights
    public ParticleLights lights;

    //Trail Renderer
    public ParticleTrailRenderers trailRenderers;

    // Use this for initialization
    public void Awake()
    {
        particlePooledObject = GetComponent<ParticlePooledObject>();

        rootParticleSystem = GetComponentInChildren<ParticleSystem>();

        //setup lights
        lights.Start(this);

        //initialize trail renderer
        trailRenderers.Start(this);
    }

    public void Start()
    {
        Restart();
    }

    public void Restart()
    {
        //auto destroy module
        if (useAutoDestroy)
        {
            autoDestroy.Start(this);
        }

        //pause module
        if (useParticlePause)
        {
            pause.Start(this);
        }

        //body renderer module
        if (useBodyRendererEmission)
        {
            bodyRenderer.SetupBodyRendererEmission();
        }

        //timed destroy module
        if (useTimedDestroy)
        {
            timedDestroy.Start(this);
        }

        //timed stop routine
        if (useTimedStop)
        {
            CoroutinesToDisable.Add(StartCoroutine(StopParticleRoutine(timeToStop)));
        }

        //timed unparent routine
        if (useTimedUnparent)
        {
            CoroutinesToDisable.Add(StartCoroutine(UnparentParticleRoutine(timeToUnparent)));
        }

        //start playing the system
        if (rootParticleSystem)
        {
            rootParticleSystem.Play(true);
        }

        //trail renderer enabling
        EnableTrailRenderers();

        //reset enabling
        if (enabled == false)
        {
            enabled = true;
        }

        RunUpdate = true;
    }

    public void DoNotRunUpdate()
    {
        RunUpdate = false;
        if (useBodyRendererEmission)
        {
            bodyRenderer.RemoveRendererEmissions();
        }
        for (int i = 0; i < CoroutinesToDisable.Count; ++i)
        {
            StopCoroutine(CoroutinesToDisable[i]);
        }
        CoroutinesToDisable.Clear();
    }

    // Update is called once per frame
    public void Update()
    {
        if (RunUpdate == false) { return; }
        if (useAutoDestroy)
        {
            autoDestroy.Update();
        }

        if (useParticlePause)
        {
            pause.Update();
        }
    }

    private void OnDisable()
    {
        if (useBodyRendererEmission)
        {
            bodyRenderer.RemoveRendererEmissions();
        }
        StopAllCoroutines();
    }

    public void StopParticles()
    {
        rootParticleSystem.Stop(true);
    }

    public void PlayParticles()
    {
        rootParticleSystem.Play(true);
    }

    public IEnumerator StopParticleRoutine(float particleDelay = 0, float lightDelay = 2f)
    {
        yield return new WaitForSeconds(particleDelay);
        StopParticles();
        StopLights(lightDelay);
    }

    public IEnumerator UnparentParticleRoutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        transform.parent = null;
    }

    public void StopLights(float endTime)
    {
        CoroutinesToDisable.Add(StartCoroutine(lights.StopLightsRoutine(endTime)));
    }

    public void FadeOutParticlesAndLights(float particleDelay = 0, float lightDelay = 2f)
    {
        CoroutinesToDisable.Add(StartCoroutine(StopParticleRoutine(particleDelay, lightDelay)));
    }

    public void EnableTrailRenderers()
    {
        foreach (ParticleTrailRenderer trail in trailRenderers.allTrailRenderers)
        {
            //do this here because ParticleTrailRenderer is not a Monobehaviour
            if (trail.DisableRoutine != null)
            {
                StopCoroutine(trail.DisableRoutine);
            }

            //enable the trailrenderer
            trail.Enable();
        }
    }

    public void DisableTrailRenderers()
    {
        foreach (ParticleTrailRenderer trail in trailRenderers.allTrailRenderers)
        {
            //do this here because ParticleTrailRenderer is not a Monobehaviour
            trail.DisableRoutine = StartCoroutine(trailRenderers.DisableTrailRendererRoutine());
        }
    }
}

