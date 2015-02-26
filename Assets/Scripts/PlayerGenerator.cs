using UnityEngine;
using System.Collections;

public class PlayerGenerator : MonoBehaviour {

    public string nextScene;
    public Transform cameraTransform;
    public Transform PlayerTransform;
    public Transform WeaponCanvasTransform;

    

    void Start() {
        //cameraTransform = camera.transform;
        //PlayerTransform = Player.transform;
        //WeaponCanvasTransform = WeaponCanvas.transform;
        spawnPlayer();
       
        //DontDestroyOnLoad(playerInstance);
        //DontDestroyOnLoad(WeaponCanvasTransform);
        //DontDestroyOnLoad(cameraTransform);
        Application.LoadLevel(nextScene);

    }

    private void spawnPlayer()
    {
        Transform playerInstance = Instantiate(PlayerTransform, new Vector3(0f, 0.5f, 0f), Quaternion.identity) as Transform;
        DontDestroyOnLoad(playerInstance);
        Transform canvasInstance = Instantiate(WeaponCanvasTransform) as Transform;
        DontDestroyOnLoad(canvasInstance);
        Transform cameraInstance = Instantiate(cameraTransform, cameraTransform.position, cameraTransform.rotation) as Transform;
        DontDestroyOnLoad(cameraInstance);
        playerInstance.Translate(Vector3.zero);
    }
}
