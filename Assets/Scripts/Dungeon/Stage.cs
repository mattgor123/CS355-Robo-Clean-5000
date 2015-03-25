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
                tiles[x, y] = new Tile("Blank", new Vector3(x, 0, y));
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
                if (this.tiles[x, y].getType() != "Blank")
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
                        tiles[x, y].setType(room.GetTile(x - randomX, y - randomY));
                        tiles[x, y].setColor(-1); //Rooms don't need different colors, only non rooms
                        
                    }
                }
                rooms.Add(room);
            }

        }
    }

    public void PlaceHalls()
    {
        int region = 1; //color region. As FloodFill exits this increments before going back in
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++) {
                Debug.Log(tiles[x, y].getColor());
                if (tiles[x, y].getType().Equals("Blank"))
                {
                    this.FloodFill(tiles[x, y], region);
                    region++;
                }
            }

        }

    }

    /*FloodFill()
     * Fill the remaining tiles of stage with maze
         Flood-fill (node, target-color, replacement-color):
         1. If the color of node is not equal to target-color, return.
         2. Set Q to an empty queue
         3. Add node to Q.
         4. For each element N of Q:
         5.         Set w and e equal to N.
         6.         Move w to the west until the color of the node to the west of w no longer matches target-color.
         7.         Move e to the east until the color of the node to the east of e no longer matches target-color.
         8.         For each node n between w and e:
         9.             Set the color of n to replacement-color.
        10.             If the color of the node to the north of n is target-color, add that node to Q.
        11.             If the color of the node to the south of n is target-color, add that node to Q.
        12. Continue looping until Q is exhausted.
        13. Return.
     */
    public void FloodFill(Tile startTile, int replace)
    {
        //Debug.Log(startTile.ToString() + " has color " + startTile.getColor());
        Queue queue = new Queue();                  //2.
        queue.Enqueue(startTile);                   //3.
        while (queue.Count > 0)                     //4.
        {
            Tile tile = (Tile) queue.Dequeue();
            int x = tile.getX();
            int y = tile.getY();
            int east = x;                           //5.
            int west = x;                           //5.
            while (tiles[east, y].getColor() == 0)  //6.
            {
                east += 1;
                if (east >= this.width)
                {
                    east -= 1; //we dont want it out of bounds.
                    break;
                }

            }
            while (this.tiles[west, y].getColor() == 0)  //7.
            {
                west -= 1;
                if (west < 0)
                {
                    west += 1; //we don't want it out of bounds.
                    break;
                }
            }
            for (int i = west; i < east + 1; i++)   //8.
            {

                bool N = false;
                bool E = false;
                bool W = false;
                bool S = false;
                if (i == west) {
                    W = true; //left most part of region, so it definitely has a West-facing wall.
                }
                if (i == east)
                {
                    E = true; //right most part of region, so it definitely has an East-facing wall.
                }
                this.tiles[i, y].setColor(replace);       //9.
                if ((y + 1 < this.width) && this.tiles[i, y + 1].getColor() == 0) //tile to north
                {
                    queue.Enqueue(this.tiles[i, y + 1]);  //10.
                }
                else
                {
                    N = true; //Since the tile to the north is out of bounds or wrong color, this tile gets a north wall
                }
                if ((y - 1 >= 0) && tiles[i, y - 1].getColor() == 0) //tile to south
                {
                    queue.Enqueue(tiles[i, y - 1]);  //11.
                }
                else
                {
                    S = true; //Since the tile to the south is out of bounds or wrong color, this tile gets a south wall
                }
                tiles[i, y].DecideType(N, S, E, W);
            }

        }   //while queue not empty                  //12.
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


