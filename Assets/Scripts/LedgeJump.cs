using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeJump : MonoBehaviour {

    //implementation details: design doc page 4

    [SerializeField] private CharacterControllerBehaviour _characterControlScript;  
    [SerializeField] private GameObject _player;

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetAxis("AButton") != 0)
            {
                //set player to correct position
                CalculateSnappingPoint(gameObject.transform.forward, gameObject.transform.position);

                //disable components to avoid conflict
                _characterControlScript.IsHanging = true;

                //set velocity to 0
                _characterControlScript.Velocity = new Vector3(0, 0, 0);
            }
        }   
    }
    private void CalculateSnappingPoint(Vector3 forwardWall, Vector3 col)
    {
        #region offsetvalues
        //tweak offset values based on character model
        float yOffset = 1.4f;
        float xzOffset = 0.33f;
        #endregion offsetvalues
        if (forwardWall.x !=0)
        {
            _player.transform.position = new Vector3(col.x + (xzOffset * -transform.forward.x), col.y - yOffset, _player.transform.position.z );
        }
        else if (forwardWall.z !=0)
        {
            _player.transform.position = new Vector3(_player.transform.position.x , col.y - yOffset, col.z + (xzOffset * -transform.forward.z));
        }
        //set correct rotation
        Vector3 newRot = new Vector3(_player.transform.eulerAngles.x, transform.eulerAngles.y, _player.transform.eulerAngles.z);
        _player.transform.rotation = Quaternion.Euler(newRot);

        _characterControlScript.CurrentHangLocation = gameObject.transform.position; 
    }
}
