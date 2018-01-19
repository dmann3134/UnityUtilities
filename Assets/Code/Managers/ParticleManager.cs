using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ParticleManager : Singleton<ParticleManager>
{
  public void Awake()
  {

  }

  public GameObject Spawn(GameObject prefab, ParticleSpawnInfo spawnInfo)
  {
    var spawnedParticle = (GameObject)GameObject.Instantiate(prefab, spawnInfo.SpawnPosition, spawnInfo.SpawnRotation);

    spawnedParticle.transform.parent = spawnInfo.parent;

    return spawnedParticle;
  }

  public GameObject Spawn(ObjectPool pool, ParticleSpawnInfo spawnInfo)
  {
    PooledObject obj = pool.GetObject();

    if (obj is ParticlePooledObject)
    {
      (obj as ParticlePooledObject).PlayPooledParticle(spawnInfo.SpawnPosition, spawnInfo.SpawnRotation);
    }

    return obj.gameObject;
  }
}


[Serializable]
public class ParticleSpawnInfo
{
  public Vector3 SpawnPosition;
  public Quaternion SpawnRotation;

  public bool useHumanoidRigLocation;
  public HumanBodyBones humanoidRigLocation = HumanBodyBones.Chest;

  public bool worldSpace;

  public bool onGround = false;

  public Actor actor;
  public Transform parent = null;

  public Vector3 PositionOffset;
  public Vector3 OverrideRotation;
  public bool usePrefabRotation = false;


  public void SetParticleSpawnInfoValues(GameObject particlePrefab, Actor myActor = null, Animator myAnimator = null)
  {
    actor = myActor;
 
    if (myAnimator != null)
    {
      parent = myAnimator.transform;

      if (useHumanoidRigLocation)
      {
        parent = myAnimator.GetBoneTransform(humanoidRigLocation);
      }
    }

    if (parent)
    {
      SpawnPosition = parent.transform.position + parent.transform.TransformDirection(PositionOffset);
    }

    if(onGround)
    {
      SpawnPosition = PhysicsUtilities.FindPointOnGround(SpawnPosition);
      SpawnPosition += new Vector3(0, .01f, 0);
    }

    SpawnRotation = usePrefabRotation ? particlePrefab.transform.rotation : parent.rotation;

    if (worldSpace)
    {
      parent = null;
    }
  }
}