using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseForce : MonoBehaviour {

	public float Amplitude = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//moveSquares ();
	}

	void moveSquares() {
		foreach (GameObject square in GameObject.FindGameObjectsWithTag("Floor")) {
			if (square.transform.position.x - transform.position.x < (Amplitude * 2) && square.transform.position.x - transform.position.x > 0) {
				square.transform.position = new Vector3 (square.transform.position.x, Amplitude - Mathf.Abs (square.transform.position.x - (transform.position.x + Amplitude)), 0);
			} else if (square.transform.position.x - transform.position.x < 0 && square.transform.position.x - transform.position.x > (Amplitude * -2)) {
				square.transform.position = new Vector3 (square.transform.position.x, -Amplitude + Mathf.Abs (square.transform.position.x - (transform.position.x - Amplitude)), 0);
			} else {
				square.transform.position = new Vector3 (square.transform.position.x, 0, 0);
			}
		}
	}
}
