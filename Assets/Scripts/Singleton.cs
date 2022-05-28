using System;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;

	private static bool onApplicationQuitting;

	private static object deadLock = new object();

	public static T Instance
	{
		get
		{
			object obj = Singleton<T>.deadLock;
			lock (obj)
			{
				if (Singleton<T>.instance == null)
				{
					Singleton<T>.instance = UnityEngine.Object.FindObjectOfType<T>();
				}
				if (Singleton<T>.instance == null)
				{
					GameObject gameObject = new GameObject(typeof(T).ToString());
					Singleton<T>.instance = gameObject.AddComponent<T>();
				}
			}
			return Singleton<T>.instance;
		}
	}
}
