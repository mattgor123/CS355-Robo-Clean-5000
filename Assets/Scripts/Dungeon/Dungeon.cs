using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class Dungeon : MonoBehaviour {
    //Entity Prefabs {
    public Transform Player;
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

    //Room miscellany
    public Transform roomLight;
    public Transform trigger;

    //Private fields
    private Vector3 origin; //Do we ever need this public? room spawn location
    public int maxRooms; //kept public so that separate floors can have increased size.
    private Queue unusedDoors; //Breadth-first room spawn.
    public float spawnRadius; //radius around player for picking rooms for enemy spawn.
    private float CELL_SIZE; //radius of each cell. as in each square is 10x10 (but 5units from center to edge)
    private Dictionary<Prefab, int> branches; //rooms/halls that branch into many paths
    private Dictionary<Prefab, int> deadends; //rooms/halls that end the path


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


        internal int numDoors()
        {
            return this.doors.Length;
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
        public void decrement() {
            this.numRooms--;
        }
    }

    //TODO Refactor prefabs into deadends and branches
    private Vector3 facingAngle;

    private void fillPrefabs()
    {

        branches = new Dictionary<Prefab, int>();
        deadends = new Dictionary<Prefab, int>();
        branches.Add( new Prefab(hall_straight,     new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0) }), 5);
        branches.Add(new Prefab(Hallway_Cross,      new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 270, 0) }), 5);
        branches.Add(new Prefab(hall_bend,          new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 270, 0), Quaternion.Euler(0, 180, 0) }), 5);
        branches.Add(new Prefab(Hallway_T,          new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 270, 0), Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 90, 0) }), 5);
        branches.Add(new Prefab(Room_2_Across,      new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 180, 0) }), 2);
        branches.Add(new Prefab(Room_2_Adjacent,    new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 270, 0) }), 2);
        branches.Add(new Prefab(Room_3,             new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 270, 0), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 90, 0) }), 2);
        branches.Add(new Prefab(Room_4,             new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, 180, 0), Quaternion.Euler(0, 270, 0) }), 2);

        deadends.Add(new Prefab(final_room,         new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 180, 0) }), 1);
        deadends.Add(new Prefab(Room_1,             new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 270, 0) }), 1);
        deadends.Add(new Prefab(Hallway_Deadend,    new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 180, 0) }), 1);



      
    }

    private int mod(int a, int b)
    {
        return (a - (b * Mathf.FloorToInt(a / b)));
    }

    private int[] split(int numRooms, int numDoors)
    {
        int[] value = new int[numDoors];
        int n = numRooms - numDoors;
        for (int i = 0; i < numDoors -1; i++)
        {
            value[i] = 1;
            int share = Mathf.CeilToInt(n * Random.value);
            n -= share;
            value[i] += share;
        }
        value[numDoors - 1] = 1 + n;


        return value;
    }

    
    /*
     * Method for placing rooms/halls with multiple doors
     * Randomly picks entrance and primary exit
     * If !willCollide(), is placed
     * If any unused doors remain, stores them in unusedDoors queue
     * @param origin: vector3 position that tracks where an object is spawned.
     * @param room: room to be placed 
     * */
    private void placeMultiDoorRoom(Prefab room, Door door)
    {

        //TODO Implement split function with Unused Doors queue
        //Place room;
        //foreach door in room, place door in queue;
        //call split function to assign each door number of rooms;
        //grab top of queue
        //repeat 
        door.decrement(); //door has been consumed now
        int numDoors = room.numDoors(); //number of doors IN THIS ROOM ONLY
        int queueRooms = numDoors - 1; //number of open doors


        Vector3 center = door.getPos() + (CELL_SIZE * door.getFace()); //move spawn point from door to center of next room
        Transform placedRoom = Instantiate(room.getTransform(), center, Quaternion.identity) as Transform; //spawn the room in normal orientation

        int entInt = (int)Random.Range(0, numDoors - 1); //pick an entrance from room
        Vector3 entrance = room.getDoor(entInt) * placedRoom.forward; //get vector pointing at the door we want to connect with the connecting door
        if (Vector3.Dot(entrance, -door.getFace()) != -1)
        {
            placedRoom.rotation = Quaternion.FromToRotation(entrance, -door.getFace()); //rotate transform so that proper door is lined up.
        }
        Debug.Log("Center for this room will be " + center);
            if (placedRoom.up != Vector3.up) //ensures everything is upright (some bug somewhere is flipping random rooms upsidedown)
            {
                placedRoom.up = Quaternion.FromToRotation(placedRoom.up, Vector3.up) * placedRoom.up;
            }
        placeLight();

        int[] distribute = split(door.getNum(), queueRooms);
        Debug.Log("There are " + (numDoors - 1) + " doors to add");
        for (int i = 1; i < numDoors; i++) {
            int index = mod(entInt + i, numDoors);
            Vector3 facing =  room.getDoor(index) * placedRoom.forward ; //gets a door that is NOT the entrance
            Vector3 position = center + (facing * CELL_SIZE);
            unusedDoors.Enqueue(new Door(position, facing, distribute[mod(i, queueRooms)]));
            Debug.Log("Added door at position " + position + " that is facing " + facing + " to queue");
        
        }
    

       /* 
        int numDoors = room.numDoors();
        int entInt = (int)Random.Range(0, numDoors - 1); //pick an entrance
        int exitInt = (int)Random.Range(0, numDoors - 1); //pick an exit

        while (exitInt == entInt) //make sure that they aren't the same door
        {
            exitInt = (exitInt + 1) % (room.numDoors());
        }

        Vector3 entrance = room.getDoor(entInt); //randomized entrance
        Vector3 exit = room.getDoor(exitInt); //randomized exit
        Quaternion rotation = Quaternion.FromToRotation(entrance, -facingAngle); //rotation needed to align entrance with previous room's exit
        origin += Quaternion.FromToRotation(room.From(), facingAngle) * room.From(); //move spawn point from edge of last object to center of next object
        Transform placedRoom = Instantiate(room.getTransform(), origin, rotation) as Transform; //spawn the room
        for (int i = 0; i < numDoors; i++)
            if (placedRoom.up != Vector3.up) //ensures everything is upright (some bug somewhere is flipping random rooms upsidedown)
            {
                placedRoom.up = Quaternion.FromToRotation(placedRoom.up, Vector3.up) * placedRoom.up;
            }
        facingAngle = rotation * exit; //direction origin should be modified in
        placeLight();
        origin += Quaternion.FromToRotation(room.To(), facingAngle) * room.To();  //move spawn point from center of new object to whichever exit
        */
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
        Vector3 entrance = room.getDoor(0) * placedRoom.forward;
        placedRoom.rotation = Quaternion.FromToRotation(entrance, -door.getFace());



        if (placedRoom.up != Vector3.up) //ensures everything is upright (some bug somewhere is flipping random rooms upsidedown)
        {
            placedRoom.up = Quaternion.FromToRotation(placedRoom.up, Vector3.up) * placedRoom.up;
        }
        placeLight();

    }

    /*
     * Casts a ray straight down at where the next room
     * would spawn. 
     * @param spawnpoint: centerpoint for next room
     * @return true if a collision would occur
     */
    private bool willCollide(Vector3 spawnpoint) {
        RaycastHit hit; //stores reference to whatever was hit 
        spawnpoint.y += 10; //to make sure Ray ray will have room to point down
        Ray ray = new Ray(spawnpoint, Vector3.down);
        return (Physics.Raycast(ray, out hit));
    }

    /*
     * Place light and associated trigger tile
     * Called right after placing a room, so 
     */ 
    private void placeLight()
    {
        Transform light = Instantiate(roomLight, origin + (Vector3.up * 4), Quaternion.Euler(90, 0, 0)) as Transform;
        Transform tile = Instantiate(trigger, origin, Quaternion.identity) as Transform;
        light.GetComponent<WallLightController>().LinkTile(tile);
        
    }

    private void spawnStart()
    { 
        Instantiate(Room_1, Vector3.zero, Quaternion.identity);
        placeLight();
        Door door = new Door( new Vector3(-5, 0, 0),Vector3.left, maxRooms);
        unusedDoors.Enqueue(door);
        facingAngle = Vector3.left;
        origin += Vector3.left*5;
    }

    private void spawnDungeon()
    {
        int lostRooms = 0;
        spawnStart();
        int limit = 0;
        //TODO
        //replace for loop with while queue not empty
        Debug.Log("There are " + unusedDoors.Count + " doors left in queue");

        while (unusedDoors.Count != 0) {
            limit++;
            Door door = (Door) unusedDoors.Dequeue();
            Debug.Log("Currently working on door at " + door.getPos() + " facing " + door.getFace());
            Debug.Log(door.getNum() + " rooms in this path");
            Vector3 position = door.getPos() + (door.getFace() * CELL_SIZE);
            Debug.Log("checking for collision at " + position);
            if (!willCollide(position))  //if the next prefab would clip inside an already existing thing, then it stops.
            {
                Debug.Log("No collision!");
                  if (door.getNum() == 1)
                  {
                      Prefab room = WeightedRandomizer.From(deadends).TakeOne();
                      placeDeadEndRoom(room, door);
                  }
            
                 else {
                    Prefab room = WeightedRandomizer.From(branches).TakeOne();
                    placeMultiDoorRoom(room, door);
                  }
            }
            else
            {
                Debug.Log("Collision detected at " + position);
                Transform placedRoom = Instantiate(closed_door, door.getPos(), Quaternion.identity) as Transform;
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
        
    }

    private void spawnPlayer()
    {
        Transform player = Instantiate(Player, new Vector3(0f, 0.5f, 0f), Quaternion.identity) as Transform;
        //Physics.OverlapSphere(player.position, spawnRadius);
        Instantiate(Camera, Camera.position, Camera.rotation);

    }

    void Awake()
    {
        CELL_SIZE = 5.0f;
        fillPrefabs();
        unusedDoors = new Queue();
        spawnDungeon();
       // spawnEnemies();
        spawnPlayer();

    }



    void Start()
    {



	}


	// Update is called once per frame
	void Update () {

	
	}

}
