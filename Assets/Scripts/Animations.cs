﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour {

    [SerializeField] private Animator _animator;
    [SerializeField] CharacterControllerBehaviour _characterControlScript;
    [SerializeField] private GameObject _player;

    // Update is called once per frame
    void Update()
    {
        SetBools();
        SetVelocityFloats();

    }
    private void SetBools()
    {
        //falling
        _animator.SetBool("IsFalling", !_player.GetComponent<CharacterController>().isGrounded);

        //hanging
        _animator.SetBool("IsHanging", _characterControlScript.IsHanging);

        //climbing
        _animator.SetBool("IsClimbing", _characterControlScript.IsClimbing);

        //pushing
        _animator.SetBool("IsPushing", _characterControlScript.IsPushing);

        //picking up
        _animator.SetBool("IsPickingUp", _characterControlScript.IsPickingUp);

        //aiming
        _animator.SetBool("IsAiming", _characterControlScript.IsAiming);

        //dying
        _animator.SetBool("IsDead", _characterControlScript.IsDead);
    }
    private void SetVelocityFloats()
    {
        //adjust velocity to use in animation
        Vector3 XZvel = Vector3.Scale(_characterControlScript.Velocity, new Vector3(1, 0, 1));
        Vector3 localVelXZ = _characterControlScript.gameObject.transform.InverseTransformDirection(XZvel);
        localVelXZ.Normalize();

        if (!_characterControlScript.IsSprinting && !_characterControlScript.IsAiming)
        {
            //divide by 2 when not sprinting, to limit animation on blend tree
            _animator.SetFloat("HorizontalVelocity", localVelXZ.x / 2);
            _animator.SetFloat("VerticalVelocity", localVelXZ.z / 2);
        }
        else
        {
            //use regular values when not sprinting
            _animator.SetFloat("HorizontalVelocity", localVelXZ.x);
            _animator.SetFloat("VerticalVelocity", localVelXZ.z);
        }
    }
}
