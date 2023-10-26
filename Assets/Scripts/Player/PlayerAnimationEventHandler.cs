using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationEventHandler : MonoBehaviour
    {
        public static readonly string MethodOnAnimationFinished = nameof(OnAnimationFinished);
        public static readonly string MethodOnAnimationGroupFinished = nameof(OnAnimationGroupFinished);


        public UnityAction<string> animationFinishAction;
        public UnityAction<string> animationGroupFinishAction;

        public void OnAnimationFinished(string clipName)
        {
            animationFinishAction?.Invoke(clipName);
        }


        /// <summary>
        /// 动画组完成事件
        /// </summary>
        public void OnAnimationGroupFinished(string group)
        {
            animationGroupFinishAction?.Invoke(group);
        }
    }
}