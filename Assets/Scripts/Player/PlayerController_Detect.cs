using Common.Detector;
using Player.FSM;
using UnityEngine;


namespace Player
{
    public partial class PlayerController
    {


        public bool IsVelocityYDown => Rigidbody.velocity.y <= 0;

        public bool IsOnGround => _groundDetector.IsOnGround;
        public bool IsOnSlope { get; private set; }
        public bool IsOnAir => !IsOnGround && !IsOnSlope;
        public bool IsHang { get; private set; }
        public Vector2 HangLowestPoint { get; private set; } // 悬挂检测的最低点，需要注意，使用此属性的前提必须先判断IsHang为true，因为此值会被缓存，未悬挂时会获取到上一次的值
        public Vector2 HangGroundPoint { get; private set; }    // 悬挂检测的地面表面的点，注意项同 HangLowestPoint




        private GroundDetector _groundDetector;

        private void FindDetectorComponents()
        {
            _groundDetector = detectorGameObject.GetComponent<GroundDetector>();
        }
    }
}