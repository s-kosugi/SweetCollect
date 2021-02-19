using UnityEngine;

/// <summary>
/// イージングクラス
/// </summary>
public class Easing : MonoBehaviour
{
	public static float InQuad(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime;
		return max * t * t + min;
	}
	public static float OutQuad(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime;
		return -max * t * (t - 2.0f) + min;
	}
	public static float InOutQuad(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime / 2.0f;
		if (t < 1.0f)
			return max / 2.0f * t * t + min;
		--t;
		return -max / 2.0f * (t * (t - 2.0f) - 1.0f) + min;
	}
	public static float InCubic(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime;
		return max * t * t * t + min;
	}
	public static float OutCubic(float t, float totaltime, float max, float min)
	{
		max -= min;
		t = t / totaltime - 1.0f;
		return max * (t * t * t + 1.0f) + min;
	}
	public static float InOutCubic(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime / 2.0f;
		if (t < 1.0f)
			return max / 2.0f * t * t * t + min;
		t -= 2.0f;
		return max / 2.0f * (t * t * t + 2.0f) + min;
	}
	public static float InQuart(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime;
		return max * t * t * t * t + min;
	}
	public static float OutQuart(float t, float totaltime, float max, float min)
	{
		max -= min;
		t = t / totaltime - 1.0f;
		return -max * (t * t * t * t - 1.0f) + min;
	}
	public static float InOutQuart(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime / 2.0f;
		if (t < 1.0f)
			return max / 2.0f * t * t * t * t + min;
		t -= 2.0f;
		return -max / 2.0f * (t * t * t * t - 2.0f) + min;
	}
	public static float InQuint(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime;
		return max * t * t * t * t * t + min;
	}
	public static float OutQuint(float t, float totaltime, float max, float min)
	{
		max -= min;
		t = t / totaltime - 1.0f;
		return max * (t * t * t * t * t + 1.0f) + min;
	}
	public static float InOutQuint(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime / 2.0f;
		if (t < 1.0f)
			return max / 2.0f * t * t * t * t * t + min;
		t -= 2.0f;
		return max / 2.0f * (t * t * t * t * t + 2.0f) + min;
	}
	public static float InSine(float t, float totaltime, float max, float min)
	{
		max -= min;
		return -max * Mathf.Cos((t * Mathf.Deg2Rad*90) / totaltime) + max + min;
	}
	public static float OutSine(float t, float totaltime, float max, float min)
	{
		max -= min;
		return max * Mathf.Sin((t * Mathf.Deg2Rad*90) / totaltime) + min;
	}
	public static float InOutSine(float t, float totaltime, float max, float min)
	{
		max -= min;
		return -max / 2.0f * (Mathf.Cos(t * (float)Mathf.PI / totaltime) - 1.0f) + min;
	}
	public static float InExp(float t, float totaltime, float max, float min)
	{
		max -= min;
		return t == 0.0f ? min : max * Mathf.Pow(2.0f, 10.0f * (t / totaltime - 1.0f)) + min;
	}
	public static float OutExp(float t, float totaltime, float max, float min)
	{
		max -= min;
		return t == totaltime ? max + min : max * (-Mathf.Pow(2.0f, -10.0f * t / totaltime) + 1.0f) + min;
	}
	public static float InOutExp(float t, float totaltime, float max, float min)
	{
		if (t == 0.0f) return min;
		if (t == totaltime) return max;
		max -= min;
		t /= totaltime / 2.0f;

		if (t < 1.0f) return max / 2.0f * Mathf.Pow(2.0f, 10.0f * (t - 1.0f)) + min;
		--t;
		return max / 2.0f * (-Mathf.Pow(2.0f, -10.0f * t) + 2.0f) + min;

	}
	public static float InCirc(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime;
		return -max * (Mathf.Sqrt(1.0f - t * t) - 1.0f) + min;
	}
	public static float OutCirc(float t, float totaltime, float max, float min)
	{
		max -= min;
		t = t / totaltime - 1.0f;
		return max * Mathf.Sqrt(1.0f - t * t) + min;
	}
	public static float InOutCirc(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime / 2.0f;
		if (t < 1.0f) return -max / 2.0f * (Mathf.Sqrt(1.0f - t * t) - 1.0f) + min;
		t -= 2.0f;
		return max / 2.0f * (Mathf.Sqrt(1.0f - t * t) + 1.0f) + min;
	}
	public static float InBack(float t, float totaltime, float max, float min, float s)
	{
		max -= min;
		t /= totaltime;
		return max * t * t * ((s + 1.0f) * t - s) + min;
	}
	public static float OutBack(float t, float totaltime, float max, float min, float s)
	{
		max -= min;
		t = t / totaltime - 1.0f;
		return max * (t * t * ((s + 1.0f) * t + s) + 1.0f) + min;
	}
	public static float InOutBack(float t, float totaltime, float max, float min, float s)
	{
		max -= min;
		s *= 1.525f;
		t /= totaltime / 2.0f;
		if (t < 1.0f) return max / 2.0f * (t * t * ((s + 1.0f) * t - s)) + min;
		t -= 2.0f;
		return max / 2.0f * (t * t * ((s + 1.0f) * t + s) + 2.0f) + min;
	}
	public static float OutBounce(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime;

		if (t < 1.0f / 2.75f)
			return max * (7.5625f * t * t) + min;
		else if (t < 2.0f / 2.75f)
		{
			t -= 1.5f / 2.75f;
			return max * (7.5625f * t * t + 0.75f) + min;
		}
		else if (t < 2.5f / 2.75f)
		{
			t -= 2.25f / 2.75f;
			return max * (7.5625f * t * t + 0.9375f) + min;
		}
		else
		{
			t -= 2.625f / 2.75f;
			return max * (7.5625f * t * t + 0.984375f) + min;
		}
	}
	public static float InBounce(float t, float totaltime, float max, float min)
	{
		max -= min;
		t /= totaltime;
		return max * Mathf.Pow(2.0f, 6.0f * (t - 1.0f)) * Mathf.Abs(Mathf.Sin(t * (float)Mathf.PI * 3.5f)) + min;
	}
	public static float InOutBounce(float t, float totaltime, float max, float min)
	{
		max -= min;

		t /= totaltime;

		if (t < 0.5f)
			return max * 8.0f * Mathf.Pow(2.0f, 8.0f * (t - 1.0f)) * Mathf.Abs(Mathf.Sin(t * (float)Mathf.PI * 7.0f)) + min;
		else
			return max * (1.0f - 8.0f * Mathf.Pow(2.0f, -8.0f * t) * Mathf.Abs(Mathf.Sin(t * (float)Mathf.PI * 7.0f))) + min;
	}
	public static float Linear(float t, float totaltime, float max, float min)
	{
		return (max - min) * t / totaltime + min;
	}
}
