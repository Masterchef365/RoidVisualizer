// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class FilterListManager : MonoBehaviour {

	public GameObject pointManagerObject;
	public List<GameObject> inputObjects;
	public List<string> filters;

	public void Add (GameObject instance) {
		GameObject newInstance = (GameObject)GameObject.Instantiate(instance);
		newInstance.transform.SetParent(instance.transform.parent);
		inputObjects.Add(newInstance);
		refreshList();
	}

	public void Subtract (GameObject instance) {
		if (inputObjects.Count > 1) {
			inputObjects.Remove(instance);
			Destroy(instance);
		}
		refreshList();
	}

	public void refreshList() {
		filters.Clear();
		foreach (GameObject inputObject in inputObjects) {
			string text = inputObject.GetComponent<InputObjectLogic>().inputFieldText.GetComponent<Text>().text;
			if (text != "") {
				filters.Add(text);
			}
		}
		pointManagerObject.SendMessage("changeFilters", filters);
	}

}
