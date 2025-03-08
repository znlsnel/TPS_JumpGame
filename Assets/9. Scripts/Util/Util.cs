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
		string[] prefabs = AssetDatabase.FindAssets("t:GameObject", new[] { prefabPath });
		string[] itemDatas = AssetDatabase.FindAssets("t:ScriptableObject", new[] { itemDataPath });

		Dictionary<string, ItemData> datas = new Dictionary<string, ItemData>();
		Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();

		foreach (string prefab in prefabs) 
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(prefab);
			GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
			string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

			items.Add(fileName, go);
		}

		foreach (string itemData in itemDatas)
		{
			string assetPath = AssetDatabase.GUIDToAssetPath(itemData);
			ItemData so = AssetDatabase.LoadAssetAtPath<ItemData>(assetPath);
			string fileName = System.IO.Path.GetFileNameWithoutExtension(assetPath);

			datas.Add(fileName, so);
		}



		foreach (var tuple in items)
		{
			GameObject go = tuple.Value;
			string key = tuple.Key;

			if (!go.TryGetComponent(out Item itemHandler))
				itemHandler = go.AddComponent<Item>();

			datas.TryGetValue(key, out itemHandler.data);
		}

		foreach (var tuple in datas)
		{
			ItemData go = tuple.Value;
			string key = tuple.Key;

			items.TryGetValue(key, out go.dropItemPrefab);
		} 

	}
}
