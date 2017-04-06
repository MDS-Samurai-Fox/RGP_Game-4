using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TurretController : MonoBehaviour {

    // References

    [Header("Collision detection")]
    public LayerMask collisionCheckLayer;
    public float turretRadius = 40;

    [Header("Projectile Information")]
    public GameObject projectilePrefab;
    public Vector3 projectileSpawnOffset;
    private bool canShoot = true;
    private float bulletSpawnTimer = 0;


    [Header("Model parts to rotate")]
    public Transform weaponHead;
    public Ease weaponRotationType;
    public float weaponRotationDuration = 1;
    public float timeToStartSeeking = 1;
    private bool canSeek = false;

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

            if (bulletSpawnTimer >= weaponRotationDuration) {
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, turretRadius, collisionCheckLayer);

        int i = 0;

        while (i < hitColliders.Length) {
            
            print(hitColliders[i].name);

            // Grab all information from it
            Vector3 catPosition = hitColliders[i].transform.position;
            Vector3 catDirection = transform.position - catPosition;

            RaycastHit hit;

            // If a cat to shoot is found
            if (Physics.Raycast(catPosition, catDirection, out hit, turretRadius)) {
                
                print(hit.transform.name);
                
                StartCoroutine(Fire(hitColliders[i].transform.GetComponent<CatController>(), catPosition, catPosition - transform.position, weaponRotationDuration));

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
        GameObject projectile = Instantiate(projectilePrefab, transform.position + projectileSpawnOffset + (forward.normalized * 2), transform.rotation);
        projectile.transform.SetParent(transform);

        projectile.GetComponent<ProjectileController> ().SetTarget(catPosition);
        
        // Start seeking again after the cat has been found
        canSeek = true;

    }

    private IEnumerator EnableSeeking(float duration) {

        yield return new WaitForSeconds(duration);
        canSeek = true;
        print("Seeking");

    }
    
    private void LockOnTarget(Vector3 position) {
        
        weaponHead.DOLookAt(position, 1).SetEase(weaponRotationType);
        
    }

}