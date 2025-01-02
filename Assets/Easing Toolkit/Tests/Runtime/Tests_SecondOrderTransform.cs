using NUnit.Framework;
using EasingToolkit.SecondOrderDynamics;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using static EasingToolkit.SecondOrderDynamics.SecondOrderTransform;

public class Tests_SecondOrderTransform
{
    [UnityTest]
    public IEnumerator T_SettingSecondOrderParametersWorks()
    {
        float testFrequency = Random.Range(0.01f, 10f);
        float testDampening = Random.Range(0.01f, 10f);
        float testInitialResponse = Random.Range(-10f, 10f);
        DynamicsType testDynamicType = (DynamicsType) Random.Range((int) 0, (int) DynamicsType.scale + 1);
        TypeOfDataInput testInputMode = (TypeOfDataInput)Random.Range((int)0, (int)TypeOfDataInput.storedTransformData + 1);
        TypeOfSpace testInputSpace = (TypeOfSpace)Random.Range((int)0, (int)TypeOfSpace.worldSpace + 1);
        TypeOfSpace testOutputSpace = (TypeOfSpace)Random.Range((int)0, (int)TypeOfSpace.worldSpace + 1);
        bool[] testAxisToFollow = { false, true, false };
        TypeOfDynamicsRefresh testRefreshMode = (TypeOfDynamicsRefresh) Random.Range((int)0, (int)TypeOfDynamicsRefresh.lateUpdate + 1);

        // Setting up basic stuff.
        GameObject gameObject = new GameObject();
        GameObject target = new GameObject();

        SecondOrderTransform secondOrderTransform = gameObject.AddComponent<SecondOrderTransform>();

        // Setting the follow Transform.
        secondOrderTransform.followTransform = target.transform;

        // Setting frequency, dampening, initial response parameters.
        secondOrderTransform.Frequency = testFrequency;
        secondOrderTransform.Dampening = testDampening;
        secondOrderTransform.InitialResponse = testInitialResponse;

        // Setting Dynamic Type
        secondOrderTransform.WhichDynamicType = testDynamicType;

        // Setting the input mode.
        secondOrderTransform.inputMode = testInputMode;

        // Setting the input space.
        secondOrderTransform.obtainTransformDataFromLocalOrWorld = testInputSpace;

        // Setting the output space.
        secondOrderTransform.applyDynamicsToLocalOrWorld = testOutputSpace;

        // Setting axis to follow.
        for (int i = 0; i < 3; i++)
        {
            secondOrderTransform.axisToFollow[i] = testAxisToFollow[i];
        }

        // Setting the mode of refresh.
        secondOrderTransform.refreshMode = testRefreshMode;


        yield return new WaitForSeconds(1);


        Assert.AreEqual(target.transform, secondOrderTransform.followTransform);

        Assert.AreEqual(testFrequency, secondOrderTransform.Frequency);
        Assert.AreEqual(testDampening, secondOrderTransform.Dampening);
        Assert.AreEqual(testInitialResponse, secondOrderTransform.InitialResponse);

        SO_Constants expectedConstants = new SO_Constants(testFrequency, testDampening, testInitialResponse);
        Assert.AreEqual(expectedConstants, secondOrderTransform.MyConstants);

        Assert.AreEqual(testDynamicType, secondOrderTransform.WhichDynamicType);

        Assert.AreEqual(testInputMode, secondOrderTransform.inputMode);

        Assert.AreEqual(testInputSpace, secondOrderTransform.obtainTransformDataFromLocalOrWorld);
        Assert.AreEqual(testOutputSpace, secondOrderTransform.applyDynamicsToLocalOrWorld);

        Assert.AreEqual(testAxisToFollow, secondOrderTransform.axisToFollow);

        Assert.AreEqual(testRefreshMode, secondOrderTransform.refreshMode);
    }

    [UnityTest]
    public IEnumerator T_BasicPositionDynamics()
    {
        // Setting up basic stuff.
        GameObject gameObject = new GameObject();
        gameObject.transform.position = Vector3.zero;
        GameObject target = new GameObject();

        SecondOrderTransform secondOrderTransform = gameObject.AddComponent<SecondOrderTransform>();

        // Setting the follow Transform.
        secondOrderTransform.followTransform = target.transform;
        secondOrderTransform.refreshMode = TypeOfDynamicsRefresh.fixedUpdate;

        secondOrderTransform.ResetDynamics();

        target.transform.position = new Vector3(10, 10, 0);

        yield return new WaitForSeconds(1);

        // Checking that our transform has moved.
        Assert.AreNotEqual(Vector3.zero, gameObject.transform.position);

        // Checking that our transform has not ended in the exact place of the target yet.
        Assert.AreNotEqual(target.transform.position, gameObject.transform.position);

        secondOrderTransform.ResetDynamics();

        yield return new WaitForFixedUpdate();
        
        // Checking that after a frame the system updates and the position of the transform is the same as the target.
        Assert.AreEqual(target.transform.position, gameObject.transform.position);
    }

    [UnityTest]
    public IEnumerator T_BasicRotationDynamics()
    {
        // Setting up basic stuff.
        GameObject gameObject = new GameObject();
        GameObject target = new GameObject();

        SecondOrderTransform secondOrderTransform = gameObject.AddComponent<SecondOrderTransform>();

        // Setting the follow Transform.
        secondOrderTransform.WhichDynamicType = DynamicsType.rotation;
        secondOrderTransform.followTransform = target.transform;
        secondOrderTransform.refreshMode = TypeOfDynamicsRefresh.fixedUpdate;

        secondOrderTransform.ResetDynamics();

        target.transform.rotation = Quaternion.Euler(0, 45, 90);

        yield return new WaitForSeconds(1);

        // Checking that our transform has moved.
        Assert.AreNotEqual(Quaternion.identity, gameObject.transform.rotation);

        // Checking that our transform has not ended in the exact place of the target yet.
        Assert.AreNotEqual(target.transform.rotation, gameObject.transform.rotation);

        secondOrderTransform.ResetDynamics();

        yield return new WaitForFixedUpdate();

        // Checking that after a frame the system updates and the position of the transform is the same as the target.
        Assert.AreEqual(target.transform.rotation, gameObject.transform.rotation);
    }

    [UnityTest]
    public IEnumerator T_BasicScaleDynamics()
    {
        // Setting up basic stuff.
        GameObject gameObject = new GameObject();
        GameObject target = new GameObject();

        SecondOrderTransform secondOrderTransform = gameObject.AddComponent<SecondOrderTransform>();

        // Setting the follow Transform.
        secondOrderTransform.WhichDynamicType = DynamicsType.scale;
        secondOrderTransform.followTransform = target.transform;
        secondOrderTransform.refreshMode = TypeOfDynamicsRefresh.fixedUpdate;

        secondOrderTransform.ResetDynamics();

        target.transform.localScale = new Vector3(10,5,10);

        yield return new WaitForSeconds(1);

        // Checking that our transform has moved.
        Assert.AreNotEqual(Vector3.zero, gameObject.transform.rotation);

        // Checking that our transform has not ended in the exact place of the target yet.
        Assert.AreNotEqual(target.transform.localScale, gameObject.transform.localScale);

        secondOrderTransform.ResetDynamics();

        yield return new WaitForFixedUpdate();

        // Checking that after a frame the system updates and the position of the transform is the same as the target.
        Assert.AreEqual(target.transform.localScale, gameObject.transform.localScale);
    }
}
