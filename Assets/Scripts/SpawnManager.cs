﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using XboxCtrlrInput;

public class SpawnManager : MonoBehaviour {

    private CameraController cc;

    [HeaderAttribute("Grids")]
    public Transform attackGrid;
    public Transform defenseGrid;
    public int attackRowSize, attackColSize;
    public int defenseRowSize, defenseColSize;

    private Transform attackCursor, defenseCursor;

    private List<Vector3> attackGridList = new List<Vector3> ();
    private List<Vector3> defenseGridList = new List<Vector3> ();
    private int currentAttack, currentDefense;
    private bool canMoveAttack, canMoveDefense;
    private float attackResetTimer, defenseResetTimer;

    // ----------------------------------

    [HeaderAttribute("Cat Prefabs")]
    public GameObject smallCatPrefab;
    public GameObject bigCatPrefab;

    [HeaderAttribute("Dog Turret Prefabs")]
    public GameObject missileTowerPrefab;
    public GameObject turretTowerPrefab;
    public GameObject plasmaTowerPrefab;
    public GameObject teslaTowerPrefab;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        cc = FindObjectOfType<CameraController> ();

    }

    // Use this for initialization
    void Start() {

        attackCursor = cc.attackPlacementCursor;
        defenseCursor = cc.defensePlacementCursor;

        // Set the attacking grid
        for (int x = 0; x < attackRowSize; x++) {

            for (int y = 0; y < attackColSize; y++) {

                attackGridList.Add(new Vector3(y, 0, -x));

            }

        }

        // Set the defense grid
        for (int x = 0; x < attackRowSize; x++) {

            for (int y = 0; y < attackColSize; y++) {

                defenseGridList.Add(new Vector3(x, 0, y));

            }

        }

    }

    // Update is called once per frame
    void Update() {

        // attackCursor.position = new Vector3(-23.5f, 0.6f, 3.5f) + attackGridList[currentAttack];
        // defenseCursor.position = new Vector3(16.5f, 0.6f, 3.5f) + defenseGridList[currentDefense];

        UpdateAttackGrid();
        UpdateDefenseGrid();

    }

    private void UpdateAttackGrid() {

        if (XCI.GetAxisRaw(XboxAxis.LeftStickX, cc.player1) > 0 && canMoveAttack) {

            canMoveAttack = false;

            attackCursor.DOMoveX(Mathf.Clamp(attackCursor.position.x + 1, -23.5f, -23.5f + attackRowSize), 0);

        } else if (XCI.GetAxisRaw(XboxAxis.LeftStickX, cc.player1) < 0 && canMoveAttack) {

            canMoveAttack = false;

            attackCursor.DOMoveX(Mathf.Clamp(attackCursor.position.x - 1, -23.5f, -23.5f + attackRowSize), 0);

        }
		
		if (XCI.GetAxisRaw(XboxAxis.LeftStickY, cc.player1) > 0 && canMoveAttack) {

            canMoveAttack = false;
			
            attackCursor.DOMoveZ(Mathf.Clamp(attackCursor.position.z + 1, 3.5f - attackColSize, 3.5f), 0);

        } else if (XCI.GetAxisRaw(XboxAxis.LeftStickY, cc.player1) < 0 && canMoveAttack) {

            canMoveAttack = false;

            attackCursor.DOMoveZ(Mathf.Clamp(attackCursor.position.z - 1, 3.5f - attackColSize, 3.5f), 0);

        }

        if (!canMoveAttack) {

            attackResetTimer += Time.deltaTime;

            if (attackResetTimer > 0.2f) {
                canMoveAttack = true;
                attackResetTimer = 0;
            }

        }

    }

    private void UpdateDefenseGrid() {
		
		if (XCI.GetAxisRaw(XboxAxis.LeftStickX, cc.player2) > 0 && canMoveDefense) {

            canMoveDefense = false;

            defenseCursor.DOMoveX(Mathf.Clamp(defenseCursor.position.x + 1, 16.5f, 16.5f + defenseRowSize), 0);

        } else if (XCI.GetAxisRaw(XboxAxis.LeftStickX, cc.player2) < 0 && canMoveDefense) {

            canMoveDefense = false;

            defenseCursor.DOMoveX(Mathf.Clamp(defenseCursor.position.x - 1, 16.5f, 16.5f + defenseRowSize), 0);

        }
		
		if (XCI.GetAxisRaw(XboxAxis.LeftStickY, cc.player2) > 0 && canMoveDefense) {

            canMoveDefense = false;
			
            defenseCursor.DOMoveZ(Mathf.Clamp(defenseCursor.position.z + 1, 3.5f - defenseColSize, 3.5f), 0);

        } else if (XCI.GetAxisRaw(XboxAxis.LeftStickY, cc.player2) < 0 && canMoveDefense) {

            canMoveDefense = false;

            defenseCursor.DOMoveZ(Mathf.Clamp(defenseCursor.position.z - 1, 3.5f - defenseColSize, 3.5f), 0);

        }

        if (!canMoveDefense) {

            defenseResetTimer += Time.deltaTime;

            if (defenseResetTimer > 0.2f) {
                canMoveDefense = true;
                defenseResetTimer = 0;
            }

        }

    }

}