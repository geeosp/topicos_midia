using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTarget : MonoBehaviour {
    public float radius;
    public float velocity;
    public Vector3 initialPosition;
	// Use this for initialization
	void Start () {
        initialPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = Vector3.zero;
        pos = new Vector2(Mathf.Sin(velocity*Time.time),0);
        transform.position = initialPosition+radius * pos;
	}
}
