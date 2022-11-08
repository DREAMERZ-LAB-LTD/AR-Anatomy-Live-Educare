using UnityEngine;
using UnityEditor;

namespace DreamerzLab.Controller
{
    [CustomEditor(typeof(ObjectTweenController))]
    public class ObjectTweenControllerEditor : Editor
    {
        private ObjectTweenController myTarget;
        private SerializedObject soTarget;

        SerializedProperty targetObj;
        SerializedProperty _enableEditorLogs;

        bool showHorizontalSettings = false;
        SerializedProperty useHorizontal;
        SerializedProperty horizontalSpeed;
        SerializedProperty horizontalSmoothness;

        bool showScaleSettings = false;
        SerializedProperty useScaleChange;
        SerializedProperty scaleSpeed;
        SerializedProperty minSize;
        SerializedProperty maxSize;
        SerializedProperty minimumScaleSensitivity;
        SerializedProperty scaleSmoothness;

        bool showMovementSettings = false;
        SerializedProperty useMovement;
        SerializedProperty raycastLayer;
        SerializedProperty moveSpeed;
        SerializedProperty minimumMovementSensitivity;
        SerializedProperty movementSmoothness;
        SerializedProperty moveDelay;

        bool showTapSettings = false;
        SerializedProperty useTap;
        SerializedProperty singleTapMaxDuration;
        SerializedProperty maxDelayBetweenSecondTap;
        SerializedProperty oneTapEvent;
        SerializedProperty twoTapEvent;
        SerializedProperty onTapHoldDuration;
        SerializedProperty onTapHoldEvent;



        private void OnEnable()
        {
            myTarget = (ObjectTweenController)target;
            soTarget = new SerializedObject(target);

            targetObj = soTarget.FindProperty("target");
            _enableEditorLogs = soTarget.FindProperty("_enableEditorLogs");

            useHorizontal = soTarget.FindProperty("useHorizontal");
            horizontalSpeed = soTarget.FindProperty("horizontalSpeed");
            horizontalSmoothness = soTarget.FindProperty("horizontalSmoothness");


            useScaleChange = soTarget.FindProperty("useScaleChange");
            scaleSpeed = soTarget.FindProperty("scaleSpeed");
            minSize = soTarget.FindProperty("minSize");
            maxSize = soTarget.FindProperty("maxSize");
            minimumScaleSensitivity = soTarget.FindProperty("minimumScaleSensitivity");
            scaleSmoothness = soTarget.FindProperty("scaleSmoothness");

            useMovement = soTarget.FindProperty("useMovement");
            raycastLayer = soTarget.FindProperty("raycastLayer");
            moveSpeed = soTarget.FindProperty("moveSpeed");
            minimumMovementSensitivity = soTarget.FindProperty("minimumMovementSensitivity");
            movementSmoothness = soTarget.FindProperty("movementSmoothness");
            moveDelay = soTarget.FindProperty("moveDelay");

            useTap = soTarget.FindProperty("useTap");
            singleTapMaxDuration = soTarget.FindProperty("singleTapMaxDuration");
            maxDelayBetweenSecondTap = soTarget.FindProperty("maxDelayBetweenSecondTap");
            oneTapEvent = soTarget.FindProperty("oneTapEvent");
            twoTapEvent = soTarget.FindProperty("twoTapEvent");
            onTapHoldDuration = soTarget.FindProperty("onTapHoldDuration");
            onTapHoldEvent = soTarget.FindProperty("onTapHoldEvent");
        }

        public override void OnInspectorGUI()
        {
            soTarget.Update();
            EditorGUI.BeginChangeCheck();

            BasicSetup();
            GUILayout.Space(5);
            HorizontalSettings();
            GUILayout.Space(5);
            ScalingSettings();
            GUILayout.Space(5);
            MovementSettings();
            GUILayout.Space(5);
            TapSettings();
            GUILayout.Space(5);

            if (EditorGUI.EndChangeCheck())
            {
                soTarget.ApplyModifiedProperties();
                EditorUtility.SetDirty(myTarget);
            }

        }

        void BasicSetup()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            EditorGUILayout.PropertyField(_enableEditorLogs);
            DrawUILine(Color.grey, 1, 2);

            EditorGUILayout.PropertyField(targetObj);

            GUILayout.Space(5);
            GUILayout.EndVertical();
        }

        void HorizontalSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(useHorizontal, GUIContent.none, GUILayout.MaxWidth(25));
            showHorizontalSettings = EditorGUILayout.Foldout(showHorizontalSettings, "Horizontal Rotation", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showHorizontalSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;

                EditorGUILayout.PropertyField(horizontalSpeed, new GUIContent("Speed"));
                EditorGUILayout.PropertyField(horizontalSmoothness, new GUIContent("Smoothness"));
            }
            if (!Selection.activeTransform)
            {
                showHorizontalSettings = false;
            }
            GUILayout.EndVertical();
        }

      


        void ScalingSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(useScaleChange, GUIContent.none, GUILayout.MaxWidth(25));
            showScaleSettings = EditorGUILayout.Foldout(showScaleSettings, "Scaling", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showScaleSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;

                EditorGUILayout.PropertyField(scaleSpeed, new GUIContent("Speed"));
                EditorGUILayout.PropertyField(minimumScaleSensitivity, new GUIContent("Min Sensitivity"));
                EditorGUILayout.PropertyField(scaleSmoothness, new GUIContent("Smoothness"));
                GUILayout.Space(5);
                GUILayout.Label("Size");
                EditorGUI.indentLevel = 1;
                EditorGUILayout.PropertyField(minSize, new GUIContent("Min"));
                EditorGUILayout.PropertyField(maxSize, new GUIContent("Max"));
                EditorGUI.indentLevel = 0;
            }
            if (!Selection.activeTransform)
            {
                showScaleSettings = false;
            }
            GUILayout.EndVertical();
        }

        void MovementSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(useMovement, GUIContent.none, GUILayout.MaxWidth(25));
            showMovementSettings = EditorGUILayout.Foldout(showMovementSettings, "Movement", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showMovementSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;

                EditorGUILayout.PropertyField(raycastLayer, new GUIContent("Raycast Layer"));
                EditorGUILayout.PropertyField(moveSpeed, new GUIContent("Speed"));
                EditorGUILayout.PropertyField(minimumMovementSensitivity, new GUIContent("Min Sensitivity"));
                EditorGUILayout.PropertyField(movementSmoothness, new GUIContent("Smoothness"));
                EditorGUILayout.PropertyField(moveDelay, new GUIContent("Delay"));
            }
            if (!Selection.activeTransform)
            {
                showMovementSettings = false;
            }
            GUILayout.EndVertical();
        }
        void TapSettings()
        {
            GUILayout.BeginVertical("Box");
            EditorGUI.indentLevel = 0;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(useTap, GUIContent.none, GUILayout.MaxWidth(25));
            showTapSettings = EditorGUILayout.Foldout(showTapSettings, "Tap", true, EditorStyles.foldout);
            GUILayout.EndHorizontal();

            if (showTapSettings)
            {
                DrawUILine(Color.grey, 1, 2);
                EditorGUI.indentLevel = 0;

                EditorGUILayout.PropertyField(singleTapMaxDuration, new GUIContent("Single tap max duration"));
                EditorGUILayout.PropertyField(maxDelayBetweenSecondTap, new GUIContent("Max delay between second tap"));
                EditorGUILayout.PropertyField(oneTapEvent, new GUIContent("One Tap"));
                EditorGUILayout.PropertyField(twoTapEvent, new GUIContent("Double Tap"));
                EditorGUILayout.PropertyField(onTapHoldDuration, new GUIContent("On Tap Hold min duration"));
                EditorGUILayout.PropertyField(onTapHoldEvent, new GUIContent("On Tap Hold Event"));
            }
            if (!Selection.activeTransform)
            {
                showTapSettings = false;
            }
            GUILayout.EndVertical();
        }
        void DrawUILine(Color color, int thickness = 1, int padding = 2)
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
            r.height = thickness;
            r.y += padding / 2;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, color);
        }
    }
}