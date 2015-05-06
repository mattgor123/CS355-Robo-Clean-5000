using UnityEngine;
using System.Collections;

public class BomberRoom : MonoBehaviour {

    [SerializeField]
    GameObject Bomber;

    [SerializeField]
    private Vector3 ms1;
    [SerializeField]
    private Vector3 ms2;
    [SerializeField]
    private Vector3 ms3;
    [SerializeField]
    private Vector3 ms4;


    // Use this for initialization
    void Awake()
    {
        GameObject d = (GameObject)Instantiate(Bomber, ms1, Quaternion.identity);
        d.GetComponent<BomberBossController>().SetLaunchDelay(30f);

        d = (GameObject)Instantiate(Bomber, ms2, Quaternion.identity);
        d.GetComponent<BomberBossController>().SetLaunchDelay(60f);

        d = (GameObject)Instantiate(Bomber, ms3, Quaternion.identity);
        d.GetComponent<BomberBossController>().SetLaunchDelay(30f);

        d = (GameObject)Instantiate(Bomber, ms4, Quaternion.identity);
        d.GetComponent<BomberBossController>().SetLaunchDelay(60f);
    }
}
