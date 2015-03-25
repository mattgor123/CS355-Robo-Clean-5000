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
    private const string Exit = "Exit";
    private Material wallMaterial;
    private Material floorMaterial;


    /*Tile Constructor */ 
    public Tile(String type, Vector3 position, Material floor, Material wall)
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

    public void Create(Transform mother, float scale)
    {
        switch (this.type)
        {
            case Rock:
                tile = new GameObject();
                tile.transform.SetParent(mother);
                tile.transform.position = this.position;
                tile.name = Rock;
                GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Quad);
                ceiling.transform.localScale = new Vector3(1, 1, 1) * scale;
                ceiling.transform.position = new Vector3(this.position.x, 4, this.position.z) * scale;
                ceiling.transform.SetParent(tile.transform);
                Renderer ceilRend = ceiling.GetComponent<Renderer>();
                ceilRend.material.color = Color.black;
                ceiling.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                ceiling.name = "Ceiling";

                GameObject NorthWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                NorthWall.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                NorthWall.transform.SetParent(tile.transform);
                NorthWall.name = "NorthWall";
                Renderer NWrend = NorthWall.GetComponent<Renderer>();
                NWrend.material = this.wallMaterial;
                NorthWall.transform.localScale = new Vector3(1, 4, 1) * scale;
                NorthWall.transform.position = new Vector3(this.position.x, 2, this.position.z + 0.5f) * scale;


                GameObject SouthWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                SouthWall.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                SouthWall.transform.SetParent(tile.transform);
                SouthWall.name = "SouthWall";
                Renderer SWrend = SouthWall.GetComponent<Renderer>();
                SWrend.material = this.wallMaterial;
                SouthWall.transform.localScale = new Vector3(1, 4, 1) * scale;
                SouthWall.transform.position = new Vector3(this.position.x, 2, this.position.z - 0.5f) * scale;




                GameObject WestWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                WestWall.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                WestWall.transform.SetParent(tile.transform);
                WestWall.name = "WestWall";
                Renderer Wrend = WestWall.GetComponent<Renderer>();
                Wrend.material = this.wallMaterial;
                WestWall.transform.localScale = new Vector3(1, 4, 1) * scale;
                WestWall.transform.position = new Vector3(this.position.x - 0.5f, 2, this.position.z) * scale;



                GameObject EastWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
                EastWall.transform.rotation = Quaternion.Euler(new Vector3(0, 270, 0));
                EastWall.transform.SetParent(tile.transform);
                EastWall.name = "EastWall";
                Renderer Erend = EastWall.GetComponent<Renderer>();
                Erend.material = this.wallMaterial;
                EastWall.transform.localScale = new Vector3(1, 4, 1) * scale;
                EastWall.transform.position = new Vector3(this.position.x + 0.5f, 2, this.position.z) * scale;
                break;

            case Floor:
                tile = new GameObject();
                tile.transform.SetParent(mother);
                tile.name = Floor;
                tile.transform.position = this.position;
                GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Quad);
                floor.transform.localScale = new Vector3(1, 1, 1) * scale;
                floor.transform.position = new Vector3(this.position.x, 0, this.position.z) * scale;
                floor.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                Renderer Frend = floor.GetComponent<Renderer>();
                GameObject.DestroyImmediate(floor.GetComponent<BoxCollider>());
                Frend.material = this.floorMaterial;
                floor.transform.SetParent(tile.transform);
                break;

            case Exit:
                tile = new GameObject();
                tile.transform.SetParent(mother);
                tile.name = Exit;
                tile.transform.position = this.position;
                GameObject exit = GameObject.CreatePrimitive(PrimitiveType.Quad);
                exit.transform.localScale = new Vector3(1, 1, 1) * scale;
                exit.transform.position = new Vector3(this.position.x, 2, this.position.z) * scale;
                exit.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                Renderer Exrend = exit.GetComponent<Renderer>();
                Exrend.material.color = Color.Lerp(Color.white, Color.yellow, 1.0f);
                BoxCollider exitBox = exit.AddComponent<BoxCollider>();
                exitBox.isTrigger = true;
            
                var glow = exit.AddComponent<Light>();
                glow.color = Color.Lerp(Color.white, Color.yellow, 1.0f);
                var exScript = exit.AddComponent<NewRoomTrigger>();
                exScript.setLevel("RampDown");
                exit.transform.SetParent(tile.transform);
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
