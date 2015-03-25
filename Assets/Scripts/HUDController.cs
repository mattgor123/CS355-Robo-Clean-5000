using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class HUDController : MonoBehaviour {
  [SerializeField] private float message_display_time;
  [SerializeField] private int max_messages;
  [SerializeField] private string health_text;
  [SerializeField] private string ammo_text;
  private List<Message> messages;
  private GameObject player;

  private void Start () {
    player = GameObject.FindGameObjectWithTag("Player");
  }
    
  private void LateUpdate () {
    UpdateMessages();
    UpdateHealth();
    UpdateAmmo();
  }

  private void UpdateMessages () {
    var time = Time.time;
    if(messages.Count > 0 || messages.Count > max_messages) {
      while(time - messages.Last().getCreation() > message_display_time) {
        messages.RemoveAt(messages.Count - 1);
      }
    }
  }

  private void UpdateHealth () {
    health_text = player.GetComponent<HealthController>()
                             .GetCurrentHealth().ToString();
  }

  private void UpdateAmmo () {
    ammo_text = player.GetComponent<WeaponBackpackController>()
                           .GetAmmo().ToString();
  }

  public void AddMessage (string text) {
    var message = new Message(text, Time.time);
    messages.Insert(0, message);
  }
}

public class Message
{
    private string text;
    private float created_at;

    public Message(string _text, float _created_at)
    {
        text = _text;
        created_at = _created_at;
    }

    public string getText()
    {
        return this.text;
    }
    public void setText(string text)
    {
        this.text = text;
    }

    public float getCreation()
    {
        return this.created_at;
    }

    public void setCreation(float time)
    {
        this.created_at = time;
    }
}