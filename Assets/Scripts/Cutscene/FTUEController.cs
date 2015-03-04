using UnityEngine;
using System.Collections;

public class FTUEController : MonoBehaviour {

    public string nextScene;

    public void OnGUI() { if (Event.current.type == EventType.keyDown) {
        ChangeLevel();
    }}

    private void ChangeLevel()
    {
        Application.LoadLevel(nextScene);
    }
}
