using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableWall : MonoBehaviour {
    GameObject[] wall;
    public GameObject[] tile;
    // Use this for initialization
    void Start () {
        wall = new GameObject[transform.childCount];
        //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "test")
        //    tile = new GameObject[1];
        //else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "test2")
        //    tile = new GameObject[2];

        for (int i = 0; i < transform.childCount; i++)
        {
            wall[i] = transform.GetChild(i).gameObject;
        }
        

    }
	
	// Update is called once per frame
	void Update () {

    }
    public bool triggerControl()
    {

        int j = 0;
        for (int k = 0; k < tile.Length; k++)
        {
            j += tile[k].name.Split('-').Length;
            print(j);
        }
        if (j <= 3 * tile.Length)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                wall[i].GetComponent<BoxCollider>().isTrigger = false;
            }
        }

        return wall[0].GetComponent<BoxCollider>().isTrigger;
    }
}
