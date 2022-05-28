using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
	private Dictionary<string, Queue<GameObject>> pools = new Dictionary<string, Queue<GameObject>>();

	public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		if (!Singleton<ObjectPool>.Instance.pools.ContainsKey(prefab.name))
		{
			Singleton<ObjectPool>.Instance.pools.Add(prefab.name, new Queue<GameObject>());
		}
		GameObject gameObject;
		if (Singleton<ObjectPool>.Instance.pools[prefab.name].Count != 0)
		{
			gameObject = Singleton<ObjectPool>.Instance.pools[prefab.name].Dequeue();
			gameObject.SetActive(true);
		}
		else
		{
			gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
		}
		gameObject.transform.position = position;
		gameObject.transform.rotation = rotation;
		gameObject.name = prefab.name;
		return gameObject;
	}

	public static void Despawn(GameObject prefab)
	{
		if (!Singleton<ObjectPool>.Instance.pools.ContainsKey(prefab.name))
		{
			Singleton<ObjectPool>.Instance.pools.Add(prefab.name, new Queue<GameObject>());
		}
		Singleton<ObjectPool>.Instance.pools[prefab.name].Enqueue(prefab);
		prefab.SetActive(false);
	}

	public static void Prespawn(GameObject prefab, int count)
	{
		if (Singleton<ObjectPool>.Instance.pools.ContainsKey(prefab.name))
		{
			return;
		}
		Singleton<ObjectPool>.Instance.pools.Add(prefab.name, new Queue<GameObject>());
		for (int i = 0; i < count; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
			Singleton<ObjectPool>.Instance.pools[prefab.name].Enqueue(gameObject);
			gameObject.name = prefab.name;
			gameObject.SetActive(false);
		}
	}
}
