using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    private float _bulletSpeed = 5f;
    [SerializeField] private Vector3 _target;
	// Update is called once per frame
	void Update ()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _target, _bulletSpeed * Time.deltaTime);
        }
        if (transform.position == _target)
        {
            Destroy(gameObject);
        }
	}
    public void SetTarget(Vector3 target)
    {
        _target = target;
    }
}
