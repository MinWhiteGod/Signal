using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RollDice : MonoBehaviour {

    protected float rollTime = 0;
    public float rollSpeed = 0.25F;
    public static bool rolling = true;
    // reference to the dice that have to be rolled
    private static ArrayList rollQueue = new ArrayList();
    // reference to all dice, created by Dice.Roll
    private static ArrayList allDice = new ArrayList();
    // reference to the dice that are rolling
    private static ArrayList rollingDice = new ArrayList();
    // control
    [HideInInspector]
    public GameObject[] controlDice = new GameObject[5];

    private GameObject spawnPoint = null;
    private string mate = "d6-yellow";
    const float diceSize = 1.2f;
    string color;
    GameObject btnDice;
    // Use this for initialization
    void Start () {
        controlDice = GameObject.FindGameObjectsWithTag("diceControl");
        for (int i = 0; i < 5; i++)
        {
            controlDice[i].SetActive(false);
        }
        
    }
    public void btnClick()
    {
        spawnPoint = GameObject.Find("spawnPoint");

        Clear();
        string a = "go_dice";
        Roll("5d" + a, mate, spawnPoint.transform.position, Force());
        //GetComponent<Button>().interactable = false;
        GameObject gameManager = GameObject.Find("GameManager");
        color = gameManager.GetComponent<PlayState>().player[gameManager.GetComponent<Move>().turn].color;
        btnDice = gameObject.GetComponent<InitGame>().btnDice[gameManager.GetComponent<Move>().turn];
        //GameObject.Find("btnSTART_" + color).GetComponent<Button>().interactable = false;
        btnDice.SetActive(false);
        StartCoroutine("Initalgo");
    }
    IEnumerator Initalgo()
    {
        yield return new WaitForSeconds(4.0f);
        for (int d = 0; d < allDice.Count; d++)
        {
            RollingDie rDie = (RollingDie)allDice[d];
            // check the type
            controlDice[d].SetActive(true);
            Vector3 controlVector = new Vector3();
            Vector3 target = GameObject.Find("frame_" + color).GetComponent<CardChild>().parts[d].transform.position;
            switch (color)
            {
                case "yellow":
                    controlVector = new Vector3(0, -56);
                    break;
                case "green":
                    controlVector = new Vector3(56, 0);
                    break;
                case "blue":
                    controlVector = new Vector3(-56, 0);
                    break;
                case "red":
                    controlVector = new Vector3(0, 56);
                    break;
            }
            controlDice[d].transform.position = target + controlVector; //new Vector3(450 + 100 * d, 325);
            controlDice[d].transform.rotation = btnDice.transform.rotation;
            switch (rDie.die.value)
            {
                case 0:
                    controlDice[d].GetComponent<Image>().sprite = Resources.Load("straight", typeof(Sprite)) as Sprite;
                    //iTween.ShakePosition(GameObject.Find("DicePlace_1"), new Vector3(0, 0.5f, 0), 0.5f);

                    //d--;
                    break;
                case 1:
                    controlDice[d].GetComponent<Image>().sprite = Resources.Load("left", typeof(Sprite)) as Sprite;
                    break;
                case 2:
                    controlDice[d].GetComponent<Image>().sprite = Resources.Load("repeat", typeof(Sprite)) as Sprite;
                    break;
                case 3:
                    controlDice[d].GetComponent<Image>().sprite = Resources.Load("right", typeof(Sprite)) as Sprite;
                    break;
                case 4:
                    controlDice[d].GetComponent<Image>().sprite = Resources.Load("straight", typeof(Sprite)) as Sprite;
                    break;
                case 5:
                    controlDice[d].GetComponent<Image>().sprite = Resources.Load("straight", typeof(Sprite)) as Sprite;
                    break;
                case 6:
                    controlDice[d].GetComponent<Image>().sprite = Resources.Load("straight", typeof(Sprite)) as Sprite;
                    break;
            }
            yield return null;
        }
        yield return new WaitForSeconds(1.0f);
        GameObject.Find("btnSTART_" + color).GetComponent<Button>().interactable = true;
        for (int i = 0; i < 5; i++)
        {
            RollingDie rDie = (RollingDie)allDice[i];

            Destroy(rDie.gameObject);
        }

        yield return null;
    }
    // Update is called once per frame
    void Update () {
        if (rolling)
        {
            // there are dice rolling so increment rolling time
            rollTime += Time.deltaTime;
            // check rollTime against rollSpeed to determine if a die should be activated ( if one available in the rolling  queue )
            if (rollQueue.Count > 0 && rollTime > rollSpeed)
            {
                // get die from rolling queue
                RollingDie rDie = (RollingDie)rollQueue[0];
                GameObject die = rDie.gameObject;
                // activate the gameObject
                die.SetActive(true);
                // apply the force impuls
                die.GetComponent<Rigidbody>().AddForce((Vector3)rDie.force, ForceMode.Impulse);
                // apply a random torque
                die.GetComponent<Rigidbody>().AddTorque(new Vector3(-50 * Random.value * die.transform.localScale.magnitude, -50 * Random.value * die.transform.localScale.magnitude, -50 * Random.value * die.transform.localScale.magnitude), ForceMode.Impulse);
                // add die to rollingDice
                rollingDice.Add(rDie);
                // remove the die from the queue
                rollQueue.RemoveAt(0);
                // reset rollTime so we can check when the next die has to be rolled
                rollTime = 0;
            }
            else
                if (rollQueue.Count == 0)
            {
                // roll queue is empty so if no dice are rolling we can set the rolling attribute to false
                if (!IsRolling())
                    rolling = false;
            }
        }
    }
    private Vector3 Force()
    {
        Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
        return Vector3.Lerp(spawnPoint.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
    }
    private bool IsRolling()
    {
        int d = 0;
        // loop rollingDice
        while (d < rollingDice.Count)
        {
            // if rolling die no longer rolling , remove it from rollingDice
            RollingDie rDie = (RollingDie)rollingDice[d];
            if (!rDie.rolling)
                rollingDice.Remove(rDie);
            else
                d++;
        }
        // return false if we have no rolling dice 
        return (rollingDice.Count > 0);
    }
    public static void Roll(string dice, string mat, Vector3 spawnPoint, Vector3 force)
    {
        rolling = true;
        // sotring dice to lowercase for comparing purposes
        int count = 5;
        string dieType = "go_dice";

        // 'd' must be present for a valid 'dice' specification
        int p = dice.IndexOf("d");
        if (p >= 0)
        {

            // instantiate the dice
            for (int d = 0; d < count; d++)
            {
                // randomize spawnPoint variation
                spawnPoint.x = spawnPoint.x - 1 + Random.value * 2;
                spawnPoint.y = spawnPoint.y - 1 + Random.value * 2;
                spawnPoint.y = spawnPoint.y - 1 + Random.value * 2;
                // create the die prefab/gameObject
                GameObject die = prefab(dieType, spawnPoint, Vector3.zero, Vector3.one * diceSize, mat);
                // give it a random rotation
                die.transform.Rotate(new Vector3(Random.value * 360, Random.value * 360, Random.value * 360));
                // inactivate this gameObject because activating it will be handeled using the rollQueue and at the apropriate time
                die.SetActive(false);
                // create RollingDie class that will hold things like spawnpoint and force, to be used when activating the die at a later stage
                RollingDie rDie = new RollingDie(die, dieType, mat, spawnPoint, force);
                // add RollingDie to allDices
                allDice.Add(rDie);
                // add RollingDie to the rolling queue
                rollQueue.Add(rDie);
            }
        }
    }
    public static int Value(string dieType)
    {
        int v = 0;
        // loop all dice
        for (int d = 0; d < allDice.Count; d++)
        {
            RollingDie rDie = (RollingDie)allDice[d];
            // check the type
            if (rDie.name == dieType || dieType == "")
                v += rDie.die.value;
            Debug.Log(rDie.die.value);
        }
        return v;
    }
    public static void Clear()
    {
        for (int d = 0; d < allDice.Count; d++)
            GameObject.Destroy(((RollingDie)allDice[d]).gameObject);

        allDice.Clear();
        rollingDice.Clear();
        rollQueue.Clear();

        rolling = false;
    }
    public static GameObject prefab(string name, Vector3 position, Vector3 rotation, Vector3 scale, string mat)
    {
        // load the prefab from Resources
        Object pf = Resources.Load(name);
        if (pf != null)
        {
            // the prefab was found so create an instance for it.
            GameObject inst = (GameObject)GameObject.Instantiate(pf, Vector3.zero, Quaternion.identity);
            if (inst != null)
            {
                // the instance could be created so set material, position, rotation and scale.
                //if (mat != "") inst.GetComponent<Renderer>().material = material(mat);
                inst.transform.position = position;
                inst.transform.Rotate(rotation);
                inst.transform.localScale = scale;
                // return the created instance (GameObject)
                return inst;
            }
        }
        else
            Debug.Log("Prefab " + name + " not found!");
        return null;
    }
}

