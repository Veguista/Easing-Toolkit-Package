using UnityEngine;

namespace EasingToolkit.SecondOrderDynamics
{
    [ExecuteAlways]
    public class SecondOrderTransform : MonoBehaviour
    {
        #region Parameters

        [SerializeField] float _frequency = 1.0f;
        [SerializeField] float _dampening = 1.0f;
        [SerializeField] float _initialResponse = 0.0f;

        // Public access
        public float Frequency
        {
            get => _frequency;

            set
            {
                if (value == _frequency)
                    return;

                // Warning if the new Frequency value is outside the accepted range (0, +_infinite]
                if (value <= 0)
                {
                    Debug.LogWarning("Negative or equal to 0 values are not supported in the [Frequency] parameter of a " +
                        "Second Order Dynamics system." + "\nThe frequency value has not changed as a result.");
                    return;
                }

                _frequency = value;

                // Updating the constants in our Dynamics.
                UpdateSecondOrderParameters();
            }
        }

        public float Dampening
        {
            get => _dampening;

            set
            {
                if(value == _dampening)
                    return;

                // Warning if the new Dampening value is outside the accepted range [0, +_infinite]
                if (value < 0)
                {
                    Debug.LogWarning("Negative values are not supported in the [Dampening] parameter of a Second Order Dynamics system." +
                    "\nThe dampening value has not changed as a result.");
                    return;
                }

                // Warning for Dampening values of 0.
                if (value == 0)
                    Debug.LogWarning("A dampening value of 0 will result in a system that won't ever deccelerate. " +
                        "Are you sure that you want to set to 0 the [Dampening] parameter of a Second Order Dynamics system?");

                _dampening = value;

                // Updating the constants in our Dynamics.
                UpdateSecondOrderParameters();
            }
        }

        public float InitialResponse
        {
            get => _initialResponse;

            set
            {
                if(value == _initialResponse)
                    return;

                _initialResponse = value;

                // Updating the constants in our Dynamics.
                UpdateSecondOrderParameters();
            }
        }


        #endregion

        public Transform followTransform;

        #region Configuration variables.

        public enum DynamicsType { position, rotation, scale };
        [SerializeField] DynamicsType _whichDynamicType = DynamicsType.position;
        public DynamicsType WhichDynamicType
        {
            get => _whichDynamicType;

            set
            {
                // Nothing happens if the new type is the same as the current type.
                if (_whichDynamicType == value)
                    return;

                _whichDynamicType = value;

                // Initializing the new dynamic.
                ResetDynamics();
            }
        }

        public enum TypeOfDataInput { followTransform, storedTransformData };
        public TypeOfDataInput inputMode = TypeOfDataInput.followTransform;

        public enum TypeOfSpace { localSpace, worldSpace };
        public TypeOfSpace obtainTransformDataFromLocalOrWorld = TypeOfSpace.worldSpace;
        public TypeOfSpace applyDynamicsToLocalOrWorld = TypeOfSpace.worldSpace;


        // When applying Second Order Dynamics to Position & Scale, we can choose to ignore certain axis.
        public bool[] axisToFollow = new bool[] { true, true, true };

        public enum TypeOfDynamicsRefresh { update, fixedUpdate, lateUpdate };
        public TypeOfDynamicsRefresh refreshMode = TypeOfDynamicsRefresh.fixedUpdate;


        [SerializeField] bool _runInEditor = false;

        #endregion

        #region Local Variables
        
        // We make _myConstants NULLABLE to make sure that we don't need to be initializing unless we want to update our constants.
        public SO_Constants MyConstants
        {
            get
            {
                if(_myConstants == null)
                    _myConstants = new SO_Constants(_frequency, _dampening, _initialResponse);

                return _myConstants.Value;
            }

            set
            {
                _myConstants = value;
            }
        }

        SO_Constants? _myConstants;

        // Dynamics
        SecondOrder_3D _positionDynamics;
        SecondOrder_3D _scaleDynamics;
        SecondOrder_Rotation _rotationDynamics;

        // Used to bypass obtaining Transform data from a real Transform.
        TransformData _storedTransformData;

        #endregion


        // We do this at OnEnable and not at Start to restart the selected Dynamic every time.
        private void OnEnable()
        {
#if UNITY_EDITOR
            if (!_runInEditor && !Application.isPlaying)
                return;
#endif

            InitializeSelectedDynamic();
        }

        private void Update()
        {
            // When the inputMode [storedTransformData] is selected, all updates to the Dynamics are controlled through the 
            // InputTransformDataAndUpdateDynamics() method.
            if (inputMode == TypeOfDataInput.storedTransformData)
                return;

#if UNITY_EDITOR        // We allow for Dynamic Updates in our Editor only if we have the _runInEditor option enabled.

            if (_runInEditor && !Application.isPlaying           // Fixed update does not work in editor mode, so we put it here.
                && (refreshMode == TypeOfDynamicsRefresh.update || refreshMode == TypeOfDynamicsRefresh.fixedUpdate))
            {
                UpdateDynamics();
                return;
            }
            else if (!Application.isPlaying)
                return;
#endif

            // We only run this code once, in our selected Refresh Mode.
            if (refreshMode != TypeOfDynamicsRefresh.update)
                return;

            UpdateDynamics();
        }

        private void LateUpdate()
        {
            // When the inputMode [storedTransformData] is selected, all updates to the Dynamics are controlled through the 
            // InputTransformDataAndUpdateDynamics() method.
            if (inputMode == TypeOfDataInput.storedTransformData)
                return;

            // We only run this code once, in our selected Refresh Mode.
            if (refreshMode != TypeOfDynamicsRefresh.lateUpdate)
                return;

#if UNITY_EDITOR    // We allow for Dynamic Updates in our Editor only if we have the _runInEditor option enabled.

            if (_runInEditor && !Application.isPlaying)
            {
                UpdateDynamics();
                return;
            }
            else if (!Application.isPlaying)
                return;
#endif

            UpdateDynamics();
        }

        private void FixedUpdate()
        {
#if UNITY_EDITOR    // We allow for Dynamic Updates in our Editor only if we have the _runInEditor option enabled.

            if (!Application.isPlaying)
            {
                Debug.LogError("How the hell is this working in Editor mode.");
                return;
            }
#endif

            // We only run this code once, in our selected Refresh Mode.
            if (refreshMode != TypeOfDynamicsRefresh.fixedUpdate)
                return;

            // When the inputMode [storedTransformData] is selected, all updates to the Dynamics are controlled through the 
            // InputTransformDataAndUpdateDynamics() method.
            if (inputMode == TypeOfDataInput.storedTransformData)
                return;

            UpdateDynamics();
        }


        #region Local Functions

        /// <summary>
        /// Updates the values of _frequency, _dampening, and initial response in the scripts Dynamics.
        /// <para>It also clamps those same values.</para>
        /// </summary>
        public void UpdateSecondOrderParameters()
        {
            // Recalculating the Constants.
            MyConstants = new SO_Constants(_frequency, _dampening, _initialResponse);

            // Applying the new Constants.
            switch (_whichDynamicType)
            {
                case DynamicsType.position:
                    _positionDynamics.UpdateConstants(MyConstants);
                    break;

                case DynamicsType.rotation:
                    _rotationDynamics.UpdateConstants(MyConstants);
                    break;

                case DynamicsType.scale:
                    _scaleDynamics.UpdateConstants(MyConstants);
                    break;
            }
        }

        /// <summary>
        /// Takes the selected second order mode (position, rotation, or scale) and Resets the selected dynamic.
        /// <para>This method assumes that any the SO_Constants values are updated.</para>
        /// This method does not initialize dynamics when the inputMode is StoredTransformData, those are initialized on the moment.
        /// </summary>
        private void InitializeSelectedDynamic()
        {
            // If we are using StoredTransformData, we initialize it at InputTransformDataAndUpdateDynamics().
            if (inputMode == TypeOfDataInput.storedTransformData)
                return;
            
            // Anything beyond this point has the inputMode [followTransform].
            if(followTransform == null)
            {
                Debug.LogError("Cannot initialize the dynamics of the SecondOrderTransform cause there is a missing reference to a " +
                    "[followTransform].\nSet the reference or change the input mode to [storedTransformData].");
                return;
            }

            switch (_whichDynamicType)
            {
                case DynamicsType.position:

                    Vector3 startingPosition = obtainTransformDataFromLocalOrWorld == TypeOfSpace.worldSpace ?
                            followTransform.position : followTransform.localPosition;

                    _positionDynamics = new SecondOrder_3D(MyConstants, startingPosition);
                    
                    break;

                case DynamicsType.rotation:
                    
                    Quaternion startingRotation = obtainTransformDataFromLocalOrWorld == TypeOfSpace.worldSpace ?
                        followTransform.rotation : followTransform.localRotation;

                    _rotationDynamics = new SecondOrder_Rotation(MyConstants, startingRotation);

                    break;

                case DynamicsType.scale:

                    Vector3 startingScale = obtainTransformDataFromLocalOrWorld == TypeOfSpace.worldSpace ? 
                        followTransform.lossyScale : followTransform.localScale;

                    _scaleDynamics = new SecondOrder_3D(MyConstants, startingScale);

                    break;
            }
        }

        /// <summary>
        /// Calls the relevant SO_Dynamic and Updates its values. Then, it applies those values.
        /// </summary>
        private void UpdateDynamics()
        {            
            if (IsFrequencyUnder0())
                return;

            // Making sure that time scale hasn't been used to pause the game.
            float deltaTime = Time.deltaTime;
            if(deltaTime <= 0)
                return;


            switch (_whichDynamicType)
            {
                case DynamicsType.position:
                    
                    Vector3 targetPosition;

                    if (inputMode == TypeOfDataInput.storedTransformData)
                        targetPosition = _storedTransformData.position;
                    else if (obtainTransformDataFromLocalOrWorld == TypeOfSpace.worldSpace)
                        targetPosition = followTransform.position;
                    else
                        targetPosition = followTransform.localPosition;


                    Vector3 easedPosition = _positionDynamics.Update(deltaTime, targetPosition);

                    Vector3 thisTransformsPosition = applyDynamicsToLocalOrWorld == TypeOfSpace.worldSpace ?
                        transform.position : transform.localPosition;

                    easedPosition = new Vector3(axisToFollow[0] ? easedPosition.x : thisTransformsPosition.x,
                                                axisToFollow[1] ? easedPosition.y : thisTransformsPosition.y,
                                                axisToFollow[2] ? easedPosition.z : thisTransformsPosition.z);


                    if(applyDynamicsToLocalOrWorld == TypeOfSpace.localSpace)
                        transform.localPosition = easedPosition;
                    else
                        transform.position = easedPosition;

                    break;

                case DynamicsType.rotation:
                    
                    Quaternion targetRotation;

                    if (inputMode == TypeOfDataInput.storedTransformData)
                        targetRotation = _storedTransformData.rotation;
                    else if (obtainTransformDataFromLocalOrWorld == TypeOfSpace.worldSpace)
                        targetRotation = followTransform.rotation;
                    else
                        targetRotation = followTransform.localRotation;

                    Quaternion easedRotation = _rotationDynamics.Update(deltaTime, targetRotation);

                    if (applyDynamicsToLocalOrWorld == TypeOfSpace.localSpace)
                        transform.localRotation = easedRotation;
                    else
                        transform.rotation = easedRotation;

                    break;

                case DynamicsType.scale:
                    
                    Vector3 targetScale;

                    if (inputMode == TypeOfDataInput.storedTransformData)
                        targetScale = _storedTransformData.scale;
                    else if (obtainTransformDataFromLocalOrWorld == TypeOfSpace.worldSpace)
                        targetScale = followTransform.lossyScale;
                    else
                        targetScale = followTransform.localScale;

                    Vector3 easedScale = _scaleDynamics.Update(deltaTime, targetScale);

                    Vector3 thisTransformsScale = applyDynamicsToLocalOrWorld == TypeOfSpace.worldSpace ?
                        transform.lossyScale : transform.localScale;

                    easedScale = new Vector3(axisToFollow[0] ? easedScale.x : thisTransformsScale.x,
                                             axisToFollow[1] ? easedScale.y : thisTransformsScale.y,
                                             axisToFollow[2] ? easedScale.z : thisTransformsScale.z);

                    if (applyDynamicsToLocalOrWorld == TypeOfSpace.localSpace)
                        transform.localScale = easedScale;
                    
                    // Because global scale cannot be directly altered (lossyScale is ReadOnly), we do a little trick.
                    else
                    {
                        Transform parent = transform.parent;

                        // By detaching and attaching to the parent, we get around the restriction.
                        transform.parent = null;
                        transform.localScale = easedScale;
                        transform.SetParent(parent);
                    }

                    break;
            }
        }

        /// <summary>
        /// We check that our _frequency has correct values (otherwise the whole thing doesn't work)
        /// </summary>
        /// <returns></returns>
        private bool IsFrequencyUnder0()
        {
            if (_frequency <= 0) 
            {
                Debug.LogError("_frequency must have a value above 0. Current value equals " + _frequency);
                return true;
            }

            return false;
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Sets the initial values of the dynamics to the input transform data.
        /// <para>This method only initializes the part of the transform (position, rotation, or scale) selected for this component.
        /// </para>Should only be used when utilizing the [storedTransformData] inputMode.
        /// </summary>
        /// <param name="transformData"></param>
        public void InitializeDynamicsThroughTransformData(TransformData transformData)
        {
            if (inputMode != TypeOfDataInput.storedTransformData)
            {
                Debug.LogError("Incorrect Input Mode is selected. Users can only initialize SecondOrderDynamics through code when " +
                    "[storedTransformData] mode is selected.");
                return;
            }

            _storedTransformData = transformData;

            switch (_whichDynamicType)
            {
                case DynamicsType.position:
                    _positionDynamics = new SecondOrder_3D(MyConstants, transformData.position);
                    break;

                case DynamicsType.rotation:
                    _rotationDynamics = new SecondOrder_Rotation(MyConstants, transformData.rotation);
                    break;

                case DynamicsType.scale:
                    _scaleDynamics = new SecondOrder_3D(MyConstants, transformData.scale);
                    break;
            }
        }

        /// <summary>
        /// Sets the transform data used by the SecondOrderDynamics to calculate the next state of the Transform it is attached to.
        /// <para>This function calls the Update of the transform this component belongs to.</para>
        /// </summary>
        /// <param name="transformData"></param>
        public void InputTransformDataAndUpdateDynamics(TransformData transformData)
        {
            if(inputMode != TypeOfDataInput.storedTransformData)
            {
                Debug.LogError("Incorrect Input Mode is selected. Users can only input transformData through code when " +
                    "[storedTransformData] mode is selected.");
                return;
            }

            _storedTransformData = transformData;

            UpdateDynamics();
        }

        /// <summary>
        /// Resets the active dynamics. 
        /// <para>If the inputMode has been set to [followTransform] and the dynamics have not been initialized, this method might
        /// result in unwanted behavior.</para>
        /// </summary>
        public void ResetDynamics()
        {
            if (inputMode == TypeOfDataInput.followTransform)
            {
                InitializeSelectedDynamic();
                return;
            }

            // inputMode == storedTransformData
            InitializeDynamicsThroughTransformData(_storedTransformData);
        }

        #endregion


#if UNITY_EDITOR
        // This makes the script execute in real time during editor if the target is selected by the user.
        private void OnDrawGizmos()
        {
            // Only continue if we're repainting the scene & we want to draw in editor.
            if (Event.current.type != EventType.Repaint || !_runInEditor)
                return;


            // Ensure continuous Update calls.
            if (!Application.isPlaying & UnityEditor.Selection.activeTransform == followTransform)
            {
                UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
                UnityEditor.SceneView.RepaintAll();
            }
        }
#endif
    }
}