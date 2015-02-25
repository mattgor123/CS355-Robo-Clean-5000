using UnityEngine;
using System.Collections;

public class PlayerGenerator : MonoBehaviour {

    public Transform Player;
    public Transform WeaponCanvas;
    public Transform Camera;

    void Start() {
        spawnPlayer();

    }

    private void spawnPlayer()
    {
        Transform player = Instantiate(Player, new Vector3(0f, 0.5f, 0f), Quaternion.identity) as Transform;
        Instantiate(WeaponCanvas);
        Instantiate(Camera, Camera.position, Camera.rotation);
        player.Translate(Vector3.zero);
    }
}
