using System;
using UnityEngine;

namespace EasingToolkit.SecondOrderDynamics
{
    // Logic of the system exposed by t3ssel8r in the following video: https://www.youtube.com/watch?v=KPoeNZZ6H4s

    public struct SecondOrder_1D
    {
        internal float _storedUnsmoothedTargetValue;     // Previous input.
        internal float _lastSmoothedValue, _yd;           // State Variables.
        internal SO_Constants _myConstants;              // The w, z, d, k1, k2, k3 constants;

        public float Update(float deltaTime, float targetValue)
        {
            // Error catching.
            if (deltaTime <= 0)
            {
                Debug.LogWarning("Second Order Easing cannot handle 0 or negative values for deltaTime.");
                return _lastSmoothedValue;
            }

            // Estimate velocity of change of the target value.
            float valueVelocity = (targetValue - _storedUnsmoothedTargetValue) / deltaTime;
            _storedUnsmoothedTargetValue = targetValue;


            float k1_stable, k2_stable;
            if (_myConstants.w * deltaTime < _myConstants.z) // Clamp k2 to guarantee stability without jitter.
            {
                k1_stable = _myConstants.k1;
                k2_stable = Mathf.Max(_myConstants.k2, deltaTime * deltaTime 
                    / 2 + deltaTime * _myConstants.k1 / 2, deltaTime * _myConstants.k1);
            }
            else    // Use pole matching when the system is very fast.
            {
                float t1 = Mathf.Exp(-_myConstants.z * _myConstants.w * deltaTime);
                float alpha = 2 * t1 * (_myConstants.z <= 1 ? Mathf.Cos(deltaTime * _myConstants.d) : (float)Math.Cosh(deltaTime * _myConstants.d));
                float beta = t1 * t1;
                float t2 = deltaTime / (1 + beta - alpha);
                k1_stable = (1 - beta) * t2;
                k2_stable = deltaTime * t2;
            }

            _lastSmoothedValue = _lastSmoothedValue + deltaTime * _yd; // Integrate position by velocity.
            _yd = _yd + deltaTime * (targetValue + _myConstants.k3 * valueVelocity - _lastSmoothedValue - k1_stable * _yd) 
                / k2_stable; // Integrate velocity by acceleration.
            
            return _lastSmoothedValue;
        }

        public void UpdateConstants(SO_Constants constants) => _myConstants = constants;

        public void Reset()
        {
            _lastSmoothedValue = _storedUnsmoothedTargetValue;
            _yd = 0;
        }


        #region Constructors

        // Regular constructor (Used in outlier cases)
        public SecondOrder_1D(float f, float z, float r, float originalValue)
        {
            _myConstants = new SO_Constants(f, z, r);

            // Initialize varibles.
            _storedUnsmoothedTargetValue = originalValue;
            _lastSmoothedValue = originalValue;
            _yd = 0;
        }


        // Fast constructor.
        public SecondOrder_1D(SO_Constants constants, float originalValue)
        {
            _myConstants = constants;

            // Initialize varibles.
            _storedUnsmoothedTargetValue = originalValue;
            _lastSmoothedValue = originalValue;
            _yd = 0;
        }

        #endregion
    }
}
