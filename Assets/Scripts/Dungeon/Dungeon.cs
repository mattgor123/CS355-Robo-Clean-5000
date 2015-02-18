using UnityEngine;
using System.Collections;

public class Dungeon : MonoBehaviour {
   // public Transform player;
    public Transform hall_straight;
    public Transform hall_bend;
    public Transform hall_bend_right;

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

        
        /*
        //Room1
        Instantiate(prefabs[20].getTransform(), origin, prefabs[20].getRotation());
        origin += prefabs[20].getDistanceTo();
        Quaternion rotation = prefabs[20].getExit(0);
        //Hall_Bend
        origin += prefabs[15].getDistanceFrom();
        Instantiate(prefabs[15].getTransform(), origin, rotation);
        origin += Vector3.RotateTowards(prefabs[15].getDistanceTo(), Vector3.left * prefabs[15].getDistanceTo().magnitude, Mathf.PI / 2, 10);
        rotation = rotation * prefabs[15].getExit(0);
        //Hall_Straight
        origin += Vector3.RotateTowards(prefabs[1].getDistanceFrom(), Vector3.left * prefabs[1].getDistanceFrom().magnitude, Mathf.PI / 2, 10);
        Instantiate(prefabs[1].getTransform(), origin, rotation);
        origin += Vector3.RotateTowards(prefabs[1].getDistanceTo(), Vector3.left * prefabs[1].getDistanceTo().magnitude, Mathf.PI / 2, 10);
        rotation = rotation * prefabs[1].getExit(0);
        //Hall_Bend
        origin += Vector3.RotateTowards(prefabs[15].getDistanceFrom(), Vector3.left * prefabs[15].getDistanceFrom().magnitude, Mathf.PI / 2, 10);
        Instantiate(prefabs[15].getTransform(), origin, rotation);
        origin += Vector3.RotateTowards(prefabs[15].getDistanceTo(), Vector3.back * prefabs[15].getDistanceTo().magnitude, Mathf.PI / 2, 10);
        rotation = rotation * prefabs[15].getExit(0);
        //Hall_Bend
        origin += Vector3.RotateTowards(prefabs[15].getDistanceFrom(), Vector3.back * prefabs[15].getDistanceFrom().magnitude, Mathf.PI / 2, 10);
        Instantiate(prefabs[15].getTransform(), origin, rotation);
        origin += Vector3.RotateTowards(prefabs[15].getDistanceTo(), Vector3.right * prefabs[15].getDistanceTo().magnitude, Mathf.PI / 2, 10);
        rotation = rotation * prefabs[15].getExit(0);
        //Hall_Straight
        origin += Vector3.RotateTowards(prefabs[1].getDistanceFrom(), Vector3.right * prefabs[1].getDistanceFrom().magnitude, Mathf.PI / 2, 10);
        Instantiate(prefabs[1].getTransform(), origin, rotation);
        origin += Vector3.RotateTowards(prefabs[1].getDistanceTo(), Vector3.right * prefabs[1].getDistanceTo().magnitude, Mathf.PI / 2, 10);
        rotation = rotation * prefabs[1].getExit(0);
        */
        
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
            origin += Quaternion.FromToRotation(prefabs[next].getDistanceFrom(), facingAngle) * prefabs[next].getDistanceFrom(); //move spawn point from edge of last object to center of next object
            Instantiate(prefabs[next].getTransform(), origin, rotation);
            facingAngle = prefabs[next].getExit(0) * facingAngle;
            origin += Quaternion.FromToRotation(prefabs[next].getDistanceTo(), facingAngle) * prefabs[next].getDistanceTo(); 

            rotation = rotation * prefabs[next].getExit(0);

        }
         //setting finishing room
        origin += Quaternion.FromToRotation(prefabs[23].getDistanceFrom(), facingAngle) * prefabs[23].getDistanceFrom(); //move spawn point from edge of last object to center of next object
        Instantiate(prefabs[23].getTransform(), origin, rotation);


        //Instantiate(player, Vector3.zero, Quaternion.identity);



	}

   // private void Instantiate(Prefab prefab, Vector3 origin, Quaternion quaternion)
   // {
   //      throw new System.NotImplementedException();
   // }
	
	// Update is called once per frame
	void Update () {

	
	}
}
