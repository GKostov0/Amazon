using System;
using System.Collections.Generic;
using UnityEngine;

namespace AMAZON.Control
{
    public class PatrolPath : MonoBehaviour
    {
        [SerializeField] private Color _firstNodeColor;
        [SerializeField] private Color _pathColor;
        [SerializeField][Range(0.0f, 1.0f)] private float _pathSize;

        private int _index = 0;

        private void OnDrawGizmos() 
        {
            foreach (Transform child in transform)
            {
                Gizmos.color = _index > 0 ? _pathColor :_firstNodeColor;
                _index++;

                Gizmos.DrawSphere(child.position, _pathSize);
                Gizmos.DrawLine(child.position, GetNextWaypoint(child));
            }

            _index = 0;
        }

        public Vector3 GetNextWaypoint(Transform child)
        {
            return child.GetSiblingIndex() == transform.childCount - 1 ? transform.GetChild(0).position : transform.GetChild(child.GetSiblingIndex() + 1).position;
        }

        public Vector3 GetWaypoint(int index)
        {
            return transform.GetChild(index).position;
        }
    }
}