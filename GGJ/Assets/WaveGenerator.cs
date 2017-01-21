using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour {
	public GameObject pulse;
	public GameObject antiPulse;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("t")) {
			makeWave ();
		}
	}


	void makeWave() {
		Instantiate (pulse, new Vector3 (-10, 0, 0), Quaternion.identity);
		Instantiate (antiPulse, new Vector3(10,0,0), Quaternion.identity);
	}
}



