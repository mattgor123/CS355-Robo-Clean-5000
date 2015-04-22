using UnityEngine;
using System.Collections;

public class ComboController : MonoBehaviour {
  [SerializeField] private float combo_delay;

  private int current_combo;
  private float last_combo_time;
  private StatisticsRecorderController stats;

  private void Start () {
    current_combo = 0;
    last_combo_time = -combo_delay;
    stats = GameObject.FindGameObjectWithTag("Player").GetComponent<StatisticsRecorderController>();
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
    stats.setCombo(current_combo);
  }
}
