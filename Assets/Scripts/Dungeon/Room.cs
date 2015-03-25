using UnityEngine;
using System.Collections;


/*Room in dungeon
 * Made up of floor tiles 
 * tiles on edges must have approrpriate walls
 */ 
public class Room {
    private int width;
    private int height;
    private int startX;
    private int startY;
    private GameObject roomObject;
    private string[,] room;
    public Room(int width, int height, int startX, int startY)
    {
        this.width = width;
        this.height = height;
        this.startX = startX;
        this.startY = startY;
        room = new string[width, height];

        //Generate all the tiles for the room
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {

                if (x == 0 && y == 0) //South + West corner piece
                {
                    room[x, y] = "SWCorner";

                }
                else if (x == 0 && y == height - 1) //North West corner piece
                {
                    room[x, y] = "NWCorner";

                }
                else if (x == width - 1 && y == 0) //south east corner piece 
                {
                    room[x, y] = "SECorner";

                }
                else if (x == width - 1 && y == height - 1) //northeast corner piece
                {
                    room[x, y] = "NECorner";

                }
                else if (x == 0) //west Edge
                {
                    room[x, y] = "WWall";

                }
                else if (x == width - 1) //east Edge
                {
                    room[x, y] = "EWall";

                }
                else if (y == 0) //south Edge
                {
                    room[x, y] = "SWall";

                }
                else if (y == height - 1) //north Edge
                {
                    room[x, y] = "NWall";

                }
                else //middle pieces (no walls)
                {
                    room[x, y] = "Floor";
                }
            }
        }
    }

    //Where each tile exists in the world
    public Vector2 getWorldCoordinates(int x, int y)
    {
        return new Vector2(x + startX, y + startY);
    }

    public GameObject GetRoom()
    {
        return roomObject;
    }

    public Vector2 GetRoomCenter()
    {
        int x = startX + this.width / 2;
        int y = startY + this.height / 2;
        return new Vector2(x, y);
    }

    public int getWidth()
    {
        return this.width;
    }
    public int getHeight()
    {
        return this.height;
    }

    public string GetTile(int x, int y)
    {
        return this.room[x, y];
    }
}
