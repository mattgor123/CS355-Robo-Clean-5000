using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Stage  {

    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    private Tile[,] tiles;
    private ArrayList rooms;
    private int currentRegion; //the color of the region being carved

    private Vector2[] CARDINAL = { Vector2.up, Vector2.up * -1, Vector2.right, Vector2.right * -1 };

    /*Constructor for stage
     * Initializes grid of tiles to standard Tiles
     */ 
    public Stage(int width, int height, Material floor, Material wall)
    {
        this.width = width;
        this.height = height;
        tiles = new Tile[width, height];
        rooms = new ArrayList();
        currentRegion = -1;
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                tiles[x, y] = new Tile("Rock", new Vector3(x, 0, y), floor, wall);
            }
        }
    }

    /*CheckPlacement()
     * Sees if a room can be generated in a spot. 
     * Considers whether new room intersects another one or goes past stage
     */ 
    public bool CheckPlacement(int roomWidth, int roomHeight, int startX, int startY)
    {
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
                //During room placement the stage is either "blank" tiles or parts of rooms
                if (this.tiles[x, y].getColor() == -1)
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
    /*PlaceRooms()
     * Try to place numTries number of rooms onto stage
     */ 
    public void PlaceRooms(int numTries) {
        for (int i = 0; i < numTries; i++)
        {
            int roomWidth = ensureOdd(Random.Range(5, 11));
            int roomHeight = ensureOdd(Random.Range(5, 11)); 
            int randomX = ensureOdd(Random.Range(0, this.width));
            int randomY = ensureOdd(Random.Range(0, this.height));
            bool overlap = CheckPlacement(roomWidth, roomHeight, randomX, randomY);
            if (!overlap)
            {
                Room room = new Room(roomWidth, roomHeight, randomX, randomY);
                for (int x = randomX; x < randomX + roomWidth; x++)
                {
                    for (int y = randomY; y < randomY + roomHeight; y++)
                    {
                        tiles[x, y].Carve();
                        tiles[x, y].setColor(-1); //Rooms don't need different colors, only non rooms
                        
                    }
                }
                rooms.Add(room);
                currentRegion++;
            }

        }
    }

    public void fillRooms()
    {
        //traverse the edge of the room
        //store the locations of tiles that are like doors
    }

    public void PlaceHalls()
    {
        for (var x = 1; x < this.width; x += 2)
        {
            for (var y = 1; y < this.height; y += 2)
            {

                if (tiles[x, y].getType() != "Rock") continue;
                FloodFill(x, y, currentRegion);
                currentRegion++;

            }
        }

    }


    public void FloodFill(int x, int y, int color)
    {
        var lastdir = Vector2.zero;
        int windingPercent = 45;
        var start = new Vector2(x ,y );
        var cells = new Queue();
        tiles[x, y].Carve();
        tiles[x, y].setColor(color);
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
                tiles[nextX, nextY].Carve();
                tiles[nextX, nextY].setColor(color);
                tiles[moreX, moreY].Carve();
                tiles[moreX, moreY].setColor(color);
                cells.Enqueue(new Vector2(moreX, moreY));

                lastdir = direction;
            }
            else
            {
                lastdir = Vector2.zero;
            }
        }
    }

    private bool inBounds(Vector2 cell, Vector2 dir) {
        var sum = cell + dir * 3;
        if (sum.x >= this.width || sum.x < 0 || sum.y > this.height || sum.y < 0)
        {
            return false;
        }
        return true;
    }

    private bool canCarve(Vector2 cell, Vector2 dir) {

        if (!inBounds(cell, dir)) {
            return false;
        }
        int x = Mathf.FloorToInt(cell.x + dir.x * 2);
        int y = Mathf.FloorToInt(cell.y + dir.y * 2);
        return tiles[x, y].getType() == "Rock";
    }

    

    /*OpenDoors
     * Picks a Blank tile between hall and room
     * Replaces hall tile, blank tile, and room tile to form entrance
     * may happen multiple times per room.
     */
    public void OpenDoors()
    {

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

    }


    bool oneIn(int num)
    {
        return Random.Range(1, num + 1) == num;
    }


    /*TODO: Make the required Tiles and add this to addJunction()
    if (rng.oneIn(4))
    {
        setTile(pos, rng.oneIn(3) ? Tiles.openDoor : Tiles.floor);
    }
    else
    {
        setTile(pos, Tiles.closedDoor);
    }
    */
    private void addJunction(Vector2 pos)
    {

        tiles[Mathf.FloorToInt(pos.x), Mathf.FloorToInt(pos.y)].setType("Floor");
    }
    /*TrimHalls
     * Remove some of the tiles at the ends of the mazes
     * Makes for fewer deadends.
     */
    public void removeDeadEnds()
    {
        var done = false;

        while (!done)
        {
            done = true;

            foreach (Tile tile in tiles)
            {
                int x = Mathf.FloorToInt(tile.pos().x);
                int y = Mathf.FloorToInt(tile.pos().y);
                if (tiles[x, y].getType() == "Rock") continue;

                // If it only has one exit, it's a dead end.
                var exits = 0;
                foreach (var dir in CARDINAL)
                {
                    int dirx = Mathf.FloorToInt(dir.x);
                    int diry = Mathf.FloorToInt(dir.y);
                    if (tiles[x + dirx, y + diry].getType() != "Rock") exits++;
                }

                if (exits != 1) continue;

                done = false;
                tiles[x, y].setType("Rock");
            }
        }
    }



    /* Create()
     * Spawn the rooms and floors added to the grid.
     */ 
    public void Create()
    {
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                tiles[x, y].Create();
                
            }
        }

        //Make tiles that are part of rooms be children of Room gameobject
        foreach (Room room in rooms)
        {
            //instantiate roomObject
            //set position to be the middle of its tiles
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
                    Vector2 position = room.getWorldCoordinates(x, y);
                    GameObject tile = tiles[Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y)].GetTile();
                    tile.transform.SetParent(roomObject.transform);


                }
            }
        }
    }

    //Return string topdown view of grid. 
    //Doesn't display nicely because console is not monotype font
    public string ToString()
    {
		string result = "Stage";
        return result;
    }


}


