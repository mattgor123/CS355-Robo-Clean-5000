using UnityEngine;
using System.Collections;


/*Room in dungeon
 * Made up of floor tiles 
 * tiles on edges must have approrpriate walls
 */ 
public class Room : MonoBehaviour {
    private Tile[,] room;
    private int width;
    private int height;

    public Room(int width, int height)
    {
        this.width = width;
        this.height = height;
        room =  new Tile[width, height];

        //Generate all the tiles for the room
        for (int x = 1; x < width; x++ )
        {
            for (int y = 0; y < height; y++)
            {
                if (x == 0 && y == 0) //Bottom left corner piece
                {
                    room[x, y] = new Tile.Floor(x, y, false, true, true, false);
                }
                else if (x == 0 && y == height - 1) //Top left corner piece
                {
                    room[x, y] = new Tile.Floor(x, y, true, false, true, false);

                }
                else if (x == width -1 && y == 0) //Bottom right corner piece 
                {
                    room[x, y] = new Tile.Floor(x, y, false, true, false, true);
                }
                else if (x == width - 1 && y == height - 1) //Top right corner piece
                {
                    room[x, y] = new Tile.Floor(x, y, true, false, false, true);
                }
                else if (x == 0) //Left Edge
                {
                    room[x, y] = new Tile.Floor(x, y, false, false, true, false);
                }
                else if (x == width - 1) //Right Edge
                {
                    room[x, y] = new Tile.Floor(x, y, false, true, false, false);
                }
                else if (y == 0) //Bottom Edge
                {
                    room[x, y] = new Tile.Floor(x, y, false, true, false, false);
                }
                else if (y == height - 1) //Top Edge
                {
                    room[x, y] = new Tile.Floor(x, y, false, true, false, false);
                }
            }
        }
    }

    public int getWidth()
    {
        return this.width;
    }
    public int getHeight()
    {
        return this.height;
    }

    internal Tile getTile(int x, int y)
    {
        return this.room[x, y];
    }
}
