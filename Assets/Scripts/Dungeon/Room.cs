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
    private bool isElevator;

    public Room(int width, int height, int startX, int startY)
    {
        this.width = width;
        this.height = height;
        this.startX = startX;
        this.startY = startY;
        room = new string[width, height];
        this.isElevator = false;

    }

	/*Deep Copy Constructor */
	public Room(Room other) {
		this.width = other.width;
		this.height = other.height;
		this.startX = other.startX;
		this.startY = other.startY;
		this.isElevator = other.isElevator;

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

    public void setStartX(int x)
    {
        this.startX = x;
    }

    public void setStartY(int y)
    {
        this.startX = y;
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
        float x = (float) startX + this.width / 2f;
        float y = (float) startY + this.height / 2f;
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

    public void setIsElevator(bool b) {
        this.isElevator = b;
    }

    public bool getIsElevator()
    {
        return this.isElevator;
    }
}
