using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeoParticleSystem : MonoBehaviour
{
    public int particleKind;
    public float particleLife;
    public long particleQuantity;
    public float particlesPerSecond;

    [Range(0, 5)]
    public float separationForce;
    [Range(0, 1)]

    public float coesionForce;
    [Range(0, 1)]
    public float alignForce;
    [Range(0, 1)]
    public float seekForce;
    [Range(0, 1)]
    public float randomForce;
    public float particleFieldOfVision;
    public float particleVelocity;
    public Transform particleTarget;
    public GeoParticle particlePrefab;
    public TextMeshPro particlesText;
    public  float separationDistance;
    [Range(1, 100)]
    public int neighborLimit = 10;
    public List<GeoParticle> particles;


    // Use this for initialization
    void Start()
    {
        lastTimeFired = Time.time;
        particles = new List<GeoParticle>();
    }

    float lastTimeFired;

    void Update()
    {
        GeoParticle.separationForce = separationForce;
        GeoParticle.coesionForce = coesionForce;
        GeoParticle.particleFieldOfVision = particleFieldOfVision;
        GeoParticle.target = particleTarget;
        GeoParticle.maxVelocity = particleVelocity;
        GeoParticle.alignForce = alignForce;
        GeoParticle.seekForce = seekForce;
        GeoParticle.wanderForce = randomForce;
        GeoParticle.separationDistance = separationDistance;
        GeoParticle.neighborLimit = neighborLimit;
    }
    private void FixedUpdate()
    {
      //  GameObject[] particles = GameObject.FindGameObjectsWithTag("Particle");
    
            particlesText.text = "" + particles.Count;
        if (particles.Count < particleQuantity)
        {
            float quantity = (Time.time - lastTimeFired) * particlesPerSecond;
            if (quantity > 1)
            {
                lastTimeFired = Time.time;
                //  print(" particles lenght: " + particles.Length + "quantity" + quantity);
                            int i = 0;
                while (i < Mathf.Min(quantity, particleQuantity - particles.Count))
                {
                    GeoParticle particle = GameObject.Instantiate(particlePrefab, transform.position+ 25*Random.onUnitSphere, transform.rotation);
                    particle.born(particleKind, particleLife);
                    particles.Add(particle);
                    i++;
                }
            }
        }
    }
}