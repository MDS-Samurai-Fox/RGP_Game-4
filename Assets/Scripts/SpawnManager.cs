﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using XboxCtrlrInput;

public class SpawnManager : MonoBehaviour {

    private GameManager gm;
    private CameraController cc;

    [HeaderAttribute("Grids")]
    public Transform attackGrid;
    public Transform defenseGrid;
    public int attackRowSize, attackColSize;
    public int defenseRowSize, defenseColSize;

    private Transform attackCursor, defenseCursor;

    private bool canMoveAttackX, canMoveAttackZ, canMoveDefenseX, canMoveDefenseZ;
    private float attackRTX, attackRTZ, defenseRTX, defenseRTZ;
    [SerializeField] private float timerReset = 0.15f;
    private bool showingAttackPanel = false, showingDefensePanel = false;
    private Vector3 originalAttackerCursorPosition, originalDefenderCursorPosition;

    // ----------------------------------

    [HeaderAttribute("Cat Prefabs")]
    public GameObject SeekingCatPrefab;
    public GameObject AttackingCatPrefab;

    [HeaderAttribute("Dog Turret Prefabs")]
    public GameObject missileTowerPrefab;
    public GameObject turretTowerPrefab;
    public GameObject plasmaTowerPrefab;
    public GameObject teslaTowerPrefab;

    public CanvasGroup attackerCanvas;
    public CanvasGroup defenderCanvas;

    private CatType catType;
    private int CatSelector = 0;
    public int maxCats = 12;

    private DogType dogType;
    private int DogSelector = 0;
    public int maxDogs = 8;

    public bool catsReady = false, dogsReady = false;

    public XboxController player;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake() {

        gm = FindObjectOfType<GameManager> ();
        cc = FindObjectOfType<CameraController> ();

    }

    // Use this for initialization
    void Start() {

        attackCursor = cc.attackPlacementCursor;
        defenseCursor = cc.defensePlacementCursor;

        originalAttackerCursorPosition = attackCursor.position;
        originalDefenderCursorPosition = defenseCursor.position;
        
        gm.NumCatsAlive = maxCats;
        gm.NumDogsAlive = maxDogs;

    }

    public void Initialize()
    {
        catsReady = false;
        dogsReady = false;

        maxCats = 12;
        maxDogs = 8;

        attackerCanvas.alpha = 1;
        defenderCanvas.alpha = 1;

        cc.attackPlacementCursor.gameObject.SetActive(true);
        cc.defensePlacementCursor.gameObject.SetActive(true);

        cc.MoveCameraToPlacementMode();
    }

    // Update is called once per frame
    void Update() {

        if (gm.gamestate == GameState.Placement) {

            if (!catsReady) {
                UpdateAttackGrid();
                
            }

            if (!dogsReady) {
                UpdateDefenseGrid();
            }

            if (catsReady && dogsReady) {

                StartCoroutine(gm.StartGame());
                attackerCanvas.alpha = 0;
                defenderCanvas.alpha = 0;
                
                cc.attackPlacementCursor.gameObject.SetActive(false);
                cc.defensePlacementCursor.gameObject.SetActive(false);

            }

        }

    }

    private void UpdateAttackGrid() {

        attackerCanvas.GetComponentInChildren<TextMeshProUGUI> ().text = catType.ToString() + "ing Cat";
        
        if (gm.IsPlayer1aCat)
        {
            player = cc.player1;
        }
        else
        {
            player = cc.player2;
        }

        if (XCI.GetButtonDown(XboxButton.A, player) || Input.GetKeyDown(KeyCode.Z)) {
            
            gm.soundManager.PlaySound(gm.soundManager.select);

            if (maxCats <= 0) {
                attackerCanvas.DOFade(1, 0);
                showingAttackPanel = true;
                attackerCanvas.GetComponentInChildren<TextMeshProUGUI> ().text = "CATS READY";
                catsReady = true;
                return;
            }

            // if (!showingAttackPanel) {

                // attackerCanvas.DOFade(1, 0.5f);
                // showingAttackPanel = true;

            // } 
            // else {

                Quaternion orientation = Quaternion.Euler(0, 90, 0);

                // Spawn cats
                switch (catType) {
                    case CatType.Attack:
                        {

                            GameObject cat = Instantiate(AttackingCatPrefab, attackCursor.position, orientation);
                            cat.transform.SetParent(attackGrid);

                        }
                        break;
                    case CatType.Seek:
                        {

                            for (int i = 0; i < 2; i++) {

                                GameObject cat = Instantiate(SeekingCatPrefab, attackCursor.position, orientation);
                                cat.transform.SetParent(attackGrid);

                            }

                        }
                        break;
                    default:
                        break;
                }

                maxCats--;
                // attackerCanvas.DOFade(0, 0.5f);
                showingAttackPanel = false;

            // }

        }

        if (XCI.GetButtonDown(XboxButton.DPadLeft, player) || Input.GetKeyDown(KeyCode.Q)) {

            CatSelector--;

            if (CatSelector < 0) {
                CatSelector = 1;
            }

            catType = (CatType) CatSelector;

        } else if (XCI.GetButtonDown(XboxButton.DPadRight, player) || Input.GetKeyDown(KeyCode.E)) {

            CatSelector++;

            if (CatSelector > 1) {
                CatSelector = 0;
            }

            catType = (CatType) CatSelector;

        }

        if ((XCI.GetAxisRaw(XboxAxis.LeftStickY, player) < 0 || Input.GetKeyDown(KeyCode.S)) && canMoveAttackX) {
            
            gm.soundManager.PlaySound(gm.soundManager.gridMove);

            canMoveAttackX = false;

            attackCursor.DOMoveX(Mathf.Clamp(attackCursor.position.x + 1, originalAttackerCursorPosition.x, originalAttackerCursorPosition.x + attackRowSize), 0);

        } else if ((XCI.GetAxisRaw(XboxAxis.LeftStickY, player) > 0 || Input.GetKeyDown(KeyCode.W)) && canMoveAttackX) {
            
            gm.soundManager.PlaySound(gm.soundManager.gridMove);

            canMoveAttackX = false;

            attackCursor.DOMoveX(Mathf.Clamp(attackCursor.position.x - 1, originalAttackerCursorPosition.x, originalAttackerCursorPosition.x + attackRowSize), 0);

        }

        if ((XCI.GetAxisRaw(XboxAxis.LeftStickX, player) > 0 || Input.GetKeyDown(KeyCode.D)) && canMoveAttackZ) {
            
            gm.soundManager.PlaySound(gm.soundManager.gridMove);

            canMoveAttackZ = false;

            attackCursor.DOMoveZ(Mathf.Clamp(attackCursor.position.z + 1, originalAttackerCursorPosition.z, originalAttackerCursorPosition.z + attackColSize), 0);

        } else if ((XCI.GetAxisRaw(XboxAxis.LeftStickX, player) < 0 || Input.GetKeyDown(KeyCode.A)) && canMoveAttackZ) {
            
            gm.soundManager.PlaySound(gm.soundManager.gridMove);

            canMoveAttackZ = false;

            attackCursor.DOMoveZ(Mathf.Clamp(attackCursor.position.z - 1, originalAttackerCursorPosition.z, originalAttackerCursorPosition.z + attackColSize), 0);

        }

        if (!canMoveAttackX) {

            attackRTX += Time.deltaTime;

            if (attackRTX > timerReset) {
                canMoveAttackX = true;
                attackRTX = 0;
            }

        }

        if (!canMoveAttackZ) {

            attackRTZ += Time.deltaTime;

            if (attackRTZ > timerReset) {
                canMoveAttackZ = true;
                attackRTZ = 0;
            }

        }

    }

    private void UpdateDefenseGrid() {

        if (gm.IsPlayer1aCat)
        {
            player = cc.player2;
        }
        else
        {
            player = cc.player1;
        }

        defenderCanvas.GetComponentInChildren<TextMeshProUGUI> ().text = dogType.ToString() + "";

        if (XCI.GetButtonDown(XboxButton.A, player) || Input.GetKeyDown(KeyCode.N)) {
            
            gm.soundManager.PlaySound(gm.soundManager.select);

            if (maxDogs <= 0) {
                defenderCanvas.DOFade(1, 0);
                defenderCanvas.GetComponentInChildren<TextMeshProUGUI> ().text = "DOGS READY";
                showingDefensePanel = true;
                dogsReady = true;
                return;
            }

            // if (!showingDefensePanel) {

                // defenderCanvas.DOFade(1, 0.5f);
                // showingDefensePanel = true;

            // } else {

                Quaternion orientation = Quaternion.Euler(0, -90, 0);

                // Spawn cats
                switch (dogType) {
                    case DogType.Missile:
                        {

                            GameObject dog = Instantiate(missileTowerPrefab, defenseCursor.position, orientation);
                            dog.transform.SetParent(defenseGrid);

                        }
                        break;
                    case DogType.Turret:
                        {

                            GameObject dog = Instantiate(turretTowerPrefab, defenseCursor.position, orientation);
                            dog.transform.SetParent(defenseGrid);

                        }
                        break;
                    case DogType.Plasma:
                        {

                            GameObject dog = Instantiate(plasmaTowerPrefab, defenseCursor.position, orientation);
                            dog.transform.SetParent(defenseGrid);

                        }
                        break;
                    case DogType.Tesla:
                        {

                            GameObject dog = Instantiate(teslaTowerPrefab, defenseCursor.position, orientation);
                            dog.transform.SetParent(defenseGrid);

                        }
                        break;
                    default:
                        break;
                }

                maxDogs--;
                // defenderCanvas.DOFade(0, 0.5f);
                showingDefensePanel = false;

            // }

        }

        if (XCI.GetButtonDown(XboxButton.DPadLeft, player) || Input.GetKeyDown(KeyCode.U)) {

            DogSelector--;

            if (DogSelector < 0) {
                DogSelector = 3;
            }

            dogType = (DogType) DogSelector;

        } else if (XCI.GetButtonDown(XboxButton.DPadRight, player) || Input.GetKeyDown(KeyCode.O)) {

            DogSelector++;

            if (DogSelector > 3) {
                DogSelector = 0;
            }

            dogType = (DogType) DogSelector;

        }

        if ((XCI.GetAxisRaw(XboxAxis.LeftStickY, player) < 0 || Input.GetKeyDown(KeyCode.K)) && canMoveDefenseX) {
            
            gm.soundManager.PlaySound(gm.soundManager.gridMove);

            canMoveDefenseX = false;

            defenseCursor.DOMoveX(Mathf.Clamp(defenseCursor.position.x + 3, originalDefenderCursorPosition.x, originalDefenderCursorPosition.x + defenseRowSize), 0);

        } else if ((XCI.GetAxisRaw(XboxAxis.LeftStickY, player) > 0 || Input.GetKeyDown(KeyCode.I)) && canMoveDefenseX) {
            
            gm.soundManager.PlaySound(gm.soundManager.gridMove);

            canMoveDefenseX = false;

            defenseCursor.DOMoveX(Mathf.Clamp(defenseCursor.position.x - 3, originalDefenderCursorPosition.x, originalDefenderCursorPosition.x + defenseRowSize), 0);

        }

        if ((XCI.GetAxisRaw(XboxAxis.LeftStickX, player) > 0 || Input.GetKeyDown(KeyCode.L)) && canMoveDefenseZ) {
            
            gm.soundManager.PlaySound(gm.soundManager.gridMove);

            canMoveDefenseZ = false;

            defenseCursor.DOMoveZ(Mathf.Clamp(defenseCursor.position.z + 3, originalDefenderCursorPosition.z, originalDefenderCursorPosition.z + defenseColSize), 0);

        } else if ((XCI.GetAxisRaw(XboxAxis.LeftStickX, player) < 0 || Input.GetKeyDown(KeyCode.J)) && canMoveDefenseZ) {
            
            gm.soundManager.PlaySound(gm.soundManager.gridMove);

            canMoveDefenseZ = false;

            defenseCursor.DOMoveZ(Mathf.Clamp(defenseCursor.position.z - 3, originalDefenderCursorPosition.z, originalDefenderCursorPosition.z + defenseColSize), 0);

        }

        if (!canMoveDefenseX) {

            defenseRTX += Time.deltaTime;

            if (defenseRTX > timerReset) {
                canMoveDefenseX = true;
                defenseRTX = 0;
            }

        }

        if (!canMoveDefenseZ) {

            defenseRTZ += Time.deltaTime;

            if (defenseRTZ > timerReset) {
                canMoveDefenseZ = true;
                defenseRTZ = 0;
            }

        }

    }

}