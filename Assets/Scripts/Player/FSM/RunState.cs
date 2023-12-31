﻿using Common.Helper;
using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class RunState : AStateBase
    {
        public static readonly int RunHash = Animator.StringToHash("Run");


        private Vector2 _direction = Vector2.zero;
        private Vector2 _velocity = Vector2.zero;
        
        
        public override StateDefine State { get; } = new StateDefine
        {
            ID = (int)PlayerStateID.Run,
            Name = "Run"
        };
        
        
        public RunState(PlayerController ctrl) : base(ctrl)
        {
        }
        
        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.PlayerDetector.IsOnGround;
        }

        public override void OnEnter(StateDefine pre)
        {
            PlayerController.UnarmedAnimator.SetBool(RunHash, true);
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.UnarmedAnimator.SetBool(RunHash, false);

            if (Mathf.Abs(PlayerController.MoveDirection.x) > Maths.TinyNum)
            {
                return;
            }
            _velocity.Set(0f, PlayerController.Rigidbody.velocity.y);
            PlayerController.Rigidbody.velocity = _velocity;
        }

        public override void OnStay()
        {
            PlayerController.Flip();
        }
        
        
        public override void OnFixedStay()
        {
            _direction.Set(PlayerController.MoveDirection.x, 0f);
            
            _velocity = Time.fixedDeltaTime * PlayerController.speed * _direction;
            
            PlayerController.Rigidbody.velocity = _velocity;
        }
    }
}