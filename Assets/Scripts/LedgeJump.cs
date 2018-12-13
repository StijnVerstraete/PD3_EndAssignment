using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeJump : MonoBehaviour {

    [SerializeField] private CharacterControllerBehaviour _characterControlScript;
    [SerializeField] private GameObject _player;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("PlayerCanGrab");
            if (Input.GetAxis("AButton") !=0)
            {
                _characterControlScript.IsHanging = true;
                _player.transform.position = other.transform.position;
                _characterControlScript.enabled = false;
            }
        }   
    }
}
