using UnityEngine;
using System.Collections;

public class TutorialTrigger : MonoBehaviour {

    [SerializeField] private TutorialController tutorial_controller;
    [SerializeField] private string message;

    private bool triggered;

    private void Start() {
        triggered = false;
    }

    public void OnCollisionEnter(Collision collision) {
        if(!triggered) {
            tutorial_controller.SetMessage(message, 3.0f);
            triggered = true;
        }
    }
}
