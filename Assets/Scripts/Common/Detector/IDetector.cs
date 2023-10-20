using UnityEngine;

namespace Common.Detector
{
    public interface IDetector
    {
        public ADetectorMonoBehaviour SetDetectorMonoBehaviour { set; }
        
        public void Init();

        public bool Detect();

        public void DrawGizmos();
    }
}