using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playertest : MonoBehaviour {

    Rigidbody2D rigid;

    void Start() {
        rigid = GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay2D(Collision2D other) {
            rigid.velocity += other.gameObject.GetComponent<Rigidbody2D>().velocity;
    }


    void Update() {
        if (Input.GetKey(KeyCode.RightArrow)) {
            rigid.velocity = new Vector2(5, rigid.velocity.y);
        } else if (Input.GetKey(KeyCode.LeftArrow)) {
            rigid.velocity = new Vector2(-5, rigid.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            rigid.velocity = new Vector2(rigid.velocity.x, 10);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            rigid.velocity = new Vector2(0, -10);
        }

    }

    float velocity;
    void LateUpdate() {
        velocity = rigid.velocity.y;
    }

    void OnCollisionEnter2D(Collision2D other) {
        waveEffect.instance.startForce(Array.IndexOf(waveEffect.instance.segments, other.gameObject), Mathf.Abs(velocity) / 2);
    }
}
