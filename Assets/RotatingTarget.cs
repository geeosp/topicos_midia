using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingTarget : MonoBehaviour {
    public float radius;
    public float velocity;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 pos = transform.position;
        pos = new Vector2(Mathf.Sin(velocity*Time.time), Mathf.Cos(velocity*Time.time));
        transform.position = radius * pos;
	}
}
