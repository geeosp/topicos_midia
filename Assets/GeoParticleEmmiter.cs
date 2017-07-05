using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoParticleEmmiter : MonoBehaviour
{

    public float particleLife;
    public long particleQuantity;
    public float particlesPerSecond;

    public int particles =0;
    public GameObject particlePrefab;
    




    // Use this for initialization
    void Start()
    {
        lastTimeFired = Time.time;
    }

    float lastTimeFired;

    void FixedUpdate()
    {
       
            float quantity = (Time.time - lastTimeFired) * particlesPerSecond;
            if (quantity > 1)
            {
                lastTimeFired = Time.time;
                //  print(" particles lenght: " + particles.Length + "quantity" + quantity);


                int i = 0;
                while (i < quantity)
                {
                    GameObject particle = GameObject.Instantiate(particlePrefab, transform.position, transform.rotation);
               //     particle.born(particleLife);
                    i++;
                particles++;
                }
            }
       
    }



}