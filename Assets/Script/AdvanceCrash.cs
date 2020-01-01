using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceCrash : MonoBehaviour {
    public bool crash = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "wall")
        {
            if(c.transform.parent.GetComponent<enableWall>().triggerControl())
                crash = true;
        }
    }

}
