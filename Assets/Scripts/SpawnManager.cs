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

    private List<Vector3> attackGridList = new List<Vector3> ();
    private List<Vector3> defenseGridList = new List<Vector3> ();
    private int currentAttack, currentDefense;
    private bool canMoveAttackX, canMoveAttackZ, canMoveDefenseX, canMoveDefenseZ;
    private float attackRTX, attackRTZ, defenseRTX, defenseRTZ;
    [SerializeField] private float timerReset = 0.15f;
    private bool showingAttackPanel = false, showingDefensePanel = false;

    // ----------------------------------

    [HeaderAttribute("Cat Prefabs")]
    public GameObject smallCatPrefab;
    public GameObject bigCatPrefab;

    [HeaderAttribute("Dog Turret Prefabs")]
    public GameObject missileTowerPrefab;
    public GameObject turretTowerPrefab;
    public GameObject plasmaTowerPrefab;
    public GameObject teslaTowerPrefab;

    public CanvasGroup attackerCanvas;
    public CanvasGroup defenderCanvas;
	
	private CatType catType;
    // private string[] catOptions = { "Cat Attack", "Cat Seek" };
    public GameObject[] CatPrefabs;
    private int CatSelector = 0;

	private DogType dogType;
    // private string[] dogOptions = { "Missile", "Turret", "Plasma", "Tesla" };
    public GameObject[] DogPrefabs;
    private int DogSelector = 0;

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

        if (gm.gamestate == GameState.Placement) {

            UpdateAttackGrid();
            UpdateDefenseGrid();

        }

        if (XCI.GetButtonDown(XboxButton.X, cc.player2)) {
			
            DogSelector++;

            if (DogSelector > 3)
                DogSelector = 0;

            // defenderCanvas.GetComponentInChildren<TextMeshProUGUI> ().text = dogOptions[DogSelector];

        }

    }

    private void UpdateAttackGrid() {

        if (XCI.GetButtonDown(XboxButton.A, cc.player1)) {
			
			if (!showingAttackPanel) {
				
				attackerCanvas.DOFade(1, 1);
				showingAttackPanel = true;
				
			} else {
				
				// Spawn cats
				switch (catType)
				{
					case CatType.Attack:
						{
							
						}
					break;
					case CatType.Seek:
						{
							
						}
					break;
					default:break;
				}
				
				attackerCanvas.DOFade(0, 1);
				showingAttackPanel = false;
				
			}

        }

        if (XCI.GetButtonDown(XboxButton.X, cc.player1)) {
			
            if (CatSelector == 0) {
				catType = CatType.Attack;
                CatSelector = 1;
			}
            else {
				catType = CatType.Seek;
                CatSelector = 0;
			}

            // attackerCanvas.GetComponentInChildren<TextMeshProUGUI> ().text = catOptions[CatSelector];
			attackerCanvas.GetComponentInChildren<TextMeshProUGUI> ().text = catType.ToString() + "ing Cat";

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