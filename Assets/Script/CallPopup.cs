using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CallPopup : MonoBehaviour {
    GameObject txtTouch;
    GameObject popup;
	// Use this for initialization
	void Start () {
        txtTouch = GameObject.Find("Touchtoscreen");
        popup = GameObject.Find("PlaySetting");
        GameObject.Find("PlaySetting").SetActive(false);

    }

    // Update is called once per frame
    void Update () {
		
	}
    public void OnPopup()
    {
        txtTouch.SetActive(false);
        popup.SetActive(true);
    }
}
