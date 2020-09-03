using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using RPG.Movement;
using RPG.Combat;
using System;
using RPG.Resources;
using GameDevTV.Utils;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover), typeof(Figther))]
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspisionTime = 3f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTollorence = 1f;
        [SerializeField] float dwellTime = 3f;
        [Range(0,1)]
        [SerializeField] float patrolSpeedFraction = 0.2f;

        Mover mover;
        Figther figther;
        Health health;
        Core.ActionScheduler actionScheduler;

        LazyValue<Vector3> guardLocation;

        float timeLastSawPlayer = Mathf.Infinity;
        float timeSinceStartedDwelling = Mathf.Infinity;
        int currentWayPoint = 0;

        void Awake()
        {
            mover = GetComponent<Mover>();
            figther = GetComponent<Figther>();
            health = GetComponent<Health>();
            actionScheduler = GetComponent<Core.ActionScheduler>();

            guardLocation = new LazyValue<Vector3>(GetPosition);
        }

        private void Start()
        {
            guardLocation.ForceInit();
        }

        Vector3 GetPosition()
        {
            return transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (health.IsDead)
            {
                actionScheduler.CancelCurrentAction();
                return;
            };

            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (Vector3.Distance(player.transform.position, transform.position) <= chaseDistance)
            {
                if (!figther.CanAttack(player.gameObject)) return;

                figther.Attack(player.gameObject);
                timeLastSawPlayer = 0f;
            }
            else if (timeLastSawPlayer <= suspisionTime)
            {
                actionScheduler.CancelCurrentAction();
            }
            else if (timeSinceStartedDwelling <= dwellTime)
            {
                actionScheduler.CancelCurrentAction();
                timeSinceStartedDwelling += Time.deltaTime;
            }
            else
            {
                PatrolBehavior();
            }

            timeLastSawPlayer += Time.deltaTime;
        }

        private void PatrolBehavior()
        {
            Vector3 nextPosition = guardLocation.value;

            if (patrolPath != null)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            mover.StartMoveAction(nextPosition, patrolSpeedFraction);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());

            return distanceToWaypoint < waypointTollorence;
        }

        private void CycleWaypoint()
        {
            timeSinceStartedDwelling = 0f;
            currentWayPoint = getNextIndex(currentWayPoint);
        }

        int getNextIndex(int i)
        {
            if (i == patrolPath.transform.childCount - 1)
                return 0;
            return i + 1;
        }

        private Vector3 GetCurrentWaypoint()
        {
            return GetWaypoint(currentWayPoint);
        }

        private Vector3 GetWaypoint(int i)
        {
            return patrolPath.transform.GetChild(i).position;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
