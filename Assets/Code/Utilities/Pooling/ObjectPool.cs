using UnityEngine;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{
	public PooledObject prefab;
	List<PooledObject> availableObjects = new List<PooledObject>();

  private static GameObject StaticAudioPoolReference;
  public static Dictionary<PooledObject, ObjectPool> StaticAudioPoolDictionary = new Dictionary<PooledObject, ObjectPool>();
	private static GameObject StaticVFXPoolReference;
	public static Dictionary<PooledObject, ObjectPool> StaticVFXPoolDictionary = new Dictionary<PooledObject, ObjectPool>();
  private static GameObject StaticMiscPoolReference;
	public static Dictionary<PooledObject, ObjectPool> StaticMiscPoolDictionary = new Dictionary<PooledObject, ObjectPool>();

  /// <summary>
  /// check to see if there is a pool of the object we want to instantiate
  /// if there is great, return it
  /// If not then create it and make a reference to the typeof object we want to pool
  /// </summary>
  /// <param name="prefab"></param>
  /// <returns></returns>
  public static ObjectPool GetPool(PooledObject prefab, int poolSize = 1)
	{
    if (prefab == null) return null;

    ObjectPool pool;

    //USED TO BE //pretty costly
    if (StaticAudioPoolReference == null)
    { // not this tho
      StaticAudioPoolReference = GameObject.FindGameObjectWithTag("AudioPool");
    }
    if (StaticVFXPoolReference == null)
    { // not this tho
      StaticVFXPoolReference = GameObject.FindGameObjectWithTag("VFXPool");
    }
    if(StaticMiscPoolReference == null)
    { // or this
      StaticMiscPoolReference = GameObject.FindGameObjectWithTag("MiscPool");
    }
    
    if (StaticAudioPoolReference && StaticAudioPoolDictionary.ContainsKey(prefab))
    {
      pool = StaticAudioPoolDictionary[prefab];
    }
    if (StaticVFXPoolReference && StaticVFXPoolDictionary.ContainsKey(prefab))
    {
      pool = StaticVFXPoolDictionary[prefab];
    }
    if (StaticMiscPoolReference && StaticMiscPoolDictionary.ContainsKey(prefab))
    {
      pool = StaticMiscPoolDictionary[prefab];
    }

    pool = CreateNewPool(prefab, poolSize);

    prefab.Pool = pool;

    return pool;
	}

  private static ObjectPool CreateNewPool(PooledObject prefab, int poolSize)
  {
    GameObject obj;
    ObjectPool pool;

    obj = new GameObject(prefab.name + " Pool");
    pool = obj.AddComponent<ObjectPool>();
    pool.prefab = prefab;

    //store the objects in corresponding categories
    if (prefab.GetComponentInChildren<AudioSource>())
    {
      StaticAudioPoolDictionary[prefab] = pool;
      obj.transform.parent = StaticAudioPoolReference.transform;
    }
    else if (prefab.GetComponentInChildren<ParticleSystem>())
    {
      StaticVFXPoolDictionary[prefab] = pool;
      obj.transform.parent = StaticVFXPoolReference.transform;
    }
    else
    {
      StaticMiscPoolDictionary[prefab] = pool;
      obj.transform.parent = StaticMiscPoolReference.transform;
    }

    //generate target number of pooled objects
    for (int i = 0; i < poolSize; i++)
    {
      pool.CreatePooledObject(i);
    }
    return pool;
  }

	/// <summary>
	/// will return the generic pooled object
	/// </summary>
	/// <returns></returns>
	public PooledObject GetObject(Actor a = null)
	{
		PooledObject obj = null;
		int lastAvailableIndex = availableObjects.Count - 1;
		if (lastAvailableIndex >= 0 && availableObjects[lastAvailableIndex] != null)
		{
      obj = availableObjects[lastAvailableIndex];
      if (obj)
      {
        obj.gameObject.SetActive(true);
        availableObjects.RemoveAt(lastAvailableIndex);
      }
		}
		else
		{
      obj = CreatePooledObject();
      int index = availableObjects.IndexOf(obj);
      availableObjects.RemoveAt(index);
		}

		if (a)
		{
			if (obj is AudioPooledObject)
			{
				(obj as AudioPooledObject).pairedActor = a;
			}
		}

		return obj;
	}

	/// <summary>
	/// This method adds a new object to our pooled objects
	/// and turns is off, that way when we access it we just turn it on
	/// </summary>
	/// <param name="obj">Generic pooled object</param>
	public void AddObject(PooledObject obj)
	{
    obj.HasBeenAddedToPool();
    if (availableObjects.Contains(obj) == false)
    {
      availableObjects.Add(obj);
    }
	}

  public PooledObject CreatePooledObject(int number = -1)
  {
    PooledObject obj;
    obj = Instantiate<PooledObject>(prefab);
    obj.transform.SetParent(transform, false);
    obj.Pool = this;
    if(number >= 0)
    {
      obj.name += " " + number;
    }
    else
    {
      obj.name += " extra";
    }
    AddObject(obj);
    return obj;
  }
}