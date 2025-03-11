using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GaugeUI : MonoBehaviour
{
    private Slider slider;
    private float gauge;

	private void Awake()
	{
		slider = GetComponentInChildren<Slider>();
        CloseUI();
	} 
	public void OpenUI()
    {
        slider.value = 0.0f;
		gameObject.SetActive(true);
	} 

	public void UpdateGauge(float value) => slider.value = value;
    
    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
