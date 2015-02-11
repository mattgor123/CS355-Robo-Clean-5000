using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed;
	public Text scoreText;
	public GUIText winText;
	private int score;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;

    private float nextFire;

	void Start() {
		score = 0;
		scoreText.text = "Count: " + score.ToString ();
		winText.text = "";
        nextFire = 0.0f;
	}

	void FixedUpdate() {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

		rigidbody.AddForce (movement * speed * Time.deltaTime);
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "PickUp") {
			other.gameObject.SetActive(false);
			score++;
			scoreText.text = "Count: " + score.ToString ();
			if (score >= 12) {
				winText.text = "YOU WIN!";
			}
		}
	}

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        }
    }
}
