using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardHandler : MonoBehaviour
{
    Transform cam;
	private void Awake()
	{
        cam = Camera.main.transform;
	}
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam.position); 
    }
}
