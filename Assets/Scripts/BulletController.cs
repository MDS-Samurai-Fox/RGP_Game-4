using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public ParticleSystem explosionParticles;

    private bool hasFoundTarget = false;

    private float speed = 50f;
    private float currentLifetime = 0;
    private float lifetime = 50;

    // Update is called once per frame
    void Update() {

        if (!hasFoundTarget)
            return;

        currentLifetime += Time.deltaTime;

        // Destroy the bullet if it exceeds the 
        if (currentLifetime >= lifetime) {
            StartCoroutine(DestroyBullet(false));
        }

    }

    private void FixedUpdate() {

        if (!hasFoundTarget)
            return;

        CheckCollisions();
        transform.position += transform.forward * speed * Time.deltaTime;

    }

    private void OnDrawGizmos() {

        if (!transform.parent)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.parent.position);

    }

    /// <summary>
    /// Set the target of the bullet and make it look towards it
    /// </summary>
    /// <param name="tc"> The turret that holds this bullet </param>
    /// <param name="targetPosition"></param>
    /// <param name="targetPositionOffset"></param>
    public void SetTarget(TurretController tc, Vector3 targetPosition) {

        lifetime = tc.bulletLifeTime;
        speed = tc.bulletSpeed;

        transform.LookAt(targetPosition + new Vector3(0, 1, 0));

        hasFoundTarget = true;

    }

    /// <summary>
    /// Destroys the bullet
    /// </summary>
    /// <param name="hasCollided"> True for when the bullet has collided with something </param>
    private IEnumerator DestroyBullet(bool hasCollided) {

        if (hasCollided) {

            if (explosionParticles)
                explosionParticles.Play();

        }

        yield return new WaitForSeconds(1f);

        transform.parent = null;
        Destroy(this.gameObject);

    }

    private void CheckCollisions() {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1)) {

            StartCoroutine(DestroyBullet(true));

        }

    }

}