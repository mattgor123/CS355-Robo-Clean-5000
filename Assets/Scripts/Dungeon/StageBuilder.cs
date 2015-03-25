using UnityEngine;
using System.Collections;

public class StageBuilder : MonoBehaviour {

    private Stage stage;
    [SerializeField]
    private int WIDTH_MUST_BE_ODD;
    [SerializeField]
    private int HEIGHT_MUST_BE_ODD;
    [SerializeField]
    private int NUMBER_ROOM_TRIES;
    [SerializeField]
    private Material wallMaterial;
    [SerializeField]
    private Material floorMaterial;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private Transform WeaponCanvas;
    [SerializeField]
    private Transform HealthCanvas;
    [SerializeField]
    private Transform AmmoCanvas;
    [SerializeField]
    private Transform NotificationCanvas;
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private Transform Gun;
    [SerializeField]
    private Transform enemy_dumb;
    [SerializeField]
    private Transform enemy_smart;
    [SerializeField]
    private Transform enemy_aggressive;
    [SerializeField]
    private GameObject hurt_canvas;
    [SerializeField]
    private AudioClip[] dungeon_backgrounds;
    [SerializeField]
    private AudioSource audio;
    [SerializeField]
    private Transform roomLight;
    [SerializeField]
    private Transform trigger;
    [SerializeField]
    private float scale; //scale of the rooms
    [SerializeField]
    private int KillCycles;
    [SerializeField]
    private float spawnRadius; //radius around player for picking rooms for enemy spawn.
    [SerializeField]
    private int maxEnemies; //max enemies to be in dungeon at any one time;
    private static int numEnemies;
    

    private void spawnPlayer()
    {
        Room room = stage.RandomRoom();
        var roomCenter = room.GetRoomCenter() * scale;
        Vector3 spawnpoint = new Vector3(roomCenter.x - 1, 1f, roomCenter.y - 1);
        Transform player = Instantiate(Player, spawnpoint, Quaternion.identity) as Transform;
        Transform weaponCanvasInstance = Instantiate(WeaponCanvas) as Transform;
        Transform healthCanvasInstance = Instantiate(HealthCanvas) as Transform;
        Transform ammoCanvasInstance = Instantiate(AmmoCanvas) as Transform;
        Transform notificationCanvasInstance = Instantiate(NotificationCanvas) as Transform;
        Transform cameraInstance = Instantiate(Camera, Camera.position, Camera.rotation) as Transform;

        //so player will persist even when new scene is loaded
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(weaponCanvasInstance);
        DontDestroyOnLoad(healthCanvasInstance);
        DontDestroyOnLoad(ammoCanvasInstance);
        DontDestroyOnLoad(notificationCanvasInstance);
        DontDestroyOnLoad(cameraInstance);
    }

    private void spawnEnemies()
    {
        Vector2 randomRoom = stage.RandomRoom().GetRoomCenter() * scale;
        if (Random.Range(0f, 1f) <= .5)
        {
            Instantiate(enemy_aggressive, new Vector3(randomRoom.x, 0, randomRoom.y) + Vector3.up * 3, Quaternion.identity);
        }
        else
        {
            Instantiate(enemy_smart, new Vector3(randomRoom.x, 0, randomRoom.y) + Vector3.up * 3, Quaternion.identity);
        }
    }


    void Awake()
    {
        stage = new Stage(WIDTH_MUST_BE_ODD, HEIGHT_MUST_BE_ODD, floorMaterial, wallMaterial, scale);
        stage.PlaceHalls();
        stage.PlaceRooms(NUMBER_ROOM_TRIES);
        //stage.removeDeadEnds();
        stage.Create(KillCycles);
    }

	// Use this for initialization
	void Start () {

        spawnPlayer();
        Player = GameObject.FindWithTag("Player").transform;

        float lastSpawn = Time.time;
        //Randomly choose which audio clip to play for this dungeon
        PlayRandom();

	}
	
	// Update is called once per frame
	void Update () {
        if (numEnemies < maxEnemies)
        {
            spawnEnemies();
            numEnemies++;
        }
    }

    void PlayRandom()
    {
        int index = Random.Range(0, dungeon_backgrounds.Length);
        Debug.Log("Now playing song " + index);
        audio.clip = dungeon_backgrounds[index];
        audio.loop = true;
        audio.Play();
    }

    public static void EnemyDied()
    {
        numEnemies--;
    }
}
