using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            for(int i = 0; i < transform.childCount - 1; i++)
            {
                Gizmos.DrawSphere(transform.GetChild(i).position, 0.5f);
                Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
            }
                Gizmos.DrawSphere(transform.GetChild(transform.childCount - 1).position, 0.5f);
            Gizmos.DrawLine(transform.GetChild(0).position, transform.GetChild(transform.childCount - 1).position);
        }
    }
}
