using System;
using UnityEngine;


public class Tile
{
    #region Tile private members
    private Vector3 position; //Position of tile in world coordinates
    private int color; //color of tile when flood filling
    private GameObject tile; //the gameobject for tile if instantiated
    private String type; //The type of tile
    /*Types of Tiles Currently */
    private const string Rock  = "Rock";      // 4 walls and ceiling generated
    private const string Floor = "Floor";     // Just a floor is generated
    private const string Exit = "Exit";       //Special Exit tile
    private const string Elevator = "Elevator"; //Center of Exit that has the trigger.
    private const string Blank = "Blank";
    private Material wallMaterial; //material to assign to walls if the tile has any
    private Material floorMaterial; //material to assign to floor if the tile has one
    private int maxColors;
    #endregion

    /*Tile Constructor */ 
    public Tile(String type, Vector3 position, Material floor, Material wall)
	{
        this.color = 0; 
        this.type = type;
        this.position = position;
        this.wallMaterial = wall;
        this.floorMaterial = floor;

        
	}

    //Returns the position relative to the grid (scale has not multipled the position)
    public Vector2 pos()
    {
        return new Vector2(this.position.x, this.position.z);
    }

    public void passMaxColors(int max) {
        this.maxColors = max;
    }
    
    public void Carve()
    {
        this.type = Floor;
    }

    //Creates the tile according to its type. 
    //TODO: Refactor more into lots of methods
    public void Create(Transform mother, float scale)
    {
        switch (this.type)
        {
            //4 walls and a ceiling make it look solid
            case Rock:
                tile = new GameObject();
                tile.transform.SetParent(mother);
                tile.transform.position = this.position;
                tile.name = Rock;
                //GameObject ceiling = CreateCeiling();
                GameObject NorthWall = CreateWall("NorthWall", 180, 0f, 0.5f);
                GameObject SouthWall = CreateWall("SouthWall", 0, 0, -0.5f);
                GameObject WestWall = CreateWall("WestWall", 90, -0.5f, 0);
                GameObject EastWall = CreateWall("EastWall", 270, 0.5f, 0);
                break;

            //Just a floor
            case Floor:
                tile = new GameObject();
                tile.transform.SetParent(mother);
                tile.name = Floor;
                tile.transform.position = this.position;
                GameObject floor = CreateFloor();
                floor.layer = LayerMask.NameToLayer("EnemySpawnable");
                break;

            case Exit:
                tile = new GameObject();
                tile.transform.SetParent(mother);
                tile.name = Exit;
                tile.transform.position = this.position;
                GameObject ceiling = CreateCeiling();
                GameObject exit = CreateExit();
                break;

            case Elevator:
                tile = new GameObject();
                tile.transform.SetParent(mother);
                tile.name = Exit;
                tile.transform.position = this.position;

                GameObject elevator = CreateElevator();
                break;

            case Blank:
                //Do nothing for now
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

    private GameObject CreateCeiling()
    {
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Quad);
        ceiling.transform.localScale = new Vector3(1, 1, 1) * StageBuilder.scale;
        ceiling.transform.position = new Vector3(this.position.x, 2, this.position.z) * StageBuilder.scale;
        ceiling.transform.SetParent(tile.transform);
        Renderer ceilRend = ceiling.GetComponent<Renderer>();
        ceilRend.material.color = Color.yellow;
        ceiling.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
        ceiling.name = "Ceiling";
        return ceiling;
    }

    private GameObject CreateWall(string wallName, float rotation, float x, float z)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        wall.transform.rotation = Quaternion.Euler(new Vector3(0, rotation, 0));
        wall.transform.SetParent(tile.transform);
        wall.name = wallName;
        Renderer rend = wall.GetComponent<Renderer>();
        rend.material = this.wallMaterial;
        wall.transform.localScale = new Vector3(1, 2, 1) * StageBuilder.scale;
        wall.transform.position = new Vector3(this.position.x + x, 1, this.position.z + z) * StageBuilder.scale;
        return wall;
    }

    //Creates floor tile. Destroys collider because of the dungeon-wide collider    
    private GameObject CreateFloor()
    {
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Quad);
        floor.transform.localScale = new Vector3(1, 1, 1) * StageBuilder.scale;
        floor.transform.position = new Vector3(this.position.x, 0, this.position.z) * StageBuilder.scale;
        floor.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        Renderer Frend = floor.GetComponent<Renderer>();
        GameObject.DestroyImmediate(floor.GetComponent<BoxCollider>());
        Frend.material = floorMaterial;
        floor.transform.SetParent(tile.transform);
        return floor;
    }

    private GameObject CreateElevator()
    {
        GameObject exit = GameObject.CreatePrimitive(PrimitiveType.Quad);
        exit.transform.localScale = new Vector3(1, 1, 1) * StageBuilder.scale;
        exit.transform.position = new Vector3(this.position.x, 0, this.position.z) * StageBuilder.scale;
        exit.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        Renderer Exrend = exit.GetComponent<Renderer>();
        Exrend.material.color = Color.green;
        BoxCollider exitBox = exit.AddComponent<BoxCollider>();
        GameObject lightObject = new GameObject();
        Light light = lightObject.AddComponent<Light>();
        light.type = LightType.Spot;
        light.spotAngle = 60f;
        light.intensity = 8f;
        
        
        lightObject.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

        lightObject.transform.position = new Vector3(this.position.x, 1, this.position.z) * StageBuilder.scale;
        
        exitBox.isTrigger = true;
        var exScript = exit.AddComponent<NewRoomTrigger>();
        //exScript.setLevel("RampDown");
        exit.transform.SetParent(tile.transform);
        return exit;
    }

    private GameObject CreateExit()
    {
        GameObject exit = GameObject.CreatePrimitive(PrimitiveType.Quad);
        exit.transform.localScale = new Vector3(1, 1, 1) * StageBuilder.scale;
        exit.transform.position = new Vector3(this.position.x, 0, this.position.z) * StageBuilder.scale;
        exit.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        Renderer Exrend = exit.GetComponent<Renderer>();
        Exrend.material.color = Color.yellow;
        exit.layer = LayerMask.NameToLayer("Exit");   
        //BoxCollider exitBox = exit.AddComponent<BoxCollider>();
        //exitBox.isTrigger = true;
        //Light glow = exit.AddComponent<Light>();
        //glow.color = Color.Lerp(Color.white, Color.yellow, 1.0f);
        //var exScript = exit.AddComponent<NewRoomTrigger>();
        //exScript.setLevel("RampDown");
        exit.transform.SetParent(tile.transform);
        return exit;
    }

}
