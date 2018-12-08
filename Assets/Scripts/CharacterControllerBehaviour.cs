using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Assertions;

public class CharacterControllerBehaviour : MonoBehaviour {

    [SerializeField] private CharacterController _charCTRL;

    [SerializeField] private Transform _absoluteForward; //this is the camera in most cases
    [SerializeField] private Transform _cameraPivot;
    private Vector3 _xzAbsoluteForward;

    private Vector3 _movement; //movement input
    public Vector3 Velocity = Vector3.zero; // [m/s]

    private float _mass = 80; // [kg[
    private float _accelerationJogging = 1.5f; // [m/s²]
    private float _accelerationSprinting = 3;// [m/s²]
    private float _rotationSpeed = 8;

    private bool IsSprinting;

    private Vector3 _finalMovement;


    void Start ()

    {
#if DEBUG
        //check if absolute forward and character controller are given
        Assert.IsNotNull(_charCTRL, "Dependency Error: This component needs a CharachterController to work.");
        Assert.IsNotNull(_absoluteForward, "Dependency Error: Set the Absolute Forward field.");
#endif

    }

    void Update ()
    {
        //get input
        _movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Debug.Log(IsSprinting);

        //check if player is sprinting
        SprintInput();

	}
    private void FixedUpdate()
    {
        ApplyGravity();

        //move
        ApplyMovement();
        _charCTRL.Move(Velocity * Time.fixedDeltaTime);
    }
    private void ApplyGravity()
    {
        //apply gravity when the characterController is not grounded
        if (!_charCTRL.isGrounded)
        {
            Velocity += Physics.gravity * Time.deltaTime; //g[m / s²] * t[s]
        }
    }
    private void ApplyMovement()
    {
        //only apply movement when character is on the ground
        if (_charCTRL.isGrounded)
        {
            //take x and z components from the absolute forward
            _xzAbsoluteForward = new Vector3(_absoluteForward.forward.x, 0, _absoluteForward.forward.z);
            //set direction in which the player looks
            Quaternion forwardRotation = Quaternion.LookRotation(_xzAbsoluteForward, Vector3.up);

            //set actual movement direction
            _finalMovement = forwardRotation * _movement;

            //move (acceleration depending on jogging or sprinting
            if (!IsSprinting)
                Velocity = _finalMovement * (_mass * _accelerationJogging) * Time.fixedDeltaTime; // F = m.a [m/s²] * t [s]
            else if (IsSprinting)
                Velocity = _finalMovement * (_mass * _accelerationSprinting) * Time.fixedDeltaTime; // F = m.a [m/s²] * t [s)]
        }
         
    }
    private void SprintInput()
    {
        if (Input.GetAxis("Sprint") > 0)
        {
            IsSprinting = true;
        }
        else if (Input.GetAxis("Sprint") <= 0)
        {
            IsSprinting = false;
        }
    }
    public void Turn(Quaternion target)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, target, _rotationSpeed * Time.fixedDeltaTime);
    }
}
