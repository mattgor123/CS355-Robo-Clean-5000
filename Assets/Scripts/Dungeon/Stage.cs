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
        for (int i = 0; i < numTries; i++)
        {
            int roomWidth = Random.Range(7, 20);
            int roomHeight = Random.Range(7, 20);
            int randomX = Random.Range(0, this.width);
            int randomY = Random.Range(0, this.height);
            bool overlap = CheckPlacement(roomWidth, roomHeight, randomX, randomY);
            if (!overlap) { //room doesn't overlap, so add it to stage
                Room room = new Room(roomWidth, roomHeight);
                for (int x = randomX; x < randomX + roomWidth; x++)
                {
                    for (int y = randomY; y < randomY + roomHeight; y++)
                    {
                        tiles[x - randomX, y - randomY] = room.getTile(x - randomX, y - randomY);
                    }
                }
            }
        }
    }

    public void Create()
    {
        for (int x = 0; x < this.width; x++)
        {
            for (int y = 0; y < this.height; y++)
            {

                    Debug.Log("Spawning things!");
                    Tile.Floor tile = this.tiles[x, y] as Tile.Floor;
                    if (tile.GetStatus())
                    {
                        tile.Create();
                    }
                    else
                    {
                        Debug.Log("Tile not Floor");
                    }
                
            }
        }
    }

}


