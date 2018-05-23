using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEventGameObject : MonoBehaviour
{

    public GameObject cafeModel;


 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            cafeModel.SetActive(false);
            Debug.Log("COLLISION!!!!");

        }
    }
}