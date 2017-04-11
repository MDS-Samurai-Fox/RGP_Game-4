using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour {

    private GameManager gm;

    public float health = 100;

    void Awake() {

        gm = FindObjectOfType<GameManager>();

    }

    public void TakeDamage(float damage) {

        health -= damage;

        if (health <= 0) {

            Destroy(this.gameObject);

        }

    }

}


