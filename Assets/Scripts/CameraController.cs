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

    private float minX, minZ, maxX, maxZ;
    public Vector3 CenterOfMap = new Vector3(0, 0, 0);
    public float maxMoveSpeed = 20;
    [SerializeField] private float mapTranslationDelay = 2;

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

        // attackCursor.transform.position = new Vector3(Mathf.Clamp(attackCursor.transform.position.x, minX, maxX), 1, Mathf.Clamp(attackCursor.transform.position.z, minZ, maxZ));
        // defenseCursor.transform.position = new Vector3(Mathf.Clamp(defenseCursor.transform.position.x, minX, maxX), 1, Mathf.Clamp(defenseCursor.transform.position.z, minZ, maxZ));

        // attackCursor.transform.DOMoveX(attackCursor.transform.position.x + (XCI.GetAxis(XboxAxis.LeftStickX, player1) * maxMoveSpeed * Time.deltaTime), 0);
        // attackCursor.transform.DOMoveZ(attackCursor.transform.position.z + (XCI.GetAxis(XboxAxis.LeftStickY, player1) * maxMoveSpeed * Time.deltaTime), 0);

        // defenseCursor.transform.DOMoveX(defenseCursor.transform.position.x + (XCI.GetAxis(XboxAxis.LeftStickX, player2) * maxMoveSpeed * Time.deltaTime), 0);
        // defenseCursor.transform.DOMoveZ(defenseCursor.transform.position.z + (XCI.GetAxis(XboxAxis.LeftStickY, player2) * maxMoveSpeed * Time.deltaTime), 0);

        // }
        // if (XCI.IsPluggedIn(1)) //for the wireless controller
        // {

        //     cursor1.transform.DOMoveX(cursor1.transform.position.x + (XCI.GetAxis(XboxAxis.LeftStickX, XboxController.Third) * maxMoveSpeed * Time.deltaTime), 0);
        //     cursor1.transform.DOMoveZ(cursor1.transform.position.z + (XCI.GetAxis(XboxAxis.LeftStickY, XboxController.Third) * maxMoveSpeed * Time.deltaTime), 0);

        // } 
        // else {

        //cursor1.transform.DOMoveX(cursor1.transform.position.x + (XCI.GetAxis(XboxAxis.RightStickX, XboxController.Third) * maxMoveSpeed * Time.deltaTime), 0);
        //cursor1.transform.DOMoveZ(cursor1.transform.position.z + (XCI.GetAxis(XboxAxis.RightStickY, XboxController.Third) * maxMoveSpeed * Time.deltaTime), 0);
        // }

        //cursor2.transform.DOMoveX(cursor2.transform.position.x + (XCI.GetAxis(XboxAxis.RightStickX, XboxController.Second) * maxMoveSpeed * Time.deltaTime), 0);
        //cursor2.transform.DOMoveZ(cursor2.transform.position.z + (XCI.GetAxis(XboxAxis.RightStickY, XboxController.Second) * maxMoveSpeed * Time.deltaTime), 0);

        // cursor1.transform.position = new Vector3(Mathf.Clamp(cursor1.transform.position.x, minX, maxX), 0, Mathf.Clamp(cursor1.transform.position.z, 0, 0));

        //}

        if (gm.gamestate == GameState.Play) {

            cursor1pos = attackCursor.transform.position;
            cursor2pos = defenseCursor.transform.position;

            float distanceBetweenCursors = Vector3.Magnitude(cursor1pos - cursor2pos);
            float deltaZ = cursor2pos.z - cursor1pos.z;
            float deltaX = cursor2pos.x - cursor1pos.x;
            //float gradient = deltaX / deltaX;

            float newXpos = Mathf.Min(cursor1pos.x, cursor2pos.x) + deltaX / 2;
            float newZpos = Mathf.Min(cursor1pos.z, cursor2pos.z) + deltaZ / 2;

            //replace with clamp
            if (distanceBetweenCursors <= 20) {
                distanceBetweenCursors = 20;
            }

            //transform.DOMoveX(newXpos, 0);
            //transform.DOMoveY(distanceBetweenCursors, 0);
            //transform.DOMoveZ(newZpos, 0);
            transform.DOMove(new Vector3(newXpos, distanceBetweenCursors, newZpos), 0);
            transform.DOLocalMoveZ(-30, 0);

            transform.DORotate(new Vector3(-45, 0, 0), 0);
            transform.DOLookAt(CenterOfMap, 0);

            //transform.local

            // Debug.Log(distanceBetweenCursors);

            //transform.rotation
        }

    }

    public void MoveCameraToPlacementMode() {
        
        transform.DOMove(new Vector3(0, 50, 0), 3).SetDelay(mapTranslationDelay).SetEase(Ease.OutQuart);
        transform.DORotate(new Vector3(90, 0, 0), 3).SetDelay(mapTranslationDelay).SetEase(Ease.OutExpo);
        
    }
    
    public void ActivateCursors() {
        
        attackCursor.gameObject.SetActive(true);
        defenseCursor.gameObject.SetActive(true);
        
    }

}