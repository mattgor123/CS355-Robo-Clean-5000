using UnityEngine;
using System.Collections;

//Builds all the fluff: lights; effects; etc
public class FluffBuilder : MonoBehaviour
{

    #region SerializeFields

    [SerializeField]
    private Transform WallLight;
    [SerializeField]
    private Transform CornerLight;

    #endregion


    private ArrayList FluffList; //list of fluff spots
    private Tile[,] Grid;        //the grid of tiles to plop spots down on
    private float Scale;        //scale factor (for coordinates)
    private float Width;        //width (X) of map
    private float Height;       //height (Z) of map

    //Builds all the fluff
    public void BuildFluff(Tile [,] gd, float scale)
    {
        //Prep variables
        FluffList = new ArrayList();
        Grid = gd;
        Scale = scale;
        Width = gd.GetLength(0);
        Height = gd.GetLength(1);

        //Loop through the grid, adding fluff spots as appropriate
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                //Check walls for adding of lights
                if (Grid[i, j].getType() == "Rock")
                {




                }

            }
        }

    }

    //Adds a wall light type fluff spot if able
    void AddWallLight(int i, int j)
    {



        //FluffList.Add(new FluffSpot(pos, rot, WallLight));
    }

    //Adds a corner light type fluff spot
    void AddCornerLight(Vector3 pos, float rot)
    {
        FluffList.Add(new FluffSpot(pos, rot, CornerLight));
    }
}
