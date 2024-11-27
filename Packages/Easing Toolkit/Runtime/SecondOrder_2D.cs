using System;
using UnityEngine;

namespace SecondOrderDynamics
{
    // Logic of the system exposed by t3ssel8r in the following video: https://www.youtube.com/watch?v=KPoeNZZ6H4s

    public struct SecondOrder_2D
    {
        internal Vector2 _storedUnsmoothedPosition;      // Previous input.
        internal Vector2 _lastSmoothedPosition, _yd;     // State Variables.
        internal SO_Constants _myConstants;              // The w, z, d, k1, k2, k3 constants;


        public Vector2 Update(float deltaTime, Vector2 targetValue, Vector2 inputVelocity)
        {
            // Error catching.
            if (deltaTime <= 0)
            {
                Debug.LogWarning("Second Order Easing cannot handle 0 or negative values for deltaTime.");
                return _lastSmoothedPosition;
            }

            float k1_stable, k2_stable;
            if (_myConstants.w * deltaTime < _myConstants.z) // Clamp k2 to guarantee stability without jitter.
            {
                k1_stable = _myConstants.k1;
                k2_stable = Mathf.Max(_myConstants.k2,
                    deltaTime * deltaTime / 2 + deltaTime * _myConstants.k1 / 2,
                    deltaTime * _myConstants.k1);
            }
            else    // Use pole matching when the system is very fast.
            {
                float t1 = Mathf.Exp(-_myConstants.z * _myConstants.w * deltaTime);
                float alpha = 2 * t1 * (_myConstants.z <= 1 ? Mathf.Cos(deltaTime * _myConstants.d)
                    : (float)Math.Cosh(deltaTime * _myConstants.d));
                float beta = t1 * t1;
                float t2 = deltaTime / (1 + beta - alpha);
                k1_stable = (1 - beta) * t2;
                k2_stable = deltaTime * t2;
            }

            _storedUnsmoothedPosition = targetValue;
            _lastSmoothedPosition = _lastSmoothedPosition + deltaTime * _yd; // Integrate targetValue by velocity.
            _yd = _yd + deltaTime * (targetValue + _myConstants.k3 * inputVelocity - _lastSmoothedPosition - k1_stable * _yd)
                / k2_stable; // Integrate velocity by acceleration.
            return _lastSmoothedPosition;
        }

        public Vector2 Update(float deltaTime, Vector2 targetValue)
        {
            // Error catching.
            if (deltaTime <= 0)
            {
                Debug.LogWarning("Second Order Easing cannot handle 0 or negative values for deltaTime.");
                return _lastSmoothedPosition;
            }

            // Calculating the velocity.
            Vector2 inputVelocity = (targetValue - _storedUnsmoothedPosition) / deltaTime;
            return Update(deltaTime, targetValue, inputVelocity);
        }

        public void ChangeConstants(SO_Constants constants) => _myConstants = constants;
        public void ChangeConstants(float frequency, float dampening, float initialResponse) => ChangeConstants(new SO_Constants(frequency, dampening, initialResponse));

        public void Reset()
        {
            _lastSmoothedPosition = _storedUnsmoothedPosition;
            _yd = Vector2.zero;
        }


        #region Constructors

        // Regular constructor (Used in outlier cases)
        internal SecondOrder_2D(float f, float z, float r, Vector2 originalVector2)
        {
            _myConstants = new SO_Constants(f, z, r);

            // Initialize varibles.
            _storedUnsmoothedPosition = originalVector2;
            _lastSmoothedPosition = originalVector2;
            _yd = Vector2.zero;
        }

        // Streamlined constructor.
        internal SecondOrder_2D(SO_Constants constants, Vector2 originalVector2)
        {
            _myConstants = constants;

            // Initialize varibles.
            _storedUnsmoothedPosition = originalVector2;
            _lastSmoothedPosition = originalVector2;
            _yd = Vector2.zero;
        }

        #endregion
    }
}