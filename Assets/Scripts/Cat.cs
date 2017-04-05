using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {

    public float Speed;
    public int Health;
    public int DamageDealt;
    public float AttackFrequency;
    public LayerMask DogCollisionMask;

    private Vector3 Velocity;

	// Use this for initialization
	void Start () {

        Velocity = new Vector3(Speed, 0.0f, 0.0f);
		
	}
	
	// Update is called once per frame
	void Update () {
        // Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
        transform.position += Velocity;
        CheckCollision();
	}

    private void CheckCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.6f, DogCollisionMask))
        {
            Velocity = new Vector3(0.0f, 0.0f, 0.0f);

            Dog DefenderDog = hit.transform.GetComponent<Dog>();

            if (DefenderDog)
            {
                DefenderDog.TakeDamage(DamageDealt);
            }
        }
    }
}
