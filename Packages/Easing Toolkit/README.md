# Readme

**Easing Toolkit** is comprised of 2 complimentary but separate parts:


## EasingTools.ApplyEase():

A function that applies an easing style from a broad collection to a value from 0 to 1.

It is found inside the **EasingTools** class. To gain access to the **EasingTools **class, users can either:
1. (Recommended) Specify that they are using the **EasingToolkit namespace**. To do so, add the following line of code at the beginning of your script "**using EasingToolkit;**".
2. Write the namespace before every instance of the **EasingTools **class (Ex. "**EasingToolkit.EasingTools.ApplyEase(inputFloat, EasingToolkit.EaseType.typeOfEase)**").


The **ApplyEase()** method takes 2 arguments:
1. A float (**inputFloat**), in the [0,1] Range (inclusive). This is the value that will be eased.
2. An EaseType enum (**typeOfEase**), indicating which easing process to be performed.


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
The Dynamic Type can be accessed and changed through the public "W_hichDynamicType_" field.

> SecondOrderTransform soTransform = GetComponent<SecondOrderTransform>();
> soTransform.WhichDynamicType = SecondOrderTransform.DynamicsType.position;

**_WARNINGS:_ **
- Changing the dynamic type forces a reset of the dynamics. The SecondOrderTransform component only tracks one dynamic at a time, and as such transitions between dynamics will not be smooth. Instead, to create such effects, it is recommended to use multiple SecondOrderTransform components simultaneously while altering their parameters.

### 

### Dynamics Configuration - Axes configuration (Apply X / Apply Y / Apply Z):
Determines whether the SecondOrderTransform component applies its output to certain axes. It works only while using the "_Position_" and "_Scale_" dynamic types.

**_Access through code:_ **
The axes configuration can be accessed and changed through the public "axisToFollow" bool array.
- axisToFollow[0] => Apply to X Axis.
- axisToFollow[1] => Apply to Y Axis.
- axisToFollow[2] => Apply to Z Axis.


> SecondOrderTransform soTransform = GetComponent<SecondOrderTransform>();
> soTransform.axisToFollow[1] = false;	// Disabling the application of the SecondOrderTransform component to the Y Axis. 

**_WARNINGS:_ **
- The "_Rotation_" dynamic type ignores this configuration.
- There is a known bug, where altering the Axes configuration through code will not be visually reflected in the Inspector. However, the changes are still taking effect.
- Turning on the configuration of an Axis during runtime will result in a sudden change, as that is not the axes' intended use case. Instead, it is recommended to use multiple SecondOrderTransform components, each with different Axes Configurations, and manipulate their parameters.



### Dynamics Configuration - Input Method:
Determines which method the SecondOrderTransform uses to obtain the input Transform information. There are possible 2 options:
- _Follow Transform_: The Transform information is obtained from another GameObject. The inspector shows the field "Follow Transform" that allows users to select which Transform to obtain the information from.
- _Stored Transform Data_: The Transform data is inputted by the user through code. The user needs to initialize and update the selected Dynamic manually.


**_Access through code:_ **
To change the Input method, users can access and alter the public "inputMode" field.

> SecondOrderTransform soTransform = GetComponent<SecondOrderTransform>();
> soTransform.inputMode = SecondOrderTransform.TypeOfDataInput.storedTransformData;

While using the "_Follow Transform_" input mode, users can access and alter the Transform that is inputting information through the public "_followTransform_" field.

> soTransform._followTransform _= < otherTransform >;

While using the "_Stored Transform Data_" input mode, users need to make use of the "_TransformData_" struct.
The "_TransformData_" struct contains 3 data fields (Vector3 "_position_", Quaternion "_rotation_", and Vector3 "_scale_"), replicating the structure of a Transform class.

To store information in a "_TransformData_" struct, create a new struct and alter a field (only those fields that will be used need to be altered):

> TransformData myTransformData = new TransformData();
> myTransformData.position = new Vector3(1, 0, -1);
> myTransformData.rotation = Quaternion.identity;

To initialize a Dynamic in "_Stored Transform Data_" input mode, call the InitializeDynamicsThroughTransformData() method and pass the TransformData struct with your desired starting Transform information.

> soTransform.InitializeDynamicsThroughTransformData(myTransformData);

Updates to the Transform the SecondOrderTransform component is attached to do not occur autonomously while using the "_Stored Transform Data_" input mode. Instead, they occur whenever the system receives a new input of TransformData. To input TransformData to the component and refresh the dynamics, use the InputTransformDataAndUpdateDynamics() method:

> soTransform.InputTransformDataAndUpdateDynamics(myTransformData);

**_WARNINGS:_ **
- When using the "_Stored Transform Data_" input method, updating the component's dynamics before initializing them will result in an error. The error will indicate that the component is trying to set certain Transform values to NaN.
- The "_Stored Transform Data_" input method requires users to initialize the Dynamics of the component every time the Dynamic type changes.



-------------------------------------

**Easing Toolkit** is a Unity Package developed by Pablo Granado Valdivielso ("Veguista").
