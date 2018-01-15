using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class ParticleManager : Singleton<ParticleManager>
{
    public void Awake()
    {

    }

    #region Local Particles
    #region Particle 
    public GameObject SpawnLocalParticle(GameObject prefab, Vector3 targetSpawnPosition, Quaternion rotation, Transform parent = null, bool onGround = false)
    {
        //calculate correct transform
        if (onGround)
        {
            targetSpawnPosition = PhysicsUtilities.FindPointOnGround(targetSpawnPosition + new Vector3(0, 10f, 0)) + new Vector3(0, .03f, 0);
        }
        var spawnedParticle = (GameObject)GameObject.Instantiate(prefab, targetSpawnPosition, rotation);
        if (parent != null) spawnedParticle.transform.parent = parent;

        return spawnedParticle;
    }

    public GameObject PlacePooledParticle(ObjectPool pool, Vector3 targetSpawnPosition, Quaternion rotation, Transform parent = null, bool onGround = false)
    {
        PooledObject obj = pool.GetObject();

        if (onGround)
        {
            targetSpawnPosition = PhysicsUtilities.FindPointOnGround(targetSpawnPosition + new Vector3(0, 10f, 0)) + new Vector3(0, .03f, 0);
        }

        if (obj is ParticlePooledObject)
        {
            (obj as ParticlePooledObject).PlayPooledParticle(targetSpawnPosition, rotation, parent);
        }

        return obj.gameObject;
    }
    #endregion
    /*
    #region Spawn Local Pooled Particle

    public GameObject SpawnLocalPooledParticle(GameObject pooledObject, Vector3 targetSpawnPosition, Quaternion rotation, Transform parent = null, bool onGround = false)
    {
        pooledObject.SetActive(true);
        //calculate correct transform
        if (onGround)
        {
            targetSpawnPosition = FindTheGround(targetSpawnPosition + new Vector3(0, 10f, 0)) + new Vector3(0, .03f, 0);
        }
        pooledObject.transform.position = targetSpawnPosition;
        pooledObject.transform.rotation = rotation;
        if (parent != null) pooledObject.transform.parent = parent;

        PooledObject obj = pooledObject.GetComponent<PooledObject>();

        if (obj is ParticlePooledObject)
        {
            (obj as ParticlePooledObject).PlayPooledParticle(targetSpawnPosition, rotation, parent);
        }

        return pooledObject;
    }

    #endregion

    #region Particle Spawn on Delay based on Time
    public void SpawnLocalTimeDelayedParticle(string prefabName, Transform targetSpawnPosition, Quaternion rotation, float delay = 0, Transform parent = null, bool onGround = false)
    {
        StartCoroutine(TimeDelayedParticle(prefabName, targetSpawnPosition, rotation, delay, parent, onGround));
    }

    public IEnumerator TimeDelayedParticle(string prefabName, Transform targetSpawnPosition, Quaternion rotation, float delay = 0f, Transform parent = null, bool onGround = false)
    {
        //process delay
        yield return new WaitForSeconds(delay);

        //calculate correct transform
        var targetPosition = targetSpawnPosition.position;
        if (onGround)
        {
            targetPosition = FindTheGround(targetPosition + new Vector3(0, 10f, 0)) + new Vector3(0, .03f, 0);
        }

        var spawnedParticle = (GameObject)GameObject.Instantiate(Resources.Load(prefabName), targetPosition, rotation);
        if (parent != null) spawnedParticle.transform.parent = parent;
    }
    #endregion

    #region Particle Spawn on Delay based on Velocity
    public void SpawnLocalVelocityDelayedParticle(string prefabName, Transform targetSpawnPosition, Rigidbody targetRigidBody, ComparisonOperator comparisonOperator, float comparisonValue, Vector3 eulerRotation, Transform parent = null, bool onGround = false)
    {
        Debug.Log("Local Player Spawned");
        //StartCoroutine(LocalVelocityDelayedParticle(prefabName, targetSpawnPosition, targetRigidBody, comparisonOperator, comparisonValue, eulerRotation, parent, onGround));
    }

    public IEnumerator LocalVelocityDelayedParticle(string prefabName, Transform targetSpawnPosition, Rigidbody targetRigidbody, ComparisonOperator comparisonOperator, float comparisonValue, Vector3 eulerRotation, Transform parent = null, bool onGround = false)
    {
        yield return new WaitForSeconds(.25f);

        while (!PhysicsUtilities.CompareRigidbodyVelocity(targetRigidbody, comparisonOperator, comparisonValue)) ;

        //calculate correct transform
        var targetPosition = targetSpawnPosition.position;
        if (onGround)
        {
            targetPosition = FindTheGround(targetPosition + new Vector3(0, 10f, 0)) + new Vector3(0, .03f, 0);
        }

        var spawnedParticle = (GameObject)PhotonNetwork.Instantiate(prefabName, targetPosition, Quaternion.Euler(eulerRotation), 0);
        if (parent != null) spawnedParticle.transform.parent = parent;
    }
    #endregion Particle Spawn on Delay based on Velocity
    #endregion Local Particles
    */
}
#endregion