using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TrapSensor : MonoBehaviour
{
	[NonSerialized] public UnityEvent onFindPlayer = new UnityEvent();
}
 