using System;
using UnityEngine;

namespace EasingToolkit.SecondOrderDynamics
{
    // Logic of the system exposed by t3ssel8r in the following video: https://www.youtube.com/watch?v=KPoeNZZ6H4s

    public struct SecondOrder_Rotation
    {
        internal Quaternion _storedUnsmoothedRotation, _lastSmoothedRotation;

        SecondOrder_1D _dynamics_X, _dynamics_Y, _dynamics_Z, _dynamics_W;

        public Quaternion Update(float deltaTime, Quaternion targetValue)
        {
            // Error catching.
            if (deltaTime <= 0)
            {
                Debug.LogWarning("Second Order Easing cannot handle 0 or negative values for deltaTime.");
                return _lastSmoothedRotation;
            }

            // Making sure the new rotation is the shortest-way-around to the previous rotation.
            bool hasUnrolledQuaternion = false;

            if (ReturnShortestWayAroundToRotation(ref targetValue, _lastSmoothedRotation))
                hasUnrolledQuaternion = true;


            _lastSmoothedRotation.x = _dynamics_X.Update(deltaTime, targetValue.x);
            _lastSmoothedRotation.y = _dynamics_Y.Update(deltaTime, targetValue.y);
            _lastSmoothedRotation.z = _dynamics_Z.Update(deltaTime, targetValue.z);
            _lastSmoothedRotation.w = _dynamics_W.Update(deltaTime, targetValue.w);


            if (hasUnrolledQuaternion)
                _lastSmoothedRotation.Normalize();

            
            return _lastSmoothedRotation;
        }

        public void ChangeConstants(SO_Constants constants)
        {
            _dynamics_X.ChangeConstants(constants);
            _dynamics_Y.ChangeConstants(constants);
            _dynamics_Z.ChangeConstants(constants);
            _dynamics_W.ChangeConstants(constants);
        }
        public void ChangeConstants(float frequency, float dampening, float initialResponse) => ChangeConstants(new SO_Constants(frequency, dampening, initialResponse));

        public void Reset()
        {
            _lastSmoothedRotation = _storedUnsmoothedRotation;

            _dynamics_X.Reset();
            _dynamics_Y.Reset();
            _dynamics_Z.Reset();
            _dynamics_W.Reset();
        }


        #region Private Quaternion functions

        // To understand what it means to unroll Quaternions, check https://theorangeduck.com/page/unrolling-rotations.

        /// <summary>
        /// Unrolls the rotation based on the old Rotation provided.
        /// </summary>
        /// <param name="newRotation">The rotation that might be unrolled if necessary.</param>
        /// <param name="oldRotation"></param>
        /// <returns>True if the rotation has been unrolled.</returns>
        private bool ReturnShortestWayAroundToRotation(ref Quaternion newRotation, Quaternion oldRotation)
        {
            if (Quaternion.Dot(newRotation, oldRotation) < 0.0f)
            {
                newRotation = NegativeOfAQuaternion(newRotation);
                return true;
            }

            // Else
            return false;
        }

        private Quaternion NegativeOfAQuaternion(Quaternion positiveQuaternion)
            => new Quaternion(-positiveQuaternion.x, -positiveQuaternion.y, -positiveQuaternion.z, -positiveQuaternion.w);

        #endregion

        #region Constructors

        // Regular constructor (Used in outlier cases)
        public SecondOrder_Rotation(float f, float z, float r, Quaternion originalRotation)
        {
            SO_Constants myConstants = new SO_Constants(f, z, r);

            _dynamics_X = new SecondOrder_1D(myConstants, originalRotation.x);
            _dynamics_Y = new SecondOrder_1D(myConstants, originalRotation.y);
            _dynamics_Z = new SecondOrder_1D(myConstants, originalRotation.z);
            _dynamics_W = new SecondOrder_1D(myConstants, originalRotation.w);

            _storedUnsmoothedRotation = originalRotation;
            _lastSmoothedRotation = originalRotation;
        }

        // Streamlined constructor.
        internal SecondOrder_Rotation(SO_Constants constants, Quaternion originalRotation)
        {
            _dynamics_X = new SecondOrder_1D(constants, originalRotation.x);
            _dynamics_Y = new SecondOrder_1D(constants, originalRotation.y);
            _dynamics_Z = new SecondOrder_1D(constants, originalRotation.z);
            _dynamics_W = new SecondOrder_1D(constants, originalRotation.w);

            _storedUnsmoothedRotation = originalRotation;
            _lastSmoothedRotation = originalRotation;
        }


        #endregion
    }
}
