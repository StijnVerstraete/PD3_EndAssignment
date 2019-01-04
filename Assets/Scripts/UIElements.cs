using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIElements : MonoBehaviour {

	[SerializeField] private CharacterControllerBehaviour _characterScript;
    [SerializeField] private Image _crosshair;
	
	// Update is called once per frame
	void Update ()
    {
        if (_characterScript.IsAiming)
            _crosshair.enabled = true;
        else
            _crosshair.enabled = false;
	}
}
