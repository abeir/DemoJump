using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Tween
{

    public enum TweenDefine
    {
        Tween, AnimationCurve
    }
    
    [Serializable]
    public class TweenType
    {
        [PropertyOrder(1)] 
        public TweenDefine tweenDefine = TweenDefine.Tween;

        [ShowIf("@this.tweenDefine==TweenDefine.Tween")]
        [SerializeField, PropertyOrder(2)]
        public TweenCurveType tweenCurve = TweenCurveType.LinearTween;
        
        [ShowIf("@this.tweenDefine==TweenDefine.AnimationCurve")]
        [SerializeField, PropertyOrder(3)]
        public AnimationCurve animationCurve = new(new Keyframe(0, 0), new Keyframe(1, 1f));


        public float DoTween(float currentTime)
        {
            return tweenDefine == TweenDefine.Tween ? Tween.Evaluate(tweenCurve, currentTime) : animationCurve.Evaluate(currentTime);
        }
    }
}