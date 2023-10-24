using System.Collections.Generic;
using Common.Event;
using UnityEngine;

namespace Player
{


    public class PlayerFxEvent : IEvent
    {
        public string Group { get; }
        public string FX { get; }

        public PlayerFxEvent(string group, string fx)
        {
            Group = group;
            FX = fx;
        }
    }


    public class PlayerFXController : MonoBehaviour, IEventListener<PlayerFxEvent>
    {
        public struct FxGroup
        {
            public SpriteRenderer spriteRenderer;
            public Animator animator;
        }


        [SerializeField]
        public PlayerController playerController;


        /// <summary>
        /// key为组名（与GameObject名一致）
        /// </summary>
        private readonly Dictionary<string, FxGroup> _fxGroups = new();


        private void Awake()
        {
            if (playerController == null)
            {
                playerController = transform.GetComponentInParent<PlayerController>();
            }

            for (var i=0; i<transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var fxGroup = new FxGroup
                {
                    spriteRenderer = child.GetComponent<SpriteRenderer>(),
                    animator = child.GetComponent<Animator>()
                };
                _fxGroups[child.name] = fxGroup;

                BindEvent(fxGroup.animator);

                // 初始时关闭所有的效果
                child.gameObject.SetActive(false);
            }
            // 解除子物体绑定关系
            transform.DetachChildren();
        }


        private void OnEnable()
        {
            EventManager.AddListener(this);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(this);
        }


        public void Play(string group, string fx)
        {
            if (!_fxGroups.TryGetValue(group, out var fxGroup))
            {
                return;
            }

            fxGroup.animator.transform.position = transform.position;
            fxGroup.animator.gameObject.SetActive(true);
            fxGroup.spriteRenderer.flipX = !playerController.facingPositive;
            fxGroup.animator.Play(fx);
        }

        public void Stop(string group)
        {
            if (!_fxGroups.TryGetValue(group, out var fxGroup))
            {
                return;
            }

            fxGroup.animator.StopPlayback();
            fxGroup.animator.gameObject.SetActive(false);
        }

        public void OnEventTriggered(PlayerFxEvent evn)
        {
            Play(evn.Group, evn.FX);
        }

        public void OnAnimationFinished(string group)
        {
            Debug.Log($" >> OnAnimationFinished: {group}");

            Stop(group);
        }


        private void BindEvent(Animator animator)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {

                Debug.Log($">>>>>> BindEvent : {clip.name}");

                AnimationEvent evn = new()
                {
                    functionName = "OnAnimationGroupFinished",
                    stringParameter = animator.gameObject.name,
                    time = clip.length
                };
                clip.AddEvent(evn);
            }
            // 动画组件所在对象上必须挂载 PlayerAnimationEventHandler
            animator.GetComponent<PlayerAnimationEventHandler>().animationGroupFinishAction += OnAnimationFinished;
        }
    }
}