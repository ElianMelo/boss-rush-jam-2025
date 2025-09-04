using UnityEngine;

public class InputTester : MonoBehaviour
{
    void Update()
    {
        // Check a range of axis numbers to be safe
        for (int i = 1; i <= 28; i++) 
        {
            // Create the axis name string
            string axisName = "joystick " + i + " axis";
            try
            {
                float axisValue = Input.GetAxis(axisName);
                // Only log axes with significant movement
                if (Mathf.Abs(axisValue) > 0.1f) 
                {
                    Debug.Log("Joystick Axis " + i + " has value: " + axisValue);
                }
            }
            catch (System.Exception)
            {
                // This is to prevent errors for axes that don't exist
                continue;
            }
        }
    }
}

