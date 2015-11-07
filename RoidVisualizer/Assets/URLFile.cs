// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class URLFile : MonoBehaviour {

    public string fileName;
    public GameObject pointManager;
    string dir;

	void Start () {
        dir = Application.dataPath + @"\" + fileName + ".txt";

        if (File.Exists(dir))
        {
            pointManager.GetComponent<PointManager>().boot(Importing.readFile(dir)[0]);
            gameObject.SetActive(false);
        }

	}

    public void setURL(string URL)
    {
        File.WriteAllText(dir, URL);
        pointManager.GetComponent<PointManager>().boot(Importing.readFile(dir)[0]);
        gameObject.SetActive(false);
    }
}
