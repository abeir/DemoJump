using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationEventHandler : MonoBehaviour
    {
        public UnityAction<string> animationFinishAction;
        

        public void OnAnimationFinished(string animationName)
        {
            animationFinishAction?.Invoke(animationName);
        }
        
    }
}