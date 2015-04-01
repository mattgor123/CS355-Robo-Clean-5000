using UnityEngine;
using System.Collections;

public class StageBuilder : MonoBehaviour
{

    #region StageBuilders private variables
    private Stage stage;
    [SerializeField]
    private int WIDTH_MUST_BE_ODD;
    [SerializeField]
    private int HEIGHT_MUST_BE_ODD;
    [SerializeField]
    private int NUMBER_ROOM_TRIES;
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
    private float Accessiblescale; //scale of the rooms, this one is editable from Unity
    [SerializeField]
    private int KillCycles;
    [SerializeField]
    private float spawnRadius; //radius around player for picking rooms for enemy spawn.
    [SerializeField]
    private int maxEnemies; //max enemies to be in dungeon at any one time;
    private static int numEnemies;
    public static float scale; //static and public so its reachable across classes
    [SerializeField]
    private Material[] floorMaterials;
    [SerializeField]
    private Material[] wallMaterials;
    #endregion


    /*Everything related to creating the dungeon itself */
    void Awake()
    {
        Application.targetFrameRate = 60    ;
        QualitySettings.vSyncCount = 0;
        scale = Accessiblescale;
        stage = new Stage(WIDTH_MUST_BE_ODD, HEIGHT_MUST_BE_ODD, floorMaterials, wallMaterials);
        //TODO: place rooms first. 
        stage._addRooms(NUMBER_ROOM_TRIES);
        stage.PlaceHalls();
        stage.createDoors();
        stage.removeDeadEnds();
        stage.Create();
        spawnPlayer();
        Player = GameObject.FindWithTag("Player").transform;
    }

	/*Spawning the player and game elements besides dungeon and enemies */
	void Start () {



        float lastSpawn = Time.time;
        //Randomly choose which audio clip to play for this dungeon
        PlayRandom();

	}
	
	/*Currently only used to ensure enemies are refreshed */
	void Update () {
        if (numEnemies < maxEnemies)
        {
            spawnEnemies();
            numEnemies++;

        }

    }

    /*Spawns player, camera, HUD and others */
    private void spawnPlayer()
    {
        //Get room to spawn in
        //TODO: add a pickSpawnableTile() class to expand number of locations
        //perhaps label Floor tiles so that OverlapSphere gets them?
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


    /* Spawns enemies
     * TODO: Use pickSpawnableTile()
     */
    private void spawnEnemies()
    {

        var spawnableTiles = Physics.OverlapSphere(Player.position, 50 * StageBuilder.scale, 1 << LayerMask.NameToLayer("EnemySpawnable"));
        var randomLocation = spawnableTiles[UnityEngine.Random.Range(0, spawnableTiles.Length)].transform.position;
        
        if (Random.Range(0f, 1f) <= .5)
        {
            Instantiate(enemy_aggressive, new Vector3(randomLocation.x, 0, randomLocation.z) + Vector3.up * 3, Quaternion.identity);
        }
        else
        {
            Instantiate(enemy_smart, new Vector3(randomLocation.x, 0, randomLocation.z) + Vector3.up, Quaternion.identity);
        }
    }

    /*Pick random background music */
    void PlayRandom()
    {
        int index = Random.Range(0, dungeon_backgrounds.Length);
        //Debug.Log("Now playing song " + index);
        audio.clip = dungeon_backgrounds[index];
        audio.loop = true;
        audio.Play();
    }

    /*Global method that let's EnemyController inform StageBuilder when an enemy dies */
    public static void EnemyDied()
    {
        numEnemies--;
    }

    public void nextLevel(int level)
    {
        stage.NextLevel(level);
    }
}
