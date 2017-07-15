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

    public string particleSufix = "p_";
    public ParticleType particleKind;
    public Transform particleTarget;
    public List<GeoParticleSystem.ParticleType> typesToChase;
    public List<GeoParticleSystem.ParticleType> typesToAvoid;
    
    public long particleQuantity;
    public float particlesPerSecond;
    public float maxVelocity;

    public float particleFieldOfVision;
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
    public List<GeoParticle> particlePrefab;
    public TextMeshPro particlesText;

    public float separationDistance;
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
    { int particlesCount = particles.Count;
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
                    GeoParticle go = particlePrefab[((int)Time.time)% particlePrefab.Count];
                    GeoParticle particle = GameObject.Instantiate(go, transform.position+ Random.onUnitSphere, transform.rotation);
                    particle.born(this,particleSufix+(particlesCount+ i) );
                    particles.Add(particle);
                    i++;
                }
            }
        }



    }
    
    private void FixedUpdate()
    {
        SqrSeparationDistance= separationDistance * separationDistance;
        //  GameObject[] particles = GameObject.FindGameObjectsWithTag("Particle");
       
        
    }
}