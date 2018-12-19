using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeJump : MonoBehaviour {

    [SerializeField] private CharacterControllerBehaviour _characterControlScript;
    [SerializeField] private GameObject _player;

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetAxis("AButton") != 0)
            {
                //disable components to avoid conflict
                _characterControlScript.IsHanging = true;

                //set player to correct position
                CalculateSnappingPoint(other.gameObject.transform.forward, other);
            }
        }   
    }
    private void CalculateSnappingPoint(Vector3 forwardWall, Collider col)
    {
        #region offsetvalues
        //tweak offset values based on character model
        float yOffset = 0.5f;
        float xzOffset = 0.2f;
        #endregion offsetvalues
        if (forwardWall.x != 0)
        {
            //- (2 * forwardWall.x)
            _player.transform.position = new Vector3(col.transform.position.x , col.transform.position.y + yOffset, col.transform.position.z  /**-Mathf.Sign(col.transform.TransformDirection(Vector3.right).z)*/ - xzOffset);
        }
        else if (forwardWall.z != 0)
        {
            //- (2 * forwardWall.z
            _player.transform.position = new Vector3(col.transform.position.x  /**-Mathf.Sign(col.transform.TransformDirection(Vector3.right).x)*/ - xzOffset, col.transform.position.y - yOffset, col.transform.position.z);
        }
        //set correct rotation (NOT WORKING, STILL NEEDS FIX)
        _player.transform.rotation = col.transform.localRotation;


        _characterControlScript.CurrentHangLocation = gameObject.transform.position; 

    }
}
