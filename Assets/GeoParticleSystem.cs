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
    public float wanderForce;
    public float particleFieldOfVision;
    public float maxVelocity;
    public float averageVelocity;
    public Transform particleTarget;
    public GeoParticle particlePrefab;
    public TextMeshPro particlesText;
    public string particleSufix = "p_";


    public float separationDistance
    ;
    public float SqrSeparationDistance;
    

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
    [Range(0,1)]
    public  float wanderRadius;

    void Update()
    {




    }
    private void FixedUpdate()
    {
        SqrSeparationDistance= separationDistance * separationDistance;
        //  GameObject[] particles = GameObject.FindGameObjectsWithTag("Particle");
        int particlesCount = particles.Count;
            particlesText.text = "" + particlesCount;
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
                    GeoParticle particle = GameObject.Instantiate(particlePrefab, transform.position+ 25*Random.onUnitSphere, transform.rotation);
                    particle.born(this,particleSufix+(particlesCount+ i) );
                    particles.Add(particle);
                    i++;
                }
            }
        }
    }
}