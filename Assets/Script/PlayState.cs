using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : MonoBehaviour {
    public Player[] player;
    InitGame initGame;

    // Use this for initialization
    void Start () {
        initGame = GetComponent<InitGame>();
    }
    public void startcoroutine()
    {
        StartCoroutine("delayStart");
    }
    IEnumerator delayStart()
    {
        yield return new WaitForSeconds(0.5f);

        player = new Player[initGame.player.Length];
        for (int i = 0; i < initGame.player.Length; i++)
        {
            player[i] = new Player(i, initGame.player[i].name.Split('_')[1]);
            GameObject.Find("card_" + player[i].color).GetComponent<SpriteRenderer>().sprite = Resources.Load("C_" + GetComponent<PlayState>().player[i].color + "_" + GetComponent<PlayState>().player[i].cardType, typeof(Sprite)) as Sprite;

        }

        yield return null;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
public class Player
{
    int index = 0;
    public string color;
    public GameObject avatar, advance;
    public int pX, pY, direction, cardType; //0=north, 1=east, 2=south, 3=west
    InitGame initGame = GameObject.Find("GameManager").GetComponent<InitGame>();

    public Player(int index, string color)
    {
        this.index = index;
        this.color = color;
        avatar = GameObject.Find("player_" + color);
        advance = GameObject.Find("Advance_" + color);
        cardType = Random.Range(1, 3);
        if (color == "yellow")
        {
            pX = 0; pY = 0;
            direction = 2;
        }
        else if (color == "blue")
        {
            pX = 0; pY = initGame.tileHorizon - 1;
            direction = 2;
        }
        else if (color == "green")
        {
            pX = initGame.tileVertical - 1; pY = 0;
            direction = 0;
        }
        else if (color == "red")
        {
            pX = initGame.tileVertical - 1; pY = initGame.tileHorizon - 1;
            direction = 0;
        }
    }
    public int getTurn()
    {
        return index;
    }
}