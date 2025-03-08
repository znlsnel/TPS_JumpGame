using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Util : MonoBehaviour
{
	static readonly string prefabPath = "Assets/4. Prefab/Items";
	static readonly string itemDataPath = "Assets/5. Datas/Item";

	[MenuItem("Tools/ItemInitialize")]
	static void ItemInitialize()
	{ 
		string[] prefabsPath = AssetDatabase.FindAssets("t:GameObject", new[] { prefabPath });
		string[] itemDatasPath = AssetDatabase.FindAssets("t:ScriptableObject", new[] { itemDataPath });

		Dictionary<string, ItemData> itemDatas = new Dictionary<string, ItemData>();
		Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

		foreach (string path in prefabsPath) 
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(path);
			GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
			string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

			prefabs.Add(fileName, go);
		}

		foreach (string path in itemDatasPath)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(path);
			ItemData so = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);
			string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

			itemDatas.Add(fileName, so);
		}



		foreach (var tuple in prefabs)
		{
			GameObject go = tuple.Value;
			string key = tuple.Key;

			if (!go.TryGetComponent(out Item itemHandler))
				itemHandler = go.AddComponent<Item>();

			if (!itemDatas.TryGetValue(key, out itemHandler.data))
			{
				// ScriptableObject 생성
				ItemData newItemData = ScriptableObject.CreateInstance<ItemData>();
				string newAssetPath = $"{itemDataPath}/{key}.asset";
				AssetDatabase.CreateAsset(newItemData, newAssetPath);
				AssetDatabase.SaveAssets();

				// Dictionary에 추가
				itemDatas.Add(key, newItemData);
				itemHandler.data = newItemData;

				Debug.Log($"Created new ItemData: {key}");
			}
		}

		foreach (var tuple in itemDatas)
		{
			ItemData go = tuple.Value;
			string key = tuple.Key;

			prefabs.TryGetValue(key, out go.dropItemPrefab);
		} 

	}
}
