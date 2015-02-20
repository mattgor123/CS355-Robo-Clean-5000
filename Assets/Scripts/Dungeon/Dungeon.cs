using UnityEngine;
using System.Collections;

public class Dungeon : MonoBehaviour {
    public Transform Player;
    public Transform Camera;
    public Transform enemy_dumb;
    public Transform enemy_smart;
    public Transform enemy_aggressive;
    public Transform hall_straight;
    public Transform hall_bend;
    //public Transform Hallway_Fork; //TODO: Make this
    //public Transform Hallway_T; //TODO: Make this
    //public Transform Hallway_Cross; //TODO: Make this
    //public Transform Hallway_Deadend; //TODO: Make this
    public Transform final_room;
    public Transform Room_1;
    public Transform Room_2_Across;
    public Transform Room_2_Adjacent;
    public Transform Room_3;
    public Transform Room_4;
    public Transform roomLight;
    public Vector3 origin;
    public int maxRooms;
    public int maxHalls;
    private Queue unusedDoors;
    private Vector4 spawnable; //
    private Prefab[] prefabs;


    private class Prefab
    {
        private Transform type;
        private Vector3 distanceTo; //distance from centerpoint to next connection.
        private Vector3 distanceFrom; //distance from edge of last object to the center point of this one
        private Quaternion rotation; //rotation around center.
        private Vector3[] doors; //array of vectors for each door of prefab pointing from the center of the object out


        public Prefab(Transform type, Vector3 distanceTo, Vector3 distanceFrom, Quaternion rotation, Vector3[] doors)
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
        public Vector3 getDoor(int index)
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
            // TODO: Complete member initialization
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
    }
    //TODO Refactor prefabs into deadends and branches
    private Vector3 facingAngle;

    private void fillPrefabs()
    {
        prefabs = new Prefab[30];
        prefabs[0] = new Prefab(hall_straight, new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.forward, Vector3.back });
        //prefabs[1] = new Prefab(hall_bend_right, new Vector3(5, 0, 0),  new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.right, Vector3.back });
        prefabs[2] = new Prefab(hall_bend, new Vector3(5, 0, 0), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.left, Vector3.back });
        prefabs[3] = new Prefab(Room_1, new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.left });
        prefabs[4] = new Prefab(Room_2_Across, new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.left, Vector3.right });
        prefabs[5] = new Prefab(Room_2_Adjacent, new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.forward, Vector3.right });
        prefabs[6] = new Prefab(Room_3, new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.left, Vector3.forward, Vector3.right });
        prefabs[7] = new Prefab(Room_4, new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.forward, Vector3.right, Vector3.back, Vector3.left });
        prefabs[8] = new Prefab(final_room, new Vector3(0, 0, 5), new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.back });
        for (int i = 9; i < 20; i++) //straight hallways
        {
            prefabs[i] = prefabs[0];
        }
        for (int i = 20; i < 25; i++) //bend_right hallways
        {
            prefabs[i] = prefabs[2];
        }
        for (int i = 25; i < 30; i++) //bend_left hallways
        {
            prefabs[i] = prefabs[2];
        }
        prefabs[1] = prefabs[2];
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
    private void placeMultiDoorRoom(Prefab room)
    {

        //TODO Implement split function with Unused Doors queue
        //Place room;
        //foreach door in room, place door in queue;
        //call split function to assign each door number of rooms;
        //grab top of queue
        //repeat 


        
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
        if (placedRoom.up != Vector3.up) //ensures everything is upright (some bug somewhere is flipping random rooms upsidedown)
        {
            placedRoom.transform.Rotate(Vector3.forward, 180);
        }
        facingAngle = rotation * exit; //direction origin should be modified in
        placeLight(origin);
        origin += Quaternion.FromToRotation(room.To(), facingAngle) * room.To();  //move spawn point from center of new object to whichever exit
    }

    /*
     * places rooms and hallways that end the path
     * @param origin: vector3 position that tracks where an object is spawned
     * @param room: room to be placed
     */
    private void placeDeadEndRoom(Prefab room)
    {
        Vector3 entrance = room.getDoor(0);
        Quaternion rotation = Quaternion.FromToRotation(entrance, -facingAngle); 
        origin += Quaternion.FromToRotation(room.From(), facingAngle) * room.From(); 
        Instantiate(room.getTransform(), origin, rotation);
        Transform placedRoom = Instantiate(room.getTransform(), origin, rotation) as Transform; //spawn the room
        if (placedRoom.up != Vector3.up) //ensures everything is upright (some bug somewhere is flipping random rooms upsidedown)
        {
            placedRoom.transform.Rotate(Vector3.forward, 180);
        }
        placeLight(origin);

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

    private void placeLight(Vector3 location)
    {
        Instantiate(roomLight, location + (Vector3.up * 4), Quaternion.Euler(90, 0, 0));
    }

    private void spawnStart(Prefab room)
    {
        Instantiate(prefabs[3].getTransform(), origin, prefabs[3].getRotation());
        placeLight(origin);
        Door door = new Door(origin, prefabs[3].getDoor(0), maxRooms);
        unusedDoors.Enqueue(door);
        Vector3 exit = prefabs[3].getDoor(0);
        facingAngle = Vector3.forward;
        facingAngle = exit;
        origin += Quaternion.FromToRotation(room.To(), facingAngle) * room.To();
    }

    private void spawnDungeon()
    {

        spawnStart(prefabs[3]);

        //TODO
        //replace for loop with while queue not empty
        for (int i = 0; i < 20; i++)
        {

            int next = (int)Random.Range(0, 30); //generate next random prefab
            Prefab room = prefabs[next];
            Vector3 position = origin + Quaternion.FromToRotation(room.From(), facingAngle) * room.From();
            if (!willCollide(position))  //if the next prefab would clip inside an already existing thing, then it stops.
            {
                if (room.numDoors() > 1)
                {
                    placeMultiDoorRoom(room);
                }
                else // dead end room
                {
                    placeDeadEndRoom(room);
                }
            }
        }

    
    }

    private void spawnEnemies()
    {
        
    }

    private void spawnPlayer()
    {
        Transform playerTransform = Instantiate(Player, Vector3.zero, Quaternion.identity) as Transform;
        Instantiate(Camera, Camera.position, Camera.rotation);

    }

    void Awake()
    {
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
