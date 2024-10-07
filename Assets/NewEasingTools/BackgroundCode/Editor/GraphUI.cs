using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasingToolkit.CustomUI
{
    public class GraphUI : VisualElement
    {
        #region UXML

        // This makes the GraphUI appear in the UI Builder.
        public new class UxmlFactory : UxmlFactory<GraphUI, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            UxmlFloatAttributeDescription float_frequency =
                new UxmlFloatAttributeDescription { name = "frequency_Attribute", defaultValue = 1 };
            UxmlFloatAttributeDescription float_dampening =
                new UxmlFloatAttributeDescription { name = "dampening_Attribute", defaultValue = 1 };
            UxmlFloatAttributeDescription float_intialresponse =
                new UxmlFloatAttributeDescription { name = "initialResponse_Attribute", defaultValue = 0 };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                var ate = ve as GraphUI;

                ate._frequencyAttribute = float_frequency.GetValueFromBag(bag, cc);
                ate._dampeningAttribute = float_dampening.GetValueFromBag(bag, cc);
                ate._initialResponseAttribute = float_intialresponse.GetValueFromBag(bag, cc);
            }
        }

        #endregion

        #region Attributes

        private float _frequencyAttribute = 1;
        public float frequency_Attribute
        {
            get => _frequencyAttribute;
            set
            {
                if (value == _frequencyAttribute)
                    return;

                MarkDirtyRepaint();
                _frequencyAttribute = value;
            }
        }

        private float _dampeningAttribute = 1;
        public float dampening_Attribute
        {
            get => _dampeningAttribute;
            set
            {
                if (value == _dampeningAttribute)
                    return;

                MarkDirtyRepaint();
                _dampeningAttribute = value;
            }
        }

        private float _initialResponseAttribute = 0;
        public float initialResponse_Attribute
        {
            get => _initialResponseAttribute;
            set
            {
                if (value == _initialResponseAttribute)
                    return;

                MarkDirtyRepaint();
                _initialResponseAttribute = value;
            }
        }

        #endregion

        #region Local Variables

        private GraphData _myGraphData;

        #endregion

        public GraphUI() 
        {
            style.flexGrow = 1;
            style.backgroundColor = graphBackgroundColor;

            generateVisualContent += OnGenerateVisualContent;
        }

        public GraphUI(GraphData newGraphData)
        {
            style.flexGrow = 1;
            style.backgroundColor = graphBackgroundColor;
            style.height = 500;
            style.width = 500;

            generateVisualContent += OnGenerateVisualContent;

            UpdateGraph(newGraphData);
        }

        #region Style

        static Color graphBackgroundColor => new Color(0.15f, 0.15f, 0.15f);
        Color normalFramingLines_Color => new Color(0.35f, 0.35f, 0.35f);
        Color graphLine_Color => Color.cyan;
        float framingLinesWidth => 1.5f;
        float graphLineWidth => 1.3f;

        #endregion


        void OnGenerateVisualContent(MeshGenerationContext mgc)
        {
            if (contentRect.width < 0.01f || contentRect.height < 0.01f)
                return; // Skip rendering when too small.

            if (_myGraphData.leftLimit >= _myGraphData.rightLimit)
                return; // We don't wanna be painting if the GraphData hasn't been initialized.

            /*{     // Simulating a graph for Debug purposes.
            
                _myGraphData.leftLimit = -0.4f;
                _myGraphData.rightLimit = 9f;
                _myGraphData.upLimit = 1.5f;
                _myGraphData.bottomLimit = -0.5f;

                _myGraphData.xLocation_FirstPoint = 0;
                _myGraphData.xLocation_LastPoint = 3.5f;
                _myGraphData.graphPoints = new float[] { 0, 0.1f, 0.25f, 0.6f, 1 };

                // Special Lines
                Color color_framingLinesAt0 = new Color(0.6f, 0.6f, 0.6f);
                Color color_framingLineAt1 = new Color(0.9f, 0.4f, 0.4f);
                float framingLinesWidth = 1.5f;

                GraphData.SpecialLine specialVertical_0 = new GraphData.SpecialLine
                { location = 0, lineColor = color_framingLinesAt0, lineThickness = framingLinesWidth };
                GraphData.SpecialLine specialHorizontal_0 = new GraphData.SpecialLine
                { location = 0, lineColor = color_framingLinesAt0, lineThickness = framingLinesWidth };
                GraphData.SpecialLine specialHorizontal_1 = new GraphData.SpecialLine
                { location = 1, lineColor = color_framingLineAt1, lineThickness = framingLinesWidth };

                _myGraphData.specialVerticalLines = new GraphData.SpecialLine[] { specialVertical_0 };
                _myGraphData.specialHorizontalLines = new GraphData.SpecialLine[] { specialHorizontal_0, specialHorizontal_1 };
            }*/


            Painter2D painter = mgc.painter2D;

            #region Drawing the graph framing lines.

            painter.lineWidth = framingLinesWidth;
            painter.strokeColor = normalFramingLines_Color;


            // Vertical lines
            float[] locationOfSpecialVerticalLines = new float[_myGraphData.specialVerticalLines.Length];
            for (int i = 0; i < _myGraphData.specialVerticalLines.Length; i++)
                locationOfSpecialVerticalLines[i] = _myGraphData.specialVerticalLines[i].location;

            painter.BeginPath();
            for (int x = (int) _myGraphData.leftLimit; x < _myGraphData.rightLimit; x++)
            {
                if (!locationOfSpecialVerticalLines.Contains<float>(x))
                    DrawVerticalLineAt(x);
            }
            painter.Stroke();
            painter.ClosePath();

            if (locationOfSpecialVerticalLines.Length != 0)
            {
                foreach (GraphData.SpecialLine specialLine in _myGraphData.specialVerticalLines)
                {
                    // We don't draw special lines outside of our confines.
                    if (specialLine.location <= _myGraphData.leftLimit || specialLine.location >= _myGraphData.rightLimit)
                        continue;

                    painter.BeginPath();
                    painter.strokeColor = specialLine.lineColor;
                    painter.lineWidth = specialLine.lineThickness;

                    DrawVerticalLineAt(specialLine.location);

                    painter.Stroke();
                    painter.ClosePath();
                }

                painter.strokeColor = normalFramingLines_Color; // Returning to the regular color.
                painter.lineWidth = framingLinesWidth;          // Returning to the regular line width.
            }


            // Horizontal lines
            float[] locationOfSpecialHorizontalLines = new float[_myGraphData.specialHorizontalLines.Length];
            for (int i = 0; i < _myGraphData.specialHorizontalLines.Length; i++)
                locationOfSpecialHorizontalLines[i] = _myGraphData.specialHorizontalLines[i].location;

            painter.BeginPath();
            for (int y = (int)_myGraphData.bottomLimit; y < _myGraphData.upLimit; y++)
            {
                if (!locationOfSpecialHorizontalLines.Contains<float>(y))
                    DrawHorizontalLineAt(y);
            }

            painter.Stroke();
            painter.ClosePath();

            if (locationOfSpecialHorizontalLines.Length != 0)
            {
                foreach (GraphData.SpecialLine specialLine in _myGraphData.specialHorizontalLines)
                {
                    // We don't draw special lines outside of our confines.
                    if (specialLine.location <= _myGraphData.bottomLimit || specialLine.location >= _myGraphData.upLimit)
                        continue;

                    painter.BeginPath();
                    painter.strokeColor = specialLine.lineColor;
                    painter.lineWidth = specialLine.lineThickness;

                    DrawHorizontalLineAt(specialLine.location);

                    painter.Stroke();
                    painter.ClosePath();
                }

                painter.strokeColor = normalFramingLines_Color; // Returning to the regular color.
                painter.lineWidth = framingLinesWidth;          // Returning to the regular line width.
            }

            #endregion

            #region Plotting the points of the Graph

            // If our graph does not have enough points, we don't draw it.
            if (_myGraphData.graphPoints.Length < 2)
                return;

            painter.BeginPath();
            painter.strokeColor = graphLine_Color;
            painter.lineWidth = graphLineWidth;

            float xPosition = _myGraphData.xLocation_FirstPoint;
            float distanceBetweenPoints = (_myGraphData.xLocation_LastPoint - _myGraphData.xLocation_FirstPoint) 
                / (_myGraphData.graphPoints.Length - 1);

            painter.MoveTo(ConvertToRectCoordinates(new Vector2(xPosition, _myGraphData.graphPoints[0])));

            for(int i = 1; i < _myGraphData.graphPoints.Length; i++)
            {
                xPosition += distanceBetweenPoints;
                painter.LineTo(ConvertToRectCoordinates(new Vector2(xPosition, _myGraphData.graphPoints[i])));
            }

            painter.Stroke();
            painter.ClosePath();

            #endregion

            #region Local Functions

            Vector2 ConvertToRectCoordinates(Vector2 inputVector)
            {
                float totalXSize = _myGraphData.rightLimit - _myGraphData.leftLimit;
                float totalYSize = _myGraphData.upLimit - _myGraphData.bottomLimit;

                Vector2 normalizedVector = new Vector2((inputVector.x - _myGraphData.leftLimit) / totalXSize, 
                                                       (inputVector.y - _myGraphData.bottomLimit) / totalYSize);

                return new Vector2(normalizedVector.x * contentRect.width + contentRect.xMin, 
                                   contentRect.yMax - normalizedVector.y * contentRect.height);
            }

            void DrawVerticalLineAt(float x)
            {
                painter.MoveTo(ConvertToRectCoordinates(new Vector2(x, _myGraphData.upLimit)));
                painter.LineTo(ConvertToRectCoordinates(new Vector2(x, _myGraphData.bottomLimit)));
            }

            void DrawHorizontalLineAt(float y)
            {
                painter.MoveTo(ConvertToRectCoordinates(new Vector2(_myGraphData.leftLimit, y)));
                painter.LineTo(ConvertToRectCoordinates(new Vector2(_myGraphData.rightLimit, y)));
            }

            #endregion
        }

        public void UpdateGraph(GraphData newGraphData)
        {
            if(newGraphData.graphPoints.Length < 2)
            {
                Debug.LogWarning("Cannot plot a graph with less than 2 points. " +
                    "Place a minimum of 2 points inside the graphPoints array to see them represented in the graph.");
            }

            _myGraphData = newGraphData;
            MarkDirtyRepaint();
        }
    }

    public struct GraphData
    {
        public float leftLimit;
        public float rightLimit;
        public float bottomLimit;
        public float upLimit;

        public float[] graphPoints;
        public float xLocation_FirstPoint;
        public float xLocation_LastPoint;

        // Special lines are those that are part of the framing but should have a different appearance than the rest of the framing.
        // For example, lines that cross the point (0,0), and thus help frame the graph.
        public SpecialLine[] specialVerticalLines;
        public SpecialLine[] specialHorizontalLines;

        public struct SpecialLine
        {
            // We don't specify whether it is horizontal or vertical, that will depend on which array the line is placed in.
            public float location;

            public Color lineColor;
            public float lineThickness;
        }
    }
}