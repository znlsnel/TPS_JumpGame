using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
	private static T instance;
	public static T Instance { 
		
		get
		{
			if (instance == null)
			{
				instance = FindFirstObjectByType<T>();
				if (instance == null)
					instance = new GameObject("Input System").AddComponent<T>();
			}
			return instance;
		}
	}

	protected virtual void Awake()
	{
		if (instance == null)
		{
			instance = this as T; 
			if (transform.parent != null)
			{
				transform.SetParent(null);
			}

			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
