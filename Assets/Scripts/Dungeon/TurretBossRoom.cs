using UnityEngine;
using System.Collections;

//Additional functionality for the turret boss room
public class TurretBossRoom : MonoBehaviour {

    [SerializeField]
    GameObject Mob;

    [SerializeField]
    private Vector3 ms1;
    [SerializeField]
    private Vector3 ms2;
    [SerializeField]
    private Vector3 ms3;
    [SerializeField]
    private Vector3 ms4;
    [SerializeField]
    private Vector3 ms5;
    [SerializeField]
    private Vector3 ms6;
    

	// Use this for initialization
	void Awake () {
        Object obs = Instantiate(Mob, ms1, Quaternion.identity);
        Instantiate(Mob, ms2, Quaternion.identity);
        Instantiate(Mob, ms3, Quaternion.identity);
        Instantiate(Mob, ms4, Quaternion.identity);
        Instantiate(Mob, ms5, Quaternion.identity);
        Instantiate(Mob, ms6, Quaternion.identity);
	}


}
