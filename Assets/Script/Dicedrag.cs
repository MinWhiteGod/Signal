using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Dicedrag : MonoBehaviour {

    public Transform pickDice;
    bool isdrag = false;
    bool isstay = false;
    Collider2D coll;
    [SerializeField]
    int index;
    
    // Use this for initialization
    void Start () {
        index = 5;
    }
	
	// Update is called once per frame
	void Update () {
        if(isstay && !isdrag)
        {
            if(coll != null)
            {
                gameObject.transform.position = coll.transform.position;
                index = int.Parse(Regex.Replace(coll.name, @"\D", ""));
                print(index);
                coll.isTrigger = false;
                isstay = false;
            }
        }
        if (Input.GetMouseButtonUp(0))
            isdrag = false;
    }

    public void Drag()
    {
        isdrag = true;
        pickDice.transform.position = Input.mousePosition;
        if(coll != null)
        {
            coll.isTrigger = true;
            coll = null;
        }

    }
    public void Drop()
    {
        isdrag = false;
    }
    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.tag == "cell")
        {
            isstay = true;
            coll = c;
        }
    }
    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.tag == "cell")
        {
            isstay = true;
            coll = c;
        }
    }
    private void OnTriggerExit2D(Collider2D c)
    {
        if(isstay)
        {
            isstay = false;
            index = 5;
        }
    }
    public int getIndex()
    {
        return index;
    }
    public Collider2D getCollider()
    {
        return coll;
    }
    public string getSprite()
    {
        return GetComponent<Image>().sprite.name;
    }
    public void setInit()
    {
        isstay = false;
        index = 5;
        if (coll != null)
        {
            coll.isTrigger = true;
            coll = null;
        }

    }
}
