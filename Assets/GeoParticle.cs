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

    // Use this for initialization
    void Start()
    {
        neighborhoods = new List<Collider>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SphereCollider sphere = GetComponent<SphereCollider>();
        sphere.radius = particleFieldOfVision;



     
       


        Rigidbody body = GetComponent<Rigidbody>();
        Vector3 desiredVelocity = new Vector3();
        desiredVelocity += seekTarget(target, velocity, particleFieldOfVision);

        


        Vector3 force = desiredVelocity - body.velocity;
        body.AddForce(force);

     




    }

    Vector3 seekTarget(Transform target, float velocity, float distanceToBreak)
    {
        Vector3 d = target.position - transform.position;




        d = velocity * Vector3.Slerp(Vector3.zero,d.normalized,  Mathf.Min(1, d.sqrMagnitude / (distanceToBreak * distanceToBreak)));



        return d;
    }

    public void born(float life)
    {
        Start();
    }



    /*

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
