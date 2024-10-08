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



-------------------------------------

**Easing Toolkit** is a Unity Package developed by Pablo Granado Valdivielso ("Veguista").
