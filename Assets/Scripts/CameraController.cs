using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using XboxCtrlrInput;

public class CameraController : MonoBehaviour {

    private GameManager gm;

    [HeaderAttribute("Controller functionality")]
    public XboxController player1, player2;

    public Transform attackCursor, defenseCursor;
    public Transform attackPlacementCursor, defensePlacementCursor;
    
    private Vector3 cursor1pos, cursor2pos;

    [HeaderAttribute("Map corners")]
    public Transform TopLeftCorner;
    public Transform TopRightCorner;
    public Transform BottomLeftCorner;
    public Transform BottomRightCorner;

    [SpaceAttribute]
    public float maxMoveSpeed = 20;
    [SerializeField]
    private float mapTranslationDelay = 2;

    [SpaceAttribute]
    public Vector3 CenterOfMap;
    public Vector3 startPosition, startRotation;

    private float minX, minZ, maxX, maxZ;
    
    
    private void Awake() {

        gm = FindObjectOfType<GameManager> ();

    }

    // Use this for initialization
    void Start() {

        cursor1pos = attackCursor.position;
        cursor2pos = defenseCursor.position;
        
        attackCursor.gameObject.SetActive(false);
        defenseCursor.gameObject.SetActive(false);

        minX = TopLeftCorner.transform.position.x;
        maxX = TopRightCorner.transform.position.x;
        maxZ = TopLeftCorner.transform.position.z;
        minZ = BottomLeftCorner.transform.position.z;

        MoveCameraToPlacementMode();

    }

    // Update is called once per frame
    void Update() {

        if (gm.gamestate == GameState.Play)
        {
            ActivateCursors();

            attackCursor.transform.position = new Vector3(Mathf.Clamp(attackCursor.transform.position.x, minX, maxX), 1, Mathf.Clamp(attackCursor.transform.position.z, minZ, maxZ));
            defenseCursor.transform.position = new Vector3(Mathf.Clamp(defenseCursor.transform.position.x, minX, maxX), 1, Mathf.Clamp(defenseCursor.transform.position.z, minZ, maxZ));

            //attackCursor.transform.DOMoveX(attackCursor.transform.position.x + (XCI.GetAxis(XboxAxis.LeftStickX, player1) * maxMoveSpeed * Time.deltaTime), 0);
            //attackCursor.transform.DOMoveZ(attackCursor.transform.position.z + (XCI.GetAxis(XboxAxis.LeftStickY, player1) * maxMoveSpeed * Time.deltaTime), 0);

            defenseCursor.transform.DOMoveX(defenseCursor.transform.position.x + (XCI.GetAxis(XboxAxis.LeftStickX, player2) * maxMoveSpeed * Time.deltaTime), 0);
            defenseCursor.transform.DOMoveZ(defenseCursor.transform.position.z + (XCI.GetAxis(XboxAxis.LeftStickY, player2) * maxMoveSpeed * Time.deltaTime), 0);

            if (XCI.IsPluggedIn(3)) //for wireless controller testing
            {
                attackCursor.transform.DOMoveX(attackCursor.transform.position.x + (XCI.GetAxis(XboxAxis.LeftStickX, XboxController.Third) * maxMoveSpeed * Time.deltaTime), 0);
                attackCursor.transform.DOMoveZ(attackCursor.transform.position.z + (XCI.GetAxis(XboxAxis.LeftStickY, XboxController.Third) * maxMoveSpeed * Time.deltaTime), 0);
            }
            else
            {
                attackCursor.transform.DOMoveX(attackCursor.transform.position.x + (XCI.GetAxis(XboxAxis.LeftStickX, XboxController.First) * maxMoveSpeed * Time.deltaTime), 0);
                attackCursor.transform.DOMoveZ(attackCursor.transform.position.z + (XCI.GetAxis(XboxAxis.LeftStickY, XboxController.First) * maxMoveSpeed * Time.deltaTime), 0);
            }

            cursor1pos = attackCursor.transform.position;
            cursor2pos = defenseCursor.transform.position;

            float distanceBetweenCursors = Mathf.Max(Vector3.Magnitude(cursor1pos - cursor2pos), 1);
            float deltaZ = cursor2pos.z - cursor1pos.z;
            float deltaX = cursor2pos.x - cursor1pos.x;

            float newXpos = Mathf.Min(cursor1pos.x, cursor2pos.x) + deltaX / 2;
            float newZpos = Mathf.Min(cursor1pos.z, cursor2pos.z) + deltaZ / 2;

            transform.DOMove(new Vector3(newXpos, distanceBetweenCursors, newZpos), 0);
            transform.DOLocalMoveZ(-1, 0);
            transform.DORotate(new Vector3(-45, 0, 0), 0);
            transform.DOLookAt(new Vector3(newXpos, 0, newZpos), 0);


        }

    }

    public void MoveCameraToPlacementMode() {
        
        transform.DOMove(startPosition, 3).SetDelay(mapTranslationDelay).SetEase(Ease.OutQuart);
        transform.DORotate(startRotation, 3).SetDelay(mapTranslationDelay).SetEase(Ease.OutQuart);
        
    }
    
    public void Game() {
        
       //transform.DOMove(new Vector3(76, 29, 167), 3).SetDelay(mapTranslationDelay).SetEase(Ease.OutQuart);
       //transform.DORotate(new Vector3(33, 213, 0), 3).SetDelay(mapTranslationDelay).SetEase(Ease.OutQuart);
        
    }
    
    public void ActivateCursors() {
        
        attackCursor.gameObject.SetActive(true);
        defenseCursor.gameObject.SetActive(true);

        attackCursor.transform.DOMoveY(10, 0);
        defenseCursor.transform.DOMoveY(10, 0);
    }

}