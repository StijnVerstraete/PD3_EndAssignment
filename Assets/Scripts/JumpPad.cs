using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour {

    //implementation details: design doc page 6
    [SerializeField] private GameObject _player;
    private float _jumpHeight = 4;

    private void OnTriggerEnter(Collider other)
    {
        //make player jump
        _player.GetComponent<CharacterControllerBehaviour>().Velocity.y += Mathf.Sqrt(2 * Physics.gravity.magnitude * _jumpHeight);
        _player.transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y + 0.3f, _player.transform.position.z);
    }
}
