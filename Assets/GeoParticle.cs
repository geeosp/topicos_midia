using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoParticle : MonoBehaviour
{


    // public static Collider[] collidersArray = new Collider[1];
    List<Collider> neighborhoods;

    public static float separationForce;
    [Range(0, 1)]
    public static float alignForce;
    [Range(0, 1)]
    public static float coesionForce;
    public static float seekForce;
    public static float particleFieldOfVision;
    public static Transform target;
    public static float velocity;
    public static float separationDistance;
    Rigidbody body; SphereCollider sphere;
    // Use this for initialization
    void Start()
    {
        neighborhoods = new List<Collider>();
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



        seekTarget(target, velocity, particleFieldOfVision, out seek_comp);
        separate(neighborhoods,velocity,  separationDistance, out separate_comp);
        align(neighborhoods, velocity, out align_comp);



        desiredVelocity = Vector3.zero
            + seek_comp * seekForce
            + align_comp * alignForce
            + coesion_comp * coesionForce
            + separate_comp * separationForce
            ;


        desiredVelocity = Vector3.ClampMagnitude(desiredVelocity * velocity, velocity);


        force = desiredVelocity- body.velocity ;
        body.AddForce(force);



    }


    void seekTarget(Transform target, float maxVelocity, float distanceToBreak, out Vector3 d)
    {
        d = target.position - transform.position;
        distanceToBreak *= distanceToBreak;
        d =  Vector3.Slerp(Vector3.zero, d.normalized, Mathf.Min(1, d.magnitude / (distanceToBreak)));
        d.Normalize();
    }

    void separate(List<Collider> others, float maxVelocity, float distanceToWork, out Vector3 d)
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



    void align(List<Collider> others, float maxVelocity, out Vector3 d)
    {
        d = Vector3.zero;
        if (others.Count > 0)
        {
            Rigidbody rb;
            foreach (Collider c in others)
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
    /*
    void coesion(List<Collider> others, Vector3 currentVelocity, float force, out Vector3 d)
    {
        d = Vector3.zero;
        if (others.Count > 0)
        {
            Rigidbody rb;
            foreach (Collider c in others)
            {
                rb = c.gameObject.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    d += rb.velocity - currentVelocity;
                }
            }
        }
        d = Vector3.ClampMagnitude(d, force);


    }

        */

    public void born(float life)
    {
        Start();
    }




    public void OnTriggerEnter(Collider c)

    {
        neighborhoods.Add(c);

    }
    public void OnTriggerExit(Collider other)
    {
        neighborhoods.Remove(other);
    }
    /*
    public void OnTriggerStay(Collider c)
    {
        if (!neighborhoods.Contains(c))
        {
            neighborhoods.Add(c);
        }
    }
    public void feeltheOther(Collider c)
    {

        if (c.gameObject != gameObject)
        {
            Rigidbody rigidBody = GetComponent<Rigidbody>();
            rigidBody.AddForce((c.gameObject.transform.position - transform.position).normalized * 2);

        }
    }


    */
    public void OnDrawGizmosSelected()
    {
        Color color = Color.yellow;
        color.a = .5f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, particleFieldOfVision);
    }
}
