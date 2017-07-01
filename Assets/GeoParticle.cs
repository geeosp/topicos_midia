using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeoParticle : MonoBehaviour
{


    // public static Collider[] collidersArray = new Collider[1];
    List<Collider> neighborhoods;
    public static float separationForce;
    public static float coesionForce;
    public static float particleFieldOfVision;

    public static Transform target;
    public static float velocity;
    Rigidbody body; SphereCollider sphere;
    // Use this for initialization
    void Start()
    {
        neighborhoods = new List<Collider>();
        body = GetComponent<Rigidbody>(); sphere = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    Vector3 tmp_vel_component;
    Vector3 desiredVelocity;
    Vector3 force;
        void FixedUpdate()
    {

        sphere.radius = particleFieldOfVision;

        desiredVelocity = Vector3.zero;
        seekTarget(target, velocity, particleFieldOfVision, out tmp_vel_component);
        desiredVelocity += tmp_vel_component;
        separate(neighborhoods, separationForce, particleFieldOfVision, out tmp_vel_component);
        desiredVelocity += tmp_vel_component;



      force= desiredVelocity - body.velocity;
        body.AddForce(force);






    }


    void seekTarget(Transform target, float velocity, float distanceToBreak, out Vector3 d)
    {
        d = target.position - transform.position;
        distanceToBreak *= distanceToBreak;
        d = velocity * Vector3.Slerp(Vector3.zero, d.normalized, Mathf.Min(1, d.sqrMagnitude / (distanceToBreak)));

    }

    void separate(List<Collider> others, float separationforce, float distanceToWork, out Vector3 d)
    {
        d = Vector3.zero;
        distanceToWork *= distanceToWork;
        foreach (Collider c in others)
        {
            Vector3 dst = transform.position - c.transform.position;
            dst = separationforce*Vector3.Slerp( dst.normalized, Vector3.zero, Mathf.Min(1, dst.sqrMagnitude / (distanceToWork)));
            d += dst;
        }
        if (others.Count > 0)
            d = d / others.Count;

    }


    public void born(float life)
    {
        Start();
    }




    public void OnTriggerEnter(Collider c)

    {
        neighborhoods.Add(c);

    }
    public void OnTriggerStay(Collider c)
    {
        if (!neighborhoods.Contains(c))
        {
            neighborhoods.Add(c);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        neighborhoods.Remove(other);
    }
    /*
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
