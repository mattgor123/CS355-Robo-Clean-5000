using UnityEngine;
using System.Collections;

public class ComboController : MonoBehaviour {
  [SerializeField] private float combo_delay;

  private int current_combo;
  private float last_combo_time;

  private void Start () {
    current_combo = 0;
    last_combo_time = -combo_delay;
  }

  private void Update () {
    if(Time.time - last_combo_time > combo_delay) {
      current_combo = 0;
      last_combo_time = -combo_delay;
    }
  }

  public int GetCurrentCombo () {
    return current_combo;
  }

  public void IncrementCombo () {
    ++current_combo;
    last_combo_time = Time.time;
    Debug.Log(current_combo.ToString());
  }
}
