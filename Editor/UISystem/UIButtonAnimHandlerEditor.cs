using UnityEditor;
using UnityEngine;

namespace NilinhoGames.UI
{
    [CustomEditor(typeof(UIButtonAnimHandler))]
    public class UIButtonAnimHandlerEditor : Editor
    {
        private int currentTab = 0;
        private string[] tabNames = { "Scale Animation", "Color Animation", "Sprite Settings", "Sound Effects" };

        public override void OnInspectorGUI()
        {
            UIButtonAnimHandler handler = (UIButtonAnimHandler)target;

            // Tabs
            currentTab = GUILayout.Toolbar(currentTab, tabNames);
            GUILayout.Space(10);

            // Show content based on selected tab
            switch (currentTab)
            {
                case 0:
                    ShowScaleAnimationTab(handler);
                    break;
                case 1:
                    ShowColorAnimationTab(handler);
                    break;
                case 2:
                    ShowSpriteSettingsTab(handler);
                    break;
                case 3:
                    ShowSoundEffectsTab(handler);
                    break;
            }

            // Update serialized properties
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowScaleAnimationTab(UIButtonAnimHandler handler)
        {
            SerializedProperty enableScaleAnimation = serializedObject.FindProperty("enableScaleAnimation");
            EditorGUILayout.PropertyField(enableScaleAnimation, new GUIContent("Enable Scale Animation"));
            if (enableScaleAnimation.boolValue)
            {
                SerializedProperty hoverScale = serializedObject.FindProperty("hoverScale");
                SerializedProperty pressedScale = serializedObject.FindProperty("pressedScale");
                SerializedProperty scaleAnimationDuration = serializedObject.FindProperty("scaleAnimationDuration");

                EditorGUILayout.PropertyField(hoverScale);
                EditorGUILayout.PropertyField(pressedScale);
                EditorGUILayout.PropertyField(scaleAnimationDuration);
            }
        }

        private void ShowColorAnimationTab(UIButtonAnimHandler handler)
        {
            SerializedProperty enableColorAnimation = serializedObject.FindProperty("enableColorAnimation");
            EditorGUILayout.PropertyField(enableColorAnimation, new GUIContent("Enable Color Animation"));
            if (enableColorAnimation.boolValue)
            {
                SerializedProperty hoverColor = serializedObject.FindProperty("hoverColor");
                SerializedProperty pressedColor = serializedObject.FindProperty("pressedColor");
                SerializedProperty colorAnimationDuration = serializedObject.FindProperty("colorAnimationDuration");

                EditorGUILayout.PropertyField(hoverColor);
                EditorGUILayout.PropertyField(pressedColor);
                EditorGUILayout.PropertyField(colorAnimationDuration);
            }
        }

        private void ShowSpriteSettingsTab(UIButtonAnimHandler handler)
        {
            SerializedProperty enableSpriteChange = serializedObject.FindProperty("enableSpriteChange");
            EditorGUILayout.PropertyField(enableSpriteChange, new GUIContent("Enable Sprite Change"));
            if (enableSpriteChange.boolValue)
            {
                SerializedProperty normalSprite = serializedObject.FindProperty("normalSprite");
                SerializedProperty hoverSprite = serializedObject.FindProperty("hoverSprite");
                SerializedProperty pressedSprite = serializedObject.FindProperty("pressedSprite");

                EditorGUILayout.PropertyField(normalSprite);
                EditorGUILayout.PropertyField(hoverSprite);
                EditorGUILayout.PropertyField(pressedSprite);
            }
        }

        private void ShowSoundEffectsTab(UIButtonAnimHandler handler)
        {
            SerializedProperty enableSoundEffects = serializedObject.FindProperty("enableSoundEffects");
            EditorGUILayout.PropertyField(enableSoundEffects, new GUIContent("Enable Sound Effects"));
            if (enableSoundEffects.boolValue)
            {
                SerializedProperty hoverSound = serializedObject.FindProperty("hoverSound");
                SerializedProperty pressSound = serializedObject.FindProperty("pressSound");
                SerializedProperty audioSource = serializedObject.FindProperty("audioSource");

                EditorGUILayout.PropertyField(hoverSound);
                EditorGUILayout.PropertyField(pressSound);
                EditorGUILayout.PropertyField(audioSource);
            }
        }
    }
}
