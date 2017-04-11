using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatController : MonoBehaviour {

    private GameManager gm;

    [Header("Collision")]
    public LayerMask dogCollisionMask;
    public CatType catType;
    public Vector3 Velocity;
    public float speed = 0.1f;
    public float health = 100;
    public float damageDealt = 1;
    public float attackFrequency = 1.0f;
    public float distanceToTarget = 1000.0f;

    private Animator animator;
    private DogController[] DogArray;
    private float attackTimer;
    private Target target;
    private bool isAttacking = false;
    private bool allDogsDead = false;
    private NavMeshAgent agent;

    private void Awake() {

        gm = FindObjectOfType<GameManager>();

    }

    // Use this for initialization
    void Start() {

        //Velocity *= speed;
        // Velocity = new Vector3(speed, 0.0f, 0.0f);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (!gm.canUpdate)
            return;

        if ((catType == CatType.Attack) && (!allDogsDead))
        {
            FindNearestDog();
            CheckCollision();
            transform.position += Velocity;
        }
        else //if (catType == CatType.seek)
        {
            SeekTarget();
            transform.position += Velocity;
        }

        //transform.position += Velocity * speed;
    }

    private void SeekTarget()
    {
        target = FindObjectOfType<Target>();

        if (target)
        {
            Vector3 DesiredVelocity = Vector3.zero;
            DesiredVelocity = target.transform.position - gameObject.transform.position;

            distanceToTarget = Vector3.Magnitude(DesiredVelocity);

            if(distanceToTarget < 3.0f)
            {
                gm.HasCatReachedTarget = true;
            }

            DesiredVelocity = Vector3.Normalize(DesiredVelocity);
            Velocity = DesiredVelocity * speed;
            transform.forward = Vector3.Normalize(DesiredVelocity);
            animator.SetTrigger("runTrigger");
        }
        
    }

    private void CheckCollision() {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.8f, dogCollisionMask)) {

            isAttacking = true;

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
                    animator.SetTrigger("attackTrigger");
                    Velocity = Vector3.zero;
                }
            }
        }

        else
        {
            isAttacking = false;
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
            Velocity = DesiredVelocity * speed;

            transform.forward = Vector3.Normalize(DesiredVelocity);

           // print(Vector3.Magnitude(ClosestDogPosition - gameObject.transform.position));


            if (Vector3.Magnitude(ClosestDogPosition - gameObject.transform.position) < 2.0f)
            {
                isAttacking = true;
            }

            if (!isAttacking)
            {
                animator.SetTrigger("runTrigger");
            }
        }

        else
        {
            allDogsDead = true;
         //   agent.velocity = Vector3.zero;
          //  agent.Stop();
            // Velocity = Vector3.zero;
            // agent.enabled = false;
             SeekTarget();
             //animator.SetTrigger("runTrigger");
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