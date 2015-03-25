using System;
using UnityEngine;


public class Tile
{
    private Vector3 position; //Position of tile in world coordinates
    private int color; //color of tile when flood filling
    private GameObject tile; //the gameobject for tile if instantiated
    private String type; //The type of tile
    /*Types of Tiles Currently */
    private const string Blank  = "Blank";      //No objects created (initial state)
    private const string SWCorner = "SWCorner"; //: Corner Piece of room
    private const string SECorner = "SECorner"; //: Corner Piece of room
    private const string NECorner = "NECorner"; //: Corner Piece of room
    private const string NWCorner = "NWCorner"; //: Corner Piece of room
    private const string NWall    = "NWall";    //: Northfacing wall on Tile
    private const string WWall    = "WWall";    //: Westfacing wall on Tile
    private const string EWall    = "EWall";    //: Eastfacing wall on Tile
    private const string SWall    = "SWall";    //: Southfacing wall on Tile
    private const string EWHall   = "EWHall";   //: Hallway with walls on E and W sides
    private const string NSHall   = "NSHall";   //: Hallway with walls on N and S sides
    private const string WNEDead  = "WNEDead";  //: Deadend with walls on W, N, E sides
    private const string NESDead  = "NESDead";  //: Deadend with walls on N, E, S sides
    private const string ESWDead  = "ESWDead";  //: Deadend with walls on E, S, W sides;
    private const string SWNDead  = "SWNDead";  //: Deadend with walls on S, W, N sides;
    private const string Floor    = "Floor";    //: Floor tile
    private const string Void     = "Void";     //: Intentional void spaces

    /*Tile Constructor */ 
    public Tile(String type, Vector3 position)
	{
        this.color = 0;
        this.type = type;
        this.position = position;
        
	}

    /*The 4 booleans are for whether a wall needs to be placed on that end of the tile
     *This is a grueling 14 if-clauses to determine which type this tile should be
     (4^2 ways for bools to be, minus 2 cases (all walls and no walls)*/
    public void DecideType(bool N, bool S, bool E, bool W)
    {
        if (!N && S && !E && W)
        {
            this.type = SWCorner;
        }
        if (!N && S && E && !W)
        {
            this.type = SECorner;
        }
        if (N && !S && E && !W)
        {
            this.type = NECorner;
        }
        if (N && !S && !E && W)
        {
            this.type = NWCorner;
        }
        if (N && !S && !E && !W)
        {
            this.type = NWall;
        }
        if (!N && !S && !E && W)
        {
            this.type = WWall;
        }
        if (!N && !S && E && !W)
        {
            this.type = EWall;
        }
        if (!N && S && !E && !W)
        {
            this.type = SWall;
        }
        if (N && S && !E && !W)
        {
            this.type = NSHall;
        }
        if (!N && !S && E && W)
        {
            this.type = EWHall;
        }
        if (N && !S && E && W)
        {
            this.type = WNEDead;
        }
        if (N && S && E && !W)
        {
            this.type = NESDead;
        }
        if (!N && S && E && W)
        {
            this.type = ESWDead;
        }
        if (N && S && !E && W)
        {
            this.type = SWNDead;
        }
        if (!N && !S && !E && !W)
        {
            this.type = Blank;
        }

    }

    public int getX()
    {
        return Mathf.FloorToInt(this.position.x);
    }

    public int getY()
    {
        return Mathf.FloorToInt(this.position.z);
    }

    public void setType(String type)
    {
        this.type = type;
    }

    public String getType()
    {
        return this.type;
    }

    public void setColor(int i) {
        this.color = i;
    }

    public int getColor()
    {
        return this.color;
    }

    public void Create() {
        String caseSwitch = this.type;
        switch (caseSwitch)
        {
            case "Blank":
                //Do nothing
                break;
            case SWCorner:
                //Create SW corner pieces
                this.CreateParent();
                this.CreateSWall();
                this.CreateWWall();
                this.CreateFloor();
                break;
            case SECorner:
                //Create SE corner pieces
                this.CreateParent();
                this.CreateEWall();
                this.CreateSWall();
                this.CreateFloor();
                break;
            case NECorner:
                //Create NE corner pieces
                this.CreateParent();
                this.CreateNWall();
                this.CreateEWall();
                this.CreateFloor();
                break;
            case NWCorner:
                //Create NW corner pieces
                this.CreateParent();
                this.CreateNWall();
                this.CreateWWall();
                this.CreateFloor();
                break;
            case NWall:
                //Create N wall piece
                this.CreateParent();
                this.CreateNWall();
                this.CreateFloor();
                break;
            case EWall:
                //Create E wall piece
                this.CreateParent();
                this.CreateEWall();
                this.CreateFloor();
                break;
            case WWall:
                //Create W wall piece
                this.CreateParent();
                this.CreateWWall();
                this.CreateFloor();
                break;
            case SWall:
                //Create S wall piece
                this.CreateParent();
                this.CreateSWall();
                this.CreateFloor();
                break;
            case EWHall:
                //Create EW Hall piece
                this.CreateParent();
                this.CreateEWall();
                this.CreateWWall();
                this.CreateFloor(); 
                break;
            case NSHall:
                //Create NS Hall piece
                this.CreateParent();
                this.CreateSWall();
                this.CreateNWall();
                this.CreateFloor();
                break;
            case WNEDead:
                //Create WNE Deadend;
                this.CreateParent();
                this.CreateWWall();
                this.CreateNWall();
                this.CreateEWall();
                this.CreateFloor();
                break;
            case NESDead:
                //Create NES Deadend;
                this.CreateParent();
                this.CreateNWall();
                this.CreateEWall();
                this.CreateSWall();
                this.CreateFloor();
                break;
            case ESWDead:
                //Create ESW Deadend;
                this.CreateParent();
                this.CreateEWall();
                this.CreateSWall();
                this.CreateWWall();
                this.CreateFloor();
                break;
            case SWNDead:
                //Create SWN Deadend;
                this.CreateParent();
                this.CreateSWall();
                this.CreateWWall();
                this.CreateNWall();
                this.CreateFloor();
                break;
            case "Floor":
                this.CreateParent();
                this.CreateFloor();
                break;
            case "Void":
                //Don't create anything
                break;
            default:
                Debug.LogError("TileType is not being set!");
                break;

        } 
    }

    public GameObject GetTile()
    {
        return this.tile;
    }

    private void CreateParent()
    {
        this.tile = new GameObject();
        this.tile.transform.position = position;
        this.tile.name = this.type + " at (" + this.position.x + ", " + this.position.z + ")";
    }

    private void CreateNWall()
    {
        float x = this.position.x;
        float z = this.position.z;
        GameObject northWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        northWall.name = "North Wall";
        northWall.transform.position = (new Vector3(x, 0.5f, z + 0.5f));
        northWall.transform.SetParent(this.tile.transform);
    }

    private void CreateEWall()
    {
        float x = this.position.x;
        float z = this.position.z;
        GameObject eastWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        eastWall.name = "East Wall";
        eastWall.transform.position = (new Vector3(x + 0.5f, 0.5f, z));
        eastWall.transform.rotation = Quaternion.Euler(new Vector3(0, 90f, 0));
        eastWall.transform.SetParent(this.tile.transform);
    }

    private void CreateSWall()
    {
        float x = this.position.x;
        float z = this.position.z;
        GameObject southWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        southWall.name = "South Wall";
        southWall.transform.position = (new Vector3(x, 0.5f, z - 0.5f));
        southWall.transform.rotation = Quaternion.Euler(new Vector3(0, 180f, 0));
        southWall.transform.SetParent(this.tile.transform);

    }

    private void CreateWWall()
    {
        float x = this.position.x;
        float z = this.position.z;
        GameObject westWall = GameObject.CreatePrimitive(PrimitiveType.Quad);
        westWall.name = "West Wall";
        westWall.transform.position = (new Vector3(x - 0.5f, 0.5f, z));
        westWall.transform.rotation = Quaternion.Euler(new Vector3(0, -90f, 0));
        westWall.transform.SetParent(this.tile.transform);

    }
    private void CreateFloor()
    {
        float x = this.position.x;
        float z = this.position.z;
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Quad);
        floor.name = "Floor";
        floor.transform.position = (new Vector3(x, 0, z));
        floor.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
        floor.transform.SetParent(this.tile.transform);
    }

    
    public String ToString()
    {
        
        return "[" + this.type + ", " + this.color + ", " + this.position + "]";
    }
}
