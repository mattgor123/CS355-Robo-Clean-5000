using System;
using UnityEngine;


public class Tile
{
    private bool used = false;
	/*Tile Constructor */ public Tile()
	{
        used = false;
	}

    public bool GetStatus()
    {
        return used;
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
            this.x = x;
            this.z = z;
            used = true;
        }

        public void Create()
        {
            if (used)
            {
                floor = GameObject.CreatePrimitive(PrimitiveType.Quad);
                floor.transform.Translate(this.x, 0.5f, this.z);
                floor.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                if (North)
                {
                    northWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    northWall.transform.Translate(new Vector3(this.x, 0.5f, this.z + 0.5f), floor.transform);

                }
                if (South)
                {
                    southWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    southWall.transform.Translate(new Vector3(this.x, 0.5f, this.z - 0.5f), floor.transform);
                    southWall.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
                }
                if (West)
                {
                    westWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    westWall.transform.Translate(new Vector3(this.x - 0.5f, 0.5f, this.z), floor.transform);
                    southWall.transform.rotation = Quaternion.Euler(new Vector3(0, -90f, 0));

                }
                if (East)
                {
                    eastWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    eastWall.transform.Translate(new Vector3(this.x + 0.5f, 0.5f, this.z), floor.transform);
                    southWall.transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));

                }
            }
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
    public Tile Blank() {
        return new Tile();        
    }

    public String toString()
    {
        return this.used.ToString();
    }
}
