using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// any item of the object pool needs to extend this
/// it allows use of the ObjectPool
/// </summary>
public class PooledObject : MonoBehaviour
{
	[System.NonSerialized] ObjectPool poolInstanceForPrefab;

	/// <summary>
	/// this will return the generic pooled item
	/// if it doesn't exist it will create one
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <returns></returns>
	public T GetPooledInstance<T>(Actor a = null) where T : PooledObject
	{
		if (!poolInstanceForPrefab)
		{
			poolInstanceForPrefab = ObjectPool.GetPool(this);
		}

		return (T)poolInstanceForPrefab.GetObject(a);
	}

	/// <summary>
	/// Associated object pool with this pooled object
	/// who it belongs to
	/// </summary>
	public ObjectPool Pool { get; set; }
	
	/// <summary>
	/// Recycles our objecto
	/// </summary>
	public void ReturnToPool()
	{
		if (Pool)
		{
			//adding an object turns it off
			Pool.AddObject(this);
      this.transform.parent = Pool.transform;
		}
		else
		{
			Destroy(gameObject);
		}
	}

  public virtual void HasBeenAddedToPool()
  {
    gameObject.SetActive(false);
  }

  public void Place(Vector3 targetPosition, Quaternion rotation, LayerMask groundLayers, Transform parent = null, bool onGround = false)
  {
    transform.parent = parent;

    if (!onGround)
    {
      transform.position = targetPosition;
    }
    else
    {
      transform.position = PhysicsUtilities.FindPointOnGround(targetPosition, groundLayers);
    }
    transform.rotation = rotation;

    this.gameObject.SetActive(true);
  }
}