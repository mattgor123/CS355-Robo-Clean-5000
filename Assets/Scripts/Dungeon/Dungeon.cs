using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class Dungeon : MonoBehaviour {
    //Entity Prefabs {
    public Transform Player;
    public Transform WeaponCanvas;
    public Transform Camera;
    public Transform enemy_dumb;
    public Transform enemy_smart;
    public Transform enemy_aggressive;

    //Branching Room Prefabs 
    public Transform hall_straight;
    public Transform hall_bend;
    //public Transform Hallway_Fork; //TODO: Make this (involves ensuring non-cardinal rotations work)
    public Transform Hallway_T; 
    public Transform Hallway_Cross;
    public Transform Room_2_Across;
    public Transform Room_2_Adjacent;
    public Transform Room_3;
    public Transform Room_4;


    //Deadend Prefabs
    public Transform Hallway_Deadend; 
    public Transform final_room;
    public Transform Room_1;
    public Transform closed_door;
    public Transform Stairway; 


    //Room miscellany
    public Transform roomLight;
    public Transform trigger;
    public float scale; //scale of the rooms

    //public  fields
    public int maxRooms; //kept public so that separate floors can have increased size.
    public float spawnRadius; //radius around player for picking rooms for enemy spawn.
    public int maxEnemies; //max enemies to be in dungeon at any one time;

    private float CELL_SIZE; //distance from center to edge of each cell. changes with scale variables
    private Queue unusedDoors; //Breadth-first room spawn.
    private int numEnemies;
    private Dictionary<Prefab, int> branches; //rooms/halls that branch into many paths
    private Dictionary<Prefab, int> deadends; //rooms/halls that end the path
    private int currentRoomsPlaced;
    private float lastSpawn;


    /*
     * TODO: Enemies need to be able to spawn safely within bounds. 
     * Enemies are spawned after the entire floor is generated.
     * Do enemies only spawn in rooms a certain distance from player (and constantly?):
     *      *

     * 
     */


    private class Prefab
    {
        private Transform type;
        private Vector3 distanceTo; //distance from centerpoint to next connection.
        private Vector3 distanceFrom; //distance from edge of last object to the center point of this one
        private Quaternion rotation; //rotation around center.
        private Quaternion[] doors; //array of vectors for each door of prefab pointing from the center of the object out


        public Prefab(Transform type, Vector3 distanceTo, Vector3 distanceFrom, Quaternion rotation, Quaternion[] doors)
        {
            this.type = type;
            this.distanceTo = distanceTo;
            this.distanceFrom = distanceFrom;
            this.rotation = rotation;
            this.doors = doors;
            
        }
        public Quaternion getRotation()
        {
            return this.rotation;
        }
        public Transform getTransform()
        {
            return this.type;
        }
        public Vector3 To()
        {
            return this.distanceTo;
        }
        public Vector3 From()
        {
            return this.distanceFrom;
        }
        public Quaternion getDoor(int index)
        {
            return this.doors[index];
        }
        public int numDoors()
        {
            return this.doors.Length;
        }
        public string toString()
        {
            return this.type.ToString();
        }
    }
    private class Door
    {
        private Vector3 position;
        private Vector3 facing;
        private int numRooms;

        public Door(Vector3 position, Vector3 facing, int numRooms)
        {
            this.position = position;
            this.facing = facing;
            this.numRooms = numRooms;
        }

        public Vector3 getPos()
        {
            return this.position;
        }
        public Vector3 getFace() 
        {
            return this.facing;
        }
        public int getNum()
        {
            return this.numRooms;
        }
        public void addNum(int num) {
            this.numRooms += num;
        }
        public void decrement() {
            this.numRooms--;
        }

    }

    //TODO Refactor prefabs into deadends and branches

    private void fillPrefabs()
    {

        branches = new Dictionary<Prefab, int>();
        deadends = new Dictionary<Prefab, int>();
        branches.Add( new Prefab(hall_straight,     new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0) }), 5);
        branches.Add(new Prefab(Hallway_Cross,      new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 270, 0) }), 5);
        branches.Add(new Prefab(hall_bend,          new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 270, 0), Quaternion.Euler(0, 180, 0) }), 5);
        branches.Add(new Prefab(Hallway_T,          new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 270, 0), Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 90, 0) }), 5);
        branches.Add(new Prefab(Room_2_Across,      new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0) }), 5);
        branches.Add(new Prefab(Room_2_Adjacent,    new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 270, 0) }), 5);
        branches.Add(new Prefab(Room_3,             new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 270, 0), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 90, 0) }), 5);
        branches.Add(new Prefab(Room_4,             new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 270, 0) }), 5);

        deadends.Add(new Prefab(final_room,         new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 270, 0) }), 1);
        //deadends.Add(new Prefab(Room_1,             new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 270, 0) }), 1);
        deadends.Add(new Prefab(Hallway_Deadend,    new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 180, 0) }), 4);



      
    }

    private int mod(int a, int b)
    {
        return (a - (b * Mathf.FloorToInt(a / b)));
    }

    private int[] split(int numRooms, int numDoors)
    {
        int[] value = new int[numDoors];
        int n = numRooms;
        for (int i = 0; i < numDoors -1; i++)
        {
            value[i] = 1;
            int share = n/numDoors;
            n -= share;
            value[i] += share;
        }
        value[numDoors - 1] = n;


        return value;
    }

    
    /*
     * Method for placing random rooms
     * Randomly picks entrance and primary exit
     * If !willCollide(), is placed
     * If any unused doors remain, stores them in unusedDoors queue
     * @param origin: vector3 position that tracks where an object is spawned.
     * @param room: room to be placed 
     * */
    private void placeMultiDoorRoom(Prefab room, Door door)
    {

        door.decrement(); //door has been consumed now
        int numDoors = room.numDoors(); //number of doors IN THIS ROOM ONLY
        int doorsToQueue = numDoors - 1; //number of open doors         
        int entInt = (int)Random.Range(0, numDoors - 1); //pick an entrance from room
        Quaternion[] exits = new Quaternion[doorsToQueue];
        int[] distribute = split(door.getNum(), doorsToQueue);



        Vector3 center = door.getPos() + (CELL_SIZE * door.getFace()); //move spawn point from door to center of next room
        Transform placedRoom = Instantiate(room.getTransform(), center, Quaternion.identity) as Transform; //spawn the room in normal orientation
        currentRoomsPlaced += 1;
        Vector3 entrance = room.getDoor(entInt) * placedRoom.forward; //get vector pointing at the door we want to connect with the connecting door
        placedRoom.forward = Quaternion.FromToRotation(entrance, -door.getFace()) * placedRoom.forward; //rotate transform so that proper door is lined up.
        placedRoom.transform.localScale = new Vector3(scale, 1, scale);
        placeLight(center);

        //Debug.Log("Center for this room will be " + center);
        if (placedRoom.up != Vector3.up) //ensures everything is upright (some bug somewhere is flipping random rooms upsidedown)
        {
            placedRoom.up = Quaternion.FromToRotation(placedRoom.up, Vector3.up) * placedRoom.up;
        }



        //Debug.Log("There are " + (doorsToQueue) + " doors to add");
        for (int i = 1; i < numDoors; i++)
        {
            exits[mod(i, doorsToQueue)] = room.getDoor(mod(entInt + i, numDoors));
        }
        for (int i = 0; i < doorsToQueue; i++)
        {
            int doorSplit = distribute[0]; //this door's share of the rooms
            Vector3 facing = exits[i] * placedRoom.forward; //gets a door that is NOT the entrance
            Vector3 position = center + (facing * CELL_SIZE);
            unusedDoors.Enqueue(new Door(position, facing, doorSplit));
           // Debug.Log("Added door at position " + position + " that is facing " + facing + " to queue");

        }


    }

    /*
     * places rooms and hallways that end the path
     * @param origin: vector3 position that tracks where an object is spawned
     * @param room: room to be placed
     */
    private void placeDeadEndRoom(Prefab room, Door door)
    {
        Vector3 center = door.getPos() + CELL_SIZE * door.getFace(); 
        Transform placedRoom = Instantiate(room.getTransform(), center, Quaternion.identity) as Transform; //spawn the room
        currentRoomsPlaced += 1;
        placedRoom.transform.localScale = new Vector3(scale, 1, scale);
        Vector3 entrance = room.getDoor(0) * placedRoom.forward;
        placedRoom.forward = Quaternion.FromToRotation(entrance, -door.getFace()) * placedRoom.forward;




        if (placedRoom.up != Vector3.up) //ensures everything is upright (some bug somewhere is flipping random rooms upsidedown)
        {
            placedRoom.up = Quaternion.FromToRotation(placedRoom.up, Vector3.up) * placedRoom.up;
        }
        placeLight(center);

    }

    /*
     * Casts a ray straight down at where the next room
     * would spawn. 
     * @param spawnpoint: centerpoint for next room
     * @return true if a collision would occur
     */
    private bool willCollide(Vector3 spawnpoint) {
        RaycastHit hit; //stores reference to whatever was hit 
        spawnpoint.y += 15; //to make sure Ray ray will have room to point down
        Ray ray = new Ray(spawnpoint, Vector3.down);
        return (Physics.Raycast(ray, out hit));
    }

    /*
     * Place light and associated trigger tile
     * Called right after placing a room, so 
     */ 
    private void placeLight(Vector3 center)
    {
        Transform light = Instantiate(roomLight, center + (Vector3.up * 4), Quaternion.Euler(90, 0, 0)) as Transform;
        Transform tile = Instantiate(trigger, center, Quaternion.identity) as Transform;
        light.GetComponent<WallLightController>().LinkTile(tile);
        
    }

    private void spawnStart()
    { 
        Transform placedRoom = Instantiate(Room_1, Vector3.zero, Quaternion.identity) as Transform;
        placedRoom.transform.localScale = new Vector3(scale, 1, scale);
        placeLight(Vector3.zero);
        Door door = new Door(Vector3.left * CELL_SIZE, Vector3.left, maxRooms);
        unusedDoors.Enqueue(door);
    }

    private void spawnDungeon()
    {
        bool stairwayPlaced = false;
        int lostRooms = 0; //when a door path collides, the number of rooms on that path are stored and put onto another branch
        spawnStart();
        int countdown = maxRooms;
        //TODO
        //replace for loop with while queue not empty
        //Debug.Log("There are " + unusedDoors.Count + " doors left in queue");

        while (unusedDoors.Count != 0) {
            //Debug.Log(currentRoomsPlaced);
            Door door = (Door) unusedDoors.Dequeue();
            door.addNum(lostRooms);
            lostRooms = 0;
            //Debug.Log("Currently working on door at " + door.getPos() + " facing " + door.getFace());
            //Debug.Log(door.getNum() + " rooms in this path");
            Vector3 position = door.getPos() + (door.getFace() * CELL_SIZE);
            //Debug.Log("Checking for collision at " + position);
            if (!willCollide(position) && currentRoomsPlaced < maxRooms)  //if the next prefab would clip inside an already existing thing, then it stops.
            {
                //Debug.Log("No collision!");
                if (door.getNum() == 1)
                {
                    //Debug.Log("Should close the path at " + door.getPos());
                    Prefab room = WeightedRandomizer.From(deadends).TakeOne();
                    placeDeadEndRoom(room, door);
                    countdown--;

                }
                else if (door.getNum() < 1) //just seal with door so we stay true to the actual room limit
                {

                    Transform placedRoom = Instantiate(closed_door, door.getPos(), Quaternion.identity) as Transform;
                    placedRoom.transform.localScale = new Vector3(scale, 1, scale);
                    placedRoom.rotation = Quaternion.FromToRotation(placedRoom.forward, door.getFace());
                    if (placedRoom.up != Vector3.up) //ensures everything is upright (some bug somewhere is flipping random rooms upsidedown)
                    {
                        placedRoom.up = Quaternion.FromToRotation(placedRoom.up, Vector3.up) * placedRoom.up;
                    }
                }

                else
                {
                    Prefab room = WeightedRandomizer.From(branches).TakeOne();
                    placeMultiDoorRoom(room, door);
                    countdown--;

                }
            }
            else
            {

                door.decrement();

                
                lostRooms += door.getNum();
                //Debug.Log("Collision detected at " + position);
                Transform placedRoom = null;
                if (stairwayPlaced || currentRoomsPlaced < maxRooms/2)
                {
                    placedRoom = Instantiate(closed_door, door.getPos(), Quaternion.identity) as Transform;
                }
                else
                {
                    placedRoom = Instantiate(Stairway, door.getPos(), Quaternion.identity) as Transform;
                    stairwayPlaced = true;

                }
                placedRoom.transform.localScale = new Vector3(scale, 1, scale);
                placedRoom.rotation = Quaternion.FromToRotation(placedRoom.forward, door.getFace());
                if (placedRoom.up != Vector3.up) //ensures everything is upright (some bug somewhere is flipping random rooms upsidedown)
                {
                    placedRoom.up = Quaternion.FromToRotation(placedRoom.up, Vector3.up) * placedRoom.up;
                }

            }

        }

    
    }

    private void spawnEnemies()
    {
        LayerMask EnemySpawnable = 1 << 8;
        Collider[] closeRooms = Physics.OverlapSphere(Player.position, spawnRadius * scale, EnemySpawnable); 
        //Debug.Log(closeRooms.Length + " collided objects");

        for (int i = numEnemies; i < maxEnemies; i++)
        {
            if (closeRooms.Length > 0)
            {
                Vector3 randomRoom = closeRooms[Random.Range(0, closeRooms.Length - 1)].transform.position;
                //Debug.Log(randomRoom);
                Instantiate(enemy_aggressive, randomRoom + Vector3.up, Quaternion.identity);

            }   
        }
    }

    
    private void spawnPlayer()
    {
        Transform player = Instantiate(Player, new Vector3(0f, 0.5f, 0f), Quaternion.identity) as Transform;
        Transform weaponCanvasInstance = Instantiate(WeaponCanvas) as Transform;
        Transform cameraInstance = Instantiate(Camera, Camera.position, Camera.rotation) as Transform;
        //player.Translate(Vector3.zero);
        player.position = Vector3.zero;

        DontDestroyOnLoad(player);
        DontDestroyOnLoad(weaponCanvasInstance);
        DontDestroyOnLoad(cameraInstance);
    }
    
     
    void Awake()
    {
        CELL_SIZE = 5.0f * scale;
        currentRoomsPlaced = 0;
        fillPrefabs();
        unusedDoors = new Queue();
        spawnDungeon();


    }



    void Start()
    {
        
        //add if statement if player returned is null?
        if (GameObject.FindWithTag("Player") == null) {
            spawnPlayer();
        }
        Player = GameObject.FindWithTag("Player").transform;
        
        
        spawnEnemies();
        lastSpawn = Time.time;



	}


	// Update is called once per frame
	void Update () {
        if (Time.time - lastSpawn >= 5.0f)
        {
            spawnEnemies();
        }
        lastSpawn = Time.time;
	}

}
