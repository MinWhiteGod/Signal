using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceColor : MonoBehaviour {
    GameObject choiceArrow;
    GameObject btnChoice;
    InitGame initgame;

    // Use this for initialization
    void Start () {
        choiceArrow = GameObject.Find("Arrow_" + name.Split('_')[1]);
        choiceArrow.SetActive(false);
        initgame = GameObject.Find("GameManager").GetComponent<InitGame>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void btnChoiceClick()
    {

        choiceArrow.SetActive(true);
        iTween.MoveTo(choiceArrow, iTween.Hash("z", choiceArrow.transform.position.z + 0.1f, "easeType", "linear", "loopType", "pingPong", "time", .2 ));
        initgame.PlayerSetting(name.Split('_')[1], choiceArrow);
    }
}
