using UnityEngine;
using System.Collections;
using System.Collections.Generic;

private class Message {
  public string text;
  public float created_at;

  public Message (string _text, _created_at) {
    text = _text;
    created_at = _created_at;
  }
}

public class HUDController : MonoBehaviour {
  [SerializeField] private float message_display_time;
  [SerializeField] private int max_messages;
  [SerializeField] private Text health_text;
  [SerializeField] private Text ammo_text;

  private List<Message> messages;
  private GameObject player;

  private void Start () {
    player = GameObject.findWithTag("Player");
  }

  private void LateUpdate () {
    UpdateMessages();
    UpdateHealth();
    UpdateAmmo();
  }

  private void UpdateMessages () {
    var time = Time.time;
    if(messages.Count > 0 || messages.Count > max_messages) {
      while(time - messages.Last().created_at > message_display_time) {
        messages.RemoveAt(messages.Count - 1);
      }
    }
  }

  private void UpdateHealth () {
    health_text.text = player.GetComponent<HealthController>()
                             .GetCurrentHealth().ToString();
  }

  private void UpdateAmmo () {
    ammo_text.text = player.GetComponent<WeaponBackpackController>()
                           .GetAmmo().ToString();
  }

  public AddMessage (string text) {
    var message = new Message(text, Time.time);
    messages.Insert(0, message);
  }
}
