using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour {

    [SerializeField] private Animator _animator;
    [SerializeField] CharacterControllerBehaviour _characterControlScript;
    [SerializeField] GameObject Player;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        _animator.SetFloat("HorizontalVelocity", Input.GetAxis("Horizontal"));
        _animator.SetFloat("VerticalVelocity", Input.GetAxis("Vertical"));
	}
}
