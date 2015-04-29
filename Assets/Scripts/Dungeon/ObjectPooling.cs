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

    public GameObject enemy_smart;
    public int enemy_smartAmount;
    private List<GameObject> enemy_smartList;

    public GameObject enemy_aggressive;
    public int enemy_aggressiveAmount;
    private List<GameObject> enemy_aggressiveList;

	void Awake () {
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

        enemy_smartList = new List<GameObject>();
        for (int i = 0; i < enemy_smartAmount; i++)
        {
            GameObject es = (GameObject)Instantiate(enemy_smart);
            es.SetActive(false);
            enemy_smartList.Add(es);
        }

        enemy_aggressiveList = new List<GameObject>();
        for (int i = 0; i < enemy_aggressiveAmount; i++)
        {
            GameObject ea = (GameObject)Instantiate(enemy_aggressive);
            ea.SetActive(false);
            enemy_aggressiveList.Add(ea);
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

        GameObject t = (GameObject)Instantiate(treasure);
        t.SetActive(false);
        treasureList.Add(t);
        return t;

        //return null;
    }

    public GameObject getEnemySmart()
    {
        for (int i = 0; i < enemy_smartList.Count; i++)
        {
            if (!enemy_smartList[i].activeInHierarchy)
            {
                return enemy_smartList[i];
            }
        }

        return null;
    }

    public GameObject getEnemyAggressive()
    {
        for (int i = 0; i < enemy_aggressiveList.Count; i++)
        {
            if (!enemy_aggressiveList[i].activeInHierarchy)
            {
                return enemy_aggressiveList[i];
            }
        }

        return null;
    }
}
