using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerController : MonoBehaviour
{
    public float oxygenLeft;
	public float oxygenMax = 300;

	static public playerController currentPlayer;

	public RectTransform tankLeft;
	public Image fadeImage;
	public Text spashText;

	private AudioSource breathingSound;
	private bool breathing = false;

	public AudioClip bumpClip;

	private float alpha = 1;
	public float alphaChange = 0;

	private Rigidbody body;

	private bool won = false;

	// Start is called before the first frame update
    void Start()
    {
		currentPlayer = this;

		body = GetComponent<Rigidbody>();
		breathingSound = GetComponent<AudioSource>();
		
		breathingSound.loop = true;
	}

    // Update is called once per frame
    void Update()
    {
		if (!won) {
			oxygenLeft = oxygenLeft - Time.deltaTime;
		}

		tankLeft.localScale = new Vector3(1, oxygenLeft/oxygenMax, 1);

		if (Input.GetKeyDown("r")) {
			restart();
		};

		alpha = alpha + alphaChange*Time.deltaTime;

		if (oxygenLeft < 30)
		{
			alpha = ((30 - oxygenLeft)/30) * (Mathf.Cos(12 * Mathf.PI * (30 - oxygenLeft)/30));

			if (oxygenLeft <= 0) {
				restart();
			}

			spashText.text = "";
		}

		//if (alpha >= 0) {
			fadeImage.color = new Color(0, 0, 0, Mathf.Max(Mathf.Min(alpha, 1), 0));
			spashText.color = new Color(0.9f, 0.9f, 0.9f, Mathf.Max(Mathf.Min(alpha, 1), 0));
		//}

		if (!breathing && alpha <= 0) {
			breathing = true;
			breathingSound.Play();
		}

		if (Input.GetMouseButtonDown(0) && !won) {
			alphaChange = -1;
		}

		if (SceneManager.sceneCount == 6 && !won) {
			winGame();
		}
	}

	private void restart() {
		if (!won) {
			transform.position = Vector3.zero;
			body.velocity = Vector3.zero;
			transform.rotation = Quaternion.identity;
			oxygenLeft = oxygenMax;

			alpha = 2;

			breathingSound.Pause();
			breathing = false;

			spashText.text = "";
		}
	}

	private void OnCollisionEnter(Collision hit)
	{
		//print(hit.impulse.magnitude);
		breathingSound.PlayOneShot(bumpClip, hit.impulse.magnitude/2);
	}

	private void winGame() {
		won = true;
		alphaChange = 1;
		alpha = -2;

		spashText.text = "You've made it to the escape pods.\n\nWithin days you'll be back down earthside, and swapping tales with your old station mates of how you managed to escape.\n\n\n" +
		"It only took you " + Time.time + " seconds to escape\n\n\n" +
		"Made for 7 day FPS 2020.";
	}
}
