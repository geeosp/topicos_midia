﻿using System.Collections;
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

    // Update is called once per frame
    Vector3 seek_comp, align_comp, separate_comp, coesion_comp, wander_comp, avoid_comp, chase_comp;
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
    public GeoParticle[] toAvoid;
    public GeoParticle toChase;

    // Use this for initialization
    void Start()
    {

        collidersarray = new Collider[psystem.neighborLimit];
        buddies = new GeoParticle[psystem.neighborLimit];
        toAvoid = new GeoParticle[psystem.neighborLimit];
        _currvelocity = Vector3.zero;
        _currAcceleration = Vector3.zero;

    }

    void FixedUpdate()
    {
        _currAcceleration = Vector3.zero;
        Array.Clear(collidersarray, 0, collidersarray.Length);
        Array.Clear(buddies, 0, buddies.Length);
        Array.Clear(toAvoid, 0, toAvoid.Length);


        GetComponent<Collider>().enabled = false;
        int n = Mathf.Min(psystem.neighborLimit, Physics.OverlapSphereNonAlloc(transform.position, psystem.particleFieldOfVision, collidersarray));

        GetComponent<Collider>().enabled = true;

        for (int i = 0, bud = 0, avoiding=0; i < n; i++)
        {
            _tmp_geoparticle = collidersarray[i].gameObject.GetComponent<GeoParticle>();
            if (_tmp_geoparticle != null)
            {
                if (_tmp_geoparticle.particleKind == psystem.particleKind)
                {
                    buddies[bud] = _tmp_geoparticle;
                    bud++;

                }
                else if (psystem.typesToAvoid.Contains(_tmp_geoparticle.particleKind))
                {
                    toAvoid[avoiding] = _tmp_geoparticle;

                }
                else if (psystem.typesToChase.Contains(_tmp_geoparticle.particleKind))
                {
                    
                }
            }


        }
        if (psystem.seekForce > 0) seekTarget(psystem.particleTarget, psystem.separationDistance, out seek_comp);
        if (psystem.separationForce > 0) separate(buddies, psystem.SqrSeparationDistance, out separate_comp);
        if (psystem.alignForce > 0) align(buddies, out align_comp);
        if (psystem.coesionForce > 0) coesion(buddies, transform.position, out coesion_comp);
        if (psystem.wanderForce > 0) wander(transform, psystem.wanderRadius, Time.time, out wander_comp);


        desiredVelocity = Vector3.zero
            + seek_comp * psystem.seekForce
            + align_comp * psystem.alignForce
            + coesion_comp * psystem.coesionForce
            + wander_comp * psystem.wanderForce
            + separate_comp * psystem.separationForce;
        ;

        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity * psystem.mainVelocity, psystem.mainVelocity);


        
        separate(toAvoid, psystem.particleFieldOfVision*psystem.particleFieldOfVision, out avoid_comp);
       
        desiredVelocity = Vector3.ClampMagnitude((desiredVelocity+ psystem.mainVelocity* psystem.chaseBoostVelocity* avoid_comp) * psystem.mainVelocity, psystem.chaseBoostVelocity* psystem.mainVelocity);








        force = desiredVelocity - getVelocity();
        //addForce2D(force);
        addForce(force);
        transform.rotation = Quaternion.LookRotation(getVelocity());
    }

   


    void wander(Transform t,float radius, float fase, out Vector3 d)
    {
        
        d = t.forward + radius * (t.right * Mathf.Sin(fase));
        d.Normalize();
    }

    void seekTarget(Transform target, float SqrdistanceToBreak, out Vector3 d)
    {
        d = target.position - transform.position;
        d.Normalize();
    }

    void separate(GeoParticle[] others, float SqrSeparationDistance, out Vector3 d)
    {
        d = Vector3.zero;

        if (others.Length > 0)
        {
            foreach (GeoParticle c in others)
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




    GeoParticle _tmp_geoparticle;


    public void OnDrawGizmos()
    {
        Color color = Color.yellow;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, psystem.particleFieldOfVision);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, psystem.separationDistance);

    }



    public void addForce2D(Vector2 force)
    {




        _currAcceleration += new Vector3(force.x, force.y);




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
