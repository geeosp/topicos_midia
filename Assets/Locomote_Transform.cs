using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locomote_Transform : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
   public  Vector3 velocity;
    public Vector3 force;
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + velocity * Time.deltaTime;
	}
    private void FixedUpdate()
    {
        velocity += force * Time.deltaTime;
        
    }
}
