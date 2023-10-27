using System.Collections.Generic;
using Common.Event;
using UnityEngine;

namespace Player
{


    public class PlayerFxEvent : IEvent
    {
        private const string dustGroupName = "DustGroup";
        private const string dustGroupFollowName = "DustGroup_follow";

        public string Group { get; }
        public string FX { get; }

        public PlayerFxEvent(string group, string fx)
        {
            Group = group;
            FX = fx;
        }

        public static void TriggerJumpDust()
        {
            EventManager.TriggerEvent(new PlayerFxEvent(dustGroupName, "JumpDust"));
        }

        public static void TriggerDoubleJumpDust()
        {
            EventManager.TriggerEvent(new PlayerFxEvent(dustGroupName, "DoubleJumpDust"));
        }

        public static void TriggerDashDust()
        {
            EventManager.TriggerEvent(new PlayerFxEvent(dustGroupName, "DashDust"));
        }

        public static void TriggerLandDust()
        {
            EventManager.TriggerEvent(new PlayerFxEvent(dustGroupName, "LandDust"));
        }

        public static void TriggerSlideDust()
        {
            EventManager.TriggerEvent(new PlayerFxEvent(dustGroupName, "SlideDust"));
        }
    }


    public class PlayerFXController : MonoBehaviour, IEventListener<PlayerFxEvent>
    {
        private const string followDustSuffix = "_follow";      // 已此为后缀的子对象作为跟随动画效果

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

            InitGroups();
            // 解除子物体绑定关系
            // transform.DetachChildren();
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
            Stop(group);
        }


        private void InitGroups()
        {
            var detachChildren = new List<Transform>();

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

                // 不以 follow 为后缀的将会被分离父子关系
                if (!child.name.EndsWith(followDustSuffix))
                {
                    detachChildren.Add(child);
                }

                // 初始时关闭所有的效果
                child.gameObject.SetActive(false);
            }

            foreach (var child in detachChildren)
            {
                child.SetParent(null);
            }
        }


        private void BindEvent(Animator animator)
        {
            var clips = animator.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                AnimationEvent evn = new()
                {
                    functionName = PlayerAnimationEventHandler.MethodOnAnimationGroupFinished,
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