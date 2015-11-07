// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DistanceListManager : MonoBehaviour {

    public GameObject lineCam;
    public List<GameObject> inputObjects;
    public List<string> points;

    public void Add(GameObject instance)
    {
        GameObject newInstance = (GameObject)GameObject.Instantiate(instance);
        newInstance.transform.SetParent(instance.transform.parent);
        inputObjects.Add(newInstance);
        refreshList();
    }

    public void Subtract(GameObject instance)
    {
        if (inputObjects.Count > 1)
        {
            inputObjects.Remove(instance);
            Destroy(instance);
        }
        refreshList();
    }

    public void refreshList()
    {
        points.Clear();
        foreach (GameObject inputObject in inputObjects)
        {
            string text = inputObject.GetComponent<InputObjectLogic>().inputFieldText.GetComponent<InputField>().text;
            if (text != "")
            {
                points.Add(text);
            }
        }
        lineCam.SendMessage("parseLines", points);
    }


}
