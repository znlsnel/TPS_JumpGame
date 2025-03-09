using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI scriptText;
    [SerializeField] private GameObject buttonParent;
	
	private List<GameObject> responseButtons = new List<GameObject>();
	private UnityEvent<bool> onEndDialog = new UnityEvent<bool>();
	private GameObject parentPanel;

	private void Awake() 
	{
		int cnt = buttonParent.transform.childCount;
		for (int i = 0; i < cnt; i++)
			responseButtons.Add(buttonParent.transform.GetChild(i).gameObject);
		parentPanel = transform.GetChild(0).gameObject;

		CloseUI();
	}
	 
	public void OpenUI(DialogNodeSO rootDialog, Action<bool> onEndDialog)
	{
		parentPanel.SetActive(true);
		this.onEndDialog.AddListener(onEndDialog.Invoke);
		UpdateDialogUI(rootDialog);
	} 

	void UpdateDialogUI(DialogNodeSO dialog)
	{
		if (dialog == null)
		{
			CloseUI();
			return;
		}

		scriptText.text = dialog.Text; 

		// 플레이어 선택지만큼 버튼 활성화
		for (int i = 0; i < dialog.Responses.Count; i++)
		{
			int idx = i;
			responseButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();

			// 버튼이 클릭되면 UpdateDialogUI가 다시 호출되도록 구현
			responseButtons[i].GetComponent<Button>().onClick.AddListener(() =>
			{
				var next = dialog.Responses[idx].nextDialog;
					UpdateDialogUI(next); 
				 
				if (next == null)
					onEndDialog?.Invoke(dialog.Responses[idx].Assept);  
			});

			responseButtons[i].GetComponentInChildren<Text>().text = dialog.Responses[i].text;
			responseButtons[i].SetActive(true);
		}

		// 사용되지 않는 버튼 비활성화
		for (int i = dialog.Responses.Count; i < responseButtons.Count; i++)
			responseButtons[i].SetActive(false);
		 
	}


	public void CloseUI()
	{
		parentPanel.SetActive(false);

	}

}
