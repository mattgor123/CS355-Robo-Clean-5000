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
