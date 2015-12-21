// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputObjectLogic : MonoBehaviour {

	public GameObject inputFieldText;
	public void subtract () {
		transform.parent.SendMessage("Subtract", gameObject);
	}

	public void add () {
		transform.parent.SendMessage("Add", gameObject);
	}

	public void changed () {
		transform.parent.SendMessage("refreshList");
	}

}
