using UnityEngine;
using System.Collections;

public class Stage  {

    [SerializeField]
    private int width;
    [SerializeField]
    private int height;
    private Tile[,] tiles;
    private ArrayList rooms;

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
                Debug.Log(tiles[x, y].GetStatus());
            }
        }
    }

    public bool CheckPlacement(int roomWidth, int roomHeight, int randomX, int randomY)
    {

        if ((randomX + roomWidth < this.width) && (randomY + roomHeight < this.height)) {
            //now check if room will overlap another room.
            for (int x = randomX; x < randomX + roomWidth; x++)
            {
                for (int y = randomY; y < randomY + roomHeight; y++)
                {
                    if (this.tiles[x, y].GetType() == typeof(Tile.Floor)) {
                        return true;
                    }
                }
            }
        }
        return false; //since the above didn't detect a collision
    }

    public void PlaceRooms(int numTries) {
       // for (int i = 0; i < 1; i++)
        //{
            int roomWidth = 3;
            int roomHeight = 3;
            int randomX = 1;
            int randomY = 1;
            Room room = new Room(roomWidth, roomHeight, randomX, randomY);
            for (int x = randomX; x < randomX + roomWidth; x++)
            {
                for (int y = randomY; y < randomY + roomHeight; y++)
                {
                    tiles[x, y] = room.getTile(x - randomX, y - randomY);
                    Debug.Log(tiles[x, y]);
                }
            }
           // bool overlap = CheckPlacement(roomWidth, roomHeight, randomX, randomY);

/*            if (!overlap) { //room doesn't overlap, so add it to stage
                Room room = new Room(roomWidth, roomHeight);
                for (int x = randomX; x < randomX + roomWidth; x++)
                {
                    for (int y = randomY; y < randomY + roomHeight; y++)
                    {
                        tiles[x - randomX, y - randomY] = room.getTile(x - randomX, y - randomY);
                        Debug.Log("Added " + room.getTile(x - randomX, y - randomY));
                    }
                }
            }
 */ 
        //}
    }

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

    public string ToString()
    {
        string result = "";
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {
                result += tiles[x, y].GetType().FullName + ", ";
            }
            result += "\n";
        }
        return result;
    }
        

}


