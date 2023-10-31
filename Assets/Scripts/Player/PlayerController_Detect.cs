using Common.Detector;
using UnityEngine;


namespace Player
{
    public partial class PlayerController
    {

        public bool IsVelocityYDown => Rigidbody.velocity.y <= 0;

        public bool IsOnGround => _groundDetector.IsOnGround;
        public bool IsOnSlope { get; private set; }
        public bool IsOnAir => !IsOnGround && !IsOnSlope;

        public bool IsTouchLedge => _ledgeDetector.IsTouchLedge;
        public int TouchLedgeDirection => _ledgeDetector.TouchLedgeDirection;
        public Vector2 TouchLedgeVerticalPoint => _ledgeDetector.TouchVerticalPoint;
        public Vector2 TouchLedgeHorizontalPoint => _ledgeDetector.TouchHorizontalPoint;



        private GroundDetector _groundDetector;
        private LedgeDetector _ledgeDetector;

        private void FindDetectorComponents()
        {
            _groundDetector = detectorGameObject.GetComponent<GroundDetector>();
            _ledgeDetector = detectorGameObject.GetComponent<LedgeDetector>();
        }
    }
}