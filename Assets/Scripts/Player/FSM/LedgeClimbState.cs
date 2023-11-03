using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using FSM;
using UnityEngine;

namespace Player.FSM
{
    public class LedgeClimbState : AStateBase
    {
        public static readonly int LedgeClimbHash = Animator.StringToHash("LedgeClimb");

        private float _gravityScale;
        private Vector3 _targetPosition;
        private bool _climbFinish;

        private TweenerCore<Vector3, Vector3, VectorOptions> _tweener;

        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.LedgeClimb,
            Name = PlayerStateID.LedgeClimb.ToString()
        };

        public LedgeClimbState(PlayerController ctrl) : base(ctrl)
        {
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.CanLedge && PlayerController.IsTouchLedge;
        }

        public override void OnEnter(StateDefine pre)
        {
            _climbFinish = false;
            ComputeTargetPosition();

            PlayerController.Rigidbody.velocity = Vector2.zero;

            _gravityScale = PlayerController.Rigidbody.gravityScale;
            PlayerController.Rigidbody.gravityScale = 0;

            PlayLedgeClimb();
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.Rigidbody.gravityScale = _gravityScale;

            StopLedgeClimb();
        }

        public override void OnStay()
        {
            if (!_climbFinish)
            {
                return;
            }
            if (PlayerController.IsOnAir)
            {
                StateMachine.Translate((int)PlayerStateID.Fall);
            }
            else
            {
                if (PlayerController.AxisXPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Run);
                }
                else
                {
                    StateMachine.Translate((int)PlayerStateID.Idle);
                }
            }
        }

        public override void OnFixedStay()
        {
        }

        private void ComputeTargetPosition()
        {
            _targetPosition = PlayerController.TouchLedgeHorizontalPoint;
        }


        private void PlayLedgeClimb()
        {
            PlayerController.UnarmedAnimator.SetBool(LedgeClimbHash, true);

            if (_tweener == null)
            {
                _tweener = PlayerController.transform.DOMove(_targetPosition, PlayerController.ledgeClimbDuration)
                    .SetEase(Ease.OutCubic)
                    .SetAutoKill(false)
                    .OnComplete(OnLedgeClimbFinished);
            }
            else
            {
                _tweener.ChangeValues(PlayerController.transform.position, _targetPosition,
                    PlayerController.ledgeClimbDuration);
            }
            _tweener.Restart();
        }

        private void StopLedgeClimb()
        {
            PlayerController.UnarmedAnimator.SetBool(LedgeClimbHash, false);

            if (_tweener.IsPlaying())
            {
                _tweener.Pause();
                _tweener.Complete();
            }
        }

        private void OnLedgeClimbFinished()
        {
            _climbFinish = true;
        }
    }
}