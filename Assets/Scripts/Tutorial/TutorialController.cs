using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {

	[SerializeField] private GameObject player;
	[SerializeField] private GameObject hud;
	[SerializeField] private GameObject camera;
	[SerializeField] private Vector3 spawn_point;

	private Stage stage;
	private Tile[,] loadedGrid;
	private ArrayList loadedRooms;

	void Awake () {
		loadedGrid = new Tile[50,50];
		loadedRooms = new ArrayList();
		stage = new Stage(loadedGrid, loadedRooms);
		stage.Create();
	}

	void Start () {
		var playerInstance = Instantiate(player, spawn_point, Quaternion.identity) as GameObject;
        var hudInstance = Instantiate(hud) as GameObject;
        var cameraInstance = Instantiate(camera, camera.transform.position, camera.transform.rotation) as GameObject;
	}
}
