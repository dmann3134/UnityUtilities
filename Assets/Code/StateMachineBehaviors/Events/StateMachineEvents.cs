using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class StateMachineEvent
{
    public string Name;
    [HideInInspector]
    public bool hasBeenFired = false;
    [Range(0, 1)]
    public float eventTime;

    public virtual void Reset()
    {

    }

    public virtual void Fire()
    {
        hasBeenFired = true;
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

