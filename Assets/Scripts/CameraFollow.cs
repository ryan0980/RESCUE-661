using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform followTarget;
	public float followDistance = 20.0f;
	public float rightDistance = 10.0f;
	public float upDistance = 5.0f;
	public float dampTrace = 20.0f;
	private Transform cam;

	// Use this for initialization
	void Start () {
		cam = Camera.main.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void LateUpdate()
	{
		cam.position = Vector3.Lerp(cam.position, followTarget.position - (followTarget.forward * followDistance) + (Vector3.right * rightDistance) + (followTarget.up * upDistance), Time.deltaTime * dampTrace);
	}
}
