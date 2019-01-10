using System.Collections;
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
        {
            if (Physics.Raycast(_screenCentre, out _hit,250, _layerMask))
            {
                Debug.DrawRay(_screenCentre.origin, _screenCentre.direction, Color.cyan, 1);
                Debug.Log("Shoot");
                _muzzleFlash.SetActive(true);
                _canShoot = false;
                _flashDuration = 5;
                if (_hit.collider.gameObject.tag == "Enemy")
                {
                    Debug.Log("EnemyHit");
                }
            }
        }   
        if (Input.GetAxis("Shoot") == 0f)
            _canShoot = true;


        //make flash go away
        _flashDuration--;
        if (_flashDuration <=0)
        {
            _muzzleFlash.SetActive(false);
        }
    }
}
