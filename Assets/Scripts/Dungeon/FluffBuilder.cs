using UnityEngine;
using System.Collections;

//Builds all the fluff: lights; effects; etc
public class FluffBuilder : MonoBehaviour
{

    #region Fluff Item Vars

    [SerializeField]
    private Transform WallLight;
    [SerializeField]
    private float WallLightProb;                    //probability of wall light
    private Color WallLightColor = Color.white;     //color of wall light
    private float WallLightFTime = 5f;              //base flicker time
    private float WallLightFSign = 0;               //sign preference (1 for all positive, -1 for all negative)

    [SerializeField]
    private Transform CornerLight;
    [SerializeField]
    private float CornerLightProb;  //probability of corner light

    [SerializeField]
    private Transform Smoke;       //Smoke w. sparks
    [SerializeField]
    private float SmokeProb;

    [SerializeField]
    private Transform Dust;     //small bits of falling dust
    [SerializeField]
    private float DustProb;

    [SerializeField]
    private Transform VolumeSteam;  //steam low on ground; Prefab taken from WaterFX Pack by Unity
    [SerializeField]
    private float VolumeSteamProb;

    /*
    [SerializeField]
    private Transform 
    [SerializeField]
    private float 
    */


    #endregion


    private GameObject Fluff;
    private Tile[,] Grid;        //the grid of tiles to plop spots down on
    private float Scale;        //scale factor (for coordinates)
    private float Width;        //width (X) of map
    private float Height;       //height (Z) of map

    //Builds all the fluff
    public void BuildFluff(Tile [,] gd, float scale)
    {
        //Prep variables
        Fluff = new GameObject();
        Fluff.name = "Fluff";
        Fluff.transform.position = new Vector3(0f, 0f, 0f);
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
                    AddParticles(i, j);
                }
            }
        }

    }

    //Destroys the fluff
    public void DestroyFluff()
    {
        GameObject.Destroy(Fluff);
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
        wall_light.SetParent(Fluff.transform);
        EMLightController EMLC = wall_light.GetComponent<EMLightController>();
        float sign = 2*(Random.value - 0.5f) + WallLightFSign;
        sign /= Mathf.Abs(sign);
        EMLC.SetFlickerTime( WallLightFTime * (1 + 0.3f*(Random.value - 0.5f) )  * sign);
        EMLC.SetColor(WallLightColor);
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

    #region Particle Systems
    //Adds particle systems
    void AddParticles(int i, int j)
    {
        //Smoke
        if (Random.value <= SmokeProb)
        {
            Vector3 pos = new Vector3(i * Scale, 0f, j * Scale);
            MakeSmoke(pos);
        }

        //VolumeSteam; mutually exclusive with smoke
        else if (Random.value <= VolumeSteamProb)
        {
            Vector3 pos = new Vector3(i * Scale, 0f, j * Scale);
            MakeVolumeSteam(pos);
        }

        //Dust
        if (Random.value <= DustProb)
        {
            Vector3 pos = new Vector3(i * Scale, 0f, j * Scale);
            MakeDust(pos);
        }
     
    }
    void MakeSmoke(Vector3 pos)
    {
        Transform thing = Instantiate(Smoke);
        thing.transform.position += pos;
        thing.SetParent(Fluff.transform);
    }
    void MakeDust(Vector3 pos)
    {
        Transform thing = Instantiate(Dust);
        thing.transform.position += pos;
        thing.SetParent(Fluff.transform);
    }
    void MakeVolumeSteam(Vector3 pos)
    {
        Transform thing = Instantiate(VolumeSteam);
        thing.transform.position += pos;
        thing.SetParent(Fluff.transform);
    }

    #endregion

    #region Setup
    public void SetupWallLights(float prob, Color color, float ftime, float fsign)
    {
        WallLightProb = prob;
        WallLightColor = color;
        WallLightFTime = ftime;
        WallLightFSign = fsign;
    }
    public void SetupSmoke(float prob)
    {
        SmokeProb = prob;
    }
    public void SetupDust(float prob)
    {
        DustProb = prob;
    }

    public void SetupPreset(string name)
    {
        if (name == "Emergency")
        {
            SmokeProb = 0.1f;
            WallLightColor = Color.red;
            WallLightFTime = .1f;
            WallLightProb = 0.75f;
        }

    }

    #endregion

}
