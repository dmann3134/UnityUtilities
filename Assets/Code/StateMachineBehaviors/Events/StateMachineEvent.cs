using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class StateEvent
{
    [HideInInspector]
    public bool hasBeenFired = false;
    [Range(0, 1)]
    public float eventTime;

    public List<ParticleEvent> ParticleEvents = new List<ParticleEvent>();
    public List<AudioEvent> AudioEvents = new List<AudioEvent>();
    public List<FunctionEvent> FunctionEvents = new List<FunctionEvent>();

    public virtual void Reset()
    {
        foreach(ParticleEvent pEvent in ParticleEvents)
        {
            pEvent.Reset();
        }
        foreach(AudioEvent aEvent in AudioEvents)
        {
            aEvent.Reset();
        }
        foreach (FunctionEvent fEvent in FunctionEvents)
        {
            fEvent.Reset();
        }
    }

    public virtual void Fire()
    {
        foreach (ParticleEvent pEvent in ParticleEvents)
        {
            pEvent.Fire();
        }
        foreach (AudioEvent aEvent in AudioEvents)
        {
            aEvent.Fire();
        }
        foreach (FunctionEvent fEvent in FunctionEvents)
        {
            fEvent.Fire();
        }
    }

    public virtual void Initialize()
    {
        foreach (ParticleEvent pEvent in ParticleEvents)
        {
            pEvent.Initialize();
        }
        foreach (AudioEvent aEvent in AudioEvents)
        {
            aEvent.Initialize();
        }
        foreach (FunctionEvent fEvent in FunctionEvents)
        {
            fEvent.Initialize();
        }
    }
}

[Serializable]
public class StateMachineEvent
{
    public string Name;
    [HideInInspector]

    public virtual void Reset()
    {

    }

    public virtual void Fire()
    {

    }

    public virtual void Pool()
    {

    }

    public virtual void Initialize()
    {
        Pool();
    }
}

[Serializable]
public class ParticleEvent : StateMachineEvent
{
    public override void Reset()
    {
        base.Reset();
    }

    public override void Fire()
    {
        base.Fire();
    }

    public override void Pool()
    {
        base.Pool();
    }
}

[Serializable]
public class AudioEvent : StateMachineEvent
{
    public override void Reset()
    {
        base.Reset();
    }

    public override void Fire()
    {
        base.Fire();
    }

    public override void Pool()
    {
        base.Pool();
    }
}


[Serializable]
public class FunctionEvent : StateMachineEvent
{
    public override void Reset()
    {
        base.Reset();
    }

    public override void Fire()
    {
        base.Fire();
    }

    public override void Pool()
    {
        base.Pool();
    }
}
