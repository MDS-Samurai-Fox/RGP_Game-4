using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour {

    public float health = 100;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void TakeDamage(float damage) {

        health -= damage;

        if (health <= 0) {

            Destroy(this.gameObject);

        }

    }

}


