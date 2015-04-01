using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class HUDController : MonoBehaviour {
  [SerializeField] private float message_display_time;
  [SerializeField] private int max_messages;
  [SerializeField] private Text weapon_text;
  [SerializeField] private Text ammo_text;
  [SerializeField] private Text notification_text;
  [SerializeField] private Text level_text;

  private List<Message> messages = new List<Message>();
  private WeaponBackpackController weapon_backpack_controller;

  private void Start () {
    GameObject player = GameObject.FindWithTag("Player");
    weapon_backpack_controller = player.GetComponent<WeaponBackpackController>();
  }
    
  private void LateUpdate () {
    UpdateWeapon();
    UpdateAmmo();
    UpdateNotification();
  }

  private void UpdateWeapon () {
    weapon_text.text = weapon_backpack_controller.GetWeaponName().ToString();
  }

  private void UpdateAmmo () {
    ammo_text.text = weapon_backpack_controller.GetAmmo().ToString() + " / ∞";
  }

  private void UpdateNotification () {
    var time = Time.time;
    string result = "";
    if(messages.Count > 0) {
      while(time - messages.Last().getCreation() > message_display_time) {
        messages.RemoveAt(messages.Count - 1);
      }
      for(var i = 0; i < messages.Count; ++i) {
        if(i < max_messages) {
          result += "\n" + messages[i].getText();
        }
      }
    }
    notification_text.text = result;
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
