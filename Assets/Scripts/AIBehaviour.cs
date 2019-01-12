using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class AIBehaviour : MonoBehaviour {

    private INode _rootNode;

    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private GameObject _player;

    [SerializeField] private AIGlobalBehaviour _aiGlobal;

    [SerializeField] private GameObject _muzzleFlash;
    private int _flashDuration = 5;

    private float _shootDelay = 1.5f;
    private float _shootDelayLength;
    private float _chanceToHit = 20; //percentage

    [SerializeField] private List<GameObject> _potentialCovers = new List<GameObject>();
    private float _maxIdleDistance = 4f;

    private bool _playerInTrigger = false;

    private bool _inCover = false;

    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private GameObject _gunBarrel;

    float _shortestDistanceToCover = 10000;
    GameObject _nearestCover;
    RaycastHit _hit;

    public int Health = 3;

    public bool IsAiming;
    public bool IsDead = false;

    public bool IsCrouching = false;

    void Start()
    {
        _rootNode =
            new SelectorNode(
                new SequenceNode(
                    new ConditionNode(CanSee),
                    new SelectorNode(
                            new ConditionNode(InCover),
                            new ActionNode(SearchCover)),
                    new ActionNode(Shoot)),
                new ActionNode(Idle));

        //set _shootDelayLength to the correct _shootDelay
        _shootDelayLength = _shootDelay;

        StartCoroutine(RunTree());
    }
    private void Update()
    {
        //hide muzzleflash (in update so that it always happens)
        _flashDuration--;
        if (_flashDuration <= 0)
        {
            _muzzleFlash.SetActive(false);
        }
        //die if health is too low
        if (Health <=0)
        {
            IsDead = true;
            _agent.enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
    }
    #region BehaviourTreeFunctions
    IEnumerator RunTree()
    {
        while (Application.isPlaying && !IsDead)
        {
            yield return _rootNode.Tick();
        }
    }
    bool CanSee()
    {
        float playerHeight = 0.5f;
        RaycastHit hit;
        if (!_aiGlobal.AIAlerted)
        {
            if (Physics.Linecast(new Vector3(transform.position.x, transform.position.y + playerHeight, transform.position.z), new Vector3(_player.transform.position.x, _player.transform.position.y + playerHeight, _player.transform.position.z), out hit))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    _aiGlobal.AIAlerted = true;
                    return true;
                }
            }
        }
        if (_aiGlobal.AIAlerted)
            return true;

        return false;
    }
    bool InCover()
    {
        RaycastHit hit;
        transform.LookAt(_player.transform, Vector3.up);
        //check if current cover is valid
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5,_layerMask)) 
        {
            Debug.Log(hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag != "Cover")
            {
                _inCover = false;
            }
            else
            {
                _inCover = true;
            }
            Debug.DrawRay(transform.position, transform.forward, Color.red, 10);
        }
        else
        {
            _inCover = false;
        }
        Debug.Log("InCover" + _inCover);
        return _inCover;
    }
    IEnumerator<NodeResult> SearchCover()
    {
        FindNewCover();
        
        yield return NodeResult.Failure;
    }
    IEnumerator<NodeResult> Shoot()
    {
        IsAiming = true;

        _shootDelay -= Time.fixedDeltaTime;
        //shoot ocassionaly
        if (_shootDelay<=0)
        {
            _shootDelay = _shootDelayLength;
            _muzzleFlash.SetActive(true);
            FireBullet();
        }
        yield return NodeResult.Success;
    }
    IEnumerator<NodeResult> Idle()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            //chance of picking a new position
            float nextDestinationChance = Random.Range(0, 100);

            if (nextDestinationChance >= 99)
            {
                //find new position to go to
                Vector3 newPos = transform.position + Random.insideUnitSphere * _maxIdleDistance;
                Debug.DrawLine(newPos, Vector3.up, Color.red, 5);
                NavMeshHit hit;
                NavMesh.SamplePosition(newPos, out hit, Random.Range(0, _maxIdleDistance),1);

                //send agent to new position
                _agent.SetDestination(hit.position);
            }
        }
        yield return NodeResult.Success;
    }
    #endregion BehaviourTreeFunctions
    private void OnTriggerEnter(Collider other)
    {
        //check if player enters possible viewrange of Ai
        if (other.tag == "Player")
        {
            _playerInTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //check if player exits possible viewrange of Ai
        if (other.tag == "Player")
        {
            _playerInTrigger = false;
        }
    }
    private void FindNewCover()
    {
            //disable aiming
            IsAiming = false;
            //get nearest cover
            Debug.Log("FindingCover");
            Vector3 destinationTarget = transform.position;
            foreach (GameObject cover in _potentialCovers)
            {
                if (_shortestDistanceToCover > Vector3.Distance(transform.position, cover.transform.position))
                {
                    _shortestDistanceToCover = Vector3.Distance(transform.position, cover.transform.position);
                    _nearestCover = cover;
                }
            }
            float offsetFromCover = 1.5f;
            
            if (Physics.Linecast(new Vector3(_player.transform.position.x,transform.position.y,_player.transform.position.z), _nearestCover.transform.position,out _hit,1<<10))
            {
                destinationTarget = new Vector3(_nearestCover.transform.position.x + (offsetFromCover * -_hit.normal.x), transform.position.y, _nearestCover.transform.position.z + (offsetFromCover * -_hit.normal.z));
                Debug.Log("Recalculate cover position");
            }
            Debug.DrawRay(destinationTarget, Vector3.up, Color.blue, 5);
            _agent.destination = destinationTarget;
    }
    private void FireBullet()
    {
        RaycastHit bulletHit;
        if (Physics.Linecast(_gunBarrel.transform.position,new Vector3(_player.transform.position.x,_player.transform.position.y + 1.2f,_player.transform.position.z),out bulletHit))
        {
            Debug.DrawLine(_gunBarrel.transform.position, new Vector3(_player.transform.position.x, _player.transform.position.y + 1.2f, _player.transform.position.z), Color.green, 1);
            //instantiate bullet
            //set bullet target depending on chance to hit
            if (_chanceToHit <= Random.Range(0,100) && bulletHit.collider.gameObject.tag == "Player")
            {
                _player.GetComponent<CharacterControllerBehaviour>().Health -= 1;
                //play hit animation
                if (_player.GetComponent<CharacterControllerBehaviour>().Health > 0)
                {
                    _player.GetComponent<Animator>().SetTrigger("IsHit");
                }
            }
            Debug.Log("AIShoots");
        }
    }

}