using UnityEngine;
using System.Collections;


/*Room in dungeon
 * Made up of floor tiles 
 * tiles on edges must have approrpriate walls
 */ 
public class Room {
    private Tile[,] room;
    private int width;
    private int height;

    public Room(int width, int height, int startX, int startY)
    {
        this.width = width;
        this.height = height;
        room =  new Tile[width + 1, height + 1];

        //Generate all the tiles for the room
        for (int x = 1; x < width; x++ )
        {
            for (int y = 1; y < height; y++)
            {
                if (x == 1 && y == 1) //South + West corner piece
                {                                                     //North, South, West, East
                    room[x, y] = new Tile.Floor(x + startX, y + startY, false, true, true, false);

                }
                else if (x == 1 && y == height - 1) //North West corner piece
                {                                                     //North, South, West, East
                    room[x, y] = new Tile.Floor(x + startX, y + startY, true, false, true, false);

                }
                else if (x == width -1 && y == 1) //south east corner piece 
                {                                                     //North, South, West, East
                    room[x, y] = new Tile.Floor(x + startX, y + startY, false, true, false, true);

                }
                else if (x == width - 1 && y == height - 1) //northeast corner piece
                {                                                     //North, South, West, East
                    room[x, y] = new Tile.Floor(x + startX, y + startY, true, false, false, true);

                }
                else if (x == 1) //west Edge
                {                                                     //North, South, West, East
                    room[x, y] = new Tile.Floor(x + startX, y + startY, false, false, true, false);

                }
                else if (x == width - 1) //east Edge
                {                                                     //North, South, West, East
                    room[x, y] = new Tile.Floor(x + startX, y + startY, false, false, false, true);

                }
                else if (y == 1) //south Edge
                {                                                     //North, South, West, East
                    room[x, y] = new Tile.Floor(x + startX, y + startY, false, true, false, false);

                }
                else if (y == height - 1) //north Edge
                {                                                     //North, South, West, East
                    room[x, y] = new Tile.Floor(x + startX, y + startY, true, false, false, false);

                }
                else //middle pieces (no walls)
                {                                                     //North, South, West, East
                    room[x, y] = new Tile.Floor(x + startX, y + startY, false, false, false, false);

                }
            }
        }
        //Generate Boundary around room

        //Bottom boundary
        int i = 0;
        int j = 0;
        for (i = 0; i < this.width + 1; i++)
        {
            room[i, j] = new Tile.Boundary();
        }

        //top boundary
        j = this.height;
        i = 0;
        for (i = 0; i < this.width + 1; i++)
        {
            room[i, j] = new Tile.Boundary();
        }

        //left boundary
        i = 0;
        j = 0;
        for (j = 0; j < this.height + 1; j++)
        {
            room[i, j] = new Tile.Boundary();
        }

        //right boundary
        i = this.width;
        j = 0;
        for (j = 0; j < this.height + 1; j++)
        {
            room[i, j] = new Tile.Boundary();
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

    public Tile getTile(int x, int y)
    {
        return this.room[x, y];
    }
}
