﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public GameObject hitEffect;
    private GameObject hitObject = null;

    private bool hasFoundTarget = false;

    private float speed = 50f;
    public float range = 0;
    private float currentLifetime = 0;
    private float lifetime = 50;

    // Update is called once per frame
    void Update() {

        if (!hasFoundTarget)
            return;

        currentLifetime += Time.deltaTime;

        // Destroy the bullet if it exceeds the 
        if (currentLifetime >= lifetime) {
            DestroyBullet(false);
        }

    }

    private void FixedUpdate() {

        if (!hasFoundTarget)
            return;
        
        transform.position += transform.forward * speed * Time.deltaTime;
        
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1)) {
            
            if (hit.transform.gameObject.CompareTag("Fire")) {
                hitObject = hit.transform.gameObject;
            }
            DestroyBullet(true);

        }

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
    private void DestroyBullet(bool hasCollided) {

        if (hasCollided) {
            
            GameObject explosionInstance = Instantiate(hitEffect, transform.position, transform.rotation);
            
            if (range > 0) {
                
                
                
            }

        }
        
        Destroy(this.gameObject);

    }

}