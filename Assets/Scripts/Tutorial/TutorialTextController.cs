using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialTextController : MonoBehaviour {

    [SerializeField] private TutorialController tutorial_controller;

    private Text tutorial_text;
	
	void Start () {
        tutorial_text = GetComponent<Text>();
	}
	
	void Update () {
        tutorial_text.text = tutorial_controller.GetMessage();
	}
}
