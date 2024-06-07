using UnityEngine;

public class Utilities : MonoBehaviour
{
    #region Progress
    public static float CalculateProgress01(float value, float min, float max)
    {
        float achieved = value - min;
        float toAchieve = max - min;
        float progress = achieved / toAchieve;
        progress = Mathf.Clamp01(progress);

        return progress;
    }

    public static float Evaluate(float progress, float resultMin, float resultMax)
    {
        float result = resultMin;
        float gain = resultMax - resultMin;
        float profit = progress * gain;
        result += profit;

        return result;
    }
    #endregion

    #region Random
    public static float RandomDirection()
    {
        return RandomState() ? 1.0f : -1.0f;
    }

    public static Quaternion RandomRotationY()
    {
        return Quaternion.Euler(new Vector3(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
    }

    private static bool RandomState(float positiveChance = 50.0f)
    {
        if (positiveChance == 100.0f)
        {
            return true;
        }
        else
        {
            return Random.Range(0.0f, 100.0f) < positiveChance;
        }
    }

    public static float RandomValue(float min, float max)
    {
        return Random.Range(min, max);
    }
    #endregion

    #region UI
    public static bool IsCursorOnUi()
    {
        return UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
    }
    #endregion
}
