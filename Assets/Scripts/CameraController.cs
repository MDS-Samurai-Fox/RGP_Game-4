using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{

    public Transform cursor1, cursor2;
    private Vector3 cursor1pos, cursor2pos;

    // Use this for initialization
    void Start()
    {

        cursor1pos = cursor1.position;
        cursor2pos = cursor2.position;

    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetAxis("Horizontal") > 0)
        //{
        //print("INPUT");
        cursor1.transform.DOMoveX(cursor1.transform.position.x + (Input.GetAxis("Horizontal") * 0.25f), 0);
        cursor1.transform.DOMoveZ(cursor1.transform.position.z + (Input.GetAxis("Vertical") * 0.25f), 0);
        //}

    }

}
