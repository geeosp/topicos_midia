using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoParticleSystem : MonoBehaviour
{

    public float particleLife;
    public long particleQuantity;
    public float particlesPerSecond;

    [Range(0, 1)]
    public float separationForce;
    [Range(0, 1)]

    public float coesionForce;
    [Range(0, 1)]
    public float alignForce;
    [Range(0, 1)]
    public  float seekForce;
    public float particleFieldOfVision;
    public float particleVelocity;
    public Transform particleTarget;
    public GeoParticle particlePrefab;
    public TextMesh particlesText;
    public  float separationDistance;




    // Use this for initialization
    void Start()
    {
        lastTimeFired = Time.time;
    }

    float lastTimeFired;

    void Update()
    {
        GeoParticle.separationForce = separationForce;
        GeoParticle.coesionForce = coesionForce;
        GeoParticle.particleFieldOfVision = particleFieldOfVision;
        GeoParticle.target = particleTarget;
        GeoParticle.velocity = particleVelocity;
        GeoParticle.alignForce = alignForce;
        GeoParticle.seekForce = seekForce;
        GeoParticle.separationDistance = separationDistance;
    }
    private void FixedUpdate()
    {
        GameObject[] particles = GameObject.FindGameObjectsWithTag("Particle");
    
            particlesText.text = "" + particles.Length;
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
                    GeoParticle particle = GameObject.Instantiate(particlePrefab, transform.position+ (new Vector3(Random.value, Random.value)), transform.rotation);
                    particle.born(particleLife);
                    i++;
                }
            }
        }
    }
}