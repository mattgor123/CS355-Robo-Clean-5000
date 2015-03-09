using System;
using UnityEngine;


public class Tile
{
    private bool used;
	/*Tile Constructor */ public Tile()
	{
        used = false;
	}


    /*FloorTile
     * The basic ground tile
     * Has walls on each side of tile depending on bool members
     */
    public class Floor : Tile
    {
        

        private GameObject floor;
        private GameObject northWall;
        private GameObject southWall;
        private GameObject westWall;
        private GameObject eastWall;
        private Vector3 position;
        private bool used;
        
        
        private float x; //x is x-coordinate of tile
        private float z; //z is z-coordinate of tile
        private bool North; //North = True if north edge of tile is blocked
        private bool South; //South = True if south edge of tile is blocked
        private bool West;  //West  = True if west edge of tile is blocked
        private bool East;  //East  = True if east edge of tile is blocked
        public Floor(float x, float z, bool North, bool South, bool West, bool East) {
            this.North = North;
            this.South = South;
            this.West = West;
            this.East = East;
            this.position = new Vector3(x, 0.5f, z);
            this.used = true;
            this.x = x;
            this.z = z;
        }


        public new void Create()
        {
            if (used)
            {
                floor = GameObject.CreatePrimitive(PrimitiveType.Quad);
                floor.name = "Floor";
                floor.transform.position = new Vector3(this.x, 0f, this.z);
                floor.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                if (North)
                {
                    northWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    northWall.name = "North Wall";
                    northWall.transform.position = (new Vector3(this.x, 0.5f, this.z + 0.5f));

                }
                if (South)
                {
                    southWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    southWall.name = "South Wall";
                    southWall.transform.position = (new Vector3(this.x, 0.5f, this.z - 0.5f));
                    southWall.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
                }
                if (West)
                {
                    westWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    westWall.name = "West Wall";
                    westWall.transform.position = (new Vector3(this.x - 0.5f, 0.5f, this.z));
                    westWall.transform.rotation = Quaternion.Euler(new Vector3(0, -90f, 0));
                }
                if (East)
                {
                    eastWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    eastWall.name = "East Wall";
                    eastWall.transform.position = (new Vector3(this.x + 0.5f, 0.5f, this.z));
                    eastWall.transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));

                }
            }
        }

        public new string ToString()
        {
            return "Floor" + this.position;
        }
    }

    public class Boundary : Tile
    {
        private bool used;
        public Boundary()
        {
            this.used = true;
        }
    }


    public class Doorway
    {

        public Doorway() { }
    }

    public class Column
    {
        public Column() { }
    }

    public class Hole
    {
        public Hole() { }
    }

    public void Create() { }

    public String ToString()
    {
        return "Regular Tile";
    }
}
