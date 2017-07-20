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
        float f = Mathf.Sin(velocity * Time.time), c = Mathf.Cos(velocity * Time.time);
        Vector3 pos = new Vector3(f, 0, 0);
        transform.position = initialPosition+radius * pos;
	}
}
