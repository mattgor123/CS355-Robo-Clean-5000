using UnityEngine;
using System.Collections;

public class CutsceneController : MonoBehaviour {

    public Animator line1;
    public Animator line2;
    public Animator line3;
    public Animator line4;
    public Animator line5;
    public Animator line6;
    public Animator line7;
    public Animator line8;

    public int linesUntilSweep1;
    public int linesUntilSweep2;

    public string nextLevel;

    private ArrayList animList;

    private int current;
    private bool sweepIsPast1;
    private bool sweepIsPast2;

	// Use this for initialization
	void Start () {
        //Animator[] anims = GameObject.FindObjectsOfType<Animator>();
        animList = new ArrayList();
        current = 0;
        sweepIsPast1 = false;
        sweepIsPast2 = false;

        animList.Add(line1);
        animList.Add(line2);
        animList.Add(line3);
        animList.Add(line4);
        animList.Add(line5);
        animList.Add(line6);
        animList.Add(line7);
        animList.Add(line8);

        //line1.SetBool("isHidden", true);
        //line2.SetBool("isHidden", true);
        //howeverText.SetBool("isHidden", true);
	
	}

	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            if (current == linesUntilSweep1 && !sweepIsPast1)
            {
                sweepIsPast1 = true;
                
                for (int i = 0; i < linesUntilSweep1; i++)
                {
                    Animator a = (Animator)animList[i];
                    //Animator a = (Animator)animList[0];
                    //a.SetBool("isHidden", true);
                    //animList.Remove(a);
                    
                    a.SetInteger("State", a.GetInteger("State") + 1);
                }
                current -= 1;
            }
            else if (current == linesUntilSweep2 && !sweepIsPast2)
            {
                sweepIsPast2 = true;
                for (int i = linesUntilSweep1; i < linesUntilSweep2; i++)
                {
                    Animator a = (Animator)animList[i];
                    a.SetInteger("State", a.GetInteger("State") + 1);
                }
                current -= 1;
            }
            else if (current < animList.Count)
            {
                Animator a = (Animator)animList[current];
                //a.SetBool("isHidden", false);
                a.SetInteger("State", a.GetInteger("State") + 1);
                //current += 1;
            }
            else
            {
                Application.LoadLevel(nextLevel);
            }
            
            current += 1;
        }
         
	
	}
}
