using System;
using TwoPiGrid.Configuration;
using TwoPiGrid.Generation;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TwoPiGrid.Editor
{
    [CustomEditor(typeof(GridConfiguration))]
    internal class SeriousGridSettingsInspector : UnityEditor.Editor
    {
        private ReorderableList reorderableList;

        private void OnEnable()
        {
            InitializeList();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            GUI.enabled = false;
            {
                var scriptProperty = serializedObject.FindProperty("m_Script");
                EditorGUILayout.PropertyField(scriptProperty, true);
            }
            GUI.enabled = true;

            EditorGUILayout.Space();

            var namespaceNameProperty = serializedObject.FindProperty("namespaceName");
            EditorGUILayout.PropertyField(namespaceNameProperty);

            EditorGUILayout.Space();

            var gridPrefixProperty = serializedObject.FindProperty("gridPrefix");
            EditorGUILayout.PropertyField(gridPrefixProperty);

            EditorGUILayout.Space();

            reorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();

            DrawExplanationText();

            if (GUILayout.Button("Generate custom grid"))
            {
                var gridConfiguration = (GridConfiguration) target;
                var generator = new CustomGridGenerator(
                    gridConfiguration.namespaceName,
                    gridConfiguration.gridPrefix,
                    gridConfiguration.customCellFields);
                generator.GenerateCustomGrid();
            }
        }

        private void InitializeList()
        {
            var property = serializedObject.FindProperty("customCellFields");
            reorderableList = new ReorderableList(
                serializedObject: serializedObject,
                elements: property,
                draggable: true,
                displayHeader: true,
                displayAddButton: true,
                displayRemoveButton: true)
            {
                drawElementCallback = DrawElement,
                drawHeaderCallback = DrawHeader,
                elementHeightCallback = GetHeightForElement
            };
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Custom Cell Fields:");
        }

        private void DrawElement(Rect rect, int index, bool isactive, bool isfocused)
        {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var fieldName = element.FindPropertyRelative("FieldName");
            var valueType = element.FindPropertyRelative("ValueType");
            var customName = element.FindPropertyRelative("Name");

            var isCustomType = IsCustomType(valueType);//valueType.enumValueIndex >= 2;
            var height = isCustomType ? rect.size.y / 2 - 3 : rect.size.y - 4;

            var fieldNameRect =
                new Rect(
                    new Vector2(rect.position.x, rect.position.y + 2),
                    new Vector2(rect.size.x / 2 - 5, height));
            var valueTypeRect =
                new Rect(
                    new Vector2(rect.position.x + rect.size.x / 2 + 5, rect.position.y + 2),
                    new Vector2(rect.size.x / 2 - 5 - 2, height));
            var customNameRect =
                new Rect(
                    new Vector2(
                        rect.position.x + rect.size.x / 2 + 5,
                        rect.position.y + rect.size.y / 2 + 1),
                    new Vector2(rect.size.x / 2 - 5 - 2, height));

            var previousLabelWidth = EditorGUIUtility.labelWidth;
            var previousLabelWrap = EditorStyles.label.wordWrap;
            EditorStyles.label.wordWrap = false;
            EditorGUIUtility.labelWidth = 70.0f;
            {
                EditorGUI.PropertyField(fieldNameRect, fieldName);
                EditorGUI.PropertyField(valueTypeRect, valueType);
                if (isCustomType)
                    EditorGUI.PropertyField(customNameRect, customName);
            }
            EditorGUIUtility.labelWidth = previousLabelWidth;
            EditorStyles.label.wordWrap = previousLabelWrap;
        }

        private float GetHeightForElement(int index)
        {
            var element = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var valueType = element.FindPropertyRelative("ValueType");

            if (!IsCustomType(valueType))//valueType.enumValueIndex < 2)
                return 1.3f * EditorGUIUtility.singleLineHeight;
            else
                return 2.5f * EditorGUIUtility.singleLineHeight;
        }

        private void DrawExplanationText()
        {
            var gridPrefix = serializedObject.FindProperty("gridPrefix").stringValue;
            if (string.IsNullOrEmpty(gridPrefix))
                gridPrefix = "Custom";

            //var previousLabelWrap = EditorStyles.label.wordWrap;
            //EditorStyles.label.wordWrap = true;

            GUILayout.Label(
                "You can add extra fields to your grid's cell, to help you customize it" +
                " further!" +
                "\nPlease set the name and value type. You can use your own custom types!" +
                "\n" +
                "\nFor example, if you set a custom field named \"magicalProperties\" of" +
                " type \"MagicalProperty\" (which is a serializable struct/class you" +
                " defined), you will be able to access it like so:" +
                "\n" +
                "\nvar grid = new " + gridPrefix + "Grid(/* initialization params */);" +
                "\n// ..." +
                "\n MagicalProperty mp = grid.GetMagicalProperty(index: 3);" +
                "\n// ..." +
                "\n" +
                "\nCheck out the documentation for more information!", EditorStyles.wordWrappedLabel);

            //EditorStyles.label.wordWrap = previousLabelWrap;
        }

        private bool IsCustomType(SerializedProperty valueType)
        {
            var enumValueIndex = valueType.enumValueIndex;

            switch (enumValueIndex)
            {
                case (int) CustomCellField.ValueTypes._bool:
                case (int) CustomCellField.ValueTypes._byte:
                case (int) CustomCellField.ValueTypes._uint:
                case (int) CustomCellField.ValueTypes._int:
                case (int) CustomCellField.ValueTypes._long:
                case (int) CustomCellField.ValueTypes._float:
                case (int) CustomCellField.ValueTypes._double:
                case (int) CustomCellField.ValueTypes._string:
                    return false;
                case (int) CustomCellField.ValueTypes._customEnum:
                case (int) CustomCellField.ValueTypes._customClass:
                case (int) CustomCellField.ValueTypes._customStruct:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
