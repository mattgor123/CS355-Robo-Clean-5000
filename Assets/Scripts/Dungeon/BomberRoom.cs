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
        d.GetComponent<BomberBossController>().SetLaunchDelay(30f + Random.value * 5);

        d = (GameObject)Instantiate(Bomber, ms2, Quaternion.identity);
        d.GetComponent<BomberBossController>().SetLaunchDelay(60f + Random.value * 5);

        d = (GameObject)Instantiate(Bomber, ms3, Quaternion.identity);
        d.GetComponent<BomberBossController>().SetLaunchDelay(30f + Random.value * 5);

        d = (GameObject)Instantiate(Bomber, ms4, Quaternion.identity);
        d.GetComponent<BomberBossController>().SetLaunchDelay(60f + Random.value * 5);

        Vector3 left = new Vector3(15, 0, 50);
        Vector3 right = new Vector3(75, 0, 50);

        for (float i = 0; i <= 40; i += 10)
        {
            d = (GameObject)Instantiate(Bomber, left + new Vector3(0, 0, i), Quaternion.identity);
            d.GetComponent<BomberBossController>().SetLaunchDelay(60f + Random.value * 60f);
            d.transform.Rotate(new Vector3(0, 90, 0));

            d = (GameObject)Instantiate(Bomber, right + new Vector3(0, 0, i), Quaternion.identity);
            d.GetComponent<BomberBossController>().SetLaunchDelay(60f + Random.value * 60f);
            d.transform.Rotate(new Vector3(0, -90, 0));
        }
    }
}
