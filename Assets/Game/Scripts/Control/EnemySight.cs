using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySight : MonoBehaviour
{
    [SerializeField] float FOVAngle = 110;
    public bool playerInSight;
    public Vector3 personalLastSighting;

    NavMeshAgent navMesh;
    SphereCollider collider;
    GameObject player;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
        collider = GetComponent<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        
    }
}
