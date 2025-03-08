using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI ItemName;
	[SerializeField] private TextMeshProUGUI description;
	[SerializeField] private TextMeshProUGUI stat;

	//[SerializeField] private TextMeshProUGUI statName;

	private void Awake()
	{
		RegistItem(null);
	}

	public void RegistItem(Item item)
	{
		ItemName.text = item != null ? item.data.name : string.Empty;
		description.text = item != null ? item.data.description : string.Empty;
		stat.text = item != null ? item.data.GetStatDescription() : string.Empty; 


	}
}
