using UnityEngine;
using System.Collections;

public class MineController : MonoBehaviour {

    [SerializeField] private GameObject explosion;

    private void OnCollisionEnter (Collision collision) {
        var explosion_instance = Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation) as GameObject;
        Destroy(gameObject);
    }
}
