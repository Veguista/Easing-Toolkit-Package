# Readme

**Easing Toolkit** is comprised of 2 complimentary but separate parts:


## EasingTools.ApplyEase():

A function that applies an easing style from a broad collection to a value from 0 to 1.

It is found inside the **EasingTools** class. To gain access to the **EasingTools **class, users can either:
1. (Recommended) Specify that they are using the **EasingToolkit namespace**. To do so, add the following line of code at the beginning of your script "**using EasingToolkit;**".
2. Write the namespace before every instance of the **EasingTools **class (Ex. "**EasingToolkit.EasingTools.ApplyEase(inputFloat, EasingToolkit.EaseType.typeOfEase)**").


The **ApplyEase()** method takes 2 arguments:
1. A float (**inputFloat**), in the [0,1] Range (inclusive). This is the value that will be eased.
2. An EaseType enum (**typeOfEase**), indicating the easing process to be performed.


All possible Easing Functions and their visualizations can be found at [easings.net](https://easings.net/).

A usual use case for the ApplyEase() method is to process an interpolation value before it is fed into a Lerp().
For example, the following code would calculate an eased translation of [type EaseInOutQuint](https://easings.net/#easeInOutQuint) between positions A and B.

> Vector3 position_a = new Vector3(0, 0, 0), position_b = new Vector3(1, 0, 3);
> float interpolationValue = < timePastInMovement / totalMovementDuration >;
> float eased_interpolationValue = EasingTools.ApplyEase(interpolationValue, EasingTools.EaseType.EaseInOutQuint);
> Vector3 easedMovement = Vector3.Lerp(position_a, position_b, eased_interpolationValue);


## SecondOrderDynamics: 

A collection of tools to ease movement, rotation, scaling, and other numerical variables procedurally.

Most users will mainly use the **SecondOrderTransform** component. When active, **SecondOrderTransform **components take control of one Transform element (position, rotation, or scale) of the GameObject they are attached to. That element's information is updated by following another Transform's position, rotation, or scale and applying an easing process. As a result, **SecondOrderTransform **allows users to create easing effects similar to those made by **EasingTools.ApplyEase()**, without the limitation of needing an endpoint to the transition.

To start using the **SecondOrderTransform** component, drag it into a GameObject's inspector from the Project tab or add it using the "Add Component" button. The following are the options that the component offers to users:


### Run Dynamics in Editor:
When enabled, the **SecondOrderTransform** script will work without entering Play Mode.

**_Access through code:_ **
This element should only be accessed through the inspector, as it is only meant for debug purposes.

**_WARNINGS:_ **
Use only as a Debug tool. SecondOrderTransform might display unintended behavior during Editor Mode:
-  Due to Unity's restrictions on script execution, the "_FixedUpdate" _refresh mode is incompatible with Editor Mode. Instead, SecondOrderTransform will default to "_Update_" refresh mode during Editor Mode.
-  Execution during Editor Mode is incompatible with the "Stored _Transform Data_" input method.


### Dynamics Configuration - Dynamic Type:
Determines whether this component should apply to the Position, Rotation, or Scale of the attached GameObject.

**_Access through code:_ **
The Dynamic Type can be accessed and changed through the "_whichDynamicType_" field.

> SecondOrderTransform soTransform = GetComponent<SecondOrderTransform>();
> soTransform.whichDynamicType = DynamicsType.position;

**_WARNING:_ **
Use only as a Debug too


-------------------------------------

**Easing Toolkit** is a Unity Package developed by Pablo Granado Valdivielso ("Veguista").
