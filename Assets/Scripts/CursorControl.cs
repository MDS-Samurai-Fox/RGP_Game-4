using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("A pressed");
            Vector3 position = gameObject.transform.position;
            position.x--;
            gameObject.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("D pressed");
            Vector3 position = gameObject.transform.position;
            position.x++;
            gameObject.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("W pressed");
            Vector3 position = gameObject.transform.position;
            position.z++;
            gameObject.transform.position = position;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("S pressed");
            Vector3 position = gameObject.transform.position;
            position.z--;
            gameObject.transform.position = position;
        }
    }
}
