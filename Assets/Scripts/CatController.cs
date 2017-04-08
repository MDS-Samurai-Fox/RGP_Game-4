using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour {

    [Header("Collision")]
    public LayerMask dogCollisionMask;
    public CatType catType;
    public Vector3 Velocity;
    public float speed = 0.1f;
    public float health = 100;
    public float damageDealt = 1;
    public float attackFrequency = 1.0f;

    private DogController[] DogArray;
    private float attackTimer;
    private Target target;


    // Use this for initialization
    void Start() {

        //Velocity *= speed;
        // Velocity = new Vector3(speed, 0.0f, 0.0f);
    }

    // Update is called once per frame
    void FixedUpdate() {
  
        if (catType == CatType.attack)
        {
            FindNearestDog();
            CheckCollision();
        }
        else if (catType == CatType.seek)
        {
            SeekTarget();
        }

        transform.position += Velocity;
        //transform.position += Velocity * speed;
    }

    private void SeekTarget()
    {
        target = FindObjectOfType<Target>();

        if (target)
        {
            Vector3 DesiredVelocity = Vector3.zero;
            DesiredVelocity = target.transform.position - gameObject.transform.position;
            DesiredVelocity = Vector3.Normalize(DesiredVelocity);
            DesiredVelocity *= speed;
            Velocity = DesiredVelocity;
            transform.forward = Vector3.Normalize(DesiredVelocity);
        }
        
    }

    private void CheckCollision() {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.8f, dogCollisionMask)) {

            Velocity = Vector3.zero;
           // print("hit");

            DogController DefenderDog = hit.transform.GetComponent<DogController>();

            if (DefenderDog)
            {
                attackTimer += Time.deltaTime;

                if (attackTimer >= attackFrequency)
                {
                    attackTimer = 0;
                    DefenderDog.TakeDamage(damageDealt);
                    Velocity = Vector3.zero;
                }

            }

        }

    }

    private void FindNearestDog()
    {
        DogArray = FindObjectsOfType<DogController>();
        float distance = 10000.0f;
        Vector3 ClosestDogPosition = Vector3.zero;

        if (DogArray.Length > 0)
        {
            foreach (DogController dog in DogArray)
            {
                float tempDistance = Vector3.Magnitude(dog.transform.position - gameObject.transform.position);

                if (tempDistance < distance)
                {
                    distance = tempDistance;
                    ClosestDogPosition = dog.transform.position;
                }
            }

            Vector3 DesiredVelocity = Vector3.zero;
            DesiredVelocity = ClosestDogPosition - gameObject.transform.position;
            DesiredVelocity = Vector3.Normalize(DesiredVelocity);
            DesiredVelocity *= speed;
            Velocity = DesiredVelocity;

            transform.forward = Vector3.Normalize(DesiredVelocity);
        }

        else
        {
            Velocity = Vector3.zero;
            SeekTarget();
       }
    }

    private bool IsAlive() {

        return (health > 0);

    }

    public void TakeDamage(float damageDealt) {

        health -= damageDealt;

        if (IsAlive()) {

        }

    }

}