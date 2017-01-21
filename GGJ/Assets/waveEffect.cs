﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveEffect : MonoBehaviour {

    public GameObject segment;
    public Color segmentColor;
    public float width;
    public float offset;
    GameObject[] segments;
    public int layer;

    public int segIndex;

    void Start() {
        segments = new GameObject[Mathf.CeilToInt(width / segment.transform.localScale.x)];
        drawSegments();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            startForce();
        }
    }

    void drawSegments() {
        Vector2 leftSide = new Vector2(transform.position.x - width / 2, transform.position.y);
        float segmentWidth = segment.transform.localScale.x;

        for (int i = 0; i < width / segmentWidth; i++) {
            GameObject seg = Instantiate(segment, leftSide + new Vector2(segmentWidth * i, 0), transform.rotation) as GameObject;
            seg.transform.parent = this.transform;
            seg.GetComponent<SpriteRenderer>().color = segmentColor;
            seg.GetComponent<SpriteRenderer>().sortingOrder = layer;
            segments[i] = seg;
        }
        
    }

    void startForce() {
        segments[segIndex].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 5);
        StartCoroutine(sendForce(segIndex - 1, 5, -1));
        StartCoroutine(sendForce(segIndex + 1, 5, 1));
    }

    IEnumerator sendForce(int blockIndex, float force, int direction) {

        if (blockIndex < segments.Length && blockIndex >= 0) {
            yield return new WaitForSeconds(0.05f);
            segments[blockIndex].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 5);
            StartCoroutine(sendForce(blockIndex + direction, 5, direction));
        }
    }

    public float height;
    void spaceSegments() {
        for (int i = 0; i < segments.Length; i++) {
            float position =  height * Mathf.Sin((i)/ 2 + Time.time * 1.5f + offset);
            Vector2 wavePosition = new Vector2(segments[i].transform.position.x, transform.position.y + position);
            segments[i].transform.position = (Vector3)wavePosition;
        }
    }
    
    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector2(width, 1));
    }
}
