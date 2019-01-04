using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour {

    [SerializeField] private GameObject _player;
    private float _delayTimer;
    private void Update()
    {
        if (_delayTimer > 0)
        {
            _delayTimer--;
        }
        //quit pushing (in update to avoid collider issues
        if (Input.GetAxis("AButton") != 0 && _delayTimer <= 0 && _player.GetComponent<CharacterControllerBehaviour>().IsPushing)
        {
            _player.GetComponent<CharacterControllerBehaviour>().IsPushing = false;
            _delayTimer = 30;

            GetComponent<Rigidbody>().isKinematic = true;
        }
        ApplyForce();
        //apply force if pushing
        if (_player.GetComponent<CharacterControllerBehaviour>().IsPushing)
        {
            ApplyForce();
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //either enter or leave pushing state
            if (Input.GetAxis("AButton") != 0 && _delayTimer <= 0)
            {
                if (_player.GetComponent<CharacterControllerBehaviour>().IsPushing == false)
                {
                    CalculateSnappingPoint(other.gameObject.transform.forward, other);
                    Debug.Log("check");
                    _player.GetComponent<CharacterControllerBehaviour>().IsPushing = true;

                    GetComponent<Rigidbody>().isKinematic = false;
                }
               //set timer to avoid multiple button presses in short succession
                _delayTimer = 30;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _player.GetComponent<CharacterControllerBehaviour>().IsPushing = false;

            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    private void CalculateSnappingPoint(Vector3 forwardWall, Collider col)
    {
        #region offsetvalues
        //tweak offset values based on character model
        float xzOffset = 0.2f;
        #endregion offsetvalues
        if (forwardWall.x != 0)
        {
            //- (2 * forwardWall.x)
            _player.transform.position = new Vector3(col.gameObject.transform.position.x - xzOffset, _player.transform.position.y, col.gameObject.transform.position.z);
        }
        else if (forwardWall.z != 0)
        {
            //- (2 * forwardWall.z
            _player.transform.position = new Vector3(col.gameObject.transform.position.x , _player.transform.position.y, col.gameObject.transform.position.z - xzOffset);
        }
        //set correct rotation
        Vector3 newRot = new Vector3(_player.transform.eulerAngles.x, transform.eulerAngles.y, _player.transform.eulerAngles.z);
        _player.transform.rotation = Quaternion.Euler(newRot);
        Debug.Log("SnappingPointCalculated");

    }
    private void ApplyForce()
    {
        if (Input.GetAxis("Vertical") != 0)
        {
            int forceToApply = 5;
            Vector3 force = new Vector3(_player.transform.forward.x* forceToApply, 0,_player.transform.forward.z* forceToApply);
            GetComponent<Rigidbody>().AddForce(force);
        }
    }

}
