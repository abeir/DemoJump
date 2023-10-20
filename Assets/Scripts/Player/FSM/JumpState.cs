using Common.Helper;
using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class JumpState : AStateBase
    {
        public static readonly int JumpHash = Animator.StringToHash("Jump");
        public static readonly int JumpCountHash = Animator.StringToHash("JumpCount");

        private Vector2 _velocity = Vector2.zero;
        
        public override StateDefine State { get; } = new StateDefine
        {
            ID = (int)PlayerStateID.Jump,
            Name = "Jump"
        };


        public JumpState(PlayerController ctrl) : base(ctrl)
        {
        }
        
        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.PlayerDetector.IsOnGround || PlayerController.JumpCount < 2;
        }

        public override void OnEnter(StateDefine pre)
        {
            Debug.Log($">>> OnEnter JumpState  pre:{pre.Name}");
            
            PlayerController.JumpCount = Mathf.Clamp(PlayerController.JumpCount + 1, 0, 2);
            
            PlayerController.Rigidbody.AddForce(new Vector2(PlayerController.MoveDirection.x, PlayerController.jumpForce), ForceMode2D.Impulse);


            PlayerController.UnarmedAnimator.SetBool(JumpHash, true);
            PlayerController.UnarmedAnimator.SetFloat(JumpCountHash, PlayerController.JumpCount);
        }

        public override void OnExit(StateDefine next)
        {
            _fallSpeed = 0;
            PlayerController.UnarmedAnimator.SetBool(JumpHash, false);
        }

        public override void OnStay()
        {
            if (PlayerController.Rigidbody.velocity.y <= Maths.TinyNum)
            {
                StateMachine.Translate((int)PlayerStateID.Fall);
            }
            PlayerController.Flip();
        }
        
        private float _fallSpeed;

        public override void OnFixedStay()
        {
            _velocity.x = Time.fixedDeltaTime * PlayerController.speed * PlayerController.MoveDirection.x;
            _velocity.y = PlayerController.Rigidbody.velocity.y;
            
            // 跳跃时减速
            if (PlayerController.Rigidbody.velocity.y > Maths.TinyNum)
            {
                _fallSpeed += Time.fixedDeltaTime * PlayerController.jumpDeceleration;
                _velocity.y -= _fallSpeed;
            }
            
            PlayerController.Rigidbody.velocity = _velocity;
        }
    }
}