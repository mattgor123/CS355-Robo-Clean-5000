using UnityEngine;
using System;

//Marks a spot to place fluff
public class FluffSpot
{

    private Vector3 pos;        //coords to place the fluff
    private float yrot;         //y rotation of the fluff
    private Transform fluff;    //the fluff object to instWantiate

    //Constructor
    public FluffSpot(Vector3 Position, float YRotation, Transform Fluff)
    {
        pos = Position;
        yrot = YRotation;
        fluff = Fluff;
    }



    #region Get and Set
    public Vector3 GetPos()
    {
        return pos;
    }

    public float GetYRot()
    {
        return yrot;
    }

    public Transform GetFluff()
    {
        return fluff;
    }


    #endregion

}
