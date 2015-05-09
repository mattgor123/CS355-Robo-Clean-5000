using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;

public class Stage  {

    #region Stages private members
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    private Tile[,] grid; //grid of Tile tiles. grid[0,0] is the bottom left of the dungeon
    private ArrayList rooms; //List containing Room objects. The rooms placed in dungeon
    private int currentRegion;  //the color of the region being carved
    private Vector2[] CARDINAL = { Vector2.up, Vector2.up * -1, Vector2.right, Vector2.right * -1 };
    private int currentLevel;                         //B0   //B1   //B2   //B3   //B4   //B5 
    private int[] Winding = new             int[]   {   60,     30,    30,    30,    30,    30,    30,    30,    30} ;
    private int[] RoomAttempts = new        int[]   {    20,    20,    20,    20,    20,    20,    20,    20,    20};
    private int[] RoomExtraSize = new       int[]   {    0,      3,     3,     3,    3,      3,     3,     3,    3};
    private float[] ColumnFrequency = new   float[] { 0.15f, 0.25f, 0.00f, 0.50f, 0.15f, 0.25f, 0.50f, 0.15f, 0.25f};
    private int[] StageWidths = new         int[]   {    20,    30,    35,    35,   35,    30 , 30, 30, 30};
    private int[] StageHeights = new        int[]   {    20,    30,    35,    35,   35,    30, 30, 30, 30 };

    private ArrayList levels; //Stores Object pairs, where the first element is the grid and the second is array of rooms
    private ArrayList spawnedRooms;
    private int elevatorSize = 5;
    private GameObject elevatorObject;
    private Room exit;
    private GameObject Facility;
    private Material floorMaterial;
    private Material wallMaterial;
    private List<Vector3> savedPlayerPositions;

    private Dictionary<Char, String> MapTexttoGrid = new Dictionary<Char, string>


    {
        {'R', "Rock"},
        {'F', "Floor"},
        {'X', "Exit"},
        {'E', "Elevator"},
        {'C', "Column"},
        {'W', "Wictory"}

    };

    private Dictionary<String, Char> MapGridtoText = new Dictionary<String, Char>


    {
        {"Rock", 'R'},
        {"Floor", 'F'},
        {"Exit", 'X'},
        {"Elevator", 'E'},
        {"Column", 'C'},
        {"Wictory", 'W'}

    };

    private FluffBuilder FBuilder;

	private GameObject player;
	private StatisticsRecorderController stats;
    #endregion

    public Stage(Tile[,] loadedGrid, ArrayList loadedRooms, FluffBuilder fluff_builder) {
        levels = new ArrayList();
        spawnedRooms = new ArrayList();
        AddLevel(loadedGrid, loadedRooms);
        FBuilder = fluff_builder;
    }

    public Stage(Material[] floors, Material[] walls, FluffBuilder fluff) 
    {
        //FloodFill algorithm needs odd-length grid
        currentLevel = 0; //start level
        levels = new ArrayList();
        spawnedRooms = new ArrayList();
        rooms = new ArrayList();
        this.width = ensureOdd(StageWidths[currentLevel]);
        this.height = ensureOdd(StageHeights[currentLevel]);
        grid = new Tile[this.width, this.height];
        currentRegion = -1;
        int i = RandomizeMaterials(floors.Length);
        this.floorMaterial = floors[i];
        this.wallMaterial = walls[i];
        this.FBuilder = fluff;
        //Initializing the grid to all Rock. Passes materials to Tile as well.
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                grid[x, y] = new Tile("Rock", new Vector3(x, 0, y),
                 this.floorMaterial, this.wallMaterial);
            }
        }
        savedPlayerPositions = new List<Vector3>();

        spawnExit();

    }

    public Stage(String textfile, Material floor, Material wall, FluffBuilder fbuilder)
    {
        levels = new ArrayList();
        spawnedRooms = new ArrayList();
        grid = ParseTextStage(textfile, floor, wall);
        this.width = grid.GetLength(0);
        this.height = grid.GetLength(1);
        this.floorMaterial = floor;
        this.wallMaterial = wall;
        this.rooms = new ArrayList();
        AddLevel(grid, this.rooms);
        this.FBuilder = fbuilder;

    }

    //Parse a text file into a grid
    public Tile[,] ParseTextStage(String textfile, Material floor, Material wall)
    {
        String[] lines = textfile.Split('\n');
        String widthSt = lines[0].TrimEnd('\n');
        String heightSt = lines[1].TrimEnd('\n');
        int width = int.Parse(widthSt);
        int height = int.Parse(heightSt);
        Tile[,] newGrid = new Tile[width, height];
        for (int i = 2; i < height + 2; i++)
        {
            for (int j = 0; j < width; j++)
            {
                Char[] linechar = lines[i].TrimEnd('\n').ToCharArray();
                newGrid[j, i - 2] = new Tile(MapTexttoGrid[linechar[j]], new Vector3(j, 0, i - 2), floor, wall);
            }
        }

            return newGrid;
    }

    private int RandomizeMaterials(int size) {
        return UnityEngine.Random.Range(0, size);
    }



    public void _addRooms()
    {

        for (var i = 0; i < RoomAttempts[currentLevel]; i++)
        {
            int roomExtraSize = RoomExtraSize[currentLevel];
            var size = UnityEngine.Random.Range(3, 5 + roomExtraSize) + 1;
            var rectangularity = UnityEngine.Random.Range(0, 1 + size / 2) * 2;
            var width = size;
            var height = size;
            if (oneIn(2))
            {
                width += rectangularity;
            }
            else
            {
                height += rectangularity;
            }

            int startx = UnityEngine.Random.Range(2 ,this.width - width - 2)  / 2 * 2 + 1;;
            int starty = UnityEngine.Random.Range(2, this.height - height - 2)  / 2 * 2 + 1;;

            var room = new Room(width, height, startx, starty);


            var overlaps = false;
            foreach (var other in rooms)
            {
                if (room.DistanceTo((Room)other))
                {
                    overlaps = true;
                    break;
                }
            }

            if (overlaps) continue;

            this.rooms.Add(room);

            currentRegion++;
            var max_x = startx + width;
            var max_y = starty + height;

            for (int x = startx; x < max_x; x++)
            {
                for (int y = starty; y < max_y; y++)
                {
                    var shouldCarveColumn = UnityEngine.Random.Range(0f, 1f);
                    if (shouldCarveColumn < ColumnFrequency[currentLevel] && x < max_x - 1 && y < max_y - 1 && x > startx && y > starty)
                    {
                        grid[x, y].CarveColumn();
                    }
                    else
                    {
                        grid[x, y].Carve();
                    }
                    grid[x, y].setColor(currentRegion);
                }
            }
        }
    }
    
    public void PlaceHalls()
    {   
        //Calls FloodFill on tiles that can be carved
        for (var x = 1; x < this.width; x += 2) //ignore border walls
        {
            for (var y = 1; y < this.height; y += 2) //also, due to rooms being oddly placed, this skips the walls that make the room
            {
                if (grid[x, y].getType() != "Rock") continue; //continue means to skip to the next iteration of forloop
                growMaze(new Vector2(x, y));
            }
        }
    }

    /*
     * Part of Munificent's algorithm       
     * Creates halls and numbers them accordingly
     * 
     */ 
    private void growMaze(Vector2 start)
    {
        var lastdir = Vector2.zero; //the direction the path last took
        int windingPercent = Winding[currentLevel]; //chance that the path will turn
        var cells = new List<Vector2>(); //carries x and y 
        int x = Mathf.FloorToInt(start.x);
        int y = Mathf.FloorToInt(start.y);
        currentRegion++; //since this is a new path, its color changes
        grid[x, y].Carve(); //Carve out start of this path
        grid[x, y].setColor(currentRegion); 
        cells.Add(start); 
        while (cells.Count > 0) //while the path can still grow
        {
            var cell = cells.Last(); 
            var unmadeCells = new ArrayList(); //list of open cells adjacent to [cell]
            foreach (var dir in CARDINAL)
            {
                //checks if tile 2 away is carvable
                if (canCarve(cell, dir))  unmadeCells.Add(dir); 
            }
            if (unmadeCells.Count > 0)
            {
                var dir = Vector2.zero; //direction that path is going to carve
                if (unmadeCells.Contains(lastdir) && UnityEngine.Random.Range(0, 100) > windingPercent)
                {
                    dir = lastdir; //keep going same direction
                }
                else
                {
                    //pick another direction
                    dir = (Vector2) unmadeCells[UnityEngine.Random.Range(0, unmadeCells.Count)];
                }
                //Carves out 2 tiles in [dir] direction
                Vector2 vCell = (Vector2)cell;
                int firstX = Mathf.FloorToInt( vCell.x + dir.x);
                int firstY = Mathf.FloorToInt(vCell.y + dir.y);
                int secondX = Mathf.FloorToInt(vCell.x + dir.x * 2);
                int secondY = Mathf.FloorToInt(vCell.y + dir.y * 2);
                grid[firstX, firstY].Carve();
                grid[secondX, secondY].Carve();
                grid[firstX, firstY].setColor(currentRegion);
                grid[secondX, secondY].setColor(currentRegion);
                cells.Add(new Vector2(secondX, secondY));
                lastdir = dir;
            }
            else
            {
                cells.Remove(cells.Last());
                lastdir = Vector2.zero;
            }
        }
    }

   

/*
* Part of Munificent's algorithm
* Remove some of the tiles at the ends of the mazes
* Makes for fewer deadends.
*/
    public void removeDeadEnds()
    {
        var done = false;
        while (!done)
        {
            done = true;

            foreach (Tile tile in grid)
            {
                Vector2 pos = tile.pos();
                if ((pos.x < 1) || (pos.x > this.width - 2) || (pos.y < 1) || (pos.y > this.height - 2)) continue;
                int x = Mathf.FloorToInt(pos.x);
                int y = Mathf.FloorToInt(pos.y);
                if (grid[x, y].getType() == "Rock") continue;

                // If it only has one exit, it's a dead end.
                var exits = 0;
                foreach (var dir in CARDINAL)
                {
                    int dirx = Mathf.FloorToInt(dir.x);
                    int diry = Mathf.FloorToInt(dir.y);
                    if (grid[x + dirx, y + diry].getType() != "Rock") exits++;
                }

                if (exits != 1) continue;

                done = false;
                grid[x, y].setType("Rock");
            }
        }
    }


    //Looks for rooms that didn't get connected and forces connections
    public void createDoors()
    {
        /*
         * Iterate through rooms
         *      Run alongside the border of the room.
         *      if the border tile is rock, check if floor exists one beyond
         *          if so, add coordinates to a list of potential doors to open
         *      if border tile is door, then door is already open
         *      if a door was already open, then quit.
         *      else randomly pick one a tile from the list and make a door
         */

        foreach (Room room in this.rooms)
        {
            List<Vector2> borderTiles = room.getBorderTiles();
            List<Vector2> potentialDoors = new List<Vector2>();
            bool hasDoor = false;
            int numDoors = 0;
            for (int i = 0; i < room.getWidth(); i++)
            {
                if (hasDoor) { break; }
                for (int j = 0; j < room.getHeight(); j++)
                {
                    if (hasDoor) break;
                    Vector2 world = room.getGridCoordinates(i, j);

                    int x = Mathf.FloorToInt(world.x);
                    int y = Mathf.FloorToInt(world.y);
                    #region Checking border for whether there's a door or can be one
                    //Left Side of room
                    if (i == 0)
                    {
                        if (grid[x - 1, y].getType() == "Rock") //Normal rock border
                        {
                            if (grid[x - 2, y].getType() == "Floor")
                            {
                                potentialDoors.Add(new Vector2(x - 1, y));
                            }
                        }
                        else
                        {
                            hasDoor = true;
                            break;
                        }
                    }
                    //Bottom Side of room
                    if (j == 0)
                    {
                        if (grid[x, y - 1].getType() == "Rock") //Normal rock border
                        {
                            if (grid[x, y - 2].getType() == "Floor")
                            {
                                potentialDoors.Add(new Vector2(x, y - 1));
                            }
                        }
                        else
                        {
                            hasDoor = true;
                            break;
                        }
                    }
                    //Top Side of room
                    if (j == room.getHeight() - 1)
                    {
                        if (grid[x, y + 1].getType() == "Rock") //Normal rock border
                        {
                            if (grid[x, y + 2].getType() == "Floor")
                            {
                                potentialDoors.Add(new Vector2(x, y + 1));
                            }
                        }
                        else
                        {
                            hasDoor = true;
                            break;
                        }
                    }
                    //Right Side of room
                    if (i == room.getWidth() - 1)
                    {
                        if (grid[x + 1, y].getType() == "Rock") //Normal rock border
                        {
                            if (grid[x + 2, y].getType() == "Floor")
                            {
                                potentialDoors.Add(new Vector2(x + 1, y));
                            }
                        }
                        else
                        {
                            hasDoor = true;
                            break;
                        }
                    }
                    #endregion
                }
            }

            //gives chance for a second or more door to be added
            
                while ((!hasDoor || UnityEngine.Random.Range(0, 100) > 40) && numDoors < potentialDoors.Count)
                {

                    Vector2 randomDoor = potentialDoors[UnityEngine.Random.Range(0, potentialDoors.Count)];
                    potentialDoors.Remove(randomDoor);
                    int doorx = Mathf.FloorToInt(randomDoor.x);
                    int doory = Mathf.FloorToInt(randomDoor.y);
                    grid[doorx, doory].Carve();
                    numDoors++;
                    hasDoor = true;
                }
            

        }
    }


    //Returns a random room that's been placed in the grid
    public Room RandomRoom()
    {
        return (Room)rooms[UnityEngine.Random.Range(1, rooms.Count)];
    }


    //Quick check to see if the next tile will be in bounds.
    //Specifically used for Municent's code
    private bool inBounds(Vector2 cell, Vector2 dir) {
        var sum = cell + dir * 3; 
        if (sum.x >= this.width || sum.x < 0 || sum.y > this.height || sum.y < 0)
        {
            return false;
        }
        return true;
    }

    private bool inBounds2(Vector2 cell, Vector2 dir)
    {
        var sum = cell + dir;
        if (sum.x >= this.width || sum.x < 0 || sum.y > this.height || sum.y < 0)
        {
            return false;
        }
        return true;
    }

    /*
     * Part of Munificent's Algorithm
     * Called from growMaze()
     * Sees if maze can continue carving in [dir] direction from [cell] tile
     * If the tile 3 away in that direction is out of bounds, then return false
     * If the tile 2 away is inside a room, then return false (We're not trying to open doors yet)
     */ 
    private bool canCarve(Vector2 cell, Vector2 dir) {
        /*
         * Returns whether a tile adjacent to cell is carvable
         */ 
        if (!inBounds(cell, dir)) {
            return false;
        }
        //Math from munificent's algorithm
        int x = Mathf.CeilToInt(cell.x + (dir.x * 2)); 
        int y = Mathf.CeilToInt(cell.y + (dir.y * 2));
        return grid[x, y].getType() == "Rock";
    }

 
    //Built because munificent's code has it. basically "one in x" chance something happens
    private bool oneIn(int num)
    {
        return UnityEngine.Random.Range(1, num + 1) == num; 
    }


    /*
     * Part of Munificent's algorithm
     * Carves the connection between two regions
     * TODO: Implement this fully. This is where door types would go if we implement doors
     */ 
    private void addJunction(Vector2 pos)
    {
        grid[Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y)].setType("Floor");
    }

    //Spawns the exit room. This code needs to be changed so the entire room is an exit instead of a 2x2 pad
    private void spawnExit() {
        //adding elevator room
        //if (this.exit == null)
        //{
            int eStartX = UnityEngine.Random.Range(2, this.width - this.elevatorSize - 2) / 2 * 2 + 1;
            int eStartY = UnityEngine.Random.Range(2, this.height - this.elevatorSize - 2) / 2 * 2 + 1;
            this.exit = new Room(this.elevatorSize, this.elevatorSize, eStartX, eStartY);
            currentRegion++;
            for (int x = eStartX; x < eStartX + this.elevatorSize; x++)
            {
                for (int y = eStartY; y < eStartY + this.elevatorSize; y++)
                {
                    grid[x, y].Carve();
                    grid[x, y].setColor(currentRegion);
                    grid[x, y].setType("Exit");
                }
            }
            grid[(int)this.exit.GetRoomCenter().x, (int)this.exit.GetRoomCenter().y].setType("Elevator");
            this.exit.setIsElevator(true);
        rooms.Add(this.exit);
    }

    private void TrimRock()
    {
        //For each rock tile, look in the cardinal directions for a floor tile. 
        //if none are found, then that Rock isn't visible anyway and can be destroyed.
        for (int x = 1; x < this.width - 1; x++)
        {
            for (int y = 1; y < this.height - 1; y ++ )
            {
                Tile tile = this.grid[x, y];
                if (tile.getType() == "Floor" || tile.getType() == "Exit") continue;
                if (grid[x - 1, y].getType() == "Floor" || grid[x - 1, y].getType() == "Exit") continue;
                if (grid[x + 1, y].getType() == "Floor" || grid[x + 1, y].getType() == "Exit") continue;
                if (grid[x, y - 1].getType() == "Floor" || grid[x, y - 1].getType() == "Exit") continue;
                if (grid[x, y + 1].getType() == "Floor" || grid[x, y + 1].getType() == "Exit") continue;
                //No floor found
                tile.setType("Blank");
            }
        }
    }

    /* Create()
     * Spawn every element of the grid
     */ 
    public void Create()
    {
        //trim out the things that won't be visible anyway
        TrimRock();


        //Store this floor in the array of levels 
		//ONLY if this is the first time on this floor
		if (currentLevel == levels.Count) {
			ArrayList level = new ArrayList ();
			Tile[,] savedGrid = new  Tile[this.width, this.height];
			for (int i = 0; i < this.width; i++) {
				for (int j = 0; j < this.height; j++) {
					savedGrid [i, j] = new Tile (this.grid [i, j]);
				}
			}
			ArrayList savedRooms = new ArrayList ();
			for (int i = 0; i < this.rooms.Count; i++) {
				savedRooms.Add (new Room ((Room)this.rooms [i]));
			}
			level.Add(savedGrid);
			level.Add(savedRooms);
			levels.Add(level);
		}
        

        //The empty gameobject that all the tiles are hidden inside (besides rooms)
        Facility = new GameObject(); 
        Facility.name = "Research Facility";
        Facility.tag = "facility";
        //The collider that spans the floor of the dungeon (saves memory over the floor tiles having them each)
        BoxCollider ground  = Facility.AddComponent<BoxCollider>();
        ground.size = new Vector3(this.width * StageBuilder.scale, 0.25f, this.height * StageBuilder.scale);
        ground.transform.position = new Vector3(this.width / 2 * StageBuilder.scale, -0.125f, this.height / 2 * StageBuilder.scale);

        //Calls each tile's Create function, passes scale downward so they all grow
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                grid[x, y].Create(Facility.transform, StageBuilder.scale);
            }
        }

        //Make tiles that are part of rooms be children of Room gameobject
        foreach (Room room in this.rooms) 
        {
            //Need to skip the rooms that have already been created (exit/entrance) 
            GameObject roomObject = room.GetRoom();
            roomObject = new GameObject();
            Vector2 center = room.GetRoomCenter();
            roomObject.name = "Room at " + center;
            roomObject.transform.position = new Vector3(center.x, 0, center.y) * StageBuilder.scale;
            for (int x = 0; x < room.getWidth(); x++)
            {
                for (int y = 0; y < room.getHeight(); y++)
                {
                    Vector2 position = room.getGridCoordinates(x, y);
                    if (grid[x, y].getType() == "Floor")
                    {
                        GameObject tile = grid[Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y)].GetTile();
                        tile.transform.SetParent(roomObject.transform);
                    }


                }
            }
            spawnedRooms.Add(roomObject);

            if (room.getIsElevator())
            {
                elevatorObject = roomObject;
                elevatorObject.transform.position = roomObject.transform.position;
                savedPlayerPositions.Add(roomObject.transform.position);

            }
        }

        FBuilder.Randomize();
        FBuilder.BuildFluff(grid, StageBuilder.scale);
    }



    private void CarveRoom(Room room, int width, int height, int startX, int startY)
    {
        //Carves out the floor tiles corresponding to room floor tiles
        //TODO: Room needs method that returns where Floor tiles only are (Since room will soon include walls)
        for (int x = startX; x < startX + width; x++)
        {
            for (int y = startY; y < startY + height; y++)
            {
                grid[x, y].Carve();
                grid[x, y].setColor(currentRegion);

            }
        }
    }


    private int ensureOdd(int num)
    {
        return num % 2 == 0 ? num + 1 : num;

    }
    
    public override string ToString()
    {
        /*TODO: Make this actually parse dungeon. Needed for Map and for Save */
		string result = "Stage";
        return result;
    }

    public void DestroyCurrentLevel() {
        //Destroy Facility
        GameObject.Destroy(Facility);
        GameObject.Destroy(GameObject.FindGameObjectWithTag("facility"));
        GameObject.Destroy(GameObject.FindGameObjectWithTag("fluff"));
        //Destroy Rooms
        rooms.Clear();
        //Destroy all rooms except for the elevator we are standing in (in this case spawnedRooms(0)
        for (int i = 0; i < spawnedRooms.Count; i++)
        {
            GameObject room = (GameObject)spawnedRooms[i];
            GameObject.Destroy(room);

        }
        spawnedRooms.Clear();
        //Destroy exit reference
        //exit = null;

        FBuilder.DestroyFluff();
        
    }

    private void CreateNewLevel()
    {
        //Create new grid of new dimensions
        int newWidth = ensureOdd(StageWidths[currentLevel]);
        int newHeight = ensureOdd(StageHeights[currentLevel]);
        Tile[,] newGrid = new Tile[newWidth, newHeight];
        for (int x = 0; x < newWidth; x++)
        {
            for (int y = 0; y < newHeight; y++)
            {
                newGrid[x, y] = new Tile("Rock", new Vector3(x, 0, y), this.floorMaterial, this.wallMaterial);
            }
        }

        //Switch active grid to current one.
        this.grid = newGrid;
        this.height = newHeight;
        this.width = newWidth;
        //Create the new exit
        spawnExit();
        //Place Rooms
        _addRooms(); //TODO: Reset needs a need an argument to decide numTries
        //Place Halls
        PlaceHalls();
        //Create Doors
        createDoors();
        //Remove Dead Ends
        removeDeadEnds();
        
    }

    private void MovePlayerToEntrance()
    {
        //Get new spawn location for player and move him there
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        Room room = this.rooms[0] as Room;
        var center = room.GetRoomCenter() * StageBuilder.scale;
        Player.transform.position = new Vector3(center.x, 0, center.y);
        
    }

    private IEnumerator WaitTwoSecs()
    {
        yield return new WaitForSeconds(2);
    }


    public void NextLevel(int level)
    {
		//start code for stat tracking
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}
		if (player != null && stats == null) {
			stats = player.GetComponent<StatisticsRecorderController>();
		}
		if (stats != null) {
			stats.resetStats();
		}
		//end code for stat tracking

        //CameraShake();
        PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        this.currentLevel = level;
        if (level >= levels.Count) //level is exactly one deeper than previous depth
        {
            DestroyCurrentLevel();
            CreateNewLevel(); //equivalent to the constructor method when stage is first made
            pc.incrementDeepestLevelVisited();
            MovePlayerToEntrance();

        }
        else //level has already been made. Must be loaded
        {
            DestroyCurrentLevel();
            LoadLevel(level);
            
        }

        pc.setCurrentFloor(level);
        //Create the dungeon
        Create();
        //Move player to the entrance
        WaitTwoSecs();


        
        GameObject.FindGameObjectWithTag("ElevatorCanvas").GetComponent<ElevatorController>().FadeIn();

    }

    public void AddLevel(Tile[,] loadedGrid, ArrayList loadedRooms) {
        // Load a temporary level
        currentLevel = -1;
        int loadedWidth = loadedGrid.GetLength(0);
        int loadedHeight = loadedGrid.GetLength (1);
        this.grid = new Tile[loadedWidth, loadedHeight];
        this.width = loadedWidth;
        this.height = loadedHeight;
        for (int i = 0; i < this.width; i++) {
            for (int j = 0; j < this.height; j++) {
                this.grid [i, j] = new Tile (loadedGrid [i, j]);
            }
        }
        this.rooms = new ArrayList ();
        for (int i = 0; i < loadedRooms.Count; i++)
        {
            this.rooms.Add(new Room((Room)loadedRooms[i]));
        }
    }

    public void LoadLevel(int toLoad)
    {

        //Load previous level's grid
        currentLevel = toLoad;
        if (toLoad > 3) currentLevel--;
        if (toLoad > 6) currentLevel--;
        Debug.Log(currentLevel);
        ArrayList level = levels[currentLevel] as ArrayList;
		Tile[,] loadedGrid =  level[0] as Tile[,];
		int loadedWidth = loadedGrid.GetLength(0);
		int loadedHeight = loadedGrid.GetLength (1);
		this.grid = new Tile[loadedWidth,loadedHeight];
		this.width = loadedWidth;
		this.height = loadedHeight;
		for (int i = 0; i < this.width; i++) {
			for (int j = 0; j < this.height; ++j) {
				this.grid [i, j] = new Tile (loadedGrid [i, j]);
			}
		}
		ArrayList loadedRooms = level[1] as ArrayList;
		this.rooms = new ArrayList ();
        for (int i = 0; i < loadedRooms.Count; ++i)
        {
            this.rooms.Add(new Room((Room)loadedRooms[i]));
        }
        player.transform.position = savedPlayerPositions[currentLevel];
	}

    private void CameraShake()
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraController cc = camera.GetComponent<CameraController>();
        cc.shake();
    }

    public String GridToString()
    {
        String result = "";
        result += this.width + '\n' + this.height + '\n';
        for (int x = this.width - 1; x >= 0; --x)
        {
            for (int y = this.height - 1; y >= 0; --y)
            {
                result += MapGridtoText[grid[x, y].getType()];
            }
            result += '\n';
        }
        return result;
    }


    public Tile[,] GetGrid()
    {
        return grid;
    }

}


