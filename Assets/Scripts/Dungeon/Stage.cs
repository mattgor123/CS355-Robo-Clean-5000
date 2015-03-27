using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class Stage  {

    #region Stage's private members
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    private Tile[,] grid; //grid of Tile tiles. grid[0,0] is the bottom left of the dungeon
    private ArrayList rooms; //List containing Room objects. The rooms placed in dungeon
    private int currentRegion;  //the color of the region being carved
    private Vector2[] CARDINAL = { Vector2.up, Vector2.up * -1, Vector2.right, Vector2.right * -1 }; 
    #endregion



    public Stage(int width, int height, Material floor, Material wall) 
    {
        //FloodFill algorithm needs odd-length grid
        this.width = ensureOdd(width); 
        this.height = ensureOdd(height);
        grid = new Tile[this.width, this.height];
        currentRegion = 0;
        rooms = new ArrayList(); 
        //Initializing the grid to all Rock. Passes materials to Tile as well.
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                grid[x, y] = new Tile("Rock", new Vector3(x, 0, y), floor, wall);
            }
        }
    }



    public void _addRooms(int numTries)
    {
        for (var i = 0; i < numTries; i++)
        {
            int roomExtraSize = 2;
            var size = UnityEngine.Random.Range(3, 3 + roomExtraSize) * 2 + 1;
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

            int startx = UnityEngine.Random.Range(0,this.width - width)  / 2 * 2 + 1;;
            int starty = UnityEngine.Random.Range(0, this.height - height)  / 2 * 2 + 1;;

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

            rooms.Add(room);

            currentRegion++;
            for (int x = startx; x < startx + width; x++)
            {
                for (int y = starty; y < starty + height; y++)
                {
                    grid[x, y].Carve();
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
        int windingPercent = 50; //chance that the path will turn
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
 * Finds tiles between regions (rooms and hallways) that could become doors
 */
    public void ConnectRegions()
    {
        int extraConnectorChance = 5;
        // Find all of the tiles that can connect two (or more) regions.
        //This means Rock, not floor                                        
        Dictionary<Vector2, HashSet<int>> connectorRegions = new Dictionary<Vector2, HashSet<int>>();
        foreach (Tile tile in grid)
        {
            Vector2 pos = tile.pos(); //goes into key field of connectorRegions
            //this if skips the borders of the grid
            if ((pos.x < 1) || (pos.x > this.width - 2) || (pos.y < 1) || (pos.y > this.height - 2)) continue;
            int x = Mathf.FloorToInt(pos.x);
            int y = Mathf.FloorToInt(pos.y);

            if (tile.getType() != "Rock") continue;
            var regions = new HashSet<int>();
            foreach (var dir in CARDINAL)
            {
                int dirx = Mathf.FloorToInt(dir.x);
                int diry = Mathf.FloorToInt(dir.y);
                if (inBounds2(pos, dir))
                {
                        var region = grid[x + dirx, y + diry].getColor();
                        regions.Add(region);
                }
            }

            if (regions.Count < 2) continue;
            connectorRegions[pos] = regions;

        }
        var connectors = connectorRegions.Keys.ToList();


        // Keep track of which regions have been merged. This maps an original
        // region index to the one it has been merged to.
        var merged = new List<int>(); //these don't need set properites, so just lists
        var openRegions = new List<int>();
        for (var i = 0; i <= currentRegion; i++)
        {
            merged.Add(i);
            openRegions.Add(i);
        }

        // Keep connecting regions until we're down to one.
        int test = openRegions.Count;
        while (test > 1)
        {
            Debug.Log(openRegions.Count);
            Vector2 connector = connectors[UnityEngine.Random.Range(0, connectors.Count())];

            // Carve the connection.
            addJunction(connector);

            // Merge the connected regions. We'll pick one region (arbitrarily) and
            // map all of the other regions to its index.
            var regions = connectorRegions[connector].Select( region => merged.ElementAt(region));
            var dest = regions.First();
            var sources = regions.Skip(1).ToList<int>();

            // Merge all of the affected regions. We have to look at *all* of the
            // regions because other regions may have previously been merged with
            // some of the ones we're merging now.
            for (var i = 0; i <= currentRegion; i++)
            {
                if (sources.Contains(merged[i]))
                {
                    merged[i] = dest;
                }
            }
            // The sources are no longer in use.

                openRegions.RemoveAll(s => sources.Contains(s));
            
            // Remove any connectors that aren't needed anymore.
            connectors.RemoveAll(s =>
            {
                // Don't allow connectors right next to each other.
                if ((connector - s).magnitude < 2) return true;

                // If the connector no long spans different regions, we don't need it.
                var spans = connectorRegions[s].Select((region) => merged[region]);

                if (spans.Count<int>() > 1) return false;

                // This connecter isn't needed, but connect it occasionally so that the
                // dungeon isn't singly-connected.
                if (oneIn(extraConnectorChance)) addJunction(s);   
             
                return true;
            });
            test--;
        }
        foreach (var item in openRegions)
        {
            Debug.Log(item.ToString());
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


    //Returns a random room that's been placed in the grid
    public Room RandomRoom()
    {
        return (Room)rooms[UnityEngine.Random.Range(0, rooms.Count)];
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
        Room room = RandomRoom();
        Vector2 center = room.GetRoomCenter();
        for (int i = Mathf.FloorToInt(center.x - 1); i < Mathf.FloorToInt(center.x + 1); i++)
        {
            for (int j = Mathf.FloorToInt(center.y - 1); j < Mathf.FloorToInt(center.y + 1); j++)
            {
                grid[i, j].setType("Exit");
            }
        }
    }

    /* Create()
     * Spawn every element of the grid
     */ 
    public void Create()
    {
        //The empty gameobject that all the tiles are hidden inside (besides rooms)
        GameObject Facility = new GameObject(); 
        Facility.name = "Research Facility";
        //The collider that spans the floor of the dungeon (saves memory over the floor tiles having them each)
        BoxCollider ground  = Facility.AddComponent<BoxCollider>();
        ground.size = new Vector3(this.width * StageBuilder.scale, 0.25f, this.height * StageBuilder.scale);
        ground.transform.position = new Vector3(this.width / 2 * StageBuilder.scale, -0.125f, this.height / 2 * StageBuilder.scale);
        spawnExit();

        //Calls each tile's Create function, passes scale downward so they all grow
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                grid[x, y].Create(Facility.transform, StageBuilder.scale);
            }
        }

        //Make tiles that are part of rooms be children of Room gameobject
        foreach (Room room in rooms)
        {
            //instantiate roomObject
            //set position to be the middle of the room
            //get position of tile from room in global coordinates
            //Get tile from Stage.tiles
            //set tile's parent to roomObject
            GameObject roomObject = room.GetRoom();
            roomObject = new GameObject();
            Vector2 center = room.GetRoomCenter();
            roomObject.name = "Room at " + center;
            roomObject.transform.position = new Vector3(center.x, 0, center.y);
            for (int x = 0; x < room.getWidth(); x++)
            {
                for (int y = 0; y < room.getHeight(); y++)
                {
                    Vector2 position = room.getGridCoordinates(x, y);
                    GameObject tile = grid[Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y)].GetTile();
                    tile.transform.SetParent(roomObject.transform);


                }
            }
        }
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

    private bool CheckPlacement(int roomWidth, int roomHeight, int startX, int startY)
    {
        /*
         * CheckPlacement()
         * Sees if a room can be generated in a spot. 
         * Considers whether new room intersects another one or goes outside stage
         */
        //check if room can fit into stage at given coordinates
        if ((startX + roomWidth >= this.width) || (startY + roomHeight >= this.height))
        {
            return true;
        }

        //now check if room will overlap another room.
        for (int x = startX; x < startX + roomWidth; x++)
        {
            for (int y = startY; y < startY + roomHeight; y++)
            {
                //During room placement the stage is either Rock tiles or parts of rooms
                //TODO: Rooms are also going to have Wall tiles, so change this
                if (this.grid[x, y].getType() == "Floor")
                {
                    return true;
                }
            }
        }

        return false; //since the above didn't detect a collision
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


}


