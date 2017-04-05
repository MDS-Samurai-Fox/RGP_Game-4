using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TurretController : MonoBehaviour {

    public LayerMask catCollisionMask;
    public float turretRadius;

    public GameObject bulletPrefab;
    private GameObject bullet;
    private Transform bulletTransform;

    private void Awake() {

        bullet = Instantiate(bulletPrefab);
        bulletTransform = bullet.transform;

    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        CheckCollision();

    }

    /// <summary>
    /// Checks for collisions with any objects
    /// </summary>
    private void CheckCollision() {

        // Get all information from the hit colliders
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, turretRadius, catCollisionMask);

        int i = 0;

        while (i < hitColliders.Length) {

            Collider col = hitColliders[i];
            Vector3 catPosition = col.transform.position;
            Vector3 catForward = col.transform.forward;

            Debug.DrawRay(catPosition, catForward * 5, Color.red);

            RaycastHit hit;

            if (Physics.Raycast(catPosition, catForward, out hit, turretRadius, catCollisionMask)) {

                Shoot(catPosition, catForward, hit.distance);
                break;

            }

            i++;

        }

    }

    /// <summary>
    /// Creates a bullet from the dog to the position of the foundcat
    /// </summary>
    /// <param name="position"></param>
    /// <param name="forward"></param>
    /// <param name="distance"></param>
    private void Shoot(Vector3 position, Vector3 forward, float distance) {

        Vector3 turretPos = transform.position;
        turretPos.y++;

        bulletTransform.position = turretPos;

        bullet.GetComponent<BulletController>().Shoot(position);

    }

}