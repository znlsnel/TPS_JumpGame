using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[Serializable]
public class PlayerResponse
{  
    [TextArea(3, 10)] public string text;  
    public DialogNodeSO nextDialog;
    public bool Assept = false;
}

[CreateAssetMenu(fileName = "DialogNoe", menuName = "My SO/New Dialog Node")]
public class DialogNodeSO : ScriptableObject 
{
    [SerializeField, TextArea(3, 10)] private string text;
    [SerializeField] private List<PlayerResponse> responses = new List<PlayerResponse>();
    
    public string Text => text; 
    public List<PlayerResponse> Responses => responses;  
}

[CreateAssetMenu(fileName = "DialogTree", menuName = "My SO/New Dialog Tree")]
public class DialogTreeSO : ScriptableObject
{
    [SerializeField] private DialogNodeSO rootNode;
     
	public DialogNodeSO RootNode => rootNode;
}

