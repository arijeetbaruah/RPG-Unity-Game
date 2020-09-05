using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Resources;
using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using RPG.Input;
using UnityEngine.InputSystem;

namespace RPG.Control
{
    [RequireComponent(typeof(Mover), typeof(Figther))]
    public class PlayerController : MonoBehaviour
    {
        Mover mover;
        Figther figther;

        [SerializeField] float maxNavMeshProjectionDistance = 1f;
        [SerializeField] float maxNavMeshPathDistance = 40f;

        [Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;

        void Awake()
        {
            mover = GetComponent<Mover>();
            figther = GetComponent<Figther>();
        }

        void Update()
        {
            InteractionSystem();
        }

        bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (hasHit)
            {
                if (Mouse.current.leftButton.isPressed)
                {
                    mover.StartMoveAction(target, 1);
                }
                SetCursor(CursorType.Movement);
                return true;
            }

            return false;
        }

        private Ray GetMouseRay()
        {
            Vector2 mousePos = GetComponent<InputManager>().mousePosition;
            return Camera.main.ScreenPointToRay(new Vector3(mousePos.x, mousePos.y, 0));
        }

        private void InteractionSystem()
        {
            if (InteractWithUI()) { 
                SetCursor(CursorType.UI); return; 
            };
            if (!GetComponent<Health>().IsDead)
            {
                if (InteractWithComponent()) return;
                if (InteractWithCombat()) return;
                if (InteractWithMovement()) return;
            }
            SetCursor(CursorType.None);
        }

        bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();
            
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit)
            {
                return false;
            }
            NavMeshHit navMeshHit;

            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;

            if (GetPathLength(path) > maxNavMeshPathDistance) return false;

            return true;
        }

        private float GetPathLength(NavMeshPath path)
        {
            Vector3[] corners = path.corners;
            float distance = 0;

            for(int i = 0; i < corners.Length-1; i++)
            {
                distance += Vector3.Distance(corners[i], corners[i + 1]);
            }

            return distance;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = GetRaycastHitSorted();
            foreach(RaycastHit hit in hits)
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach(IRaycastable raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private bool InteractWithUI()
        {
            return EventSystem.current.IsPointerOverGameObject();
        }

        bool InteractWithCombat()
        {
            RaycastHit[] hits = GetRaycastHitSorted();

            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] targets = hit.transform.GetComponents<IRaycastable>();

                foreach (IRaycastable target in targets)
                {
                    if (target.HandleRaycast(this))
                    {
                        SetCursor(target.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] GetRaycastHitSorted()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            float[] distances = new float[hits.Length];
            for(int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);

            return hits;
        }

        Dictionary<CursorType, CursorMapping> mappings = null;

        private CursorMapping GetCursorMapping(CursorType type)
        {
            if (mappings == null)
            {
                mappings = new Dictionary<CursorType, CursorMapping>();
                foreach (CursorMapping mapping in cursorMappings)
                {
                    mappings[mapping.type] = mapping;
                }
            }            

            return mappings[type];
        }

        public void SetCursor(CursorType type)
        {
            CursorMapping cursorMapping = GetCursorMapping(type);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }
    }
}

