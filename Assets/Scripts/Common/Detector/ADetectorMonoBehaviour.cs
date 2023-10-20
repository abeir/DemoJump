using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Detector
{
    public abstract class ADetectorMonoBehaviour : MonoBehaviour
    {
        [Title("Required")]
        [SerializeField, Required]
        public Transform ownerTransform;
        
        
        [Title("Ground Detector")]
        [SerializeField]
        public bool groundDebugger = true;
        [SerializeField]
        public LayerMask groundLayer;
        [SerializeField]
        public Rect detectBox;
        [SerializeField]
        public Color detectBoxColor;




    }
}