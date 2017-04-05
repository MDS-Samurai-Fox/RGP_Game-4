using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TurretController : MonoBehaviour {

    [Header("Collision")]
    public LayerMask collisionMask;
    public float turretRadius;

    [Header("Bullet Information")]
    public GameObject bulletPrefab;
    public Vector3 bulletSpawnOffset;

    [Range(1, 2)]
    public float bulletAccuracy = 1.5f;

    [Range(0.5f, 2)]
    public float bulletSpawnDelay = 1;

    [Range(2, 4)]
    public float bulletLifeTime = 2.5f;

    [Range(55, 85)]
    public float bulletSpeed = 70f;

    private GameObject bullet;
    private Transform bulletTransform;

    private bool canShoot = true;
    private float bulletSpawnTimer = 0;

    private void Awake() {



    }

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (!canShoot) {

            bulletSpawnTimer += Time.deltaTime;

            if (bulletSpawnTimer >= bulletSpawnDelay) {
                canShoot = true;
                bulletSpawnTimer = 0;
            }

        }

    }

    private void FixedUpdate() {

        if (canShoot) {

            FindNearestEnemy();

        }

    }

    private void OnDrawGizmos() {

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, turretRadius);

    }

    /// <summary>
    /// Checks for collisions with any objects
    /// </summary>
    private void FindNearestEnemy() {

        // Get all information from the hit colliders
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, turretRadius, collisionMask);

        int i = 0;

        while (i < hitColliders.Length) {

            // Grab all information from it
            Vector3 catPosition = hitColliders[i].transform.position;
            Vector3 catDirection = transform.position - catPosition;

            RaycastHit hit;

            if (Physics.Raycast(catPosition, catDirection, out hit, turretRadius)) {

                Debug.DrawRay(catPosition, catDirection, Color.green);
                Vector3 catVelocity = hitColliders[i].transform.GetComponent<Cat>().Velocity;
                Fire(catPosition + (catVelocity * bulletAccuracy), catDirection);
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
    private void Fire(Vector3 position, Vector3 forward) {

        canShoot = false;

        bullet = Instantiate(bulletPrefab);

        bulletTransform = bullet.transform;

        Vector3 turretPos = transform.position;
        turretPos.y++;

        bulletTransform.position = turretPos + bulletSpawnOffset - (forward.normalized * 2);

        bulletTransform.SetParent(transform);

        bullet.GetComponent<BulletController>().SetTarget(this, position, bulletSpawnOffset);

    }

}