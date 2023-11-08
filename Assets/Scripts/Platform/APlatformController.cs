using System.Collections;
using Common.Event;
using UnityEngine;

namespace Platform
{
    public abstract class APlatformController : MonoBehaviour, IEventListener<SuspendPlatformEvent>
    {
        [SerializeField]
        public float suspendDuration = 0.5f;


        protected PlatformEffector2D platformEffector;

        private Coroutine _suspendPlatformCoroutine;

        protected virtual void Awake()
        {
            platformEffector = GetComponent<PlatformEffector2D>();
        }

        protected virtual void OnEnable()
        {
            EventManager.AddListener(this);
        }

        protected virtual void OnDisable()
        {
            EventManager.RemoveListener(this);
        }

        public void OnEventTriggered(SuspendPlatformEvent evn)
        {
            if (_suspendPlatformCoroutine != null)
            {
                StopCoroutine(_suspendPlatformCoroutine);
                _suspendPlatformCoroutine = null;
            }

            _suspendPlatformCoroutine = StartCoroutine(SuspendPlatformCoroutine(evn.layer));
        }

        private IEnumerator SuspendPlatformCoroutine(int layer)
        {
            var colliderMask = platformEffector.colliderMask;

            platformEffector.colliderMask = colliderMask & ~(1 << layer);
            yield return new WaitForSeconds(suspendDuration);
            platformEffector.colliderMask = colliderMask;
        }
    }
}