using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    public float oxygenLeft;
	public float oxygenMax = 300;

	static public playerController currentPlayer;

	public RectTransform tankLeft;
	public Image fadeImage;

	private AudioSource breathingSound;
	private bool breathing = false;

	public AudioClip bumpClip;

	private float alpha;

	private Rigidbody body;

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
        oxygenLeft = oxygenLeft - Time.deltaTime;

		tankLeft.localScale = new Vector2(1, oxygenLeft/oxygenMax);

		if (Input.GetKeyDown("r")) {
			restart();
		};

		alpha = Mathf.Max(alpha - Time.deltaTime, 0);

		if (oxygenLeft < 30)
		{
			alpha = ((30 - oxygenLeft)/30) * (Mathf.Cos(12 * Mathf.PI * (30 - oxygenLeft)/30));

			if (oxygenLeft <= 0) {
				restart();
			}
		}

		if (alpha > 0) {
			fadeImage.color = new Color(0, 0, 0, Mathf.Min(alpha, 1));
		}

		if (!breathing && alpha <= 0) {
			breathing = true;
			breathingSound.Play();
		}
	}

	private void restart() {
		transform.position = Vector3.zero;
		body.velocity = Vector3.zero;
		transform.rotation = Quaternion.identity;
		oxygenLeft = oxygenMax;

		alpha = 2;

		breathingSound.Pause();
		breathing = false;
	}

	private void OnCollisionEnter(Collision hit)
	{
		print(hit.impulse.magnitude);
		breathingSound.PlayOneShot(bumpClip, hit.impulse.magnitude/2);
	}
}
