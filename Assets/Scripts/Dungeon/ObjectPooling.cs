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

    public int bulletAmount;

    public GameObject machineGunBullet;
    private List<GameObject> machineGunBulletList;

    public GameObject pistolBullet;
    private List<GameObject> pistolBulletList;

    public GameObject crazyGunBullet;
    private List<GameObject> crazyGunBulletList;

    public GameObject rayGunBullet;
    private List<GameObject> rayGunBulletList;


	void Awake () {
        explosionList = new List<GameObject>();
        StartCoroutine(makeExplosions());

        treasureList = new List<GameObject>();
        StartCoroutine(makeTreasures());

        enemy_smartList = new List<GameObject>();
        StartCoroutine(makeEnemySmarts());

        enemy_aggressiveList = new List<GameObject>();
        StartCoroutine(makeEnemyAggressives());

        pistolBulletList = new List<GameObject>();
        StartCoroutine(makePistolBullets());

        crazyGunBulletList = new List<GameObject>();
        StartCoroutine(makeCrazyGunBullets());

        machineGunBulletList = new List<GameObject>();
        StartCoroutine(makeMachineGunBullets());

        rayGunBulletList = new List<GameObject>();
        StartCoroutine(makeRayGunBullets());
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
        GameObject e = (GameObject)Instantiate(enemy_smart);
        e.SetActive(false);
        enemy_smartList.Add(e);
        return e;
        //return null;
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

    private IEnumerator makeExplosions()
    {
        while (explosionList.Count < explosionAmount)
        {
            for (int i = 0; i < 10; i++) {
                GameObject e = (GameObject)Instantiate(explosion);
                e.SetActive(false);
                explosionList.Add(e);
            }
            yield return null;
        }
    }

    private IEnumerator makeTreasures()
    {
        while (treasureList.Count < treasureAmount)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject t = (GameObject)Instantiate(treasure);
                t.SetActive(false);
                treasureList.Add(t);
            }
            yield return null;
        }
    }

    private IEnumerator makeEnemySmarts()
    {
        while (enemy_smartList.Count < enemy_smartAmount)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject e = (GameObject)Instantiate(enemy_smart);
                e.SetActive(false);
                enemy_smartList.Add(e);
            }
            yield return null;
        }
    }

    private IEnumerator makeEnemyAggressives()
    {
        while (enemy_aggressiveList.Count < enemy_aggressiveAmount)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject e = (GameObject)Instantiate(enemy_aggressive);
                e.SetActive(false);
                enemy_aggressiveList.Add(e);
            }
            yield return null;
        }
    }

    private IEnumerator makePistolBullets()
    {
        while (pistolBulletList.Count < bulletAmount)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject b = (GameObject)Instantiate(pistolBullet);
                b.SetActive(false);
                pistolBulletList.Add(b);
            }
            yield return null;
        }
    }

    private IEnumerator makeCrazyGunBullets()
    {
        while (crazyGunBulletList.Count < bulletAmount)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject b = (GameObject)Instantiate(crazyGunBullet);
                b.SetActive(false);
                crazyGunBulletList.Add(b);
            }
            yield return null;
        }
    }

    private IEnumerator makeMachineGunBullets()
    {
        while (machineGunBulletList.Count < bulletAmount)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject b = (GameObject)Instantiate(machineGunBullet);
                b.SetActive(false);
                machineGunBulletList.Add(b);
            }
            yield return null;
        }
    }

    private IEnumerator makeRayGunBullets()
    {
        while (rayGunBulletList.Count < bulletAmount)
        {
            for (int i = 0; i < 10; i++)
            {
                GameObject b = (GameObject)Instantiate(rayGunBullet);
                b.SetActive(false);
                rayGunBulletList.Add(b);
            }
            yield return null;
        }
    }

    public GameObject getPistolBullet()
    {
        for (int i = 0; i < pistolBulletList.Count; i++)
        {
            if (!pistolBulletList[i].activeInHierarchy)
            {
                return pistolBulletList[i];
            }
        }

        GameObject t = (GameObject)Instantiate(pistolBullet);
        t.SetActive(false);
        pistolBulletList.Add(t);
        return t;
    }

    public GameObject getCrazyGunBullet()
    {
        for (int i = 0; i < crazyGunBulletList.Count; i++)
        {
            if (!crazyGunBulletList[i].activeInHierarchy)
            {
                return crazyGunBulletList[i];
            }
        }

        GameObject t = (GameObject)Instantiate(crazyGunBullet);
        t.SetActive(false);
        crazyGunBulletList.Add(t);
        return t;
    }

    public GameObject getMachineGunBullet()
    {
        for (int i = 0; i < machineGunBulletList.Count; i++)
        {
            if (!machineGunBulletList[i].activeInHierarchy)
            {
                return machineGunBulletList[i];
            }
        }

        GameObject t = (GameObject)Instantiate(machineGunBullet);
        t.SetActive(false);
        machineGunBulletList.Add(t);
        return t;
    }

    public GameObject getRayGunBullet()
    {
        for (int i = 0; i < rayGunBulletList.Count; i++)
        {
            if (!rayGunBulletList[i].activeInHierarchy)
            {
                return rayGunBulletList[i];
            }
        }

        GameObject t = (GameObject)Instantiate(rayGunBullet);
        t.SetActive(false);
        rayGunBulletList.Add(t);
        return t;
    }

}
