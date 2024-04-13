using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UtilsComplements.Editor
{
    public static class GizmosUtilities
    {
        #region DrawSphere
        public struct DrawSphereProperties
        {
            public float Radius { get; set; }

            public static DrawSphereProperties DefaultProperty { get; private set; }

            static DrawSphereProperties()
            {
                DefaultProperty = new DrawSphereProperties()
                {
                    Radius = 1
                };
            }

            public DrawSphereProperties(DrawSphereProperties copy)
            {
                Radius = copy.Radius;
            }
        }

        public static void DrawSphere(Vector3 origin, bool drawControl = true)
        {
            DrawSphere(origin, Color.white, drawControl);
        }

        public static void DrawSphere(Vector3 origin, Color gizmosColor, bool drawControl = true)
        {
            DrawSphere(origin, gizmosColor, DrawSphereProperties.DefaultProperty, drawControl);
        }

        public static void DrawSphere(Vector3 origin, Color gizmosColor, DrawSphereProperties properties,
                                      bool drawControl = true)
        {
            if (!drawControl)
                return;

            Gizmos.color = gizmosColor;
            Gizmos.DrawWireSphere(origin, properties.Radius);
            Gizmos.color = Color.white;
        }
        #endregion

        #region DrawCurve

        public delegate Vector3 Function(float time);
        //public delegate Vector3 Function<T>(T parameters);

        public struct DrawCurveProperties
        {
            public float MinValue { get; set; }
            public float MaxValue { get; set; }
            public int DefinitionOfCurve { get; set; }
            public bool DrawDots { get; set; }
            public float DotRadius { get; set; }
            public int DottedLinesSpace { get; set; }

            public static DrawCurveProperties DefaultValues { get; private set; }

            static DrawCurveProperties()
            {
                DefaultValues = new DrawCurveProperties()
                {
                    MinValue = 0,
                    MaxValue = 1,
                    DefinitionOfCurve = 5,
                    DottedLinesSpace = 1,
                    DotRadius = 0.1f,
                    DrawDots = true
                };
            }

            public DrawCurveProperties(DrawCurveProperties copy)
            {
                MinValue = copy.MinValue;
                MaxValue = copy.MaxValue;
                DefinitionOfCurve = copy.DefinitionOfCurve;
                DottedLinesSpace = copy.DottedLinesSpace;
                DotRadius = copy.DotRadius;
                DrawDots = copy.DrawDots;
            }
        }

        public static void DrawCurve(Function function, bool drawControl = true)
        {
            DrawCurve(function, DrawCurveProperties.DefaultValues, drawControl);
        }

        public static void DrawCurve(Function function, DrawCurveProperties properties,
                                     bool drawControl = true)
        {
            DrawCurve(function, Color.white, properties, drawControl);
        }

        public static void DrawCurve(Function function, Color gizmosColor,
                                     DrawCurveProperties properties, bool drawControl = true)
        {
            if (!drawControl)
                return;

#if UNITY_EDITOR
            Gizmos.color = gizmosColor;
            Handles.color = gizmosColor;
            static void resetColors()
            {
                Gizmos.color = Color.white;
                Handles.color = Color.white;
            }

            #region Curve

            int divisions = properties.DefinitionOfCurve;
            if (divisions <= 1)
            {
                resetColors();
                return;
            }
            Vector3 lastPos = Vector3.zero;
            for (int i = 0; i <= divisions; i++)
            {
                float evaluatedValue = Mathf.Lerp(properties.MinValue, properties.MaxValue,
                                                  ((float)i) / divisions);

                Vector3 pos = function(evaluatedValue);
                if (i != 0)
                {
                    Handles.DrawDottedLine(lastPos, pos, properties.DottedLinesSpace);
                }
                if (properties.DrawDots)
                {
                    Gizmos.DrawWireSphere(pos, properties.DotRadius);
                }

                lastPos = pos;
            }

            #endregion

            resetColors();
#endif
        }

        #endregion
    }

}