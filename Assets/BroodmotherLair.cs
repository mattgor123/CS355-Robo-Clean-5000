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
    private GameObject player;
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
        player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = playerSpawn;
        Instantiate(boss, bossSpawn, Quaternion.identity);
        log = GameObject.FindWithTag("Log").GetComponent<NotificationLog>();


	}



	// Update is called once per frame
	void Update () {
	
	}
}
