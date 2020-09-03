using UnityEngine;

namespace RPG.Core
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        void LateUpdate()
        {
            transform.position = target.position;
        }
    }
}
