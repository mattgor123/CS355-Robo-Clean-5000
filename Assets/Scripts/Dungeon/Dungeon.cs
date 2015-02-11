using UnityEngine;
using System.Collections;

public class Dungeon : MonoBehaviour {
    public Transform player;
    public Transform hall_straight;
    public Transform hall_bend;
    //public Transform Hallway_Fork;
    //public Transform Hallway_T;
    //public Transform Hallway_Cross;
    //public Transform Hallway_Deadend;
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
        prefabs[0] = new Prefab(hall_straight,   new Vector3(0, 0, 5), new Vector3(0, 0, 5),  1, Quaternion.Euler(0, 0, 0),   new Quaternion[] { Quaternion.identity});
        for (int i = 1; i < 15; i++)
        {
            prefabs[i] = prefabs[0];
        }

        prefabs[15] = new Prefab(hall_bend, new Vector3(0, 0, -5), new Vector3(0, 0, -5), 1, Quaternion.Euler(0, 0, 0), new Quaternion[] { Quaternion.Euler(0, -90, 0) });
        for (int i = 16; i < 20; i++)
        {
            prefabs[i] = prefabs[15];
        }
        prefabs[20] = new Prefab(Room_1,          new Vector3(0, 0, 15), new Vector3(0, 0, 5), 0, Quaternion.Euler(0, 90, 0),  new Quaternion[] { Quaternion.identity });
        prefabs[21] = new Prefab(Room_2_Across,   new Vector3(0, 0, 5), new Vector3(0, 0, 5), 1, Quaternion.Euler(0, 90, 0),  new Quaternion[] { Quaternion.identity });
        prefabs[22] = new Prefab(Room_2_Adjacent, new Vector3(0, 0, 5), new Vector3(0, 0, 5), 1, Quaternion.Euler(0, 90, 0),  new Quaternion[] { Quaternion.Euler(0, 0, 0) });
        prefabs[23] = new Prefab(Room_3,          new Vector3(0, 0, 5), new Vector3(0, 0, 5), 2, Quaternion.Euler(0, 90, 0),  new Quaternion[] { Quaternion.identity, Quaternion.Euler(0, 0, 0) });
        prefabs[24] = new Prefab(Room_4,          new Vector3(0, 0, 5), new Vector3(0, 0, 5), 3, Quaternion.Euler(0, 0, 0),   new Quaternion[] { Quaternion.identity, Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, -90, 0) });



    }

	void Start () {
     
        
        //Room1
        Debug.Log(origin.ToString());
        Instantiate(prefabs[20].getTransform(), origin, prefabs[20].getRotation());
        origin += prefabs[20].getDistanceTo();
        Debug.Log(origin.ToString());
        Quaternion rotation = prefabs[20].getExit(0);
        //Hall_Bend
        origin += prefabs[15].getDistanceFrom();
        Debug.Log(origin.ToString());
        Instantiate(prefabs[15].getTransform(), origin, rotation);
        origin += Vector3.RotateTowards(prefabs[15].getDistanceTo(), Vector3.left * prefabs[15].getDistanceTo().magnitude, Mathf.PI / 2, 10);
        Debug.Log(origin.ToString());
        rotation = rotation * prefabs[15].getExit(0);
        //Hall_Straight
        origin += Vector3.RotateTowards(prefabs[1].getDistanceFrom(), Vector3.left * prefabs[1].getDistanceFrom().magnitude, Mathf.PI / 2, 10);
        Debug.Log(origin.ToString());
        Instantiate(prefabs[1].getTransform(), origin, rotation);
        origin += Vector3.RotateTowards(prefabs[1].getDistanceTo(), Vector3.left * prefabs[1].getDistanceTo().magnitude, Mathf.PI / 2, 10);
        Debug.Log(origin.ToString());
        rotation = rotation * prefabs[1].getExit(0);
        //Hall_Bend
        origin += Vector3.RotateTowards(prefabs[15].getDistanceFrom(), Vector3.left * prefabs[15].getDistanceFrom().magnitude, Mathf.PI / 2, 10);
        Debug.Log(origin.ToString());
        Instantiate(prefabs[15].getTransform(), origin, rotation);
        origin += Vector3.RotateTowards(prefabs[15].getDistanceTo(), Vector3.back * prefabs[15].getDistanceTo().magnitude, Mathf.PI / 2, 10);
        Debug.Log(origin.ToString());
        rotation = rotation * prefabs[15].getExit(0);
        //Hall_Bend
        origin += Vector3.RotateTowards(prefabs[15].getDistanceFrom(), Vector3.back * prefabs[15].getDistanceFrom().magnitude, Mathf.PI / 2, 10);
        Debug.Log(origin.ToString());
        Instantiate(prefabs[15].getTransform(), origin, rotation);
        origin += Vector3.RotateTowards(prefabs[15].getDistanceTo(), Vector3.right * prefabs[15].getDistanceTo().magnitude, Mathf.PI / 2, 10);
        rotation = rotation * prefabs[15].getExit(0);
        //Hall_Straight
        origin += Vector3.RotateTowards(prefabs[1].getDistanceFrom(), Vector3.right * prefabs[1].getDistanceFrom().magnitude, Mathf.PI / 2, 10);
        Debug.Log(origin.ToString());
        Instantiate(prefabs[1].getTransform(), origin, rotation);
        origin += Vector3.RotateTowards(prefabs[1].getDistanceTo(), Vector3.right * prefabs[1].getDistanceTo().magnitude, Mathf.PI / 2, 10);
        Debug.Log(origin.ToString());
        rotation = rotation * prefabs[1].getExit(0);

       /* for (int i = 0; i < 10; i++)
        {
            int next = (int)Random.Range(0, 25);
            origin += prefabs[next].getDistanceFrom();
            Instantiate(prefabs[next].getTransform(), origin, rotation);
            origin += prefabs[next].getDistanceTo();
            rotation = rotation * prefabs[next].getExit(0);

        }
        * /
        //Instantiate(prefabs[next], origin, Quaternion.identity);
      /*  origin.y -= 0.7f;
        Instantiate(Room_1, origin, Quaternion.Euler(0, 90, 0));
        origin.z = +15; //Radius of room = 5, then +10 for the hallway
        Instantiate(hall_straight, origin, Quaternion.identity);
        origin.z += 10; 
        Instantiate(hall_straight, origin, Quaternion.identity);
        origin.z += 10;
        Instantiate(hall_straight, origin, Quaternion.identity);
        origin.z += 5;
        Instantiate(Room_2_Across, origin, Quaternion.Euler(0, 90, 0));
        origin.z += 10;
        Instantiate(Room_2_Adjacent, origin, Quaternion.Euler(0, 90, 0));
        */
        Instantiate(player, Vector3.zero, Quaternion.identity);



	}

   // private void Instantiate(Prefab prefab, Vector3 origin, Quaternion quaternion)
   // {
   //      throw new System.NotImplementedException();
   // }
	
	// Update is called once per frame
	void Update () {

	
	}
}
