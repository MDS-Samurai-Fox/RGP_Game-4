using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using XboxCtrlrInput;

public class CameraController : MonoBehaviour
{

    public Transform cursor1, cursor2;
    private Vector3 cursor1pos, cursor2pos;

    [SpaceAttribute]
    public Transform TopLeftCorner;
    public Transform TopRightCorner;
    public Transform BottomLeftCorner;
    public Transform BottomRightCorner;

    //public XboxController controller;

    private GameManager gm;
    private float minX, minZ, maxX, maxZ;

    public Vector3 CenterOfMap = new Vector3(0, 0, 0);

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    // Use this for initialization
    void Start()
    {
        cursor1pos = cursor1.position;
        cursor2pos = cursor2.position;

        minX = TopLeftCorner.transform.position.x;
        maxX = TopRightCorner.transform.position.x;
        maxZ = TopLeftCorner.transform.position.z;
        minZ = BottomLeftCorner.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.gamestate == GameState.Placement)
        {          
            transform.DOMoveX(0, 2);
            transform.DOMoveY(100, 2);
            transform.DOMoveZ(10, 2);
            transform.DORotate(new Vector3(90, 0, 0), 2);

            //transform.rotation
        }
            float maxMoveSpeed = 20;

            //cursor1.transform.position = new Vector3(Mathf.Clamp(cursor1.transform.position.x, minX, maxX), 0, Mathf.Clamp(cursor1.transform.position.z, minZ, maxZ));
            //cursor2.transform.position = new Vector3(Mathf.Clamp(cursor2.transform.position.x, minX, maxX), 0, Mathf.Clamp(cursor2.transform.position.z, minZ, maxZ));

            cursor1.transform.DOMoveX(cursor1.transform.position.x + (XCI.GetAxis(XboxAxis.LeftStickX, XboxController.First) * maxMoveSpeed * Time.deltaTime), 0);
            cursor1.transform.DOMoveZ(cursor1.transform.position.z + (XCI.GetAxis(XboxAxis.LeftStickY, XboxController.First) * maxMoveSpeed * Time.deltaTime), 0);

            //cursor1.transform.DOMoveX(cursor1.transform.position.x + (XCI.GetAxis(XboxAxis.RightStickX, XboxController.Third) * maxMoveSpeed * Time.deltaTime), 0);
            //cursor1.transform.DOMoveZ(cursor1.transform.position.z + (XCI.GetAxis(XboxAxis.RightStickY, XboxController.Third) * maxMoveSpeed * Time.deltaTime), 0);

            cursor2.transform.DOMoveX(cursor2.transform.position.x + (XCI.GetAxis(XboxAxis.LeftStickX, XboxController.Second) * maxMoveSpeed * Time.deltaTime), 0);
            cursor2.transform.DOMoveZ(cursor2.transform.position.z + (XCI.GetAxis(XboxAxis.LeftStickY, XboxController.Second) * maxMoveSpeed * Time.deltaTime), 0);

        //cursor2.transform.DOMoveX(cursor2.transform.position.x + (XCI.GetAxis(XboxAxis.RightStickX, XboxController.Second) * maxMoveSpeed * Time.deltaTime), 0);
        //cursor2.transform.DOMoveZ(cursor2.transform.position.z + (XCI.GetAxis(XboxAxis.RightStickY, XboxController.Second) * maxMoveSpeed * Time.deltaTime), 0);


        // cursor1.transform.position = new Vector3(Mathf.Clamp(cursor1.transform.position.x, minX, maxX), 0, Mathf.Clamp(cursor1.transform.position.z, 0, 0));

        //}

        if (gm.gamestate == GameState.Play)
        {
            cursor1pos = cursor1.transform.position;
            cursor2pos = cursor2.transform.position;

            float distanceBetweenCursors = Vector3.Magnitude(cursor1pos - cursor2pos);
            float deltaZ = cursor2pos.z - cursor1pos.z;
            float deltaX = cursor2pos.x - cursor1pos.x;
            float gradient = deltaX / deltaX;

            float newXpos = Mathf.Min(cursor1pos.x, cursor2pos.x) - deltaX / 2;
            float newZpos = Mathf.Min(cursor1pos.z, cursor2pos.z) - deltaZ / 2;

            //replace with clamp
            if (distanceBetweenCursors <= 10)
            {
                distanceBetweenCursors = 10;
            }

            
            
            transform.DORotate(new Vector3(-45, 0, 0), 0);
            transform.DOLookAt(CenterOfMap, 0);

            transform.DOMoveX(newXpos, 0);
            transform.DOMoveY(distanceBetweenCursors, 0);
            transform.DOMoveZ(newZpos, 0);

            //transform.local

            Debug.Log(distanceBetweenCursors);

            //transform.rotation
        }


    }

}