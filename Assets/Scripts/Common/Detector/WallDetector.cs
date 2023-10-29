using System;
using UnityEngine;

namespace Common.Detector
{
    public class WallDetector : MonoBehaviour, IDetector
    {
        [SerializeField]
        public bool debugger = true;
        [SerializeField]
        public LayerMask detectLayer;
        [SerializeField]
        public Rect detectBox;
        [SerializeField]
        public Color detectColor;
        
        
        public bool IsByWall { get; private set; }

        public bool Detect()
        {

            return false;
        }

        public void DrawGizmos()
        {
            Gizmos.color = detectColor;
        }


        private void FixedUpdate()
        {
            IsByWall = Detect();
        }

        private void OnDrawGizmos()
        {
            if (debugger)
            {
                DrawGizmos();
            }
        }
    }
}