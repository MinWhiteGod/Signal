using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySetting : MonoBehaviour {
    GameObject[] choice = new GameObject[4];
    public GameObject toggleGroup;
    public GameObject btnStart;
    GameObject popup;
    //Transform[] color = new Transform[4];
    int[] c = { 0, 1, 1, 3 };
    int num = 2;
    int sceneIndex = 3;
    //int turn = 0;
    
        // Use this for initialization
    void Start () {
        PlayerPrefs.DeleteAll();
        popup = GameObject.Find("PlaySetting");
        popup.SetActive(false);
        //for (int i = 0; i < choice.Length; i++)
        //{
        //    choice[i] = GameObject.Find("choice" + i);
        //    choice[i].SetActive(false);
        //}
        //choice[0].SetActive(true);
        //color[0] = GameObject.Find("green").transform;
        //color[1] = GameObject.Find("yellow").transform;
        //color[2] = GameObject.Find("red").transform;
        //color[3] = GameObject.Find("blue").transform;
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void SelectPlayerNumber(int num)
    {
        this.num = num;
        btnStart.SetActive(true);
        //for (int i = 0; i < 4; i++)
        //{
        //    choice[i].SetActive(false);
        //}
        //for (int i = 0; i < num; i++)
        //{
        //    choice[i].SetActive(true);
        //    for (int j = 0; j < num; j++)
        //        if (i != j)
        //            if (c[i] == c[j])
        //                c[i] = (Mathf.Abs(c[i]) + 1) % 4;
        //}
        //for(int i = 0; i < num; i ++)
        //{
        //    choice[i].transform.position = color[c[i]].position + new Vector3(0, 10, 0);
        //}
    }
    //public void SelectPlayerColor(int select)
    //{
    //    for (int i = 0; i < num; i++)
    //        if (c[i] == select)
    //            break;

    //    c[turn] = select;
    //    choice[turn].transform.position = color[c[turn]].position + new Vector3(0, 10, 0);

    //    turn = (Mathf.Abs(turn) + 1) % num;
    //}
    public void onStart()
    {
        PlayerPrefs.SetInt("num", num);
        //for(int i = 0; i < num; i++)
        //    PlayerPrefs.SetString("player" + i, color[c[i]].name);

        PlayerPrefs.Save();
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
    public void onOffline()
    {
        toggleGroup.SetActive(true);
        popup.SetActive(true);
        sceneIndex = 2;
    }
    public void onOnline()
    {
        toggleGroup.SetActive(true);
        popup.SetActive(true);
        sceneIndex = 3;
    }

}
