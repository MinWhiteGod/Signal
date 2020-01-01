using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Move : MonoBehaviour {
    public Dicedrag[] diceControl;
    public int turn = 0;
    public GameObject ending;
    public GameObject win;
    public GameObject replay;
    GameObject[,] tile;
    RollDice rollDice;
    Player turnPlayer;
    Image fail;
    CardChild[] Card;
    GameObject[] btnDice;
    int tileH;
    int tileV;
    private static ArrayList moveStack = new ArrayList();
    int[] getPartsCount;

    // Use this for initialization
    void Start () {
        diceControl = new Dicedrag[5];
        rollDice = GameObject.Find("GameManager").GetComponent<RollDice>();
        fail = GameObject.Find("fail").GetComponent<Image>();
        fail.gameObject.SetActive(false);
    }
    public void startcoroutine()
    {
        StartCoroutine("delayStart");
    }
    IEnumerator delayStart()
    {
        yield return new WaitForSeconds(0.5f);
        InitGame initGame = GetComponent<InitGame>();
        tile = initGame.tile;
        Card = new CardChild[initGame.player.Length];
        getPartsCount = new int[initGame.player.Length];
        getPartsCount.Initialize();
        btnDice = initGame.btnDice;
        tileH = initGame.tileHorizon;
        tileV = initGame.tileVertical;
        for (int i = 0; i < initGame.player.Length; i++)
        {
            Card[i] = GameObject.Find("card_" + initGame.player[i].name.Split('_')[1]).GetComponent<CardChild>();
        }
        for (int i=  0; i < diceControl.Length; i++)
            diceControl[i] = rollDice.controlDice[i].GetComponent<Dicedrag>();

        yield return new WaitForSeconds(0.1f);
        turnPlayer = GetComponent<PlayState>().player[turn];
        StartCoroutine("displayIndex");
        yield return null;
    }
    // Update is called once per frame
    void Update () {
    }
    public void moveAvator()
    {
        turnPlayer = GetComponent<PlayState>().player[turn];
        int privRepeat = 0;

        for (int i = 0; i < 5; i++)
        {
            //if (diceControl[i].getIndex() == 5)
            //{
            //    turnPlayer.advance.GetComponent<AdvanceCrash>().crash = true;
            //    print(diceControl[i].getCollider());
            //}

            for (int j = i + 1; j < 5; j++)
                if (diceControl[i].getIndex() > diceControl[j].getIndex())
                {
                    Dicedrag temp = diceControl[i];
                    diceControl[i] = diceControl[j];
                    diceControl[j] = temp;
                }
        }

        moveStack.Add(turnPlayer.pX);
        moveStack.Add(turnPlayer.pY);
        moveStack.Add(turnPlayer.direction);
        for (int i = 0; i < 5; i++)
        {
            if(diceControl[i].getIndex() != 5)
                switch (diceControl[i].getSprite())
                {
                    case "straight":
                        if (turnPlayer.direction == 0)
                            turnPlayer.pX--;
                        else if (turnPlayer.direction == 1)
                            turnPlayer.pY++;
                        else if (turnPlayer.direction == 2)
                            turnPlayer.pX++;
                        else if (turnPlayer.direction == 3)
                            turnPlayer.pY--;

                        moveStack.Add(turnPlayer.pX + "," + turnPlayer.pY);
                        break;
                    case "left":
                        turnPlayer.direction--;
                        if (turnPlayer.direction < 0)
                            turnPlayer.direction += 4;
                        turnPlayer.direction = turnPlayer.direction % 4;
                        moveStack.Add("1");
                        break;
                    case "right":
                        turnPlayer.direction++;
                        turnPlayer.direction %= 4;
                        moveStack.Add("-1");
                        break;
                    case "repeat":
                        for (int j = privRepeat; j < i; j++)
                        {
                            switch (diceControl[j].getSprite())
                            {
                                case "straight":
                                    if (turnPlayer.direction == 0)
                                        turnPlayer.pX--;
                                    else if (turnPlayer.direction == 1)
                                        turnPlayer.pY++;
                                    else if (turnPlayer.direction == 2)
                                        turnPlayer.pX++;
                                    else if (turnPlayer.direction == 3)
                                        turnPlayer.pY--;

                                    moveStack.Add(turnPlayer.pX + "," + turnPlayer.pY);
                                    break;
                                case "left":
                                    turnPlayer.direction--;
                                    if (turnPlayer.direction < 0)
                                        turnPlayer.direction += 4;
                                    turnPlayer.direction = turnPlayer.direction % 4;
                                    moveStack.Add("1");
                                    break;
                                case "right":
                                    turnPlayer.direction++;
                                    turnPlayer.direction %= 4;
                                    moveStack.Add("-1");
                                    break;
                            }
                        }
                        privRepeat = i + 1;
                        break;
                }

        }
        StartCoroutine("moving");
    }
    IEnumerator moving()
    {
        GameObject.Find("btnSTART_" + turnPlayer.color).GetComponent<Button>().interactable = false;
        for (int i = 3; i < moveStack.Count; i++)
        {
            if (moveStack[i].ToString().Split(',').Length == 2)
            {
                int px = int.Parse(moveStack[i].ToString().Split(',')[0]);
                int py = int.Parse(moveStack[i].ToString().Split(',')[1]);
                print(px + " " + py);
                if (px < 0 || py < 0 || px > tileV - 1 || py > tileH - 1)
                    turnPlayer.advance.GetComponent<AdvanceCrash>().crash = true;
                else
                {
                    print(tile[px, py]);
                    iTween.MoveTo(turnPlayer.advance, iTween.Hash("x", tile[px, py].transform.position.x, "z", tile[px, py].transform.position.z, "easeType", "linear", "time", 0.1f));

                    if (tile[px, py].name.Split('-').Length > 3)
                    {
                        print(tile[px, py].GetComponent<SpriteRenderer>().sprite.name);
                        print("2");
                        if (tile[px, py].name.Split('-')[3] != turnPlayer.color)
                            turnPlayer.advance.GetComponent<AdvanceCrash>().crash = true;
                        print("3");
                        if (turnPlayer.cardType == 1)
                        {
                            if (GameObject.Find(tile[px, py].name + "-parts").GetComponent<SpriteRenderer>().sprite.name == "c2" || GameObject.Find(tile[px, py].name + "-parts").GetComponent<SpriteRenderer>().sprite.name == "c3" || GameObject.Find(tile[px, py].name + "-parts").GetComponent<SpriteRenderer>().sprite.name == "c6")
                                turnPlayer.advance.GetComponent<AdvanceCrash>().crash = true;
                        }
                        else if (turnPlayer.cardType == 2)
                        {
                            if (GameObject.Find(tile[px, py].name + "-parts").GetComponent<SpriteRenderer>().sprite.name == "c1" || GameObject.Find(tile[px, py].name + "-parts").GetComponent<SpriteRenderer>().sprite.name == "c4" || GameObject.Find(tile[px, py].name + "-parts").GetComponent<SpriteRenderer>().sprite.name == "c5")
                                turnPlayer.advance.GetComponent<AdvanceCrash>().crash = true;
                        }
                    }

                }

            }
            else
                iTween.RotateBy(turnPlayer.advance, iTween.Hash("z", 0.25 * int.Parse(moveStack[i].ToString()), "easeType", "linear", "time", 0.05f));

             yield return new WaitForSeconds(0.11f);
        }

        if (turnPlayer.advance.GetComponent<AdvanceCrash>().crash)
        {
            print("fail");
            for (int j = 0; j < 2; j++)
            {
                GetComponent<AudioSource>().clip = Resources.Load("fx/8-bits2/bigHooter", typeof(AudioClip)) as AudioClip;
                GetComponent<AudioSource>().Play();
                fail.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.5f);
                fail.gameObject.SetActive(false);
                yield return new WaitForSeconds(0.5f);
            }
            turnPlayer.pX = int.Parse(moveStack[0].ToString());
            turnPlayer.pY = int.Parse(moveStack[1].ToString());
            turnPlayer.direction = int.Parse(moveStack[2].ToString());

        }
        else
        {
            for (int i = 3; i < moveStack.Count; i++)
            {
                if (moveStack[i].ToString().Split(',').Length == 2)
                {
                    int px = int.Parse(moveStack[i].ToString().Split(',')[0]);
                    int py = int.Parse(moveStack[i].ToString().Split(',')[1]);
                    iTween.MoveTo(turnPlayer.avatar, iTween.Hash("x", tile[px, py].transform.position.x, "z", tile[px, py].transform.position.z, "islocal", true, "easeType", "linear", "time", 0.4f));
                    GetComponent<AudioSource>().clip = Resources.Load("fx/8-bits2/cuteSonar", typeof(AudioClip)) as AudioClip;
                    GetComponent<AudioSource>().Play();
                    if (tile[px, py].name.Split('-').Length > 3)
                    {
                        if (tile[px, py].name.Split('-')[3] == turnPlayer.color)
                        {
                            GameObject p = GameObject.Find(tile[px, py].name + "-parts");
                            GameObject target = Card[turn].parts[int.Parse(Regex.Replace(p.GetComponent<SpriteRenderer>().sprite.name, @"\D", "")) - 1];
                            iTween.MoveTo(p, iTween.Hash("position", target.transform.position, "islocal", true, "easeType", "easeInOutSine", "time", 0.5f));
                            iTween.RotateTo(p, iTween.Hash("rotation", target.transform.rotation.eulerAngles, "islocal", true, "time", 0.5f));
                            iTween.ScaleTo(p, iTween.Hash("scale", target.transform.lossyScale, "islocal", true, "time", 0.5f));
                            tile[px, py].name = px + "-" + py + "-card";
                            if (turnPlayer.cardType == 1)
                            {
                                if (target.name == "1" || target.name == "4" || target.name == "5")
                                {
                                    getPartsCount[turn]++;
                                    if (getPartsCount[turn] > 2)
                                        StartCoroutine("End");
                                }
                            }
                            else if (turnPlayer.cardType == 1)
                            {
                                if (target.name == "2" || target.name == "3" || target.name == "6")
                                {
                                    getPartsCount[turn]++;
                                    if (getPartsCount[turn] > 2)
                                        StartCoroutine("End");
                                }
                            }
                        }
                    }
                }
                else
                {
                    iTween.RotateBy(turnPlayer.avatar, iTween.Hash("z", 0.25 * int.Parse(moveStack[i].ToString()), "easeType", "linear", "time", 0.4f));
                    GetComponent<AudioSource>().clip = Resources.Load("fx/8-bits2/alienApproach", typeof(AudioClip)) as AudioClip;
                    GetComponent<AudioSource>().Play();
                }

                yield return new WaitForSeconds(0.5f);
            }
        }

        for (int i = 0; i < diceControl.Length; i++)
        {
            diceControl[i].setInit();
            diceControl[i].gameObject.SetActive(false);
        }

        turnPlayer.advance.transform.position = turnPlayer.avatar.transform.position;
        turnPlayer.advance.transform.rotation = turnPlayer.avatar.transform.rotation;
        turnPlayer.advance.GetComponent<AdvanceCrash>().crash = false;

        moveStack.Clear();
        StopCoroutine("displayIndex");
        turnPlayer.avatar.transform.Find("point").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        turn++;
        turn %= GetComponent<PlayState>().player.Length;
        btnDice[turn].SetActive(true);
        yield return new WaitForSeconds(0.01f);
        StartCoroutine("displayIndex");
        yield return null;
    }
    IEnumerator displayIndex()
    {
        float temp = 0;
        GetComponent<PlayState>().player[turn].avatar.transform.Find("point").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        while (true)
        {
            if (GetComponent<PlayState>().player[turn].avatar.transform.Find("point").GetComponent<SpriteRenderer>().color.r > 0.9f)
                temp = -0.005f;
            else if (GetComponent<PlayState>().player[turn].avatar.transform.Find("point").GetComponent<SpriteRenderer>().color.r < 0.4f)
                temp = 0.01f;
            GetComponent<PlayState>().player[turn].avatar.transform.Find("point").GetComponent<SpriteRenderer>().color += new Color(temp, temp, temp);
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator End()
    {
        StopCoroutine("moving");
        if (turnPlayer.color == "red")
            ending.transform.eulerAngles = new Vector3(0, 0, 0);
        else if (turnPlayer.color == "blue")
            ending.transform.eulerAngles = new Vector3(0, 0, 90);
        else if (turnPlayer.color == "green")
            ending.transform.eulerAngles = new Vector3(0, 0, 270);
        else if (turnPlayer.color == "yellow")
            ending.transform.eulerAngles = new Vector3(0, 0, 180);
        win.SetActive(true);
        replay.SetActive(true);
        GetComponent<AudioSource>().clip = Resources.Load("fx/8-bits2/gameOver", typeof(AudioClip)) as AudioClip;
        GetComponent<AudioSource>().Play();

        yield return 0;
    }
}