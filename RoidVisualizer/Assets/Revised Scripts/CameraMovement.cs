// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour {
	
	public float mouseSpeed = 0.2f;
	public float scaleDivisor = 200f;

	void Update () {


		if (Input.GetMouseButton (0)) {
			Vector3 newRot = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0) * mouseSpeed + transform.rotation.eulerAngles;
			transform.rotation = Quaternion.Euler (newRot);
		}

		if (Input.GetMouseButton (1)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				hit.collider.SendMessage("hit");
			}
		}

		float speed;
		if (Input.GetKey("left shift")){
			speed = 350f * Time.deltaTime;
		} else {
			speed = 100f * Time.deltaTime;
		}
		
		if (Input.GetKey ("w")) {
			transform.Translate(Vector3.forward*speed, Space.Self);
		}
		else
		{
			if (Input.GetKey ("s")) {
				transform.Translate(Vector3.back*speed, Space.Self);
			}
		}
		
		if (Input.GetKey ("a")) {
			transform.Translate(Vector3.left*speed,Space.Self);
		}
		else
		{
			if (Input.GetKey ("d")) {
				transform.Translate(Vector3.right*speed,Space.Self);
			}
		}
		
		if (Input.GetKey ("space")) {
			transform.Translate(Vector3.up*speed,Space.Self);
		}
		else
		{
			if (Input.GetKey ("c")) {
			}
		}
	}

	public void setMouseSpeed (float input) {
		mouseSpeed = input * 5f;
	}

	public void moveToObserve (GPSDefinition.GPSPoint position) {
		Debug.Log("LOLZ");
		StartCoroutine (quickMove(transform.position, position.unityPosition + (transform.forward) * (scaleDivisor / -3f))); //Moved back a bit
	}

	IEnumerator quickMove (Vector3 from, Vector3 to) {
		for (float i = 0; i < 1; i += 0.01f) {
			transform.position = Vector3.Lerp(from, to, i);
			yield return new WaitForSeconds(0.005f);
		}
	}


}