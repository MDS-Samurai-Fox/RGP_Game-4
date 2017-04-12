﻿using System.Collections;
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
    private bool isPrepped = false;
    private NavMeshAgent agent;
    int randomtarget;

    public GameObject[] PrepTargets;
    public Transform finalTarget;

    private void Awake() {

        gm = FindObjectOfType<GameManager> ();

    }

    // Use this for initialization
    void Start() {

        //Velocity *= speed;
        // Velocity = new Vector3(speed, 0.0f, 0.0f);
        animator = GetComponent<Animator> ();
        agent = GetComponent<NavMeshAgent> ();
        PrepTargets = GameObject.FindGameObjectsWithTag("Goal");
        randomtarget = Random.Range(0, PrepTargets.Length);
    }

    // Update is called once per frame
    void FixedUpdate() {

        if (!gm.canUpdate)
            return;

        if ((catType == CatType.Attack) && (!allDogsDead)) {
            FindNearestDog();
            CheckCollision();
            transform.position += Velocity;
        } else //if (catType == CatType.seek)
        {
            SeekTarget();
            transform.position += Velocity;
        }
    }

    private void SeekTarget() {
        if (isPrepped)
        {
            target = FindObjectOfType<Target>();

            if (target)
            {
                float distanceToTarget = Vector3.Magnitude(target.transform.position - gameObject.transform.position);

                if (distanceToTarget < 7.0f)
                {
                    print("REACHED THE TARGET");
                    //    gm.StopGame();
                    gm.HasCatReachedTarget = true;
                }
                agent.destination = target.transform.position;
                animator.SetTrigger("runTrigger");
            }
        }
        else
        {
            agent.destination = PrepTargets[randomtarget].transform.position;
            animator.SetTrigger("runTrigger");

            float distanceToTarget = Vector3.Magnitude(PrepTargets[randomtarget].transform.position - gameObject.transform.position);
            if (distanceToTarget < 7.0f)
            {
                print("REACHED THE TARGET");
                isPrepped = true;
            }
        }

    }

    private void CheckCollision() {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 5.0f, dogCollisionMask)) {

            isAttacking = true;

            Velocity = Vector3.zero;

            DogController DefenderDog = hit.transform.GetComponent<DogController> ();

            if (DefenderDog) {
                attackTimer += Time.deltaTime;

                if (attackTimer >= attackFrequency) {
                    gm.soundManager.PlayCatAttack();
                    attackTimer = 0;
                    DefenderDog.TakeDamage(damageDealt);
                    animator.SetTrigger("attackTrigger");
                    //   print("attack anim trigger");
                    Velocity = Vector3.zero;
                }
            }
        } else {
            isAttacking = false;
        }

    }

    private void FindNearestDog() {
        DogArray = FindObjectsOfType<DogController> ();
        float distance = 10000.0f;
        Vector3 ClosestDogPosition = Vector3.zero;

        if (DogArray.Length > 0) {
            foreach(DogController dog in DogArray) {
                float tempDistance = Vector3.Magnitude(dog.transform.position - gameObject.transform.position);

                if (tempDistance < distance) {
                    distance = tempDistance;
                    ClosestDogPosition = dog.transform.position;
                }
            }

            agent.destination = ClosestDogPosition;

            // print(Vector3.Magnitude(ClosestDogPosition - gameObject.transform.position));

            if (Vector3.Magnitude(ClosestDogPosition - gameObject.transform.position) < 5.0f) {
                isAttacking = true;
            }

            if (!isAttacking) {
                animator.SetTrigger("runTrigger");
            }
        } else {
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

        if (health <= 0)
        {

            Destroy(this.gameObject);

        }

    }

}