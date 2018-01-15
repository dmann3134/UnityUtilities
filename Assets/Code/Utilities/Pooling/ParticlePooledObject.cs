using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ParticlePooledObject : PooledObject
{
  private List<ParticleSystem> particleSystems;

  private bool hasBeenInitialized = false;

  //private ParticleUtilities particleUtilities;

  public Actor Owner;

    /*
  private bool ParticleUtilitiesEnable
  {
    set
    {
      if (particleUtilities == null)
      {
        //FindUtil();
      }
      if (particleUtilities)
      {
        if (IsProjectile || value == false)
        {
          particleUtilities.DoNotRunUpdate();
        }
        else if (value)
        {
          particleUtilities.Restart();
        }
      }
    }
  }
  */

  private bool IsProjectile;

  public void Initialize()
  {
    particleSystems = new List<ParticleSystem>();
    particleSystems.AddRange(GetComponentsInChildren<ParticleSystem>());
    //FindUtil();
    ResetParticles(false);
  }

  /*
  private void FindUtil()
  {
    particleUtilities = GetComponent<ParticleUtilities>();
  }
  */

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
    PlayTheParticles();

    Animator anim = GetComponent<Animator>();
    if (anim) { ResetAnimator(anim); }
  }

  public void PlayPooledParticle(Vector3 targetPosition, Quaternion rotation, LayerMask groundLayers, Transform parent = null, bool onGround = false, Actor owner = null)
  {
    CheckInitialization();

    if (!onGround)
    {
      transform.position = targetPosition;
    }
    else
    {
      transform.position = PhysicsUtilities.FindPointOnGround(targetPosition, groundLayers);
      transform.position += new Vector3(0, 0.01f, 0);
    }


    /*
    if(particleUtilities && particleUtilities.UseOverrideRotation)
    {
      transform.rotation = Quaternion.Euler(particleUtilities.OverrideRotation);
    }
    else
    {
      transform.rotation = rotation;
    }
    */

        transform.rotation = rotation;

        transform.parent = parent;

    Owner = owner;

    Animator anim = GetComponent<Animator>();
    if (anim) { ResetAnimator(anim); }

    this.enabled = true;
    //ParticleUtilitiesEnable = true;
    PlayTheParticles();
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
    //ParticleUtilitiesEnable = true;
    PlayTheParticles();
  }

  private void PlayTheParticles()
  {
    particleSystems[0].Play(true);
  }

  public virtual void ResetParticles(bool returnToPool = true)
  {
    //stop any playing particles, only need the top roo
    particleSystems[0].Stop(true);

    //move to way the heck away
    transform.position = new Vector3(-10000, -10000, -10000);
    //dont think we ever assign this
    transform.parent = Pool.transform;

    //generate a seed
    uint seed = (uint)UnityEngine.Random.Range(0, 99999999);

    foreach (ParticleSystem particle in particleSystems)
    {
      //particle.randomSeed = seed;
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
