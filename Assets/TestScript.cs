using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasingToolkit.SecondOrderDynamics;

public class TestScript : MonoBehaviour
{
    [SerializeField] Transform target;

    [Space(10)]

    [SerializeField] float frequency = 3;
    [SerializeField] float dampening = 1;
    [SerializeField] float initialResponse = 0;

    SecondOrder_Rotation mySecondOrderRotation;

    private void Awake()
    {
        mySecondOrderRotation = new SecondOrder_Rotation(frequency, dampening, initialResponse, transform.localRotation);
    }

    private void FixedUpdate()
    {
        mySecondOrderRotation.UpdateConstants(new SO_Constants(frequency, dampening, initialResponse));
        transform.rotation = mySecondOrderRotation.Update(Time.deltaTime, target.localRotation);
    }
}
