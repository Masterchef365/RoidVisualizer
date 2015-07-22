// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;

public class ButtonClickCam : MonoBehaviour {

	//THIS IS BAD PRACTICE. NOTE TO SELF: FIX THIS SOON, DO NOT DO AGAIN...
	GameObject cam;
	void Start () {
		cam = Camera.main.gameObject;
	}
	
	public void Click() {
		cam.SendMessage ("MoveToObserve", gameObject.name);
	}
}
