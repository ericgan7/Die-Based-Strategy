using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations {

    public static float EaseOutElastic(float from, float to, float time)
    {
        float p = 0.3f;
        float s = p / 4.0f;
        float diff = (to - from);
        return from + diff + (diff * Mathf.Pow(2.0f, -10.0f * time) * Mathf.Sin((time - s) * (2 * Mathf.PI) / p));
    }

    public static IEnumerator ScreenShake(float intensity, float time)
    {
        float t = 0.0f;
        while (t < time)
        {
            t += Time.deltaTime;
            Camera.main.transform.localPosition = Random.insideUnitSphere * intensity;
            yield return new WaitForEndOfFrame();
        }
        Camera.main.transform.localPosition = Vector3.zero;
    }
}
