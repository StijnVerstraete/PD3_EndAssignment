﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {


    [SerializeField] private CharacterControllerBehaviour _charCTRL;

    [SerializeField] private GameObject _muzzleFlash;

    private GameObject _lastBulletFired;
    private Ray _screenCentre;

    private bool _canShoot; //ensure only one bullet gets fired per shot
    private int _flashDuration = 5;

    RaycastHit _hit;

    [SerializeField] LayerMask _layerMask;

    void Update ()
    {
        _screenCentre = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Input.GetAxis("Shoot")>0.1f && _charCTRL.IsAiming && _canShoot)
            if (Physics.Raycast(_screenCentre, out _hit,250, _layerMask))
            {
                _muzzleFlash.SetActive(true);
                _canShoot = false;
                _flashDuration = 5;
                if (_hit.collider.gameObject.tag == "Enemy")
                {
                    _hit.collider.gameObject.GetComponent<AIBehaviour>().Health -= 1;
                    //play hit animation
                    if (_hit.collider.gameObject.GetComponent<AIBehaviour>().Health > 0 )
                        _hit.collider.gameObject.GetComponent<Animator>().SetTrigger("IsHit");
                }
            }
        ShootDelays();
    }
    private void ShootDelays()
    {
        //make flash go away
        _flashDuration--;
        if (_flashDuration <= 0)
            _muzzleFlash.SetActive(false);
        //enable shooting when trigger is no longer pressed
        if (Input.GetAxis("Shoot") == 0f)
            _canShoot = true;
    }
}
