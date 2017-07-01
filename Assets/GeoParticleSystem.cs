using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoParticleSystem : MonoBehaviour
{

    public float particleLife;
    public long particleQuantity;
    public float particlesPerSecond;
    public float separationForce;
    public float coesionForce;
    public float particleFieldOfVision;
    public float particleVelocity;
    public Transform particleTarget;
    public GeoParticle particlePrefab;






    // Use this for initialization
    void Start()
    {
        lastTimeFired = Time.time;
    }

    float lastTimeFired;

    void FixedUpdate()
    {
        GeoParticle.separationForce = separationForce;
        GeoParticle.coesionForce = coesionForce;
        GeoParticle.particleFieldOfVision = particleFieldOfVision;
        GeoParticle.target = particleTarget;
        GeoParticle.velocity = particleVelocity;
        GameObject[] particles = GameObject.FindGameObjectsWithTag("Particle");

        if (particles.Length < particleQuantity)
        {

            float quantity = (Time.time - lastTimeFired) * particlesPerSecond;
            if (quantity > 1)
            {
                lastTimeFired = Time.time;
                //  print(" particles lenght: " + particles.Length + "quantity" + quantity);


                int i = 0;
                while (i < Mathf.Min(quantity, particleQuantity - particles.Length))
                {
                    GeoParticle particle = GameObject.Instantiate(particlePrefab, transform.position, transform.rotation);
                    particle.born(particleLife);
                    i++;
                }
            }
        }
    }
}