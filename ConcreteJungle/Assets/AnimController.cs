using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour {

    private Animator _animator = null;

	// Use this for initialization
	void Start ()
    {
        _animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	public void ExplodeOnClick ()
    {
        _animator.SetBool("isOpen", true);
    }

    public void ImplodeOnClick()
    {
        _animator.SetBool("isOpen", false);
    }
}
