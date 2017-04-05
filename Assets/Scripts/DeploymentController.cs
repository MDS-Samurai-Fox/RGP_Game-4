using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeploymentController : MonoBehaviour {

    public GameObject Player1Cursor;
    public GameObject Player2Cursor;

    public GameObject[] spawns;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            GameObject spawn = Instantiate(spawns[0], Player1Cursor.transform.position, Quaternion.identity);   
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            GameObject spawn = Instantiate(spawns[1], Player1Cursor.transform.position, Quaternion.identity);
        }
    }
}