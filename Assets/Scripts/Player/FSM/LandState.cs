﻿using Common.Helper;
using FSM;
using Player.FX;
using UnityEngine;

namespace Player.FSM
{
    public class LandState : AStateBase
    {
        public static readonly int LandHash = Animator.StringToHash("Land");
        // 动画名称
        private const string landClipName = "Land";

        public override StateDefine State => new StateDefine()
        {
            ID = (int)PlayerStateID.Land,
            Name = "Land"
        };

        public LandState(PlayerController ctrl) : base(ctrl)
        {
            BindEvent();
        }

        public override bool CanEnter(StateDefine pre)
        {
            return PlayerController.IsOnGround || PlayerController.IsOnSlope;
        }

        public override void OnEnter(StateDefine pre)
        {
            PlayerController.UnarmedAnimator.SetBool(LandHash, true);

            PlayerFxEvent.TriggerLandDust();
        }

        public override void OnExit(StateDefine next)
        {
            PlayerController.UnarmedAnimator.SetBool(LandHash, false);
        }

        public override void OnStay()
        {
        }

        public override void OnFixedStay()
        {
        }


        private void BindEvent()
        {
            var clips = PlayerController.UnarmedAnimator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name != landClipName)
                {
                    continue;
                }
                AnimationEvent evn = new()
                {
                    functionName = PlayerAnimationEventHandler.MethodOnAnimationFinished,
                    stringParameter = clip.name,
                    time = clip.length
                };
                clip.AddEvent(evn);

                // 动画组件所在对象上必须挂载 PlayerAnimationEventHandler
                PlayerController.UnarmedAnimator.GetComponent<PlayerAnimationEventHandler>().animationFinishAction += OnLandFinished;
            }
        }

        private void OnLandFinished(string clipName)
        {
            if (clipName != landClipName)
            {
                return;
            }
            if (PlayerController.IsOnAir)
            {
                if (PlayerController.IsVelocityYDown)
                {
                    StateMachine.Translate((int)PlayerStateID.Fall);
                }
            }
            else
            {
                PlayerController.ResetJumpCount();

                if (PlayerController.JumpPressed)
                {
                    StateMachine.Translate((int)PlayerStateID.Jump);
                }
                else if (Mathf.Abs(PlayerController.MoveDirection.x) > 0)
                {
                    StateMachine.Translate((int)PlayerStateID.Run);
                }
                else if (Mathf.Abs(PlayerController.MoveDirection.x) < Maths.TinyNum)
                {
                    StateMachine.Translate((int)PlayerStateID.Idle);
                }
            }
        }
    }
}