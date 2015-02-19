using UnityEngine;
using System.Collections;

public class Dungeon : MonoBehaviour {
   // public Transform player;
    public Transform hall_straight;
    public Transform hall_bend;
    //public Transform hall_bend_right;

    //public Transform Hallway_Fork;
    //public Transform Hallway_T;
    //public Transform Hallway_Cross;
    //public Transform Hallway_Deadend;
    public Transform final_room;
    public Transform Room_1;
    public Transform Room_2_Across;
    public Transform Room_2_Adjacent;
    public Transform Room_3;
    public Transform Room_4;
    public Vector3 origin;
    public Vector3 rotation;
    public int maxRooms;
    public int maxHalls;
    private float floor;
	// Use this for initialization

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
        public Vector3 getDistanceTo()
        {
            return this.distanceTo;
        }
        public Vector3 getDistanceFrom()
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
    private Prefab[] prefabs;



    void Awake()
    {
        prefabs = new Prefab[30];
        prefabs[0] = new Prefab(hall_straight,   new Vector3(0, 0, 5),  new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.forward, Vector3.back });
        //prefabs[1] = new Prefab(hall_bend_right, new Vector3(5, 0, 0),  new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.right, Vector3.back });
        prefabs[2] = new Prefab(hall_bend,       new Vector3(5, 0, 0),  new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.left, Vector3.back });
        prefabs[3] = new Prefab(Room_1,          new Vector3(0, 0, 5),  new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.left });
        prefabs[4] = new Prefab(Room_2_Across,   new Vector3(0, 0, 5),  new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.left, Vector3.right });
        prefabs[5] = new Prefab(Room_2_Adjacent, new Vector3(0, 0, 5),  new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.forward, Vector3.right });
        prefabs[6] = new Prefab(Room_3,          new Vector3(0, 0, 5),  new Vector3(0, 0, 5), Quaternion.Euler(0, 0, 0), new Vector3[] { Vector3.left, Vector3.forward, Vector3.right });
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

    void Start()
    {

        //set up the starting room
        Instantiate(prefabs[3].getTransform(), origin, prefabs[3].getRotation());
        //pick random door
        Vector3 facingAngle = Vector3.forward;
        Vector3 exit = prefabs[3].getDoor((int)Random.Range(0, prefabs[3].numDoors() - 1));
        Quaternion rotation = Quaternion.FromToRotation(facingAngle, exit); //find rotation from z-axis, since all the doors are relative to that.
        facingAngle = exit;
        Vector3 to = prefabs[3].getDistanceTo(); //shortener
        Vector3 from = prefabs[3].getDistanceFrom(); //shortener
        origin += Quaternion.FromToRotation(to, facingAngle) * to;
        //for loop to generate set number of space
        for (int i = 0; i < 20; i++)
        {

            int next = (int)Random.Range(0, 30); //generate next random prefab
            to = prefabs[next].getDistanceTo(); //shortener
            from = prefabs[next].getDistanceFrom(); //shortener
            int entInt = 0;
            Vector3 position = origin + Quaternion.FromToRotation(from, facingAngle) * from;
            position.y += 10;
            RaycastHit hit;
            Ray ray = new Ray(position, Vector3.down);
            if (!Physics.Raycast(ray, out hit))
            {
                //if the next prefab would clip inside an already existing thing, then it stops.
                //TODO: Store unused doors so that they can be referenced when the current path is consumed



                if (prefabs[next].numDoors() > 1)
                {
                    entInt = (int)Random.Range(0, prefabs[next].numDoors() - 1); //pick an entrance
                    int exitInt = (int)Random.Range(0, prefabs[next].numDoors());
                    while (exitInt == entInt)
                    {
                        exitInt = (exitInt + 1) % (prefabs[next].numDoors());
                    }

                    Vector3 entrance = prefabs[next].getDoor(entInt); //randomized entrance for rooms with multiple doors
                    exit = prefabs[next].getDoor(exitInt); //randomized exit for rooms with multiple doors

                    if (Vector3.Dot(entrance, facingAngle) != -1)
                    {
                        rotation = Quaternion.FromToRotation(entrance, -facingAngle); //we want these two vectors facing each other
                    }
                    if (rotation.eulerAngles.z != 0)
                    {
                        rotation = Quaternion.Euler(0, 0, 180) * rotation;
                    }
                    origin += Quaternion.FromToRotation(from, facingAngle) * from; //move spawn point from edge of last object to center of next object
                    Instantiate(prefabs[next].getTransform(), origin, rotation); //span object 'entrance' with lined up thanks to 'rotation'
                    exit = rotation * exit; //make exit relative now
                    facingAngle = exit; //direction origin should be modified in
                    origin += Quaternion.FromToRotation(to, facingAngle) * to;  //move spawn point from center of new object to whichever exit
                }
                else //room1 or final_room
                {
                    Vector3 entrance = prefabs[next].getDoor(entInt);
                    if (Vector3.Dot(entrance, facingAngle) != -1)
                    {
                        rotation = Quaternion.FromToRotation(entrance, -facingAngle); //we want these two vectors facing each other
                    }
                    origin += Quaternion.FromToRotation(from, facingAngle) * from; //move spawn point from edge of last object to center of next object
                    if (rotation.eulerAngles.z != 0)
                    {
                        rotation = Quaternion.Euler(0, 0, 180) * rotation;
                    }
                    Instantiate(prefabs[next].getTransform(), origin, rotation); //span object 'entrance' with lined up thanks to 'rotation'
                    break;
                }
            }
        }




/* Version without random door selection
    private class Prefab
    {
        private Transform type;
        private Vector3 distanceTo; //distance from centerpoint to next connection.
        private Vector3 distanceFrom; //distance from edge of last object to the center point of this one
        private Quaternion rotation; //rotation around center.
        private int numExits; //number of openings that can be filled by another room/hallway. NOT INCLUDING ALREADY USED ENTRANCE
        private Quaternion[] exitAngles; //the angle relative to the current object's center point


        public Prefab(Transform type, Vector3 distanceTo, Vector3 distanceFrom, int numExits, Quaternion rotation, Quaternion[] exitAngles)
        {
            this.type = type;
            this.distanceTo = distanceTo;
            this.distanceFrom = distanceFrom;
            this.rotation = rotation; 
            this.numExits = numExits;
            this.exitAngles = exitAngles;

        }
        public Quaternion getRotation()
        {
            return this.rotation;
        }
        public Transform getTransform()
        {
            return this.type;
        }
        public Vector3 getDistanceTo()
        {
            return this.distanceTo;
        }
        public Vector3 getDistanceFrom()
        {
            return this.distanceFrom;
        }
        public Quaternion getExit(int index)
        {
            return this.exitAngles[index];
        }

        internal int getNumExits()
        {
            return this.numExits;
        }
    }
    private Prefab[] prefabs;



    void Awake() {
        prefabs = new Prefab[25];
        prefabs[0] = new Prefab(hall_straight,   new Vector3(0, 0, 5), new Vector3(0, 0, 5),  1, Quaternion.Euler(0, 0, 0),   new Quaternion[] { Quaternion.Euler(0,0,0)});
        for (int i = 1; i < 10; i++)
        {
            prefabs[i] = prefabs[0];
        }
        prefabs[10] = new Prefab(hall_bend_right, new Vector3(5, 0, 0), new Vector3(0, 0, 5), 1, Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 90, 0) });
        for (int i = 11; i < 15; i++)
        {
            prefabs[i] = prefabs[10];
        }

        prefabs[15] = new Prefab(hall_bend, new Vector3(-5, 0, 0), new Vector3(0, 0, 5), 1, Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, -90, 0) });
        for (int i = 15; i < 20; i++)
        {
            prefabs[i] = prefabs[15];
        }
        prefabs[24] = new Prefab(Room_1,          new Vector3(0, 0, 5), new Vector3(0, 0, 5), 0, Quaternion.Euler(0, 0, 0),  new Quaternion[] { Quaternion.Euler(0, -90, 0) });
        prefabs[21] = new Prefab(Room_2_Across,   new Vector3(0, 0, 5), new Vector3(0, 0, 5), 1, Quaternion.Euler(0, 0, 0),  new Quaternion[] { Quaternion.identity });
        prefabs[22] = new Prefab(Room_2_Adjacent, new Vector3(0, 0, 5), new Vector3(0, 0, 5), 1, Quaternion.Euler(0, 90, 0),  new Quaternion[] { Quaternion.Euler(0, -90, 0) });
       // prefabs[23] = new Prefab(Room_3,          new Vector3(0, 0, 5), new Vector3(0, 0, 5), 2, Quaternion.Euler(0, 90, 0),  new Quaternion[] { Quaternion.identity, Quaternion.Euler(0, 90, 0) });
        prefabs[20] = new Prefab(Room_4,          new Vector3(0, 0, 5), new Vector3(0, 0, 5), 3, Quaternion.Euler(0, 0, 0),   new Quaternion[] { Quaternion.identity, Quaternion.Euler(0, 90, 0), Quaternion.Euler(0, -90, 0) });
        prefabs[21] = prefabs[20];
        prefabs[22] = prefabs[20];
        prefabs[23] = new Prefab(final_room,      new Vector3(0, 0, 5), new Vector3(0, 0, 5), 0, Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, 180, 0) });


    }

	void Start () {

        //set up the starting room
        Vector3 facingAngle = new Vector3(0, 0, 1);
        Instantiate(prefabs[24].getTransform(), origin, prefabs[24].getRotation());
        facingAngle = prefabs[24].getExit(0) * facingAngle;
        origin += Quaternion.FromToRotation(prefabs[24].getDistanceTo(), facingAngle) * prefabs[24].getDistanceTo();
        Quaternion rotation = prefabs[24].getExit(0);
        //for loop to generate set number of space
        for (int i = 0; i < 10; i++)
        {

            int next = (int)Random.Range(0, 23); //generate next random prefab
            Vector3 to = prefabs[next].getDistanceTo(); //shortener
            Vector3 from = prefabs[next].getDistanceFrom(); //shortener
            Quaternion exit = prefabs[next].getExit((int)Random.Range(0, prefabs[next].getNumExits())); //randomized exit for rooms with multiple
            origin += Quaternion.FromToRotation(from, facingAngle) * from; //move spawn point from edge of last object to center of next object

            //This section is for calculating the rotation necessary to line up a door of the new object with chosen the door of the previous one
            //  while (Vector3.Dot(rotation * -(to), facingAngle) != -1) {
            //      rotation = rotation * Quaternion.Euler(0, 90, 0);
            //  }
            Debug.Log(Vector3.Dot((rotation * -(to)).normalized, facingAngle.normalized));

            Instantiate(prefabs[next].getTransform(), origin, rotation); //span object 'entrance' with lined up thanks to 'rotation'
            facingAngle = exit * facingAngle; //direction origin should be modified in
            origin += Quaternion.FromToRotation(to, facingAngle) * to;  //move spawn point from center of new object to whichever exit

            rotation = rotation * exit; //Modify rotation relative to chosen exit, so next room lines up.

        }
        //setting finishing room
        origin += Quaternion.FromToRotation(prefabs[23].getDistanceFrom(), facingAngle) * prefabs[23].getDistanceFrom(); //move spawn point from edge of last object to center of next object
        Instantiate(prefabs[23].getTransform(), origin, rotation);


        //Instantiate(player, Vector3.zero, Quaternion.identity);
    */


	}


	
	// Update is called once per frame
	void Update () {

	
	}
}
