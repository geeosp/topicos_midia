using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GeoParticleSystem : MonoBehaviour
{
    public enum ParticleType
    {
        BIRD, FALCON, FISH, SHARK
    }
    public bool is2D;
    public string particleSufix = "p_";
    public List<GeoParticle> particlePrefab;
    public ParticleType particleKind;
    public Transform particleTarget;
    public List<GeoParticleSystem.ParticleType> typesToChase;
    public List<GeoParticleSystem.ParticleType> typesToAvoid;

    public long particleQuantity;
    public float particlesPerSecond;
    public float mainVelocity;


    [Range(1, 100)]
    public int neighborLimit = 10;
    [Range(0, 5)]
    public float separationForce;
    [Range(0, 1)]
    public float coesionForce;
    [Range(0, 1)]
    public float alignForce;
    [Range(0, 1)]
    public float seekForce;
    [Range(0, 1)]
    public float wanderForce;
    [Range(1, 20)]
    public float chaseBoostVelocity;


    public float particleFieldOfVision;
    public float SqrParticleFieldOfVision;
    public float separationDistance;
    public float SqrSeparationDistance;

    public List<GeoParticle> particles;


    // Use this for initialization
    void Start()
    {
        lastTimeFired = Time.time;
        particles = new List<GeoParticle>();
    }

    float lastTimeFired;
    [Range(0, 1)]
    public float wanderRadius;

    


    void Update()
    {
        int particlesCount = particles.Count;
        if (particlesCount < particleQuantity)
        {
            float quantity = (Time.time - lastTimeFired) * particlesPerSecond;
            if (quantity > 1)
            {
                lastTimeFired = Time.time;
                //  print(" particles lenght: " + particles.Length + "quantity" + quantity);
                int i = 0;
                while (i < Mathf.Min(quantity, particleQuantity - particlesCount))
                {
                    GeoParticle go = particlePrefab[((int)Time.time) % particlePrefab.Count];
                    GeoParticle particle = GameObject.Instantiate(go, Random.insideUnitCircle, transform.rotation);
                    particle.born(this, particleSufix + (particlesCount + i));
                    particles.Add(particle);
                    i++;
                }
            }
        }



    }

    private void FixedUpdate()
    {
        SqrSeparationDistance = separationDistance * separationDistance;
        SqrParticleFieldOfVision = particleFieldOfVision * particleFieldOfVision;


    }
}