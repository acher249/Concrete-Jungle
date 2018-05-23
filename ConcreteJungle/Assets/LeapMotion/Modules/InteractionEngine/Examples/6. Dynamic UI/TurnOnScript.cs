using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnScript : MonoBehaviour {

    public GameObject cafeModel;
    

    public void Start () 
    {

        cafeModel = GameObject.FindGameObjectWithTag("CafeModel");
        //cafeModel.SetActive(false);

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CafeModel"))
        {
            cafeModel.SetActive(false);
            Debug.Log("COLLISION!!!!");
          
        }
    }
}
