using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour {

    //an animator for each line of text that moves separately
    public Animator line1;
    public Animator line2;
    public Animator line3;
    public Animator line4;
    public Animator line5;
    public Animator line6;
    public Animator line7;
    public Animator line8;

    //how many lines have gone by until the screen needs to be cleared
    public int linesUntilSweep1;
    public int linesUntilSweep2;
    public int linesUntilSweep3;

    //The name of the next scene
    public string nextLevel;

    //A list of all the Animator lines
    private ArrayList animList;

    //which line we are at
    private int current;

    //Whether the screen has already been cleared
    private bool sweepIsPast1;
    private bool sweepIsPast2;
    private bool sweepIsPast3;

	void Start () {
        //Animator[] anims = GameObject.FindObjectsOfType<Animator>();
        
        current = 0;
        sweepIsPast1 = false;
        sweepIsPast2 = false;

        //add all lines into list
        animList = new ArrayList();
        animList.Add(line1);
        animList.Add(line2);
        animList.Add(line3);
        animList.Add(line4);
        animList.Add(line5);
        animList.Add(line6);
        animList.Add(line7);
        animList.Add(line8);
	}

	void Update () {
        
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            //time to do first sweep
            if (current == linesUntilSweep1 && !sweepIsPast1)
            {
                sweepIsPast1 = true;
                //for every line on screen
                //add 1 to State which should move it off screen
                for (int i = 0; i < linesUntilSweep1; i++)
                {
                    Animator a = (Animator)animList[i];
                    a.SetInteger("State", a.GetInteger("State") + 1);
                }
                current -= 1;
            }
            //time to do second sweep
            else if (current == linesUntilSweep2 && !sweepIsPast2)
            {
                sweepIsPast2 = true;
                //for every line on screen
                //add 1 to State which should move it off screen
                for (int i = linesUntilSweep1; i < linesUntilSweep2; i++)
                {
                    Animator a = (Animator)animList[i];
                    a.SetInteger("State", a.GetInteger("State") + 1);
                }
                current -= 1;
            }
            else if (current == linesUntilSweep3 && !sweepIsPast3)
            {
                sweepIsPast3 = true;
                //for every line on screen
                //add 1 to State which should move it off screen
                for (int i = linesUntilSweep2; i < linesUntilSweep3; i++)
                {
                    Animator a = (Animator)animList[i];
                    a.SetInteger("State", a.GetInteger("State") + 1);
                }
                current -= 1;
            }
            //still going through lines
            else if (current < animList.Count)
            {
                //add one to current line which should move it onto the screen
                Animator a = (Animator)animList[current];
                a.SetInteger("State", a.GetInteger("State") + 1);
            }
            //we've gone through all the lines
            else
            {
                //open the next scene
                Application.LoadLevel(nextLevel);
            }
            current += 1;
        }
	}
}
