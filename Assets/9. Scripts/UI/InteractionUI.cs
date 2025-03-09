using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI objectName;
	[SerializeField] private TextMeshProUGUI description;
	[SerializeField] private TextMeshProUGUI stat;

	//[SerializeField] private TextMeshProUGUI statName;

	private void Awake()
	{
		Init();
	}  

	public void ShowObjectInfo(string name, string description, string stat = "")
	{
		this.objectName.text = name;
		this.description.text = description;
		this.stat.text = stat;
	}

	public void Init()
	{
		ShowObjectInfo("", "", "");
	}
}
