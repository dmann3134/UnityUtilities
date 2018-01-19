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
    foreach (ParticleEvent pEvent in ParticleEvents)
    {
      pEvent.Reset();
    }
    foreach (AudioEvent aEvent in AudioEvents)
    {
      aEvent.Reset();
    }
    foreach (FunctionEvent fEvent in FunctionEvents)
    {
      fEvent.Reset();
    }

    hasBeenFired = false;
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

    hasBeenFired = true;
  }

  public virtual void Initialize(Actor actor = null, Animator animator = null)
  {
    foreach (ParticleEvent pEvent in ParticleEvents)
    {
      pEvent.Initialize(actor, animator);
    }
    foreach (AudioEvent aEvent in AudioEvents)
    {
      aEvent.Initialize(actor, animator);
    }
    foreach (FunctionEvent fEvent in FunctionEvents)
    {
      fEvent.Initialize(actor, animator);
    }
  }
}

[Serializable]
public class StateMachineEvent
{
  public string Name;

  [HideInInspector]
  public Actor actor;
  [HideInInspector]
  public Animator animator;

  public virtual void Reset()
  {

  }

  public virtual void Fire()
  {

  }

  public virtual void Pool()
  {

  }

  public virtual void Initialize(Actor myActor = null, Animator myAnimator = null)
  {
    actor = myActor;
    animator = myAnimator;
    Pool();
  }
}

[Serializable]
public class ParticleEvent : StateMachineEvent
{
  public GameObject particlePrefab;
  private ParticlePooledObject particlePooledObject;
  private GameObject spawnedParticle;

  public StateMachineParticleSpawnInfo SpawnInfo;


  public override void Reset()
  {
    base.Reset();
  }

  public override void Fire()
  {
    //make sure you have a source
    if (particlePrefab)
    {
      SpawnInfo.SetParticleSpawnInfoValues(particlePrefab, actor, animator);

      //if it is pooled, play from pool
      if (particlePooledObject != null)
      {
        spawnedParticle = ParticleManager.Instance.Spawn(particlePooledObject.Pool, SpawnInfo);
      }
      //if it is not pooled, fire via ParticleManager spawning
      else if (particlePrefab)
      {
        spawnedParticle = ParticleManager.Instance.Spawn(particlePrefab, SpawnInfo);
      }
    }
    base.Fire();
  }

  public override void Pool()
  {
    if (particlePooledObject)
    {
      ObjectPool.GetPool(particlePooledObject, 5);
    }
    base.Pool();
  }

  public override void Initialize(Actor myActor = null, Animator myAnimator = null)
  {
    if (particlePrefab)
    {
      //assign the pooled object if it exists
      particlePooledObject = particlePrefab.GetComponent<ParticlePooledObject>();
    }
    base.Initialize(myActor, myAnimator);
  }

  [Serializable]
  public class StateMachineParticleSpawnInfo : ParticleSpawnInfo
  {
    public bool StopOnStateExit = false;
    public bool UnparentOnStateExit = false;
  }
}

[Serializable]
public class AudioEvent : StateMachineEvent
{
  public AudioPooledObject audioPrefab;

  public override void Reset()
  {
    base.Reset();
  }

  public override void Fire()
  {
    if (audioPrefab != null)
    {
      (audioPrefab.Pool.GetObject() as AudioPooledObject).Play();
    }
    base.Fire();
  }

  public override void Pool()
  {
    if (audioPrefab)
    {
      ObjectPool.GetPool(audioPrefab, 5);
    }
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
