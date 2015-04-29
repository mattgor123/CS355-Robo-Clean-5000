using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooling : MonoBehaviour {

    public GameObject explosion;
    public int explosionAmount;
    private List<GameObject> explosionList;

    public GameObject treasure;
    public int treasureAmount;
    private List<GameObject> treasureList;

	// Use this for initialization
	void Start () {
        explosionList = new List<GameObject>();
        for (int i = 0; i < explosionAmount; i++)
        {
            GameObject e = (GameObject)Instantiate(explosion);
            e.SetActive(false);
            explosionList.Add(e);
        }

        treasureList = new List<GameObject>();
        for (int i = 0; i < treasureAmount; i++)
        {
            GameObject t = (GameObject)Instantiate(treasure);
            t.SetActive(false);
            treasureList.Add(t);
        }
	}

    public GameObject getExplosion()
    {
        for (int i = 0; i < explosionList.Count; i++)
        {
            if (!explosionList[i].activeInHierarchy)
            {
                return explosionList[i];
            }
        }

        return null;
    }

    public GameObject getTreasure()
    {
        for (int i = 0; i < treasureList.Count; i++)
        {
            if (!treasureList[i].activeInHierarchy)
            {
                return treasureList[i];
            }
        }

        return null;
    }
}
