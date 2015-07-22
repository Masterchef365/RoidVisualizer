using UnityEngine;
using System.Collections;

public class FilterObjectAction : MonoBehaviour {

	//THIS IS BAD PRACTICE. NOTE TO SELF: FIX THIS SOON, DO NOT DO AGAIN...
	GameObject cam;
	void Start () {
		cam = Camera.main.gameObject;
	}

	public void action (string arg) {
		cam.SendMessage ("Filter", arg);
		if (arg == "+" | arg == "-") {
			if (arg == "+") {
				GameObject madeNew = GameObject.Instantiate(gameObject);
				madeNew.transform.parent = transform.parent;
			} else {
				if (gameObject.name.Contains("(Clone)")) {
					Destroy(gameObject);
				}
			}
		} 
	}
}
