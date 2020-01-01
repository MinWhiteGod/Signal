using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleTouch : MonoBehaviour {
    float t;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        t += Time.deltaTime;
        if(name == "Touch" && t > 12)
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    public void onTouch()
    {
        print(t);
        if (name != "Touch"/* || t > 3*/)
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
