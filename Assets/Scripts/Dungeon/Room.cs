using UnityEngine;
using System.Collections;
using System.Collections.Generic;


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

    }

    public bool DistanceTo(Room room)
    {
        //http://stackoverflow.com/questions/306316/determine-if-two-rectangles-overlap-each-other
        return (this.startX <= (room.startX + room.width) && (this.startX + this.width) >= room.startX &&
            this.startY <= (room.startY + room.height) && (this.startY + this.height) >= room.startY);

    }

    public List<Vector2> getBorderTiles()
    {
        List<Vector2> list = new List<Vector2>();
        for (int i = 0; i < this.width; i++)
        {
            for (int j = 0; j < this.height; j++)
            {
                //Left Side of room
                if (i == 0)
                {
                    list.Add(getGridCoordinates(i - 1, j));
                }
                //Bottom Side of room
                if (j == 0 )
                {
                    list.Add(getGridCoordinates(i, j - 1));

                }
                //Top Side of room
                if (j == this.height - 1) {
                    list.Add(getGridCoordinates(i, j + 1));
                }
                //Right Side of room
                if (i == this.width - 1) {
                    list.Add(getGridCoordinates(i + 1, j));

                }

            }
        }

            return list;
    }

    //Where each tile exists in the world
    public Vector2 getGridCoordinates(int x, int y)
    {
        return new Vector2(x + startX, y + startY);
    }

    public Vector2 getGridCoordinates(Vector2 pos)
    {

        return new Vector2(pos.x + startX, pos.y + startY);
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
