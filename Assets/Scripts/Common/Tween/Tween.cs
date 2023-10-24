using System;
using UnityEngine;

namespace Common.Tween
{
    
    public enum TweenCurveType
    {
        LinearTween,        
        EaseInQuadratic,    EaseOutQuadratic,   EaseInOutQuadratic,
        EaseInCubic,        EaseOutCubic,       EaseInOutCubic,
        EaseInQuartic,      EaseOutQuartic,     EaseInOutQuartic,
        EaseInQuintic,      EaseOutQuintic,     EaseInOutQuintic,
        EaseInSinusoidal,   EaseOutSinusoidal,  EaseInOutSinusoidal,
        EaseInBounce,       EaseOutBounce,      EaseInOutBounce,
        EaseInOverhead,     EaseOutOverhead,    EaseInOutOverhead,
        EaseInExponential,  EaseOutExponential, EaseInOutExponential,
        EaseInElastic,      EaseOutElastic,     EaseInOutElastic,
        EaseInCircular,     EaseOutCircular,    EaseInOutCircular,
        AntiLinearTween,    AlmostIdentity
    }
    
    public static class Tween
    {
	    
	    public static float Evaluate(TweenCurveType tp, float currentTime)
	    {
		    return tp switch
		    {
			    TweenCurveType.LinearTween => LinearTween(currentTime),
			    TweenCurveType.AntiLinearTween => AntiLinearTween(currentTime),
			    TweenCurveType.EaseInQuadratic => EaseInQuadratic(currentTime),
			    TweenCurveType.EaseOutQuadratic => EaseOutQuadratic(currentTime),
			    TweenCurveType.EaseInOutQuadratic => EaseInOutQuadratic(currentTime),
			    TweenCurveType.EaseInCubic => EaseInCubic(currentTime),
			    TweenCurveType.EaseOutCubic => EaseOutCubic(currentTime),
			    TweenCurveType.EaseInOutCubic => EaseInOutCubic(currentTime),
			    TweenCurveType.EaseInQuartic => EaseInQuartic(currentTime),
			    TweenCurveType.EaseOutQuartic => EaseOutQuartic(currentTime),
			    TweenCurveType.EaseInOutQuartic => EaseInOutQuartic(currentTime),
			    TweenCurveType.EaseInQuintic => EaseInQuintic(currentTime),
			    TweenCurveType.EaseOutQuintic => EaseOutQuintic(currentTime),
			    TweenCurveType.EaseInOutQuintic => EaseInOutQuintic(currentTime),
			    TweenCurveType.EaseInSinusoidal => EaseInSinusoidal(currentTime),
			    TweenCurveType.EaseOutSinusoidal => EaseOutSinusoidal(currentTime),
			    TweenCurveType.EaseInOutSinusoidal => EaseInOutSinusoidal(currentTime),
			    TweenCurveType.EaseInBounce => EaseInBounce(currentTime),
			    TweenCurveType.EaseOutBounce => EaseOutBounce(currentTime),
			    TweenCurveType.EaseInOutBounce => EaseInOutBounce(currentTime),
			    TweenCurveType.EaseInOverhead => EaseInOverhead(currentTime),
			    TweenCurveType.EaseOutOverhead => EaseOutOverhead(currentTime),
			    TweenCurveType.EaseInOutOverhead => EaseInOutOverhead(currentTime),
			    TweenCurveType.EaseInExponential => EaseInExponential(currentTime),
			    TweenCurveType.EaseOutExponential => EaseOutExponential(currentTime),
			    TweenCurveType.EaseInOutExponential => EaseInOutExponential(currentTime),
			    TweenCurveType.EaseInElastic => EaseInElastic(currentTime),
			    TweenCurveType.EaseOutElastic => EaseOutElastic(currentTime),
			    TweenCurveType.EaseInOutElastic => EaseInOutElastic(currentTime),
			    TweenCurveType.EaseInCircular => EaseInCircular(currentTime),
			    TweenCurveType.EaseOutCircular => EaseOutCircular(currentTime),
			    TweenCurveType.EaseInOutCircular => EaseInOutCircular(currentTime),
			    TweenCurveType.AlmostIdentity => AlmostIdentity(currentTime),
			    _ => throw new ArgumentOutOfRangeException(nameof(tp), tp, null)
		    };
	    }
        
        
        public static float LinearTween(float t)
		{
			return t;
		}

		public static float AntiLinearTween(float t)
		{
			return 1 - t;
		}

		// Almost Identity 

		public static float AlmostIdentity(float t)
		{
			return t * t * (2.0f - t);
		}

		// Quadratic    ---------------------------------------------------------------------------------------------------------------------------

		public static float EaseInQuadratic(float t)
		{
			return t * t;
		}

		public static float EaseOutQuadratic(float t)
		{
			return 1 - EaseInQuadratic(1 - t);
		}

		public static float EaseInOutQuadratic(float t)
		{
			if (t < 0.5f)
			{
				return EaseInQuadratic(t * 2f) / 2f;
			}
			else
			{
				return 1 - EaseInQuadratic((1f - t) * 2f) / 2;
			}
		}

		// Cubic        ---------------------------------------------------------------------------------------------------------------------------

		public static float EaseInCubic(float t)
		{
			return t * t * t;
		}

		public static float EaseOutCubic(float t)
		{
			return 1 - EaseInCubic(1 - t);
		}

		public static float EaseInOutCubic(float t)
		{
			if (t < 0.5f)
			{
				return EaseInCubic(t * 2f) / 2f;
			}
			else
			{
				return 1 - EaseInCubic((1f - t) * 2f) / 2;
			}
		}

		// Quartic      ---------------------------------------------------------------------------------------------------------------------------

		public static float EaseInQuartic(float t)
		{
			return Mathf.Pow(t, 4f);
		}

		public static float EaseOutQuartic(float t)
		{
			return 1 - EaseInQuartic(1 - t);
		}

		public static float EaseInOutQuartic(float t)
		{
			if (t < 0.5f)
			{
				return EaseInQuartic(t * 2f) / 2f;
			}
			else
			{
				return 1 - EaseInQuartic((1f - t) * 2f) / 2;
			}
		}

		// Quintic      ---------------------------------------------------------------------------------------------------------------------------

		public static float EaseInQuintic(float t)
		{
			return Mathf.Pow(t, 5f);
		}

		public static float EaseOutQuintic(float t)
		{
			return 1 - EaseInQuintic(1 - t);
		}

		public static float EaseInOutQuintic(float t)
		{
			if (t < 0.5f)
			{
				return EaseInQuintic(t * 2f) / 2f;
			}
			else
			{
				return 1 - EaseInQuintic((1f - t) * 2f) / 2;
			}
		}

		// Bounce       ---------------------------------------------------------------------------------------------------------------------------

		public static float EaseInBounce(float t)
		{
			float p = 0.3f;
			return Mathf.Pow(2, -10 * t) * Mathf.Sin((t - p / 4) * (2 * Mathf.PI) / p) + 1;
		}

		public static float EaseOutBounce(float t)
		{
			return 1 - EaseInBounce(1 - t);
		}

		public static float EaseInOutBounce(float t)
		{
			if (t < 0.5f)
			{
				return EaseInBounce(t * 2f) / 2f;
			}
			else
			{
				return 1 - EaseInBounce((1f - t) * 2f) / 2;
			}
		}

		// Sinusoidal   ---------------------------------------------------------------------------------------------------------------------------

		public static float EaseInSinusoidal(float t)
		{
			return 1 + Mathf.Sin(Mathf.PI / 2f * t - Mathf.PI / 2f);
		}

		public static float EaseOutSinusoidal(float t)
		{
			return 1 - EaseInSinusoidal(1 - t);
		}

		public static float EaseInOutSinusoidal(float t)
		{
			if (t < 0.5f)
			{
				return EaseInSinusoidal(t * 2f) / 2f;
			}
			else
			{
				return 1 - EaseInSinusoidal((1f - t) * 2f) / 2;
			}
		}

		// Overhead     ---------------------------------------------------------------------------------------------------------------------------

		public static float EaseInOverhead(float t)
		{
			float back = 1.6f;
			return t * t * ((back + 1f) * t - back);
		}

		public static float EaseOutOverhead(float t)
		{
			return 1 - EaseInOverhead(1 - t);
		}

		public static float EaseInOutOverhead(float t)
		{
			if (t < 0.5f)
			{
				return EaseInOverhead(t * 2f) / 2f;
			}
			else
			{
				return 1 - EaseInOverhead((1f - t) * 2f) / 2;
			}
		}

		// Exponential  ---------------------------------------------------------------------------------------------------------------------------

		public static float EaseInExponential(float t)
		{
			return t == 0f ? 0f : Mathf.Pow(1024f, t - 1f);
		}

		public static float EaseOutExponential(float t)
		{
			return 1 - EaseInExponential(1 - t);
		}

		public static float EaseInOutExponential(float t)
		{
			if (t < 0.5f)
			{
				return EaseInExponential(t * 2f) / 2f;
			}
			else
			{
				return 1 - EaseInExponential((1f - t) * 2f) / 2;
			}
		}

		// Elastic      ---------------------------------------------------------------------------------------------------------------------------

		public static float EaseInElastic(float t)
		{
			if (t == 0f) { return 0f; }
			if (t == 1f) { return 1f; }
			return -Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t - 0.1f) * (2f * Mathf.PI) / 0.4f);
		}

		public static float EaseOutElastic(float t)
		{
			return 1 - EaseInElastic(1 - t);
		}

		public static float EaseInOutElastic(float t)
		{
			if (t < 0.5f)
			{
				return EaseInElastic(t * 2f) / 2f;
			}
			else
			{
				return 1 - EaseInElastic((1f - t) * 2f) / 2;
			}
		}

		// Circular     ---------------------------------------------------------------------------------------------------------------------------

		public static float EaseInCircular(float t)
		{
			return 1f - Mathf.Sqrt(1f - t * t);
		}

		public static float EaseOutCircular(float t)
		{
			return 1 - EaseInCircular(1 - t);
		}

		public static float EaseInOutCircular(float t)
		{
			if (t < 0.5f)
			{
				return EaseInCircular(t * 2f) / 2f;
			}
			else
			{
				return 1 - EaseInCircular((1f - t) * 2f) / 2;
			}
		}
    }
}