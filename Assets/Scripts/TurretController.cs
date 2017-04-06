using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TurretController : MonoBehaviour {

    // References

    [Header("Collision")]
    public LayerMask collisionMask;
    public float turretRadius;

    [Header("Bullet Information")]
    public GameObject bulletPrefab;
    public Vector3 bulletSpawnOffset;
    private GameObject bullet;
    private Transform bulletTransform;
    private bool canShoot = true;
    private float bulletSpawnTimer = 0;

    [Range(1, 2)]
    public float bulletAccuracy = 1.5f;

    [Range(0.1f, 2)]
    public float bulletSpawnDelay = 1;

    [Range(2, 4)]
    public float bulletLifeTime = 2.5f;

    [Range(30, 85)]
    public float bulletSpeed = 70f;

    [Header("Model parts to rotate")]
    public Transform turretGun;
    public Ease turretRotationEaseType;
    public float timeToStartSeeking = 1;
    private bool canSeek = false;

    private void Awake() {
        

    }

    // Use this for initialization
    void Start() {

        StartCoroutine(EnableSeeking(timeToStartSeeking));

    }

    // Update is called once per frame
    void Update() {

        if (!canSeek)
            return;

        if (!canShoot) {

            bulletSpawnTimer += Time.deltaTime;

            if (bulletSpawnTimer >= bulletSpawnDelay) {
                canShoot = true;
                bulletSpawnTimer = 0;
            }

        }

    }

    private void FixedUpdate() {

        if (!canSeek)
            return;

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

            // If a cat to shoot is found
            if (Physics.Raycast(catPosition, catDirection, out hit, turretRadius)) {
                
                StartCoroutine(Fire(hitColliders[i].transform.GetComponent<CatController>(), catPosition, catPosition - transform.position, bulletSpawnDelay));

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
    private IEnumerator Fire(CatController cat, Vector3 position, Vector3 forward, float delay) {
        
        canSeek = false;
        
        Vector3 catPosition = cat.transform.position;
        
        LockOnTarget(catPosition);
        // Look at the cat
        // turretGun.DOLookAt(position, delay).SetEase(turretRotationEaseType);
        
        yield return new WaitForSeconds(delay);
        
        
        
        // Stop shooting
        canShoot = false;
        
        // Create the bullet within the turret
        bullet = Instantiate(bulletPrefab);
        bulletTransform = bullet.transform;
        bulletTransform.position = transform.position + bulletSpawnOffset + (forward.normalized * 2);
        bulletTransform.SetParent(transform);

        bullet.GetComponent<BulletController> ().SetTarget(this, catPosition);
        
        // Start seeking again after the cat has been found
        canSeek = true;

    }

    private IEnumerator EnableSeeking(float duration) {

        yield return new WaitForSeconds(duration);
        canSeek = true;
        print("Seeking");

    }
    
    private void LockOnTarget(Vector3 position) {
        
        turretGun.DOLookAt(position, 1).SetEase(turretRotationEaseType);
        
    }

}