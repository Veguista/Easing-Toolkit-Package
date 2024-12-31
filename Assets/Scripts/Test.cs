using EasingToolkit;
using EasingToolkit.SecondOrderDynamics;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SecondOrder_1D secondOrder1D = new SecondOrder_1D(1, 1, 0, 0);
        SecondOrder_2D secondOrder2D = new SecondOrder_2D(1, 1, 0, Vector2.zero);
        SecondOrder_3D secondOrder3D = new SecondOrder_3D(1, 1, 0, Vector3.zero);
        SecondOrder_Rotation secondOrderRot = new SecondOrder_Rotation(1, 1, 0, Quaternion.identity);

        secondOrder1D.Update(0.1f, 1 * 0.1f);
        secondOrder2D.Update(0.1f, Vector2.one * 0.1f);
        secondOrder3D.Update(0.1f, Vector3.one * 0.1f);
        secondOrderRot.Update(0.1f, Quaternion.Euler(90 * 0.1f, 90 * 0.1f, 0));

        print("Second Order 1D (1,1,0) after 1 sec : " + secondOrder1D.Update(1f, 1) + "\n");
        print("Second Order 2D (1,1,0) after 1 sec : " + secondOrder2D.Update(1f, Vector2.one) + "\n");
        print("Second Order 3D (1,1,0) after 1 sec : " + secondOrder3D.Update(1f, Vector3.one) + "\n");
        print("Second Order Rotation (1,1,0) after 1 sec : " + secondOrderRot.Update(1f, Quaternion.Euler(90, 90, 0)) + "\n");
    }
}
