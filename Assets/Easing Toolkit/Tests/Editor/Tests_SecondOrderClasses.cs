using NUnit.Framework;
using EasingToolkit.SecondOrderDynamics;
using UnityEngine;

public class Tests_SecondOrderClasses : MonoBehaviour
{
    [Test]
    public void T_FirstUpdateEqualZero()
    {
        SecondOrder_1D secondOrder1D = new SecondOrder_1D(1, 1, 0, 0);
        SecondOrder_2D secondOrder2D = new SecondOrder_2D(1, 1, 0, Vector2.zero);
        SecondOrder_3D secondOrder3D = new SecondOrder_3D(1, 1, 0, Vector3.zero);
        SecondOrder_Rotation secondOrderRot = new SecondOrder_Rotation(1, 1, 0, Quaternion.identity);

        Assert.AreEqual(0, secondOrder1D.Update(1.0f, 1));
        Assert.AreEqual(Vector2.zero, secondOrder2D.Update(1.0f, Vector2.one));
        Assert.AreEqual(Vector3.zero, secondOrder3D.Update(1.0f, Vector3.one));
        Assert.AreEqual(Quaternion.identity, secondOrderRot.Update(1.0f, Quaternion.Euler(90, 90, 0)));
    }

    [Test]
    public void T_SecondUpdateAfterOneSec()
    {
        SecondOrder_1D secondOrder1D = new SecondOrder_1D(1, 1, 0, 0);
        SecondOrder_2D secondOrder2D = new SecondOrder_2D(1, 1, 0, Vector2.zero);
        SecondOrder_3D secondOrder3D = new SecondOrder_3D(1, 1, 0, Vector3.zero);
        SecondOrder_Rotation secondOrderRot = new SecondOrder_Rotation(1, 1, 0, Quaternion.identity);

        secondOrder1D.Update(0.1f, 1 * 0.1f);
        secondOrder2D.Update(0.1f, Vector2.one * 0.1f);
        secondOrder3D.Update(0.1f, Vector3.one * 0.1f);
        secondOrderRot.Update(0.1f, Quaternion.Euler(90 * 0.1f, 90 * 0.1f, 0));

        Assert.AreEqual(0.314159274f, secondOrder1D.Update(1.0f, 1));
        Assert.AreEqual(new Vector2(0.314159274f, 0.314159274f), secondOrder2D.Update(1.0f, Vector2.one));
        Assert.AreEqual(new Vector3(0.314159274f, 0.314159274f, 0.314159274f), secondOrder3D.Update(1.0f, Vector3.one));
        Assert.AreEqual(new Quaternion(0.245726675f, 0.245726675f, -0.0193391107f, 0.980660617f), secondOrderRot.Update(1.0f, Quaternion.Euler(90, 90, 0)));
    }

    [Test]
    public void T_SecondUpdateAfterOneSecDifFrequency()
    {
        SecondOrder_1D secondOrder1D = new SecondOrder_1D(5, 1, 0, 0);
        SecondOrder_2D secondOrder2D = new SecondOrder_2D(5, 1, 0, Vector2.zero);
        SecondOrder_3D secondOrder3D = new SecondOrder_3D(5, 1, 0, Vector3.zero);
        SecondOrder_Rotation secondOrderRot = new SecondOrder_Rotation(5, 1, 0, Quaternion.identity);

        secondOrder1D.Update(0.1f, 1 * 0.1f);
        secondOrder2D.Update(0.1f, Vector2.one * 0.1f);
        secondOrder3D.Update(0.1f, Vector3.one * 0.1f);
        secondOrderRot.Update(0.1f, Quaternion.Euler(90 * 0.1f, 90 * 0.1f, 0));

        Assert.AreEqual(0.915439665f, secondOrder1D.Update(1.0f, 1));
        Assert.AreEqual(new Vector2(0.915439665f, 0.915439665f), secondOrder2D.Update(1.0f, Vector2.one));
        Assert.AreEqual(new Vector3(0.915439665f, 0.915439665f, 0.915439665f), secondOrder3D.Update(1.0f, Vector3.one));
        Assert.AreEqual(new Quaternion(0.716031551f, 0.716031551f, -0.0563529097f, 0.943646371f), secondOrderRot.Update(1.0f, Quaternion.Euler(90, 90, 0)));
    }

    [Test]
    public void T_SecondUpdateAfterOneSecDifDampening()
    {
        SecondOrder_1D secondOrder1D = new SecondOrder_1D(1, 0.15f, 0, 0);
        SecondOrder_2D secondOrder2D = new SecondOrder_2D(1, 0.15f, 0, Vector2.zero);
        SecondOrder_3D secondOrder3D = new SecondOrder_3D(1, 0.15f, 0, Vector3.zero);
        SecondOrder_Rotation secondOrderRot = new SecondOrder_Rotation(1, 0.15f, 0, Quaternion.identity);

        secondOrder1D.Update(0.1f, 1 * 0.1f);
        secondOrder2D.Update(0.1f, Vector2.one * 0.1f);
        secondOrder3D.Update(0.1f, Vector3.one * 0.1f);
        secondOrderRot.Update(0.1f, Quaternion.Euler(90 * 0.1f, 90 * 0.1f, 0));

        Assert.AreEqual(0.348132759f, secondOrder1D.Update(1.0f, 1));
        Assert.AreEqual(new Vector2(0.348132789f, 0.348132789f), secondOrder2D.Update(1.0f, Vector2.one));
        Assert.AreEqual(new Vector3(0.348132789f, 0.348132789f, 0.348132789f), secondOrder3D.Update(1.0f, Vector3.one));
        Assert.AreEqual(new Quaternion(0.272299796f, 0.272299796f, -0.0214304626f, 0.978569269f), secondOrderRot.Update(1.0f, Quaternion.Euler(90, 90, 0)));
    }

    [Test]
    public void T_SecondUpdateAfterOneSecDifInitialResponse()
    {
        SecondOrder_1D secondOrder1D = new SecondOrder_1D(1, 1, -1, 0);
        SecondOrder_2D secondOrder2D = new SecondOrder_2D(1, 1, -1, Vector2.zero);
        SecondOrder_3D secondOrder3D = new SecondOrder_3D(1, 1, -1, Vector3.zero);
        SecondOrder_Rotation secondOrderRot = new SecondOrder_Rotation(1, 1, -1, Quaternion.identity);

        secondOrder1D.Update(0.1f, 1 * 0.1f);
        secondOrder2D.Update(0.1f, Vector2.one * 0.1f);
        secondOrder3D.Update(0.1f, Vector3.one * 0.1f);
        secondOrderRot.Update(0.1f, Quaternion.Euler(90 * 0.1f, 90 * 0.1f, 0));

        Assert.AreEqual(-0.185840711f, secondOrder1D.Update(1.0f, 1));
        Assert.AreEqual(new Vector2(-0.185840711f, -0.185840711f), secondOrder2D.Update(1.0f, Vector2.one));
        Assert.AreEqual(new Vector3(-0.185840711f, -0.185840711f, -0.185840711f), secondOrder3D.Update(1.0f, Vector3.one));
        Assert.AreEqual(new Quaternion(-0.145359471f, -0.145359471f, 0.0114400387f, 1.01144016f), secondOrderRot.Update(1.0f, Quaternion.Euler(90, 90, 0)));
    }
}
