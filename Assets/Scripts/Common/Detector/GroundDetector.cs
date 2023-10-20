using UnityEngine;

namespace Common.Detector
{
    public class GroundDetector : IDetector
    {
        private ADetectorMonoBehaviour _detector;
        
        
        private readonly Collider2D[] _groundColliders = new Collider2D[2];
        

        public ADetectorMonoBehaviour SetDetectorMonoBehaviour
        {
            set => _detector = value;
        }
        

        public void Init()
        {
        }

        public bool Detect()
        {
            var count = Physics2D.OverlapBoxNonAlloc((Vector2)_detector.transform.position + _detector.detectBox.center, 
                    _detector.detectBox.size, 0, _groundColliders, _detector.groundLayer);
            return count > 0;
        }

        public void DrawGizmos()
        {
            Gizmos.color = _detector.detectBoxColor;
            Gizmos.DrawWireCube(_detector.transform.position + (Vector3)_detector.detectBox.center, _detector.detectBox.size);
        }
    }
}