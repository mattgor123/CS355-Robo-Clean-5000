using System;
using UnityEngine;


public class Tile
{
    private Vector3 position; //Position of tile in world coordinates
    private int color; //color of tile when flood filling
    private GameObject tile; //the gameobject for tile if instantiated
    private String type; //The type of tile
    /*Types of Tiles Currently */
    private const string Rock  = "Rock";      //No objects created (initial state)
    private const string Floor = "Floor";      //No objects created (initial state)
    private Material wallMaterial;
    private Material floorMaterial;


    /*Tile Constructor */ 
    public Tile(String type, Vector3 position, Material wall, Material floor)
	{
        this.color = 0;
        this.type = type;
        this.position = position;
        this.wallMaterial = wall;
        this.floorMaterial = floor;

        
	}

    public Vector2 pos()
    {
        return new Vector2(this.position.x, this.position.z);
    }

    public void Carve()
    {
        this.type = Floor;
    }

    public void Create()
    {
        switch (this.type)
        {
            case Rock:
                tile = new GameObject();
                tile.transform.position = this.position;
                tile.name = Rock;
                GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Quad);
                ceiling.transform.position = new Vector3(this.position.x, 4, this.position.z);
                ceiling.transform.SetParent(tile.transform);
                Renderer ceilRend = ceiling.GetComponent<Renderer>();
                ceilRend.material.color = Color.black;
                ceiling.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                ceiling.name = "Ceiling";

                GameObject NorthWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                NorthWall.transform.position = new Vector3(this.position.x, 2, this.position.z + 0.5f);
                NorthWall.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                NorthWall.transform.SetParent(tile.transform);
                NorthWall.name = "NorthWall";
                Renderer NWrend = NorthWall.GetComponent<Renderer>();
                NWrend.material = this.wallMaterial;
                NorthWall.transform.localScale = new Vector3(1, 4, 1);

                GameObject SouthWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                SouthWall.transform.position = new Vector3(this.position.x, 2, this.position.z - 0.5f);
                SouthWall.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                SouthWall.transform.localScale = new Vector3(1, 4, 1);
                SouthWall.transform.SetParent(tile.transform);
                SouthWall.name = "SouthWall";
                Renderer SWrend = SouthWall.GetComponent<Renderer>();
                SWrend.material = this.wallMaterial;


                GameObject WestWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                WestWall.transform.position = new Vector3(this.position.x - 0.5f, 2, this.position.z);
                WestWall.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                WestWall.transform.SetParent(tile.transform);
                WestWall.name = "WestWall";
                WestWall.transform.localScale = new Vector3(1, 4, 1);
                Renderer Wrend = WestWall.GetComponent<Renderer>();
                Wrend.material = this.wallMaterial;

                GameObject EastWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                EastWall.transform.position = new Vector3(this.position.x + 0.5f, 2, this.position.z);
                EastWall.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                EastWall.transform.SetParent(tile.transform);
                EastWall.name = "EastWall";
                EastWall.transform.localScale = new Vector3(1, 4, 1);
                Renderer Erend = EastWall.GetComponent<Renderer>();
                Erend.material = this.wallMaterial;

                break;
            case Floor:
                tile = new GameObject();
                tile.name = Floor;
                tile.transform.position = this.position;
                GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Quad);
                floor.transform.localScale = new Vector3(1, 1, 1);
                floor.transform.position = new Vector3(this.position.x, 2, this.position.z);
                floor.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                Renderer Frend = floor.GetComponent<Renderer>();
                Frend.material = this.floorMaterial;
                floor.transform.SetParent(tile.transform);
                break;
        }

      
    }

    public void setType(string type)
    {
        this.type = type;
    }

    public string getType()
    {
        return this.type;
    }

    public GameObject GetTile()
    {
        return this.tile;
    }

    public void setColor(int i)
    {
        this.color = i;
    }

    public int getColor()
    {
        return this.color;
    }

}
