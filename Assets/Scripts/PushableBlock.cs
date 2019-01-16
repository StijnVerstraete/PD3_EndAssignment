using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBlock : MonoBehaviour {

    [SerializeField] private GameObject _player;
    private float _colliderEnableDelay;
    private void Update()
    {
        //check if over the edge
        if (_player.GetComponent<CharacterControllerBehaviour>().IsPushing)
        {
            if (!Physics.Raycast(transform.GetChild(0).transform.position, Vector3.down, 1f))
            {
                _player.GetComponent<CharacterControllerBehaviour>().IsPushing = false;
                _colliderEnableDelay = 2;
            }
        }

        //enable collider
        if (_colliderEnableDelay > 0)
            _colliderEnableDelay--;
        else if (_colliderEnableDelay <= 0)
            _player.GetComponent<CapsuleCollider>().enabled = true;
    }
    private void FixedUpdate()
    {
        //apply force if pushing
        if (_player.GetComponent<CharacterControllerBehaviour>().IsPushing)
           ApplyForce();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            //either enter or leave pushing state
            if (Input.GetAxis("AButton") != 0)
            {
                if (_player.GetComponent<CharacterControllerBehaviour>().IsPushing == false)
                {
                    CalculateSnappingPoint(other.gameObject.transform.forward, other.transform.position);

                    _player.GetComponent<CharacterControllerBehaviour>().IsPushing = true;
                    //enable IK
                    _player.GetComponent<IKControl>().IKActive = true;
                    //disable collider
                    _player.GetComponent<CapsuleCollider>().enabled = false;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _player.GetComponent<CharacterControllerBehaviour>().IsPushing = false;
            _colliderEnableDelay = 2;
            //disable IK
            _player.GetComponent<IKControl>().IKActive = false;
        }
    }
    private void CalculateSnappingPoint(Vector3 forwardWall, Vector3 col)
    {
        #region offsetvalues
        //tweak offset values based on character model
        float xzOffset = 0.35f;
        #endregion offsetvalues
        if (forwardWall.x != 0)
            _player.transform.position = new Vector3(col.x + (xzOffset * -transform.forward.x), _player.transform.position.y, _player.transform.position.z);
        else if (forwardWall.z != 0)
            _player.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, col.z + (xzOffset * -transform.forward.z));

        //set correct rotation
        Vector3 newRot = new Vector3(_player.transform.eulerAngles.x, transform.eulerAngles.y, _player.transform.eulerAngles.z);
        _player.transform.rotation = Quaternion.Euler(newRot);
    }
    private void ApplyForce()
    {
        float forceToApply = 10;

        if (Input.GetAxis("Vertical") > 0)
            GetComponent<Rigidbody>().AddForce(_player.transform.forward * forceToApply,ForceMode.Acceleration);
    }
}