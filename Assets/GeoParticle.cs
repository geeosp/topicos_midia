using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoParticle : MonoBehaviour
{


    // public static Collider[] collidersArray = new Collider[1];
    // public List<Collider> neighbors;

    public int particleKind;

    [Range(0, 5)]
    public static float separationForce;
    [Range(0, 1)]
    public static float alignForce;
    [Range(0, 1)]
    public static float coesionForce;
    [Range(0, 1)]
    public static float seekForce;
    [Range(0, 1)]
    public static float wanderForce;
    public static float particleFieldOfVision;
    public static Transform target;
    public static float maxVelocity;
    static float SqrSeparationDistance;
    static float _separationDistance;
    public static float separationDistance
    {
        get
        {
            return _separationDistance;
        }
        set
        {
            SqrSeparationDistance = value * value;
            _separationDistance = value;
        }
    }
    [Range(1, 100)]
    public static int neighborLimit;
 
    //SphereCollider sphere;
    // Use this for initialization
    void Start()
    {
        //  neighbors = new List<Collider>();
    
        //sphere = GetComponent<SphereCollider>();
        collidersarray = new Collider[neighborLimit];
        buddies = new GeoParticle[neighborLimit];
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

        int n = Mathf.Min(neighborLimit, Physics.OverlapSphereNonAlloc(transform.position, particleFieldOfVision, collidersarray));
        for (int i = n; i < collidersarray.Length; i++)
        {
            collidersarray[i] = null;
        }
        int bud = 0;
        for (int i = 0; i < n; i++)
        {
            _tmp_collider = collidersarray[i].gameObject.GetComponent<GeoParticle>();
            if (_tmp_collider != null && _tmp_collider.particleKind == particleKind)
            {
                buddies[bud] = _tmp_collider;
                bud++;
            }
        }
      




        if (seekForce > 0) seekTarget(target, SqrSeparationDistance, out seek_comp);
        if (separationForce > 0) separate(collidersarray, separationDistance, out separate_comp);
        if (alignForce > 0) align(buddies, out align_comp);
        if (coesionForce > 0) coesion(buddies, transform.position, out coesion_comp);
        if (wanderForce > 0) wander(transform.forward, wanderForce, out random_comp);

        desiredVelocity = Vector3.zero
            + seek_comp * seekForce
            + align_comp * alignForce
            + coesion_comp * coesionForce
            + random_comp * wanderForce
            + separate_comp * separationForce;
          ;
        
        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity * maxVelocity, maxVelocity);
      
        force = desiredVelocity - getVelocity();
      
        addForce(force);
        transform.rotation = Quaternion.LookRotation(getVelocity());

    }

    void wander(Vector3 forward, float radius, out Vector3 d)
    {

        d = forward;
        d += Random.insideUnitSphere * radius;
        d.Normalize();
    }
    void seekTarget(Transform target, float SqrdistanceToBreak, out Vector3 d)
    {
        d = target.position - transform.position;
       // distanceToBreak *= distanceToBreak;
     d = Vector3.Slerp(Vector3.zero, d.normalized, Mathf.Min(1, d.magnitude / (SqrdistanceToBreak)));
        d.Normalize();
    }

    void separate(Collider[] others, float distanceToWork, out Vector3 d)
    {
        d = Vector3.zero;

        if (others.Length > 0)
        {
            foreach (Collider c in others)
            {
                if (c != null&&((c.transform.position - transform.position).sqrMagnitude< SqrSeparationDistance))
                {
                    Vector3 dst = transform.position - c.transform.position;
                   // d = Vector3.Slerp( d.normalized, Vector3.zero,Mathf.Min(1, d.magnitude / (SqrSeparationDistance)));
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
              
                    d += c.getVelocity();
              
            }

            d.Normalize();
        }


    }

    void coesion(GeoParticle[]others, Vector3 currPosition, out Vector3 d)
    {
        d = Vector3.zero;
        if (others.Length > 0)
        {

            foreach (GeoParticle c in others)
            {
                d += c.transform.position;
            }


            d = (d / others.Length) - currPosition;

            d.Normalize();
        }



    }



    public void born(int particleKind, float life)
    {
        this.particleKind = particleKind;
        Start();
    }




    GeoParticle _tmp_collider;


    public void OnDrawGizmos()
    {
        Color color = Color.yellow;
        color.a = .5f;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, particleFieldOfVision);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, separationDistance);

    }







    Vector3 _currvelocity;
    Vector3 _currAcceleration;
    
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
        _currvelocity += _currAcceleration * Time.deltaTime;
        transform.position += _currvelocity * Time.deltaTime;
        _currAcceleration = Vector3.zero;
   
        }


    public void Update()
    {
        UpdatePosition();
    }
}
