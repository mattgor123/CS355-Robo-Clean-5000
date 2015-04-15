using UnityEngine;
using System.Collections;

public class IntroCameraController : MonoBehaviour {

	[SerializeField] private Transform player;

	void Update () {
    	transform.Translate(Vector3.right * Time.deltaTime);
    	transform.LookAt(player);
	}
}
