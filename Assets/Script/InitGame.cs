using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InitGame : MonoBehaviour {
    private static string[] playerinfo;
    public int tileHorizon = 16;
    public int tileVertical = 11;
    int tileCradCount = 27;
    float tilescale = 0;
    public GameObject[] player;
    public GameObject[,] tile;
    public GameObject[] LoadTile;
    public ArrayList loadtilearr = new ArrayList();
    [HideInInspector]
    public GameObject[] partTile;
    public GameObject[] parts;
    private int[] partPlacement;
    public GameObject[] btnDice;
    GameObject[] arrow;
    int settingIndex = 0;

    // Use this for initialization
    void Start()
    {
        print(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "test")
        {
            tileHorizon = 16; tileVertical = 11; tileCradCount = 30; tilescale = 0;
        }
        else if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "test2")
        {
            tileHorizon = 9; tileVertical = 6; tileCradCount = 36; tilescale = 0.06f;
        }
        int num = PlayerPrefs.GetInt("num", 1);
        playerinfo = new string[num + 1];
        playerinfo[0] = num.ToString();
        //for(int i = 0; i < num; i++)
        //{
        //    playerinfo[i + 1] = PlayerPrefs.GetString("player" + i, "green");
        //    print(i + 1 +" " + playerinfo[i+1]);
        //}
        player = new GameObject[int.Parse(playerinfo[0])];
        partPlacement = new int[int.Parse(playerinfo[0]) * 6];
        parts = new GameObject[int.Parse(playerinfo[0]) * 6];
        tile = new GameObject[tileVertical, tileHorizon];
        partTile = new GameObject[tileCradCount];
        LoadTile = new GameObject[255];
        btnDice = new GameObject[player.Length];
        arrow = new GameObject[player.Length];

        //random start
        int rnd = Random.Range(0, 4);
        print(rnd);
        if (player.Length == 2)
            if(rnd < 2)
            {
                playerinfo[1] = RandomColor(1);
                playerinfo[2] = RandomColor(3);
            }
            else
            {
                playerinfo[1] = RandomColor(3);
                playerinfo[2] = RandomColor(1);
            }
        else
        for (int i = 0; i < player.Length; i++)
        {
            playerinfo[i + 1] = RandomColor(rnd);
            rnd++;
            rnd %= 4;
        }
        onStart();
        //
    }
    string RandomColor(int rnd)
    {
        if (rnd == 0) return "blue";
        else if (rnd == 1) return "red";
        else if (rnd == 2) return "green";
        else if (rnd == 3) return "yellow";

        return "";
    }
    public void PlayerSetting(string color, GameObject arrow)
    {
        playerinfo[settingIndex + 1] = color;
        if (this.arrow[settingIndex] != null)
            this.arrow[settingIndex].SetActive(false);
        this.arrow[settingIndex] = arrow;

        settingIndex++;
        if (settingIndex == player.Length && !GameObject.Find("START").GetComponent<Button>().interactable)
            GameObject.Find("START").GetComponent<Button>().interactable = true;
        settingIndex %= player.Length;

    }
    public void onStart(/*GameObject btn*/)
    {
        //btn.SetActive(false);
        //for(int i = 0; i < player.Length; i++)
        //    arrow[i].SetActive(false);
        GameObject[] playerEnable = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < player.Length; i++)
            player[i] = GameObject.Find("player_" + playerinfo[i + 1]);
        for (int i = 0; i < playerEnable.Length; i++)
            playerEnable[i].SetActive(false);
        for (int i = 0; i < player.Length; i++)
            player[i].SetActive(true);

        for (int i = 0; i < player.Length; i++)
        {
            btnDice[i] = GameObject.Find("btnDICE_" + player[i].name.Split('_')[1]);
            btnDice[i].GetComponent<Button>().interactable = true;
        }
        GameObject.Find("btnDICE_yellow").SetActive(false);
        GameObject.Find("btnDICE_green").SetActive(false);
        GameObject.Find("btnDICE_blue").SetActive(false);
        GameObject.Find("btnDICE_red").SetActive(false);
        //GameObject.Find("choice_yellow").SetActive(false);
        //GameObject.Find("choice_green").SetActive(false);
        //GameObject.Find("choice_blue").SetActive(false);
        //GameObject.Find("choice_red").SetActive(false);

        btnDice[0].SetActive(true);

        GameObject[] playerUI = GameObject.FindGameObjectsWithTag("playerUI");
        for (int i = 0; i < playerUI.Length; i++)
            playerUI[i].SetActive(false);

        for (int j = 0; j < int.Parse(playerinfo[0]); j++)
            for (int i = 0; i < playerUI.Length; i++)
                if (playerUI[i].name.Split('_')[1] == playerinfo[j + 1])
                    playerUI[i].SetActive(true);

        //LoadTile = GameObject.FindGameObjectsWithTag("tile");

        //int k = 0;
        int l = 0;
        tile[0, 0] = GameObject.Find(tileVertical + "-" + tileHorizon + "-yellow");
        tile[tileVertical - 1, 0] = GameObject.Find(tileVertical + "-1-green");
        tile[0, tileHorizon - 1] = GameObject.Find("1-" + tileHorizon + "-blue");
        tile[tileVertical - 1, tileHorizon - 1] = GameObject.Find(tileVertical + "-" + tileHorizon + "-red");
        for (int i = 0; i < tileVertical; i++)
            for (int j = 0; j < tileHorizon; j++)
            {
                if (tile[i, j] == null)
                    tile[i, j] = GameObject.Find(i+1 + "-" + (j+1));
                if (tile[i, j] == null)
                {
                    tile[i, j] = GameObject.Find(i+1 + "-" + (j+1) + "-card");
                    if (tile[i, j] != null)
                    {
                        partTile[l++] = tile[i, j];
                    }
                }
            }
        //for (int j = 0; j < tileVertical; j++)
        //    for (int i = 0; i < tileHorizon; i++)
        //    {
        //        for (int k = 0; k < LoadTile.Length; k++)
        //        {
        //            string[] tileName = LoadTile[k].name.Split('-');
        //            if (int.Parse(tileName[0]) - 1 == j && int.Parse(tileName[1]) - 1 == i)
        //                tile[i, j] = LoadTile[k];

        //            if (tileName.Length > 2)
        //                if (tileName[2] == "card" && l < tileCradCount)
        //                {
        //                    partTile[l++] = LoadTile[k];
        //                }

        //        }
        //    }
        l = 0;

        for (int i = 0; i < partPlacement.Length; i++)
        {
            partPlacement[i] = Random.Range(0, tileCradCount);
            for (int j = 0; j < i; j++)
                if (partPlacement[i] == partPlacement[j])
                    i--;
        }
        GetComponent<Move>().startcoroutine();
        GetComponent<PlayState>().startcoroutine();
        StartCoroutine("MakeParts");
    }

    IEnumerator MakeParts()
    {
        yield return new WaitForSeconds(0.5f);
        int m = 0;
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < int.Parse(playerinfo[0]); j++)
            {
                parts[i] = (GameObject)GameObject.Instantiate(Resources.Load("card"), Vector3.zero, Quaternion.identity);
                parts[i].transform.position = partTile[partPlacement[m]].transform.position + new Vector3(0, 0.1f, 0);
                parts[i].transform.localRotation = player[0].transform.localRotation;
                parts[i].transform.Rotate(new Vector3(0, 0, 90 * Random.Range(0, 4)));
                parts[i].transform.localScale += new Vector3(tilescale, tilescale);
                parts[i].GetComponent<SpriteRenderer>().sprite = Resources.Load("parts-" + playerinfo[j + 1] + "/c" + (i + 1).ToString(), typeof(Sprite)) as Sprite;

                if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "test2")
                    if(GetComponent<PlayState>().player[j].cardType == 1)
                    {
                        if (parts[i].GetComponent<SpriteRenderer>().sprite.name == "c2" || parts[i].GetComponent<SpriteRenderer>().sprite.name == "c3" || parts[i].GetComponent<SpriteRenderer>().sprite.name == "c6")
                        {
                            Destroy(parts[i]);
                            yield return null;
                            continue;
                        }
                    }
                    else if (GetComponent<PlayState>().player[j].cardType == 2)
                    {
                        if (parts[i].GetComponent<SpriteRenderer>().sprite.name == "c1" || parts[i].GetComponent<SpriteRenderer>().sprite.name == "c4" || parts[i].GetComponent<SpriteRenderer>().sprite.name == "c5")
                        {
                            Destroy(parts[i]);
                            yield return null;
                            continue;
                        }
                    }
                partTile[partPlacement[m]].name += "-" + playerinfo[j + 1];
                parts[i].name = partTile[partPlacement[m++]].name + "-parts";
                parts[i].tag = playerinfo[j + 1];
                GetComponent<AudioSource>().clip = Resources.Load("fx/8-bits2/laserKiss", typeof(AudioClip)) as AudioClip;
                GetComponent<AudioSource>().Play();

                yield return new WaitForSeconds(0.2f);
            }
        }
        m = 0;
    }

    public int getTileHorizon()
    {
        return tileHorizon;
    }
    public int getTileVertical()
    {
        return tileVertical;
    }
    // Update is called once per frame
    void Update () {
		
	}
}