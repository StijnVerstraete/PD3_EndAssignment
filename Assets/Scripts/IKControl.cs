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
            #region gunrelated
            //gun pickup IK
            if (RightHandPosition != null)
            {
                SetGunIK();
            }
            //gun aim IK
            else if (_charCTRL.IsAiming)
            {
                SetAimIK();
            }
            #endregion gunrelated
            #region blockrelated
            else if (_charCTRL.IsPushing)
            {
                //set hand positions
                SetPushingIK(_charCTRL.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.LeftShoulder),AvatarIKGoal.LeftHand);
                SetPushingIK(_charCTRL.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.RightShoulder),AvatarIKGoal.RightHand);
            }
            #endregion blockrelated
            else
            {
                SetDefaultIK();
            }   
        }
    }
    private void SetPushingIK(Transform Shoulder, AvatarIKGoal ikGoal)
    {
        RaycastHit hit;
        Debug.Log("SetPushingIK");
        if (Physics.Raycast(Shoulder.position,_charCTRL.transform.forward, out hit, 1 << 13))
        {
            _anim.SetIKPosition(ikGoal, hit.point);
            _anim.SetIKPositionWeight(ikGoal, 1);
        }
    }
    private void SetGunIK()
    {
        _anim.SetIKPosition(AvatarIKGoal.RightHand, RightHandPosition.position);
        _anim.SetIKRotation(AvatarIKGoal.RightHand, RightHandPosition.rotation);

        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
    }
    private void SetAimIK()
    {
        _anim.SetIKPosition(AvatarIKGoal.RightHand, new Vector3(_rightHand.position.x, _aimTarget.position.y, _rightHand.position.z));
        _anim.SetIKPosition(AvatarIKGoal.LeftHand, new Vector3(_leftHand.position.x, _aimTarget.position.y, _leftHand.position.z));

        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
    }
    private void SetDefaultIK()
    {
        _anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
        _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);

        _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
    }
}
