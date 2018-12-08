using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    [SerializeField] private Transform _cameraPivot;
    [SerializeField] private float _cameraSensitivity;

    [SerializeField] private Vector2 _yLimit = new Vector2(); //camera limit

    [SerializeField] private CharacterControllerBehaviour _charCTRLBehavior;

    [SerializeField] private GameObject _character;

    private RaycastHit _hit;

    private float _yRotation;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //calculate rotation
        transform.Rotate(new Vector3(0, Input.GetAxis("JoystickRightX"), 0) * _cameraSensitivity * Time.deltaTime);
        _yRotation += Input.GetAxis("JoystickRightY") * _cameraSensitivity/2 * Time.deltaTime;

        //clamp y rotation
        _yRotation = Mathf.Clamp(_yRotation, _yLimit.x, _yLimit.y);

        //implement y rotation
        _cameraPivot.eulerAngles = new Vector3(_yRotation, _cameraPivot.eulerAngles.y, _cameraPivot.eulerAngles.z);

        //camera position
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(_character.transform.position.x, _character.transform.position.y + 1f, _character.transform.position.z), Time.deltaTime * _cameraSensitivity);

        SetCharacterTurnTarget();
    }
    private void SetCharacterTurnTarget()
    {
        if (Input.GetAxis("Vertical") > 0.25f)
        {
            _charCTRLBehavior.Turn(Quaternion.Euler(new Vector3(0, transform.rotation.eulerAngles.y, 0)));
        }
    }
}