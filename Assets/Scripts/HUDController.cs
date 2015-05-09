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
  [SerializeField] private Text combo_text;
  [SerializeField] private ComboController combo_controller;

  private WeaponBackpackController weapon_backpack_controller;
  //private NotificationLog notification_log;
  private LogScript notification_log;

  PlayerController playerC;

  private void Start () {
    GameObject player = GameObject.FindWithTag("Player");
    playerC = player.GetComponent<PlayerController>();
    weapon_backpack_controller = player.GetComponent<WeaponBackpackController>();
    GameObject log = GameObject.FindWithTag("Log");
    //notification_log = log.GetComponent<NotificationLog>();
    notification_log = log.GetComponent<LogScript>();
  }
    
    private void LateUpdate () {
        UpdateWeapon();
        UpdateAmmo();
        UpdateNotification();
        UpdateCombo();
        string floor;
        if (playerC.getCurrentFloor() == -1)
        {
            floor = "Training";
        }
        else
        {
            floor = "B" + playerC.getCurrentFloor();
        }

        SetLevelText(floor);
     }

  private void UpdateWeapon () {
    weapon_text.text = weapon_backpack_controller.GetWeaponName().ToString();
  }

  private void UpdateAmmo () {
    ammo_text.text = weapon_backpack_controller.GetAmmo().ToString() + " / ∞";
  }

  private void UpdateNotification () {
    //notification_text.text = "" + notification_log.getCurrentNotification();
    notification_text.text = "" + notification_log.getNotifications();
  }

  private void UpdateCombo () {
    combo_text.text = "X" + combo_controller.GetCurrentCombo().ToString();
  }

  public void SetLevelText (string new_text) {
    level_text.text = new_text;
  }
}
