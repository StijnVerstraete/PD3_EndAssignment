using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKControl : MonoBehaviour {

    [SerializeField] private Animator _anim;
    [SerializeField] private CharacterControllerBehaviour _charCTRL;

    [SerializeField] private Transform _aimTarget;

    [SerializeField] private Transform _rightHand;
    [SerializeField] private Transform _leftHand;

    public Transform RightHandPosition;
    public bool IKActive;


    private void OnAnimatorIK()
    {
        if (IKActive)
        {
            //gun pickup IK
            if (RightHandPosition != null)
            {
                _anim.SetIKPosition(AvatarIKGoal.RightHand, RightHandPosition.position);
                _anim.SetIKRotation(AvatarIKGoal.RightHand, RightHandPosition.rotation);

                _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
            }

            //gun aim IK
            else if (_charCTRL.IsAiming)
            {
                _anim.SetIKPosition(AvatarIKGoal.RightHand, new Vector3(_rightHand.position.x,_aimTarget.position.y,_rightHand.position.z));
                _anim.SetIKPosition(AvatarIKGoal.LeftHand, new Vector3(_leftHand.position.x,_aimTarget.position.y,_leftHand.position.z));

                _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            }
            else
            {
                _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

                _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }
}
