using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderScript: MonoBehaviour {

	// Use this for initialization
	void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.name == "BoxColliderModel")
        {

            Destroy(col.gameObject);
            Debug.Log("Collision!!!!");

        }
    

	}
	
	// Update is called once per frame
	void Update () {
        
    }
}
