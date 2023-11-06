using System.Collections.Generic;
using Common.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Player.FX
{
    public class PlayerParticleFXController : MonoBehaviour, IEventListener<PlayerParticleFxEvent>
    {

        [FoldoutGroup("Run Dust"), SerializeField]
        public Vector3 runRightPosition;      // 向右奔跑时的偏移位置
        [FoldoutGroup("Run Dust"), SerializeField]
        public Vector3 runRightRotation;       // 向右奔跑时的旋转值
        [FoldoutGroup("Run Dust"), SerializeField]
        public Vector3 runLeftPosition;       // 向左奔跑时的偏移位置
        [FoldoutGroup("Run Dust"), SerializeField]
        public Vector3 runLeftRotation;       // 向左奔跑时的旋转值



        private Dictionary<string, ParticleSystem> _fxParticles = new();

        private void Awake()
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                var child = transform.GetChild(i);
                var particle = child.GetComponent<ParticleSystem>();
                _fxParticles[child.name] = particle;

                particle.Stop();
            }
        }

        private void OnEnable()
        {
            EventManager.AddListener(this);
        }

        private void OnDisable()
        {
            EventManager.RemoveListener(this);
        }

        public void OnEventTriggered(PlayerParticleFxEvent evn)
        {
            if (!_fxParticles.TryGetValue(evn.FX, out var particle))
            {
                Debug.LogWarning($"not found PlayerParticleFxEvent: {evn.FX}");
                return;
            }

            Debug.Log($"{evn.FX}  {evn.Direction}  {evn.Start}");

            if (!evn.Start)
            {
                particle.Stop();
                return;
            }
            SetShape(particle, evn);
            particle.Play();
        }

        private void SetShape(ParticleSystem particle, PlayerParticleFxEvent evn)
        {
            var fx = particle.gameObject.name;
            if (fx == PlayerParticleFxEvent.RunDust)
            {
                var shape = particle.shape;
                shape.enabled = true;
                if (evn.Direction == 1)
                {
                    shape.position = runRightPosition;
                    shape.rotation = runRightRotation;
                }
                else if (evn.Direction == -1)
                {
                    shape.position = runLeftPosition;
                    shape.rotation = runLeftRotation;
                }
            }
        }
    }
}