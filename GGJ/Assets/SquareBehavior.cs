using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareBehavior : MonoBehaviour {

	private int Amplitude = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float FinalYPos = 0;
		foreach (GameObject pulse in GameObject.FindGameObjectsWithTag("Pulse")) {
			float xPos = transform.position.x;
			float xPulsePos = pulse.transform.position.x;
			if (xPos - xPulsePos < (Amplitude * 2) && xPos - xPulsePos > 0) {
				FinalYPos += Amplitude - Mathf.Abs (xPos - (xPulsePos + Amplitude));
				//transform.position = new Vector3 (xPos, Amplitude - Mathf.Abs (xPos - (xPulsePos + Amplitude)), 0);
			} else if (xPos - xPulsePos < 0 && xPos - xPulsePos > (Amplitude * -2)) {
				FinalYPos += -Amplitude + Mathf.Abs (xPos - (xPulsePos - Amplitude));
				//transform.position = new Vector3 (xPos, -Amplitude + Mathf.Abs (xPos - (xPulsePos - Amplitude)), 0);
			} else {
				//transform.position = new Vector3 (xPos, 0, 0);
			}

		}
		transform.position = new Vector3 (transform.position.x, FinalYPos, 0);
	}
}
