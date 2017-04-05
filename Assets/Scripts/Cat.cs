using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {

    [Header("Collision")]
    public LayerMask dogCollisionMask;
    public Vector3 Velocity;
    public float speed = 0.1f;
    public float health = 100;
    public float damageDealt = 1;
    public float dttackFrequency = 1;


    // Use this for initialization
    void Start() {

        //Velocity *= speed;

    }

    // Update is called once per frame
    void FixedUpdate() {

        transform.position += Velocity * speed;
        CheckCollision();

    }

    private void CheckCollision() {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.6f, dogCollisionMask)) {

            Velocity = new Vector3(0.0f, 0.0f, 0.0f);

            Dog DefenderDog = hit.transform.GetComponent<Dog>();

            if (DefenderDog) {

                DefenderDog.TakeDamage(damageDealt);

            }

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