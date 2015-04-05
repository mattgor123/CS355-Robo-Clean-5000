using UnityEngine;
using System.Collections;

//Builds all the fluff: lights; effects; etc
public class FluffBuilder : MonoBehaviour
{

    #region SerializeFields

    [SerializeField]
    private Transform WallLight;
    [SerializeField]
    private float WallLightProb;    //probability of wall light

    [SerializeField]
    private Transform CornerLight;
    [SerializeField]
    private float CornerLightProb;  //probability of corner light

    [SerializeField]
    private Transform GasOne;       //Gas type one
    [SerializeField]
    private float GasOneProb;




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
        for (int i = 1; i < Width - 1; i++)
        {
            for (int j = 1; j < Height - 1; j++)
            {
                //Check walls
                if (Grid[i, j].getType() == "Rock")
                {
                    AddWallLight(i, j);
                }

                //Check floors
                if (Grid[i, j].getType() == "Floor")
                {
                    //AddCornerLight(i, j); not fully functional yet
                    AddGas(i, j);
                }
            }
        }

    }

    #region Wall Light
    //Adds a wall light if able
    void AddWallLight(int i, int j)
    {
        Vector3 pos = new Vector3(0f, 0f, 0f);
        float rot = 0;
        bool add = false;

        if (Random.value <= WallLightProb)
        {
            //Check for adjacent walls
            if (Grid[i + 1, j].getType() == "Rock" && Grid[i - 1, j].getType() == "Rock")
            {
                //Check perpendicular for a floor
                if (Grid[i, j + 1].getType() == "Floor")
                {
                    rot = 180;
                    pos = new Vector3(i * Scale, 0f, j * Scale - Scale/2);
                    add = true;
                }
                else if (Grid[i, j - 1].getType() == "Floor")
                {
                    rot = 0;
                    pos = new Vector3(i * Scale, 0f, j * Scale + Scale/2);
                    add = true;
                }
            }

            if (Grid[i, j+1].getType() == "Rock" && Grid[i, j-1].getType() == "Rock")
            {
                //Check perpendicular for a floor
                if (Grid[i+1, j].getType() == "Floor")
                {
                    rot = 90;
                    pos = new Vector3(i * Scale + Scale / 2, 0f, j * Scale);
                    add = true;
                }
                else if (Grid[i-1, j].getType() == "Floor")
                {
                    rot = 270;
                    pos = new Vector3(i * Scale - Scale / 2, 0f, j * Scale);
                    add = true;
                }
            }

            if (add)
            {
                MakeWallLight(pos, rot);
            }
        }
    }

    //Makes a wall light
    void MakeWallLight(Vector3 pos, float rot)
    {
        Transform wall_light = Instantiate(WallLight);
        wall_light.transform.position += pos;
        wall_light.transform.eulerAngles = new Vector3(0f, rot, 0f);
        wall_light.SetParent(this.transform);
        EMLightController EMLC = wall_light.GetComponent<EMLightController>();
        float sign = Random.value - 0.5f;
        sign /= Mathf.Abs(sign);
        EMLC.SetFlickerTime( (5f + 2f*Random.value) * sign);
    }
    #endregion

    #region Corner Light
    //Adds a corner light if able
    void AddCornerLight(int i, int j)
    {
        Vector3 pos = new Vector3(0f, 0f, 0f);
        float rot = 0;
        bool add = false;

        if (Random.value <= CornerLightProb)
        {
            //Check for corner (i,j is the center floor tile)
            if (Grid[i - 1, j].getType() == "Rock" && Grid[i, j+1].getType() == "Rock")
            {
                rot = 0;
                pos = new Vector3(i * Scale - Scale / 2, 0f, j * Scale - Scale / 2);
                add = true;
            }
            else if (Grid[i + 1, j].getType() == "Rock" && Grid[i, j + 1].getType() == "Rock")
            {
                rot = 90;
                pos = new Vector3(i * Scale + Scale / 2, 0f, j * Scale - Scale / 2);
                add = true;
            }
            else if (Grid[i + 1, j].getType() == "Rock" && Grid[i, j - 1].getType() == "Rock")
            {
                rot = 180;
                pos = new Vector3(i * Scale + Scale / 2, 0f, j * Scale + Scale / 2);
                add = true;
            }
            else if (Grid[i - 1, j].getType() == "Rock" && Grid[i, j - 1].getType() == "Rock")
            {
                rot = 270;
                pos = new Vector3(i * Scale - Scale / 2, 0f, j * Scale + Scale / 2);
                add = true;
            }

            if (add)
            {
                MakeCornerLight(pos, rot);
            }
        }
    }

    //Makes a corner light
    void MakeCornerLight(Vector3 pos, float rot)
    {
        Transform c_light = Instantiate(CornerLight);
        c_light.transform.position += pos;
        c_light.transform.eulerAngles = new Vector3(0f, rot, 0f);
        c_light.SetParent(this.transform);
        EMLightController EMLC = c_light.GetComponent<EMLightController>();
        float sign = Random.value - 0.5f;
        sign /= Mathf.Abs(sign);
        EMLC.SetFlickerTime((5f + 2f * Random.value) * sign);
    }
    #endregion

    #region Gases
    //Adds gases
    void AddGas(int i, int j)
    {
        //Type one gas
        if (Random.value <= GasOneProb)
        {
            Vector3 pos = new Vector3(i * Scale, 0f, j * Scale);
            MakeGasOne(pos);
        }

        //Type two gas
    }
    //Makes the type one gas
    void MakeGasOne(Vector3 pos)
    {
        Transform g_one = Instantiate(GasOne);
        g_one.transform.position += pos;
        g_one.SetParent(this.transform);
    }
    #endregion

}
