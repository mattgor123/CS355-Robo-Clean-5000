using UnityEngine;
using System.Collections;

public class NewRoomTrigger : MonoBehaviour {
    [SerializeField]
    string level;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.transform.position = Vector3.zero;
            //Debug.Log("Player NEW POSITION");
            //Debug.Log(other.gameObject.transform.position);
            Application.LoadLevel(level);
        }
    }
}
