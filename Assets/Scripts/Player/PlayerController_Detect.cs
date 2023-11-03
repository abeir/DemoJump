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

        public bool IsOnOneWayPlatform => _groundDetector.IsOnOneWayPlatform;

        public bool IsTouchLedge => _ledgeDetector.IsTouchLedge;
        /// <summary>
        /// 触碰边缘的方向，0表示未碰到边缘，1为右侧碰到，-1为左侧碰到
        /// </summary>
        public int TouchLedgeDirection => _ledgeDetector.TouchLedgeDirection;
        public Vector2 TouchLedgeVerticalPoint => _ledgeDetector.TouchVerticalPoint;
        public Vector2 TouchLedgeHorizontalPoint => _ledgeDetector.TouchHorizontalPoint;



        private GroundDetector _groundDetector;
        private LedgeDetector _ledgeDetector;
        private WallDetector _wallDetector;

        public Rect GroundDetectorBox => _groundDetector.box;

        private void FindDetectorComponents()
        {
            _groundDetector = detectorGameObject.GetComponent<GroundDetector>();
            _ledgeDetector = detectorGameObject.GetComponent<LedgeDetector>();
            _wallDetector = detectorGameObject.GetComponent<WallDetector>();
        }

        public void PauseDetectGround(float t = Mathf.Infinity)
        {
            _groundDetector.Pause(t);
        }

        public void ResumeDetectGround()
        {
            _groundDetector.Resume();
        }

        public void PauseDetectLedge(float t = Mathf.Infinity)
        {
            _ledgeDetector.Pause(t);
        }

        public void ResumeDetectLedge()
        {
            _ledgeDetector.Resume();
        }

        public void PauseDetectWall(float t = Mathf.Infinity)
        {
            _wallDetector.Pause(t);
        }

        public void ResumeDetectWall()
        {
            _wallDetector.Resume();
        }
    }
}