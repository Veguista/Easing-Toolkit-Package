using UnityEngine;
using EasingToolkit.SecondOrderDynamics;

public class TestScript : MonoBehaviour
{
    SecondOrderTransform secondOrderTransform;

    private void Awake()
    {
        secondOrderTransform = GetComponent<SecondOrderTransform>();
    }

    private void Update()
    {
        secondOrderTransform.Frequency += 0.01f;
    }
}
