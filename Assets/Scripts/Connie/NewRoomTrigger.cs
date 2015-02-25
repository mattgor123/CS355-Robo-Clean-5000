using UnityEngine;
using System.Collections;

public class NewRoomTrigger : MonoBehaviour {
    [SerializeField]
    int level;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Application.LoadLevel(level);
        }
    }
}
