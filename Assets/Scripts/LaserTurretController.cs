using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class LaserTurretController : MonoBehaviour {

    private GameManager gm;

    [Header("Collision detection")]
    public LayerMask collisionCheckLayer;
    public float turretRadius = 40;
    public float minimumTargetDistance = 5;

    [Header("Projectile Information")]
    public LineRenderer laserRenderer;
    private List<Transform> laserSpawnLocations = new List<Transform>();

    [Range(0.1f, 0.5f)]
    public float maxlaserLifetime = 0.1f;
    [Range(0.1f, 0.5f)]
    public float laserShootingDelay = 0.5f;

    private bool isShooting = false;
    private bool hasLockedTarget = false;
    private float laserLifetime = 0;

    [Header("Model parts to rotate")]
    public Transform weaponHead;
    public Ease weaponRotationType;
    public float weaponRotationDuration = 1;
    public float timeToBeginSeeking = 1;
    private bool canSeek = false;
    private bool canUpdate = false;

    [SerializeField]
    private GameObject target = null;

    private void Awake() {

        gm = FindObjectOfType<GameManager>();

    }

    // Use this for initialization
    void Start() {

        FindFirePoints();
        StartCoroutine(EnableSeeking(timeToBeginSeeking));

    }

    // Update is called once per frame
    void Update() {

        if (!gm.canUpdate)
            return;

        if (hasLockedTarget && isShooting) {

            laserLifetime += Time.deltaTime;

            if (laserLifetime >= maxlaserLifetime) {

                laserRenderer.gameObject.SetActive(false);
                isShooting = false;
                laserLifetime = 0;

            }

        }
        else if (hasLockedTarget && !isShooting) {

            laserLifetime += Time.deltaTime;

            if (laserLifetime >= laserShootingDelay) {

                laserRenderer.gameObject.SetActive(true);
                isShooting = true;
                laserLifetime = 0;

            }

        }

    }

    private void FixedUpdate() {

        if (!gm.canUpdate)
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

                if (isShooting) {
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
            Gizmos.DrawLine(laserSpawnLocations[0].position, target.transform.position);
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

                    if (target)
                        return;

                    print("Found target: " + cat.name);
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

        laserRenderer.gameObject.SetActive(true);
        //isShooting = false;

        Vector3 forward = transform.position - target.transform.position;

        int randomChild = Random.Range(0, laserSpawnLocations.Count);

        laserRenderer.SetPosition(0, laserSpawnLocations[randomChild].position);
        laserRenderer.SetPosition(1, target.transform.position);

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
        isShooting = true;

    }

    private void SeekTarget() {

        if (weaponHead) {
            weaponHead.LookAt(target.transform.position - new Vector3(0, 3, 0));
        }

        if (Vector3.Distance(transform.position, target.transform.position) > turretRadius + 1) {
            ResetSeekingState();
        }

    }

    private void ResetSeekingState() {

        StopAllCoroutines();
        target = null;
        canSeek = true;
        isShooting = false;
        hasLockedTarget = false;
        laserLifetime = 0;

    }

    private void FindFirePoints() {

        Transform t = transform.GetChild(0).GetChild(0).GetChild(1);
        int length = t.childCount;
        for (int i = 0; i < length; i++) {

            laserSpawnLocations.Add(t.GetChild(i));

        }

    }

}