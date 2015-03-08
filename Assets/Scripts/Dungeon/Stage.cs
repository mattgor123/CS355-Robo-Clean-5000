using UnityEngine;
using System.Collections;

public class Stage  {

    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    private Tile[,] tiles;
    private ArrayList rooms;

    /*Constructor for stage
     * Initializes grid of tiles to standard Tiles
     */ 
    public Stage(int width, int height)
    {
        this.width = width;
        this.height = height;
        tiles = new Tile[width, height];
        rooms = new ArrayList();
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                tiles[x, y] = new Tile();
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
                if (this.tiles[x, y].GetType() != typeof(Tile))
                {
                    return true;
                }
            }
        }

        return false; //since the above didn't detect a collision
    }
    /*PlaceRooms()
     * Try to place numTries number of rooms onto stage
     */ 
    public void PlaceRooms(int numTries) {
        for (int i = 0; i < numTries; i++)
        {
            int roomWidth = Random.Range(4, 10); 
            int roomHeight = Random.Range(4, 10); 
            int randomX = Random.Range(0, this.width);
            int randomY = Random.Range(0, this.height);

            bool overlap = CheckPlacement(roomWidth, roomHeight, randomX, randomY);

            if (!overlap)
            {
                Room room = new Room(roomWidth, roomHeight, randomX, randomY);
                for (int x = randomX; x < randomX + roomWidth; x++)
                {
                    for (int y = randomY; y < randomY + roomHeight; y++)
                    {
                        tiles[x, y] = room.getTile(x - randomX, y - randomY);
                        rooms.Add(room);
                    }
                }
            }

        }
    }

    /*FloodFill()
     * Fill the remaining tiles of stage with maze
     */
    public void FloodFill()
    {


    }

    /*OpenDoors
     * Picks a Blank tile between hall and room
     * Replaces hall tile, blank tile, and room tile to form entrance
     * may happen multiple times per room.
     */ 
    public void OpenDoors()
    {
        //iterate through stage.rooms
        //iterate through boundary tiles and pick 1 or maybe more to turn into doors

    }
    /*TrimHalls
     * Remove some of the tiles at the ends of the mazes
     * Makes for fewer deadends.
     */
    public void TrimHalls()
    {


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
                        if (this.tiles[x, y].GetType().FullName.Equals("Tile+Floor"))
                        {
                            Tile.Floor tile = this.tiles[x, y] as Tile.Floor;
                            tile.Create();
                        }
            }
        }
    }

    //Return string topdown view of grid. 
    //Doesn't display nicely because console is not monotype font
    public string ToString()
    {
        string result = "";
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                if (tiles[x, y].GetType().FullName.Equals("Tile"))
                {
                    result += "_";
                }
                if (tiles[x, y].GetType().FullName.Equals("Tile+Floor"))
                {
                    result += "+";
                }
                if (tiles[x, y].GetType().FullName.Equals("Tile+Boundary"))
                {
                    result += "=";
                }
            }
            result += "\n";
        }
        return result;
    }
        

}


