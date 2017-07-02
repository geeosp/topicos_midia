using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoParticle : MonoBehaviour
{

   
    // public static Collider[] collidersArray = new Collider[1];
    public List<Collider> neighbors;
    public List<GeoParticle> buddies;
    public int particleKind;

    [Range(0, 1)]
    public static float separationForce;
    [Range(0, 1)]
    public static float alignForce;
    [Range(0, 1)]
    public static float coesionForce;
    [Range(0, 1)]
    public static float seekForce;
    public static float particleFieldOfVision;
    public static Transform target;
    public static float velocity;
    public static float separationDistance;
    public static int neighborLimit;
    Rigidbody body; SphereCollider sphere;
    // Use this for initialization
    void Start()
    {
        neighbors = new List<Collider>();
        body = GetComponent<Rigidbody>(); sphere = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    Vector3 seek_comp, align_comp, separate_comp, coesion_comp;
    Vector3 desiredVelocity;
    Vector3 force;


    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(body.velocity);
    }
    void FixedUpdate()
    {
        sphere.radius = particleFieldOfVision;

        seekTarget(target, particleFieldOfVision, out seek_comp);
        separate(neighbors, separationDistance, out separate_comp);
        align(buddies, out align_comp);
        coesion(buddies, transform.position, out coesion_comp);


        desiredVelocity = Vector3.zero
            + seek_comp * seekForce
            + align_comp * alignForce
            + coesion_comp * coesionForce
            + separate_comp * separationForce
            ;

        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity * velocity, velocity);
        force = desiredVelocity - body.velocity;
        body.AddForce(force);



    }


    void seekTarget(Transform target, float distanceToBreak, out Vector3 d)
    {
        d = target.position - transform.position;
        distanceToBreak *= distanceToBreak;
        d = Vector3.Slerp(Vector3.zero, d.normalized, Mathf.Min(1, d.magnitude / (distanceToBreak)));
        d.Normalize();
    }

    void separate(List<Collider> others, float distanceToWork, out Vector3 d)
    {
        d = Vector3.zero;

        if (others.Count > 0)
        {

            foreach (Collider c in others)
            {
                Vector3 dst = transform.position - c.transform.position;
                dst = Vector3.Slerp(dst, Vector3.zero, Mathf.Min(1, dst.sqrMagnitude / (distanceToWork)));
                d += dst;
            }
            //     d = d / others.Count;
            d.Normalize();
        }

    }


    Rigidbody rb;
    void align(List<GeoParticle> others, out Vector3 d)
    {
        d = Vector3.zero;
        if (others.Count > 0)
        {
            
            foreach (GeoParticle c in others)
            {
                rb = c.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    d += rb.velocity;
                }
            }

            d.Normalize();
        }


    }

    void coesion(List<GeoParticle> others, Vector3 currPosition, out Vector3 d)
    {
        d = Vector3.zero;
        if (others.Count > 0)
        {

            foreach (GeoParticle c in others)
            {
                d += c.transform.position;
            }


            d = (d / others.Count) - currPosition;

            d.Normalize();
        }



    }



    public void born(int particleKind, float life)
    {
        this.particleKind = particleKind;
        Start();
    }




        GeoParticle _tmp_collider;





    public void OnTriggerEnter(Collider other)

    {
        if (neighbors.Count < neighborLimit)
            neighbors.Add(other);
        _tmp_collider = other.gameObject.GetComponent<GeoParticle>();
        if(_tmp_collider!=null&&_tmp_collider.particleKind == particleKind)
        {
            buddies.Add(_tmp_collider);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        neighbors.Remove(other);
        _tmp_collider = other.gameObject.GetComponent<GeoParticle>();
        if (_tmp_collider != null)
        {
            buddies.Remove(_tmp_collider);
        }

    }
   
    public void OnDrawGizmosSelected()
    {
        Color color = Color.yellow;
        color.a = .5f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, particleFieldOfVision);
    }
   
}
