using UnityEngine;
using System.Collections;

public class StageBuilder : MonoBehaviour {

    private Stage stage;
    [SerializeField]
    private int WIDTH_MUST_BE_ODD;
    [SerializeField]
    private int HEIGHT_MUST_BE_ODD;
    [SerializeField]
    private int NUMBER_ROOM_TRIES;
    [SerializeField]
    private Material wallMaterial;
    [SerializeField]
    private Material floorMaterial;



	// Use this for initialization
	void Start () {
        stage = new Stage(WIDTH_MUST_BE_ODD, HEIGHT_MUST_BE_ODD, floorMaterial, wallMaterial);
        stage.PlaceHalls();
        stage.PlaceRooms(NUMBER_ROOM_TRIES);
        
        stage.fillRooms();
        //stage.removeDeadEnds();
        stage.Create();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
