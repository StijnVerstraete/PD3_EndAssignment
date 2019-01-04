using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour {

	[SerializeField] private Transform _IKTarget;
    [SerializeField] private IKControl _IKControl;
    [SerializeField] private CharacterControllerBehaviour _characterControlScript;

    [SerializeField] private Transform _rightHand; 
    private bool _pickedUp = false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetAxis("AButton") != 0 && !_pickedUp)
        {
            _characterControlScript.IsPickingUp = true;
            _characterControlScript.Gun = gameObject;
            _IKControl.IKActive = true;
            _IKControl.RightHandPosition = _IKTarget;

            _pickedUp = true;
            Debug.Log("Pickup");

            //avoid sliding when there is still some velocity
            _characterControlScript.Velocity = new Vector3(0, 0, 0);
        }
    }

}
