using UnityEngine;
using System.Collections;

public class BroodmotherLair : MonoBehaviour {

    [SerializeField]
    GameObject boss;
    [SerializeField]
    private Vector3 playerSpawn;
    [SerializeField]
    private Vector3 bossSpawn;
    [SerializeField]
    private Material floor_material;
    [SerializeField]
    private Material wall_material;
    [SerializeField]
    private float scale;
    [SerializeField]
    private TextAsset lair;
    [SerializeField]
    private FluffBuilder fluff_builder;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject hud;
    [SerializeField]
    private GameObject cam;
    [SerializeField]
    private GameObject triggerTile;
    private NotificationLog log;



    private Stage stage;
    private Tile[,] loadedGrid;
    private ArrayList loadedRooms;

	// Use this for initialization
	void Awake () {
        StageBuilder.scale = scale;
        stage = new Stage(lair.ToString(), floor_material, wall_material, fluff_builder);
        stage.Create();
        var playerInstance = Instantiate(player, playerSpawn, Quaternion.Euler(0, 180, 0)) as GameObject;
        var bossInstance = Instantiate(boss, bossSpawn, Quaternion.identity) as GameObject;
        var hudInstance = Instantiate(hud) as GameObject;
        var cameraInstance = Instantiate(cam, cam.transform.position, cam.transform.rotation) as GameObject;
        log = GameObject.FindWithTag("Log").GetComponent<NotificationLog>();


	}



	// Update is called once per frame
	void Update () {
	
	}
}
