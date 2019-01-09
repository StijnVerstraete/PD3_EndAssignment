using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAnimations : MonoBehaviour {

    [SerializeField] private AIBehaviour _aiScript;
    [SerializeField] private Animator _animator;
	
	// Update is called once per frame
	void Update ()
    {
        //adjust velocity to use in animation
        Vector3 XZvel = Vector3.Scale(_aiScript.gameObject.GetComponent<NavMeshAgent>().velocity, new Vector3(1, 0, 1));
        Vector3 localVelXZ = _aiScript.gameObject.transform.InverseTransformDirection(XZvel);
        localVelXZ.Normalize();

        //divide by 2 when not sprinting, to limit animation on blend tree
        _animator.SetFloat("HorizontalVelocity", localVelXZ.x );
        _animator.SetFloat("VerticalVelocity", localVelXZ.z );

        //check if AI is aiming
        _animator.SetBool("IsAiming", _aiScript.IsAiming);
    }
}
