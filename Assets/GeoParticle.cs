using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoParticle : MonoBehaviour
{

    int particleKind
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
    }

    // Update is called once per frame
    Vector3 seek_comp, align_comp, separate_comp, coesion_comp, random_comp;
    Vector3 desiredVelocity;
    Vector3 force;

    public Collider[] collidersarray;
    public GeoParticle[] buddies;

    void FixedUpdate()
    {
        //    sphere.radius = particleFieldOfVision;

        // UpdatePosition();

        collidersarray.Initialize();
        buddies.Initialize();

        int n = Mathf.Min(psystem.neighborLimit, Physics.OverlapSphereNonAlloc(transform.position, psystem.particleFieldOfVision, collidersarray));
        for (int i = n; i < collidersarray.Length; i++)
        {
            collidersarray[i] = null;
        }
        int bud = 0;
        for (int i = 0; i < n; i++)
        {
            _tmp_collider = collidersarray[i].gameObject.GetComponent<GeoParticle>();

            if (_tmp_collider != null &&(string.Compare(_tmp_collider.gameObject.name, gameObject.name)!=0)&&         _tmp_collider.particleKind == psystem.particleKind)
            {
                buddies[bud] = _tmp_collider;
                bud++;
            }
        }




        if (psystem.seekForce > 0) seekTarget(psystem.particleTarget, psystem.separationDistance, out seek_comp);
        if (psystem.separationForce > 0) separate(collidersarray, psystem.SqrSeparationDistance, out separate_comp);
        if (psystem.alignForce > 0) align(buddies, out align_comp);
        if (psystem.coesionForce > 0) coesion(buddies, transform.position, out coesion_comp);
        if (psystem.wanderForce > 0) wander(transform.forward,transform.up,transform.right, psystem.wanderRadius, Time.time , out random_comp);


        desiredVelocity = Vector3.zero
            + seek_comp * psystem.seekForce
         + align_comp * psystem.alignForce
           + coesion_comp * psystem.coesionForce
            + random_comp * psystem.wanderForce
               + separate_comp * psystem.separationForce;
        ;

        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity * psystem.maxVelocity, psystem.maxVelocity);

        force = desiredVelocity - getVelocity();

        addForce(force);
        transform.rotation = Quaternion.LookRotation(getVelocity());
    }



    float _angle;
    Vector3 _b1, _b2;
    void wander(Vector3 forward,Vector3 up,Vector3 right,float radius,  float fase,  out Vector3 d)
    {

        d = forward + radius * (right*Mathf.Sin(fase) + up*Mathf.Cos(fase)); 
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
                if (c != null && ((c.transform.position - transform.position).sqrMagnitude < SqrSeparationDistance))
                {
                    Vector3 dst = transform.position - c.transform.position;
                    d = d.normalized * Mathf.Lerp(1, .5f, Mathf.Min(1, d.sqrMagnitude / (SqrSeparationDistance)));
                    d += dst;
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
            foreach (GeoParticle c in others)
            {
                if (c != null)
                {
                    d += c.getVelocity();
                }
            }
            d.Normalize();
        }
    }

    void coesion(GeoParticle[] others, Vector3 currPosition, out Vector3 d)
    {
        d = Vector3.zero;
        if (others.Length > 0)
        {
        foreach (GeoParticle c in others)
            {
                if (c != null)
                {

                    d += c.transform.position;
                }
            }
           d = (d / others.Length) - currPosition;
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
        color.a = .5f;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, psystem.particleFieldOfVision);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, psystem.separationDistance);

    }






    Vector3 _currvelocity;
    Vector3 _currAcceleration;

    [SerializeField]
    float cur_Velocity;

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
        _currvelocity += _currAcceleration* Time.smoothDeltaTime;
        transform.position += _currvelocity * Time.smoothDeltaTime;
        _currAcceleration = Vector3.zero;

        cur_Velocity = _currvelocity.magnitude;

    }
}
