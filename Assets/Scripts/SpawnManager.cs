using System.Collections;
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

    }

    // Update is called once per frame
    void Update() {

        if (gm.gamestate == GameState.Placement) {

            UpdateAttackGrid();
            UpdateDefenseGrid();

        }

    }

    private void UpdateAttackGrid() {

        attackerCanvas.GetComponentInChildren<TextMeshProUGUI> ().text = catType.ToString() + "ing Cat";

        if (XCI.GetButtonDown(XboxButton.A, cc.player1)) {
            
            if (maxCats <= 0) 
                return;

            if (!showingAttackPanel) {

                attackerCanvas.DOFade(1, 0.5f);
                showingAttackPanel = true;

            } else {

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

                            GameObject cat = Instantiate(SeekingCatPrefab, attackCursor.position, orientation);
                            cat.transform.SetParent(attackGrid);

                        }
                        break;
                    default:
                        break;
                }
                
                maxCats--;
                attackerCanvas.DOFade(0, 0.5f);
                showingAttackPanel = false;

            }

        }

        if (XCI.GetButtonDown(XboxButton.DPadLeft, cc.player1)) {

            CatSelector--;

            if (CatSelector < 0) {
                CatSelector = 1;
            }

            catType = (CatType)CatSelector;

        } else if (XCI.GetButtonDown(XboxButton.DPadRight, cc.player1)) {

            CatSelector++;

            if (CatSelector > 1) {
                CatSelector = 0;
            }

            catType = (CatType)CatSelector;

        }

        if (XCI.GetAxisRaw(XboxAxis.LeftStickX, cc.player1) > 0 && canMoveAttackX) {

            canMoveAttackX = false;

            attackCursor.DOMoveX(Mathf.Clamp(attackCursor.position.x + 1, -23.5f, -23.5f + attackRowSize), 0);

        } else if (XCI.GetAxisRaw(XboxAxis.LeftStickX, cc.player1) < 0 && canMoveAttackX) {

            canMoveAttackX = false;

            attackCursor.DOMoveX(Mathf.Clamp(attackCursor.position.x - 1, -23.5f, -23.5f + attackRowSize), 0);

        }

        if (XCI.GetAxisRaw(XboxAxis.LeftStickY, cc.player1) > 0 && canMoveAttackZ) {

            canMoveAttackZ = false;

            attackCursor.DOMoveZ(Mathf.Clamp(attackCursor.position.z + 1, 3.5f - attackColSize, 3.5f), 0);

        } else if (XCI.GetAxisRaw(XboxAxis.LeftStickY, cc.player1) < 0 && canMoveAttackZ) {

            canMoveAttackZ = false;

            attackCursor.DOMoveZ(Mathf.Clamp(attackCursor.position.z - 1, 3.5f - attackColSize, 3.5f), 0);

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
        
        defenderCanvas.GetComponentInChildren<TextMeshProUGUI> ().text = dogType.ToString() + "";

        if (XCI.GetButtonDown(XboxButton.A, cc.player2)) {
            
            if (maxDogs <= 0) 
                return;

            if (!showingDefensePanel) {

                defenderCanvas.DOFade(1, 0.5f);
                showingDefensePanel = true;

            } else {

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
                defenderCanvas.DOFade(0, 0.5f);
                showingAttackPanel = false;

            }

        }

        if (XCI.GetButtonDown(XboxButton.DPadLeft, cc.player2)) {

            DogSelector--;

            if (DogSelector < 0) {
                DogSelector = 1;
            }

            dogType = (DogType)DogSelector;

        } else if (XCI.GetButtonDown(XboxButton.DPadRight, cc.player2)) {

            DogSelector++;

            if (DogSelector > 3) {
                DogSelector = 0;
            }
            
            dogType = (DogType)DogSelector;

        }

        if (XCI.GetAxisRaw(XboxAxis.LeftStickX, cc.player2) > 0 && canMoveDefenseX) {

            canMoveDefenseX = false;

            defenseCursor.DOMoveX(Mathf.Clamp(defenseCursor.position.x + 1, 16.5f, 16.5f + defenseRowSize), 0);

        } else if (XCI.GetAxisRaw(XboxAxis.LeftStickX, cc.player2) < 0 && canMoveDefenseX) {

            canMoveDefenseX = false;

            defenseCursor.DOMoveX(Mathf.Clamp(defenseCursor.position.x - 1, 16.5f, 16.5f + defenseRowSize), 0);

        }

        if (XCI.GetAxisRaw(XboxAxis.LeftStickY, cc.player2) > 0 && canMoveDefenseZ) {

            canMoveDefenseZ = false;

            defenseCursor.DOMoveZ(Mathf.Clamp(defenseCursor.position.z + 1, 3.5f - defenseColSize, 3.5f), 0);

        } else if (XCI.GetAxisRaw(XboxAxis.LeftStickY, cc.player2) < 0 && canMoveDefenseZ) {

            canMoveDefenseZ = false;

            defenseCursor.DOMoveZ(Mathf.Clamp(defenseCursor.position.z - 1, 3.5f - defenseColSize, 3.5f), 0);

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