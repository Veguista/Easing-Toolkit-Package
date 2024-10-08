using UnityEngine;
using EasingToolkit.SecondOrderDynamics;

public class TestScript : MonoBehaviour
{
    SecondOrderTransform secondOrderTransform;

    private void Awake()
    {
        secondOrderTransform = GetComponent<SecondOrderTransform>();
    }

    float timer = 5;
    float timeElapsed = 0;

    private void Update()
    {
        if (timeElapsed < timer)
        {
            timeElapsed += Time.deltaTime;

            if(timeElapsed >= timer)
            {
                secondOrderTransform.whichDynamicType = SecondOrderTransform.DynamicsType.scale;
            }
        }
    }
}
