using UnityEngine;
using System.Collections;

public class StageBuilder : MonoBehaviour
{

    #region StageBuilders private variables
    private Stage stage;
    [SerializeField]
    private Transform Player;
    [SerializeField]
    private Transform HUD;
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
    private Transform turret;
    [SerializeField]
    private GameObject hurt_canvas;
    [SerializeField]
    private AudioClip[] dungeon_backgrounds;
    [SerializeField]
    private AudioSource audio;
    [SerializeField]
    private Transform WallLight;
    [SerializeField]
    private Transform CornerLight;
    [SerializeField]
    private Transform trigger;
    [SerializeField]
    private float Accessiblescale; //scale of the rooms, this one is editable from Unity
    [SerializeField]
    private int KillCycles;
    [SerializeField]
    private float spawnRadius; //radius around player for picking rooms for enemy spawn.
    [SerializeField]
    private int enemiesPerLevel;
    private int maxEnemies; //max enemies to be in dungeon at any one time;
    private static int numEnemies;
    public static float scale; //static and public so its reachable across classes
    [SerializeField]
    private Material[] floorMaterials;
    [SerializeField]
    private Material[] wallMaterials;
    [SerializeField]
    private FluffBuilder FBuilder;
    private Transform player;
    private Transform hudInstance;
    private Transform cameraInstance;
    //weights of enemy. row is floor, column is weight in order of aggressive, smart, turret
    //ex: 0.3, 0.6, 1. if random float is 0-0.3, aggressive
    //if float is 0.3-0.6, smart
    //if float is 0.6-1, turret
    private float[,] enemyWeights;
    private ObjectPooling pool;
    #endregion

    /*Everything related to creating the dungeon itself */
    void Awake()
    {
        Application.targetFrameRate = 60    ;
        QualitySettings.vSyncCount = 0;
        scale = Accessiblescale;
        stage = new Stage(floorMaterials, wallMaterials, FBuilder);
        //TODO: place rooms first. 
        stage._addRooms();
        stage.PlaceHalls();
        stage.createDoors();
        stage.removeDeadEnds();
        stage.Create();
        spawnPlayer();
        Player = GameObject.FindWithTag("Player").transform;
        maxEnemies = (Player.gameObject.GetComponent<PlayerController>().getCurrentFloor() + 1) * enemiesPerLevel;
        numEnemies = 0;

        enemyWeights = new float[6, 3];

        enemyWeights[0, 0] = 0.5f;
        enemyWeights[0, 1] = 1f;
        enemyWeights[0, 2] = 0;

        enemyWeights[1, 0] = 0.4f;
        enemyWeights[1, 1] = 0.8f;
        enemyWeights[1, 2] = 1f;

        enemyWeights[2, 0] = 0.35f;
        enemyWeights[2, 1] = 0.7f;
        enemyWeights[2, 2] = 1f;

        enemyWeights[3, 0] = 0.35f;
        enemyWeights[3, 1] = 0.7f;
        enemyWeights[3, 2] = 1f;

        enemyWeights[4, 0] = 0.35f;
        enemyWeights[4, 1] = 0.7f;
        enemyWeights[4, 2] = 0.3f;

        enemyWeights[5, 0] = 0.35f;
        enemyWeights[5, 1] = 0.35f;
        enemyWeights[5, 2] = 0.3f;

        pool = GameObject.Find("ObjectPool").GetComponent<ObjectPooling>();
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


        var spawnableTiles = Physics.OverlapSphere(Player.position, 50 * StageBuilder.scale, 1 << LayerMask.NameToLayer("EnemySpawnable"));
        var spawnpoint = spawnableTiles[UnityEngine.Random.Range(0, spawnableTiles.Length)].transform.position;
        if (player == null)
        {
            player = Instantiate(Player, spawnpoint, Quaternion.identity) as Transform;
        }
        if (hudInstance == null)
        {
            hudInstance = Instantiate(HUD) as Transform;
        }
        if (cameraInstance == null)
        {
            cameraInstance = Instantiate(Camera, Camera.position, Camera.rotation) as Transform;
        }

        //so player will persist even when new scene is loaded
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(hudInstance);
        DontDestroyOnLoad(cameraInstance);
    }


    /* Spawns enemies
     * TODO: Use pickSpawnableTile()
     */
    private void spawnEnemies()
    {

        var spawnableTiles = Physics.OverlapSphere(Player.position, 50 * StageBuilder.scale, 1 << LayerMask.NameToLayer("EnemySpawnable"));
        var randomLocation = spawnableTiles[UnityEngine.Random.Range(0, spawnableTiles.Length - 1)].transform.position;

        int floor = Player.gameObject.GetComponent<PlayerController>().getCurrentFloor();

        float chance = Random.Range(0f, 1f);

        if (chance <= enemyWeights[floor, 0])
        {
            //Instantiate(enemy_aggressive, new Vector3(randomLocation.x, 0, randomLocation.z) + Vector3.up * 1.5f, Quaternion.identity);
            GameObject ea = pool.getEnemyAggressive();
            ea.transform.position = new Vector3(randomLocation.x, 0, randomLocation.z) + Vector3.up * 1.5f;
            ea.transform.rotation = Quaternion.identity;
            ea.SetActive(true);
        }
        else if (chance <= enemyWeights[floor, 1])
        {
            //Instantiate(enemy_smart, new Vector3(randomLocation.x, 0, randomLocation.z) + Vector3.up, Quaternion.identity);
            GameObject es = pool.getEnemySmart();
            es.transform.position = new Vector3(randomLocation.x, 0, randomLocation.z) + Vector3.up;
            es.transform.rotation = Quaternion.identity;
            es.SetActive(true);
        }
        else if (chance <= enemyWeights[floor, 2])
        {
            Instantiate(turret, new Vector3(randomLocation.x, 0, randomLocation.z) + Vector3.up, Quaternion.identity);
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

    public static int NumberEnemies()
    {
        return numEnemies;
    }

    public void nextLevel(int level)
    {
        foreach(var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            //GameObject.Destroy(enemy);
            enemy.SetActive(false);
        }
        numEnemies = 0;
        stage.NextLevel(level);
        maxEnemies = (Player.gameObject.GetComponent<PlayerController>().getCurrentFloor() + 1) * enemiesPerLevel;
    }
}
