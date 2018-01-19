using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlePooledObject : PooledObject
{
  private List<ParticleSystem> particleSystems;
  private bool hasBeenInitialized = false;
  public Actor Owner;

  public void Initialize()
  {
    particleSystems = new List<ParticleSystem>();
    particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>());
    ResetParticles(false);
  }

  private void Update()
  {
    if (particleSystems != null && !particleSystems[0].IsAlive(true) &&
      Pool.transform != this.transform.parent)
    {
      ResetParticles();
    }
  }

  public void PlayPooledParticle()
  {
    CheckInitialization();
    //start playing all particles
    particleSystems[0].Play(true);

    Animator anim = GetComponent<Animator>();
    if (anim) { ResetAnimator(anim); }
  }

  public void PlayPooledParticle(ParticleSpawnInfo spawnInfo)
  {
    CheckInitialization();

    transform.position = spawnInfo.SpawnPosition;

    transform.rotation = spawnInfo.SpawnRotation;

    transform.parent = spawnInfo.parent;

    Owner = spawnInfo.actor;

    Animator anim = GetComponent<Animator>();
    if (anim) { ResetAnimator(anim); }

    this.enabled = true;
    particleSystems[0].Play(true);
  }

  public void PlayPooledParticle(Vector3 targetPosition, Quaternion rotation, Transform parent = null, Actor owner = null)
  {
    CheckInitialization();

    transform.parent = parent;

    transform.position = targetPosition;

    transform.rotation = rotation;
    Owner = owner;
    Animator anim = GetComponent<Animator>();
    if (anim) { ResetAnimator(anim); }

    this.enabled = true;
    particleSystems[0].Play(true);

  }

  public virtual void ResetParticles(bool returnToPool = true)
  {
    //stop any playing particles, only need the top roo
    particleSystems[0].Stop(true);

    //move far out
    transform.position = new Vector3(-10000, -10000, -10000);

    //dont think we ever assign this
    transform.parent = Pool.transform;

    foreach (ParticleSystem particle in particleSystems)
    {
      if (particle)
      {
        particle.Simulate(0, true, true);
      }
    }

    if (returnToPool)
    {
      //have to wait until particle system is done playing
      ReturnToPool();
    }
  }

  public void CheckInitialization()
  {
    if (!hasBeenInitialized)
    {
      Initialize();
      hasBeenInitialized = true;
    }
  }

  public override void HasBeenAddedToPool()
  {
    //base.HasBeenAddedToPool();
    this.enabled = false;
    //FindUtil();
    if (particleSystems == null)
    {
      particleSystems = new List<ParticleSystem>();
      particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>());
    }

    Animator anim = GetComponent<Animator>();
    if (anim) { ResetAnimator(anim); }

    //ParticleUtilitiesEnable = false;
    transform.position = new Vector3(-10000, -10000, -10000);
    // DO NOTHING
  }

  protected void ResetAnimator(Animator anim)
  {
    AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
    anim.Play(stateInfo.fullPathHash, 0, 0f);
  }
}
