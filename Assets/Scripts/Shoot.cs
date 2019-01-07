using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _gunBarrel;

    [SerializeField] private CharacterControllerBehaviour _charCTRL;

    private GameObject _lastBulletFired;
    private Ray _screenCentre;

    private bool _canShoot; //ensure only one bullet gets fired per shot

    RaycastHit _hit;

    [SerializeField] LayerMask _layerMask;

    void Update ()
    {
        _screenCentre = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Input.GetAxis("Shoot")>0.1f && _charCTRL.IsAiming && _canShoot)
        {
            if (Physics.Raycast(_screenCentre, out _hit,250, _layerMask))
            {
                _lastBulletFired = Instantiate(_bullet, _gunBarrel.transform.position, _gunBarrel.transform.rotation);
                _lastBulletFired.GetComponent<Bullet>().SetTarget(_hit.point);
                Debug.Log("Shoot");
                _canShoot = false;
            }
        }
        if (Input.GetAxis("Shoot") == 0f)
            _canShoot = true;
        Debug.DrawRay(_screenCentre.origin, _screenCentre.direction);
    }
}
