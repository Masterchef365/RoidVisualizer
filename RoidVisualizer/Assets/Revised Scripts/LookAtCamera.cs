// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
	GameObject cam;
	void Start () {
		cam = GameObject.FindObjectOfType<Camera> ().gameObject;
	}

	void LateUpdate () {
		transform.rotation = cam.transform.rotation;
	}
}
