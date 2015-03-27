using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        currentRegion = -1;
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

    public void PlaceRooms(int numTries)
    {
        for (int i = 0; i < numTries; i++)
        {
            //Randomly create room size
            //Note: Dimensions do not include walls around room (TODO fix that)
            int roomWidth = ensureOdd(Random.Range(5, 11)); //5 floor tiles, not 3 floor and 2 walls
            int roomHeight = ensureOdd(Random.Range(5, 11)); 
            //Randomly create location for room. Doesn't include 0 and max so that dungeon border is thick
            int randomX = ensureOdd(Random.Range(1, this.width - 1));
            int randomY = ensureOdd(Random.Range(1, this.height -1));
            //Only places room if it will remain in bounds and doesn't overlap other rooms
            if (!CheckPlacement(roomWidth, roomHeight, randomX, randomY))
            {
                //Create room to be placed
                Room room = new Room(roomWidth, roomHeight, randomX, randomY);
                CarveRoom(room, roomWidth, roomHeight, randomX, randomY);
                rooms.Add(room); 
                currentRegion++; /*Note: Maybe rooms do need an initial color for algorithm. Check it */
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
                growMaze(x, y, currentRegion);
                currentRegion++;
            }
        }
    }

    /*
     * Part of Munificent's algorithm       
     * Creates halls and numbers them accordingly
     * 
     */ 
    private void growMaze(int x, int y, int color)
    {
        var lastdir = Vector2.zero;
        int windingPercent = 90;
        var start = new Vector2(x ,y );
        var cells = new Queue();
        grid[x, y].Carve();
        grid[x, y].setColor(color);
        cells.Enqueue(start);
        while (cells.Count > 0)
        {
            var cell = cells.Dequeue();
            
            var unmadeCells = new ArrayList();
            foreach (var dir in CARDINAL)
            {
                if (canCarve((Vector2)cell, dir))
                {
                    unmadeCells.Add(dir);
                }
            }
            if (unmadeCells.Count > 0)
            {
                var direction = Vector2.zero;
                if (unmadeCells.Contains(lastdir) && Random.Range(0, 100) > windingPercent)
                {
                    direction = lastdir;
                }
                else
                {
                    direction = (Vector2)unmadeCells[Random.Range(0, unmadeCells.Count)];
                }
                Vector2 vCell = (Vector2)cell;
                int nextX = Mathf.FloorToInt( vCell.x + direction.x);
                int nextY = Mathf.FloorToInt(vCell.y + direction.y);
                int moreX = Mathf.FloorToInt(vCell.x + direction.x * 2);
                int moreY = Mathf.FloorToInt(vCell.y + direction.y * 2);
                grid[nextX, nextY].Carve();
                grid[nextX, nextY].setColor(color);
                grid[moreX, moreY].Carve();
                grid[moreX, moreY].setColor(color);
                cells.Enqueue(new Vector2(moreX, moreY));

                lastdir = direction;
            }
            else
            {
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
        /*
        // Find all of the tiles that can connect two (or more) regions.
        Dictionary<Vector2, List<int>> connectorRegions = new Dictionary<Vector2, List<int>>();
        foreach (Tile tile in tiles)
        {
            if ((tile.pos().x < 1 ) || (tile.pos().x > this.width - 2) || (tile.pos().y < 1) || (tile.pos().y > this.height -2)) continue;  
            int x = Mathf.FloorToInt(tile.pos().x);
            int y = Mathf.FloorToInt(tile.pos().y);
            // Can't already be part of a region.
            if (tile.getType() != "Rock") continue;

            var regions = new List<int>();
            foreach (var dir in CARDINAL)
            {
                int dirx = Mathf.FloorToInt(dir.x);
                int diry = Mathf.FloorToInt(dir.y);
                if (inBounds(tile.pos(), dir))
                {
                    var region = tiles[x + dirx, y + diry].getColor();
                    if (region != null) regions.Add(region);
                }
            }

            if (regions.Count < 2) continue;

            connectorRegions[new Vector2(x, y)] = regions;

        }
        var connectors = connectorRegions.Keys.ToList();


        // Keep track of which regions have been merged. This maps an original
        // region index to the one it has been merged to.
        var merged = new ArrayList();
        var openRegions = new HashSet<int>();
        for (var i = 0; i <= currentRegion; i++)
        {
            merged.Add(i);
            openRegions.Add(i);
        }

        // Keep connecting regions until we're down to one.
        while (openRegions.Count > 1)
        {
            var connector = connectors[Random.Range(0, connectors.Count() + 1)];

            // Carve the connection.
            addJunction(connector);

            // Merge the connected regions. We'll pick one region (arbitrarily) and
            // map all of the other regions to its index.
            var regions = connectorRegions[connector]
                .Select((region) => merged[region]);
            var dest = regions.First();
            var sources = regions;

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
            List<int> clone = new List<int>();
            foreach (int item in sources)
            {
                clone.Add(item);
            }
            for (int i = 0; i < sources.ToList().Count; i++)
            {
                openRegions.Remove((int)sources.ToList()[i]);
            }

            // Remove any connectors that aren't needed anymore.
            List<Vector2> copy = new List<Vector2>();
            foreach (Vector2 pos in connectors)
            {
                // Don't allow connectors right next to each other.
                if ((connector - pos).magnitude < 2)
                {
                    copy.Add(new Vector2(pos.x, pos.y));
                    continue;
                }
                // If the connector no long spans different regions, we don't need it.
 
                var _regions = connectorRegions[pos].Select((region) => merged[region])
                    .ToList().Distinct();

                // This connecter isn't needed, but connect it occasionally so that the
                // dungeon isn't singly-connected.
                int extraConnectorChance = 20;
                if (oneIn(extraConnectorChance)) addJunction(pos);

            }

            for (int i = 0; i < copy.Count(); i++)
            {
                connectors.Remove(copy[i]);
            }

        }


        //iterate through stage.rooms
        //iterate through boundary tiles and pick 1 or maybe more to turn into doors
        */
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
                int x = Mathf.FloorToInt(tile.pos().x);
                int y = Mathf.FloorToInt(tile.pos().y);
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
        return (Room)rooms[Random.Range(0, rooms.Count)];
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
        int x = Mathf.FloorToInt(cell.x + dir.x * 2); 
        int y = Mathf.FloorToInt(cell.y + dir.y * 2);
        return grid[x, y].getType() == "Rock";
    }

 
    //Built because munificent's code has it. basically "one in x" chance something happens
    private bool oneIn(int num)
    {
        return Random.Range(1, num + 1) == num; 
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
                grid[x, y].setColor(-1); //Rooms don't need different colors at this point    
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
                if (this.grid[x, y].getColor() == -1)
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


