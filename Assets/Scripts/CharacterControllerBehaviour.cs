using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Assertions;

public class CharacterControllerBehaviour : MonoBehaviour {

    [SerializeField] private CharacterController _charCTRL;
    [SerializeField] private IKControl _iKControl;

    [SerializeField] private Transform _absoluteForward; //this is the camera in most cases
    [SerializeField] private Transform _cameraPivot;
    private Vector3 _xzAbsoluteForward;

    private Vector3 _movement; //movement input
    public Vector3 Velocity = Vector3.zero; // [m/s]

    private float _mass = 80; // [kg[
    private float _accelerationJogging = 1.5f; // [m/s²]
    private float _accelerationSprinting = 3;// [m/s²]
    private float _accelerationAiming = 0.75f;// [m/s²]
    private float _rotationSpeed = 8;

    public int Health = 5;

    public bool IsDead = false;
    public bool IsSprinting;
    public bool IsHanging;
    public bool IsClimbing;
    public bool IsPushing; //pushing block
    public bool IsPickingUp;
    public bool IsAiming;

    [SerializeField] private Vector3 _finalMovement;

    public Vector3 CurrentHangLocation;

    //gun
    public GameObject Gun;
    [SerializeField] private Transform _rightHand;
    
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

        //check if player is sprinting or aiming
        SprintInput();
        AimInput();

        ApplyHanging();

        //die if health is too low
        if (Health <= 0)
        {
            IsDead = true;
            GetComponent<CharacterControllerBehaviour>().enabled = false;
        }

    }
    private void FixedUpdate()
    {  
        if (!IsClimbing && !IsHanging)
        {
            ApplyGround();
            ApplyGravity();
        }
        //move
        if (_charCTRL.enabled)
        {      
            ApplyMovement();
            PushState();
            _charCTRL.Move(Velocity * Time.fixedDeltaTime);
        }
    }
    private void ApplyGravity()
    {
        //apply gravity
        Velocity += Physics.gravity * Time.fixedDeltaTime; //g[m / s²] * t[s]
    }
    private void ApplyGround()
    {
        if (_charCTRL.isGrounded)
        {
            //ground velocity
            Velocity -= Vector3.Project(Velocity, Physics.gravity.normalized);
        }
    }
    private void ApplyMovement()
    {
        //only apply movement when character is on the ground
        if ((_charCTRL.isGrounded||IsPushing) && !IsHanging && !IsClimbing && !IsPickingUp)
        {
            //take x and z components from the absolute forward
            _xzAbsoluteForward = Vector3.Scale(_absoluteForward.forward,new Vector3(1,0,1));
            //set direction in which the player looks

            Quaternion forwardRotation = Quaternion.LookRotation(_xzAbsoluteForward, Vector3.up);

            //set actual movement direction
            _finalMovement = forwardRotation * _movement;
            _finalMovement.y = Velocity.y;

            //move (acceleration depending on jogging, aiming or sprinting
            if (IsAiming)
                Velocity = _finalMovement * (_mass * _accelerationAiming) * Time.fixedDeltaTime;
            else if (!IsSprinting)
                Velocity = _finalMovement * (_mass * _accelerationJogging) * Time.fixedDeltaTime; // F = m.a [m/s²] * t [s]
            else if (IsSprinting)
                Velocity = _finalMovement * (_mass * _accelerationSprinting) * Time.fixedDeltaTime; // F = m.a [m/s²] * t [s)]
        }
         
    }
    private void SprintInput()
    {
        if (Input.GetAxis("Sprint") > 0 && !IsAiming)
        {
            IsSprinting = true;
        }
        else if (Input.GetAxis("Sprint") <= 0)
        {
            IsSprinting = false;
        }
    }
    private void AimInput()
    {
        if (Input.GetButton("RightJoystickClick") && Gun != null)
        {
            IsAiming = true;
            Debug.Log("aim");
        }
        else
        {
            IsAiming = false;
        }
    }
    private void ApplyHanging()
    {
        if (IsHanging)
        {
            _charCTRL.enabled = false;
            if (Input.GetAxis("Vertical") > 0.5f)
            {
                IsHanging = false;
                IsClimbing = true;
            }
        }
    }
    private void PushState()
    {
        if (IsPushing)
        {
            //avoid walking sideways while pushing
            Velocity = Vector3.Scale(Velocity, transform.forward);
            //avoid walking backwards while pushing

        }
    }
    public void Turn(Quaternion target)
    {
        if (!IsPushing)
            transform.rotation = Quaternion.Slerp(transform.rotation, target, _rotationSpeed * Time.fixedDeltaTime);
    }
    //animationevents
    public void FinishClimbing()
    {
        IsClimbing = false;
        transform.position = new Vector3(transform.position.x + (transform.forward.x/2), CurrentHangLocation.y, transform.position.z + (transform.forward.z/2));
        _charCTRL.enabled = true;
        Debug.Log("EventTrigger");
    }
    public void FinishPickingUp()
    {
        IsPickingUp = false;
    }
    public void GunPickedUp()
    {
        Debug.Log("GunPickedUp");
        Gun.transform.parent = _rightHand;
        Gun.transform.position = _rightHand.transform.position;

        //stop gun picking up IK
        _iKControl.RightHandPosition = null;
    }
}
