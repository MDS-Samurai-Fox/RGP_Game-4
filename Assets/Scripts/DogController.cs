using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour {

    private GameManager gm;

    public float health = 100;

    void Awake() {

        gm = FindObjectOfType<GameManager>();

    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (!gm.canUpdate)
            return;

    }

    public void TakeDamage(float damage) {

        health -= damage;

        if (health <= 0) {

            Destroy(this.gameObject);

        }

    }

}


