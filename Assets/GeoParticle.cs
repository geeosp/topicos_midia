using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GeoParticle : MonoBehaviour
{

    GeoParticleSystem.ParticleType particleKind
    {
        get
        {
            return psystem.particleKind;
        }
    }




    public GeoParticleSystem psystem;


    // Use this for initialization
    void Start()
    {

        collidersarray = new Collider[psystem.neighborLimit];
        buddies = new GeoParticle[psystem.neighborLimit];
        _currvelocity = Vector3.zero;
        _currAcceleration = Vector3.zero;

    }

    // Update is called once per frame
    Vector3 seek_comp, align_comp, separate_comp, coesion_comp, wander_comp;
    [SerializeField]
    Vector3 desiredVelocity;
    [SerializeField]
    Vector3 force;
    [SerializeField]
    Vector3 _currvelocity;
    [SerializeField]
    Vector3 _currAcceleration;

   


    public Collider[] collidersarray;
    public GeoParticle[] buddies;

    void FixedUpdate()
    {
        _currAcceleration = Vector3.zero;
        Array.Clear(collidersarray, 0, collidersarray.Length);
        Array.Clear(buddies, 0, buddies.Length); 



        GetComponent<Collider>().enabled = false;
        int n = Mathf.Min(psystem.neighborLimit, Physics.OverlapSphereNonAlloc(transform.position, psystem.particleFieldOfVision, collidersarray));

        GetComponent<Collider>().enabled = true;

        for (int i = 0, bud = 0; i < n; i++)
        {
            _tmp_collider = collidersarray[i].gameObject.GetComponent<GeoParticle>();
            if (_tmp_collider != null && _tmp_collider.particleKind == psystem.particleKind)
            {
                buddies[bud] = _tmp_collider;
                bud++;
            }
        }

        if (psystem.seekForce > 0) seekTarget(psystem.particleTarget, psystem.separationDistance, out seek_comp);
        if (psystem.separationForce > 0) separate(collidersarray, psystem.SqrSeparationDistance, out separate_comp);
        if (psystem.alignForce > 0) align(buddies, out align_comp);
        if (psystem.coesionForce > 0) coesion(buddies, transform.position, out coesion_comp);
        if (psystem.wanderForce > 0) wander(transform.forward, transform.up, transform.right, psystem.wanderRadius, Time.time, out wander_comp);


        desiredVelocity = Vector3.zero
            + seek_comp * psystem.seekForce
            + align_comp * psystem.alignForce
            + coesion_comp * psystem.coesionForce
            + wander_comp * psystem.wanderForce
            + separate_comp * psystem.separationForce;
        ;


        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity * psystem.maxVelocity, psystem.maxVelocity);

        force = desiredVelocity - getVelocity();
        addForce(force);
        transform.rotation = Quaternion.LookRotation(getVelocity());
    }




    void wander(Vector3 forward, Vector3 up, Vector3 right, float radius, float fase, out Vector3 d)
    {

        d = forward + radius * (right * Mathf.Sin(fase));
        d.Normalize();
    }

    void seekTarget(Transform target, float SqrdistanceToBreak, out Vector3 d)
    {
        d = target.position - transform.position;
        d.Normalize();
    }

    void separate(Collider[] others, float SqrSeparationDistance, out Vector3 d)
    {
        d = Vector3.zero;

        if (others.Length > 0)
        {
            foreach (Collider c in others)
            {
                if (c != null)
                {
                    Vector3 dst = transform.position - c.transform.position;
                    if (dst.sqrMagnitude > .01f && dst.sqrMagnitude < SqrSeparationDistance)
                    {

                        // d = d.normalized * Mathf.Lerp(1, .5f, Mathf.Min(1, d.sqrMagnitude / (SqrSeparationDistance)));
                        d += dst;
                    }
                }
            }
            //     d = d / others.Count;
            d.Normalize();
        }

    }


    void align(GeoParticle[] others, out Vector3 d)
    {
        d = Vector3.zero;
        if (others.Length > 0)
        {
            int i = 0;
            foreach (GeoParticle c in others)
            {
                if (c != null)
                {
                    i++;
                    d += c.getVelocity().normalized;
                }
            }
            if (i > 0) d = d / i;
        }
    }

    void coesion(GeoParticle[] others, Vector3 currPosition, out Vector3 d)
    {
        d = Vector3.zero;
        int i = 0;

        foreach (GeoParticle c in others)
        {
            if (c != null)
            {
                i++;
                d += c.transform.position;
            }
        }
        if (i > 0)
        {
            d = (d / i) - currPosition;
            d.Normalize();
        }



    }



    public void born(GeoParticleSystem psystem, string name)
    {
        this.psystem = psystem;
        gameObject.name = name;
        Start();
    }




    GeoParticle _tmp_collider;


    public void OnDrawGizmos()
    {
        Color color = Color.yellow;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, psystem.particleFieldOfVision);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, psystem.separationDistance);

    }



    public void addForce(Vector3 force)
    {

        _currAcceleration += force;

    }
    public Vector3 getVelocity()
    {
        // return body.velocity;
        return _currvelocity;
    }
    public void UpdatePosition()
    {

    }


    public void Update()

    {
        _currvelocity += _currAcceleration * Time.deltaTime;
        transform.position += _currvelocity * Time.deltaTime;
        _currAcceleration = Vector3.zero;

    }
}
