using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardChild : MonoBehaviour {
    public GameObject[] parts = new GameObject[6];
    PlayState playState;
	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject[] getPart()
    {
        return parts;
    }
}
