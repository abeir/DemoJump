using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationEventHandler : MonoBehaviour
    {
        public UnityAction<string> animationFinishAction;
        public UnityAction<string> animationGroupFinishAction;

        public void OnAnimationFinished(string animationName)
        {
            animationFinishAction?.Invoke(animationName);
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