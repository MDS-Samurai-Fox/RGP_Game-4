using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TurretController : MonoBehaviour {

    [Header("Collision detection")]
    public LayerMask collisionCheckLayer;
    public float turretRadius = 40;
    public float minimumTargetDistance = 5;

    [Header("Projectile Information")]
    public GameObject projectilePrefab;
    private List<Transform> projectileSpawnLocations = new List<Transform>();

    [Range(0.1f, 2)]
    public float projectileCreationDelay = 1;
    private bool canShoot = true;
    private bool hasLockedTarget = false;
    private float projectileSpawnTimer = 0;

    [Header("Model parts to rotate")]
    public Transform weaponHead;
    public Ease weaponRotationType;
    public float weaponRotationDuration = 1;
    public float timeToBeginSeeking = 1;
    private bool canSeek = false;
    private bool canUpdate = false;

    [SerializeField]
    private GameObject target = null;

    // Use this for initialization
    void Start() {

        FindFirePoints();
        StartCoroutine(EnableSeeking(timeToBeginSeeking));

    }

    // Update is called once per frame
    void Update() {

        if (!canUpdate)
            return;

        if (hasLockedTarget && !canShoot) {

            projectileSpawnTimer += Time.deltaTime;

            if (projectileSpawnTimer >= projectileCreationDelay) {
                canShoot = true;
                projectileSpawnTimer = 0;
            }

        }

    }

    private void FixedUpdate() {

        if (!canUpdate)
            return;

        // Find the nearest enemy
        if (!target) {

            FindTarget();

        }
        else {

            if (!hasLockedTarget) {

                StartCoroutine(LockOnTarget());

            }
            else {

                if (canShoot) {
                    Fire();
                }

                SeekTarget();

            }

        }

    }

    private void OnDrawGizmos() {

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, turretRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, minimumTargetDistance);
        if (target) {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(projectileSpawnLocations[0].position, target.transform.position);
        }

    }

    /// <summary>
    /// Checks for collisions with any objects
    /// </summary>v
    private void FindTarget() {

        // Get all information from the hit colliders
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, turretRadius, collisionCheckLayer);

        int i = 0;

        while (i < hitColliders.Length) {

            GameObject cat = hitColliders[i].gameObject;

            RaycastHit hit;

            // If a cat to shoot is found
            if (Physics.Raycast(transform.position, cat.transform.position - transform.position, out hit, turretRadius)) {

                if (hit.distance > minimumTargetDistance) {

                    if (target) {
                        Debug.Log("Same target: " + cat.name);
                        return;
                    }

                    Debug.Log("Found target: " + cat.name);
                    target = cat;
                    canSeek = false;
                    return;

                }

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
    private void Fire() {

        // Stop shooting
        canShoot = false;

        Vector3 forward = transform.position - target.transform.position;

        int randomChild = Random.Range(0, projectileSpawnLocations.Count);

        GameObject projectile = Instantiate(projectilePrefab, projectileSpawnLocations[randomChild].position, transform.rotation);

        projectile.transform.SetParent(transform);

        projectile.GetComponent<ProjectileController>().SetTarget(target.transform.position);

    }

    private IEnumerator EnableSeeking(float duration) {

        if (weaponHead) {
            weaponHead.DOMove(new Vector3(transform.position.x, 50, transform.position.z), duration).From().SetEase(Ease.OutCirc);
        }
        yield return new WaitForSeconds(duration);
        canSeek = true;
        canUpdate = true;

    }

    private IEnumerator LockOnTarget() {

        if (weaponHead) {
            weaponHead.DOLookAt(target.transform.position - new Vector3(0, 3, 0), weaponRotationDuration).SetEase(weaponRotationType);
            yield return new WaitForSeconds(weaponRotationDuration);
        }

        hasLockedTarget = true;

    }

    private void SeekTarget() {

        if (weaponHead) {
            weaponHead.LookAt(target.transform.position - new Vector3(0, 3, 0));
        }
        
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (distance > turretRadius + 1) {
            ResetSeekingState();
        } else if (distance < minimumTargetDistance + 1) {
            ResetSeekingState();
        }

    }

    private void ResetSeekingState() {
        
        StopAllCoroutines();
        target = null;
        canSeek = true;
        canShoot = true;
        hasLockedTarget = false;
        projectileSpawnTimer = 0;

    }

    private void FindFirePoints() {

        Transform t = transform.GetChild(0).GetChild(0).GetChild(1);
        int length = t.childCount;
        for (int i = 0; i < length; i++) {

            projectileSpawnLocations.Add(t.GetChild(i));

        }

    }

}