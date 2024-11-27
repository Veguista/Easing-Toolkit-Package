using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using SecondOrderDynamics;
using UnityEditor.UIElements;

namespace CustomUI
{
    [CustomEditor(typeof(SecondOrderTransform))]
    public class SecondOrderTransformEditor : Editor
    {
        public VisualTreeAsset inspector_VisualTree_UXML;

        // Reference to the original script that creates the Editor.
        SecondOrderTransform transformScript;

        // References to parts of the inspector.
        VisualElement containerRunDynamicsInEditor, activeAxisContainer, containerFollowTransform, 
            containerNoFollowTransformWarning, containerStoredDataWarning, dataOriginDropdown, refreshModeDropdown;
        GraphUI graphVisualElement;

        public override VisualElement CreateInspectorGUI()
        {
            transformScript = target as SecondOrderTransform;

            // Create a new VisualElement to be the root of our Inspector UI.
            VisualElement myInspectorRoot = new VisualElement();
            inspector_VisualTree_UXML.CloneTree(myInspectorRoot);

            // Binding our runDynamicsInEditor Toggle.
            containerRunDynamicsInEditor = myInspectorRoot.Q<VisualElement>("containerRunDynamicsInEditor");
            Toggle runInEditorToggle = containerRunDynamicsInEditor.Q<ScalableButton>("runDynamicsInEditorToggle").Q<Toggle>();
            runInEditorToggle.bindingPath = "_runInEditor";
            runInEditorToggle.RegisterCallback<ChangeEvent<bool>>(ChangeRunInEditor);

            // Altering the Editor when we change our type of Dinamic.
            activeAxisContainer = myInspectorRoot.Q<VisualElement>("axisContainer");
            myInspectorRoot.Q<DropdownField>("dynamicTypeDropdown").RegisterCallback<ChangeEvent<string>>(ChangeDynamicType);

            // Binding and setting up our axis.
            Button applyXAxisButton = myInspectorRoot.Q<Button>("applyXAxisButton");
            Button applyYAxisButton = myInspectorRoot.Q<Button>("applyYAxisButton");
            Button applyZAxisButton = myInspectorRoot.Q<Button>("applyZAxisButton");

            StyleButtonToFitBool(applyXAxisButton, transformScript.axisToFollow[0]);
            StyleButtonToFitBool(applyYAxisButton, transformScript.axisToFollow[1]);
            StyleButtonToFitBool(applyZAxisButton, transformScript.axisToFollow[2]);

            applyXAxisButton.clicked += () => PressedButtonForAxis(applyXAxisButton, 0);
            applyYAxisButton.clicked += () => PressedButtonForAxis(applyYAxisButton, 1);
            applyZAxisButton.clicked += () => PressedButtonForAxis(applyZAxisButton, 2);


            // Setting up events for how the InputMode and the followTransform work.
            containerFollowTransform = myInspectorRoot.Q<VisualElement>("containerFollowTransform");
            containerNoFollowTransformWarning = myInspectorRoot.Q<VisualElement>("containerNoFollowTransformWarning");
            containerStoredDataWarning = myInspectorRoot.Q<VisualElement>("containerStoredDataWarning");
            dataOriginDropdown = myInspectorRoot.Q<VisualElement>("dataOriginDropdown");
            refreshModeDropdown = myInspectorRoot.Q<VisualElement>("refreshModeDropdown");

            myInspectorRoot.Q<PropertyField>("followTransformPropertyField").RegisterValueChangeCallback(ChangedFollowTransform);
            myInspectorRoot.Q<DropdownField>("inputMethodDropdown").RegisterCallback<ChangeEvent<string>>(ChangedInputMethod);


            // Setting up our dynamics parameters.
            Slider frequencySlider = myInspectorRoot.Q<Slider>("frequencySlider");
            Slider dampeningSlider = myInspectorRoot.Q<Slider>("dampeningSlider");
            Slider initialResponseSlider = myInspectorRoot.Q<Slider>("initialResponseSlider");

            frequencySlider.Q<Label>().style.minWidth = 115;
            dampeningSlider.Q<Label>().style.minWidth = 115;
            initialResponseSlider.Q<Label>().style.minWidth = 115;

            frequencySlider.RegisterValueChangedCallback(UpdateDynamicParameters);
            dampeningSlider.RegisterValueChangedCallback(UpdateDynamicParameters);
            initialResponseSlider.RegisterValueChangedCallback(UpdateDynamicParameters);

            // Initializing our Graph.
            graphVisualElement = myInspectorRoot.Q<GraphUI>();
            UpdateGraphVisuals();

            // Return the finished Inspector UI.
            return myInspectorRoot;
        }

        #region Graphing the data

        const int _resolution = 100;

        static GraphData CalculateGraph(SO_Constants constants, float frequency)
        {
            GraphData graphData;

            // We use a 1D SO_Dynamics class to plot our values.
            SecondOrder_1D graphDynamics = new SecondOrder_1D(constants, 0);

            // Size of the graph in the X Axis.
            graphData.leftLimit = Mathf.Clamp(-10 / Mathf.Sqrt(frequency), -.3f, -.8f);
            graphData.rightLimit = Mathf.Min(7 / frequency + 1f, 9.5f);

            // Calculating the initial and final points.
            graphData.xLocation_FirstPoint = 0;
            graphData.xLocation_LastPoint = graphData.rightLimit;

            float distanceBetweenPoints = (graphData.xLocation_LastPoint - graphData.xLocation_FirstPoint) / (_resolution - 1);

            List<float> myResults = new List<float>();

            for (int i = 0; i < _resolution; i++)
                myResults.Add(graphDynamics.Update(distanceBetweenPoints, 1));

            graphData.graphPoints = myResults.ToArray();

            // Calculating Y limits.
            graphData.bottomLimit = Mathf.Min(Mathf.Min(myResults.ToArray()) - 0.3f, -.4f);
            graphData.upLimit = Mathf.Max(Mathf.Max(myResults.ToArray()) + 0.3f, 1.4f);

            // Special lines. (At X = 0; Y = 0; and Y = 1)
            Color color_framingLinesAtVertical_0 = new Color(0.6f, 0.6f, 0.6f);
            Color color_framingLinesAtVertical_10 = new Color(0.45f, 0.2f, 0.2f);
            Color color_framingLinesAtHorizontal_0 = new Color(.45f, .25f, .6f, 1);
            Color color_framingLineAtHorizontal_1 = new Color(.5f, .8f, .3f, 1);
            float framingLinesWidth = 2f;

            GraphData.SpecialLine specialVertical_0 = new GraphData.SpecialLine 
                { location = 0, lineColor = color_framingLinesAtVertical_0, lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialVertical_1 = new GraphData.SpecialLine
                { location = 1, lineColor = Color.Lerp(color_framingLinesAtVertical_0, color_framingLinesAtVertical_10, 0.1f), lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialVertical_2 = new GraphData.SpecialLine
                { location = 2, lineColor = Color.Lerp(color_framingLinesAtVertical_0, color_framingLinesAtVertical_10, 0.2f), lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialVertical_3 = new GraphData.SpecialLine
                { location = 3, lineColor = Color.Lerp(color_framingLinesAtVertical_0, color_framingLinesAtVertical_10, 0.3f), lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialVertical_4 = new GraphData.SpecialLine
                { location = 4, lineColor = Color.Lerp(color_framingLinesAtVertical_0, color_framingLinesAtVertical_10, 0.4f), lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialVertical_5 = new GraphData.SpecialLine
                { location = 5, lineColor = Color.Lerp(color_framingLinesAtVertical_0, color_framingLinesAtVertical_10, 0.5f), lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialVertical_6 = new GraphData.SpecialLine
                { location = 6, lineColor = Color.Lerp(color_framingLinesAtVertical_0, color_framingLinesAtVertical_10, 0.6f), lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialVertical_7 = new GraphData.SpecialLine
                { location = 7, lineColor = Color.Lerp(color_framingLinesAtVertical_0, color_framingLinesAtVertical_10, 0.7f), lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialVertical_8 = new GraphData.SpecialLine
                { location = 8, lineColor = Color.Lerp(color_framingLinesAtVertical_0, color_framingLinesAtVertical_10, 0.8f), lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialVertical_9 = new GraphData.SpecialLine
                { location = 9, lineColor = Color.Lerp(color_framingLinesAtVertical_0, color_framingLinesAtVertical_10, 0.9f), lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialVertical_10 = new GraphData.SpecialLine
                { location = 10, lineColor = Color.Lerp(color_framingLinesAtVertical_0, color_framingLinesAtVertical_10, 1f), lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialHorizontal_0 = new GraphData.SpecialLine
                { location = 0, lineColor = color_framingLinesAtHorizontal_0, lineThickness = framingLinesWidth };
            GraphData.SpecialLine specialHorizontal_1 = new GraphData.SpecialLine
                { location = 1, lineColor = color_framingLineAtHorizontal_1, lineThickness = framingLinesWidth };

            graphData.specialVerticalLines = new GraphData.SpecialLine[] { specialVertical_0, specialVertical_1, specialVertical_2, 
                specialVertical_3, specialVertical_4, specialVertical_5, specialVertical_6, specialVertical_7, specialVertical_8, 
                specialVertical_9, specialVertical_10 }; 
            graphData.specialHorizontalLines = new GraphData.SpecialLine[] { specialHorizontal_0, specialHorizontal_1 };

            return graphData;
        }

        private void UpdateGraphVisuals()
        {
            graphVisualElement.UpdateGraph(CalculateGraph(transformScript.MyConstants, transformScript.Frequency));
        }

        #endregion

        #region RegisterCallback methods

        // Changing the RunInEditor Toggle color.
        private void ChangeRunInEditor(ChangeEvent<bool> evt)
        {
            if (evt.newValue == true)
            {
                containerRunDynamicsInEditor.style.backgroundColor = _onButtonBackgroundColor;
                containerRunDynamicsInEditor.Q<Label>().style.color = _onButtonTextColor;
            }
            else
            {
                containerRunDynamicsInEditor.style.backgroundColor = _offButtonBackgroundColor;
                containerRunDynamicsInEditor.Q<Label>().style.color = _offButtonTextColor;
            }
        }

        // Dynamics Type dropdown.
        private void ChangeDynamicType(ChangeEvent<string> evt)
        {
            if (evt.newValue == evt.previousValue)
                return;

            switch (evt.newValue)
            {
                case "Position":
                    ToggleVisibilityOfAxisContainer(true);
                    break;

                case "Rotation":
                    ToggleVisibilityOfAxisContainer(false);
                    break;

                case "Scale":
                    ToggleVisibilityOfAxisContainer(true);
                    break;

                default:
                    Debug.LogError("Dynamic type [" + evt.newValue + "]not recognized.");
                    return;
            }

            // We force an initiation of the dynamics.
            transformScript.ResetDynamics();

            // Local functions.
            void ToggleVisibilityOfAxisContainer(bool visible)
            {
                if (visible)
                    activeAxisContainer.style.display = DisplayStyle.Flex;
                else
                    activeAxisContainer.style.display = DisplayStyle.None;
            }
        }

        // Axis Buttons
        private void PressedButtonForAxis(Button button, int whichButton)
        {
            bool selectedAxisNewValue = false;

            switch (whichButton)
            {
                case 0: // X Axis Button
                    selectedAxisNewValue = !transformScript.axisToFollow[0];
                    transformScript.axisToFollow[0] = selectedAxisNewValue;
                    break;

                case 1: // Y Axis Button
                    selectedAxisNewValue = !transformScript.axisToFollow[1];
                    transformScript.axisToFollow[1] = selectedAxisNewValue;
                    break;

                case 2: // Z Axis Button
                    selectedAxisNewValue = !transformScript.axisToFollow[2];
                    transformScript.axisToFollow[2] = selectedAxisNewValue;
                    break;

                default:
                    Debug.LogError("Button " + whichButton + " does not exist amongst the axis buttons.");
                    break;
            }

            // Changing the visuals of the button.
            StyleButtonToFitBool(button, selectedAxisNewValue);
        }

        // Changed Input Mode
        private void ChangedInputMethod(ChangeEvent<string> evt)
        {
            if (evt.newValue == evt.previousValue)
                return;

            switch (evt.newValue)
            {
                case "Follow Transform":
                    containerFollowTransform.style.display = DisplayStyle.Flex;
                    containerStoredDataWarning.style.display = DisplayStyle.None;
                    dataOriginDropdown.style.display = DisplayStyle.Flex;
                    refreshModeDropdown.style.display = DisplayStyle.Flex;
                    break;

                case "Stored Transform Data":
                    containerFollowTransform.style.display = DisplayStyle.None;
                    containerStoredDataWarning.style.display = DisplayStyle.Flex;
                    dataOriginDropdown.style.display = DisplayStyle.None;
                    refreshModeDropdown.style.display = DisplayStyle.None;
                    break;

                default:
                    Debug.LogError("Input mode " + evt.newValue + " does not exist amongst the possible modes in this function.");
                    return;
            }
        }

        // Changed Follow Transform Property
        private void ChangedFollowTransform(SerializedPropertyChangeEvent evt)
        {
            Transform newTransform = evt.changedProperty.boxedValue as Transform;

            if (newTransform != null)
                containerNoFollowTransformWarning.style.display = DisplayStyle.None;
            else
                containerNoFollowTransformWarning.style.display = DisplayStyle.Flex;
        }

        // Updating our dynamic parameters.
        private void UpdateDynamicParameters(ChangeEvent<float> evt)
        {
            transformScript.UpdateSecondOrderParameters();
            UpdateGraphVisuals();
        }

        #endregion

        #region Colors

        // Axis buttons
        Color _offButtonBackgroundColor = new Color(0.28f, 0.22f, 0.22f);
        Color _onButtonBackgroundColor = new Color(0.42f, 0.82f, 0.31f);

        Color _offButtonTextColor = new Color(0.75f, 0.75f, 0.75f);
        Color _onButtonTextColor = new Color(0.16f, 0.16f, 0.16f);

        #endregion

        #region Local methods

        private void StyleButtonToFitBool(in Button button, bool boolIsOn)
        {
            if(boolIsOn)
            {
                button.style.backgroundColor = _onButtonBackgroundColor;
                button.style.color = _onButtonTextColor;
            }
            else
            {
                button.style.backgroundColor = _offButtonBackgroundColor;
                button.style.color = _offButtonTextColor;
            }
        }

        #endregion
    }
}

