// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;

public class ButtonClickCam : MonoBehaviour {

	GameObject cam;
	void Start () {
		cam = Camera.main.gameObject;
	}
	
	public void Click() {
		cam.SendMessage ("MoveToObserve", gameObject.name);
	}
}
