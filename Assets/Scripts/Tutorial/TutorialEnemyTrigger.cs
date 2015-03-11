using UnityEngine;
using System.Collections;

public class TutorialEnemyTrigger : MonoBehaviour {

    [SerializeField] private TutorialController tutorial_controller;
    [SerializeField] private Transform spawn_point;
    [SerializeField] private GameObject enemy;
    [SerializeField] private string message;

    private bool triggered;

    private void Start()
    {
        triggered = false;
    }

    public void OnCollisionEnter(Collision collision) {
        if (!triggered) {
            tutorial_controller.SetMessage(message, 3.0f);
            var enemy_instance = Instantiate(enemy, spawn_point.position, spawn_point.rotation);
            triggered = true;
        }
    }
}
