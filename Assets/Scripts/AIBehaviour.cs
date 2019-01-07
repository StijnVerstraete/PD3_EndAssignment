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
    private float _maxIdleDistance = 5f;

    private bool _playerInTrigger = false;

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
                
        StartCoroutine(RunTree());
    }
    private void Update()
    {
        
    }
    #region BehaviourTreeFunctions
    IEnumerator RunTree()
    {
        while (Application.isPlaying)
        {
            yield return _rootNode.Tick();
        }
    }
    bool CanSee()
    {
        float playerHeight = 0.5f;
        RaycastHit hit;
        if (Physics.Linecast(new Vector3(transform.position.x, transform.position.y + playerHeight, transform.position.z), new Vector3(_player.transform.position.x, _player.transform.position.y + playerHeight, _player.transform.position.z), out hit))
        {
            Debug.DrawLine(new Vector3(transform.position.x, transform.position.y + playerHeight, transform.position.z), new Vector3(_player.transform.position.x, _player.transform.position.y + playerHeight, _player.transform.position.z), Color.red, 20);
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("CanSeePlayer");
                return true;
               
            }
        }
        Debug.Log("cannotseeplayer");
        return false;
    }
    bool InCover()
    {
        return false;
    }
    IEnumerator<NodeResult> SearchCover()
    {
        yield return NodeResult.Success;
    }
    IEnumerator<NodeResult> Shoot()
    {
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
                Vector3 newPos = Random.insideUnitSphere * _maxIdleDistance;
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

}