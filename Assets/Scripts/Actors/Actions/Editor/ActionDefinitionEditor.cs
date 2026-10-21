using Actors.Actions.Definitions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Actors.Actions.Editor
{
    [CustomEditor(typeof(ActionDefinition))]
    public sealed class ActionDefinitionEditor : UnityEditor.Editor
    {
        private SerializedProperty _effectsProperty;
        private ReorderableList _effectsList;

        private void OnEnable()
        {
            _effectsProperty = serializedObject.FindProperty("effects");
            _effectsList = new ReorderableList(
                serializedObject,
                _effectsProperty,
                draggable: true,
                displayHeader: true,
                displayAddButton: true,
                displayRemoveButton: true)
            {
                drawHeaderCallback = DrawHeader,
                drawElementCallback = DrawElement,
                elementHeightCallback = GetElementHeight,
                onAddDropdownCallback = ShowAddEffectMenu,
                onRemoveCallback = RemoveEffect
            };
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using (new EditorGUI.DisabledScope(true))
                EditorGUILayout.PropertyField(serializedObject.FindProperty("m_Script"));

            DrawPropertiesExcluding(serializedObject, "m_Script", "effects");
            EditorGUILayout.Space();
            _effectsList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }

        private static void DrawHeader(Rect rect) =>
            EditorGUI.LabelField(rect, "Effects");

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty effect = _effectsProperty.GetArrayElementAtIndex(index);
            GUIContent label = new(GetEffectLabel(effect));

            rect.y += EditorGUIUtility.standardVerticalSpacing;
            rect.height = EditorGUI.GetPropertyHeight(effect, label, includeChildren: true);
            EditorGUI.PropertyField(rect, effect, label, includeChildren: true);
        }

        private float GetElementHeight(int index)
        {
            SerializedProperty effect = _effectsProperty.GetArrayElementAtIndex(index);
            GUIContent label = new(GetEffectLabel(effect));
            return EditorGUI.GetPropertyHeight(effect, label, includeChildren: true) +
                   EditorGUIUtility.standardVerticalSpacing * 2f;
        }

        private void ShowAddEffectMenu(Rect buttonRect, ReorderableList list)
        {
            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Damage"),
                false,
                () => AddEffect<DamageEffectDefinition>());
            menu.AddItem(new GUIContent("Heal"),
                false,
                () => AddEffect<HealEffectDefinition>());
            menu.AddItem(new GUIContent("Push"),
                false,
                () => AddEffect<PushEffectDefinition>());
            menu.AddItem(new GUIContent("Aggro"),
                false,
                () => AddEffect<AggroEffectDefinition>());
            menu.AddItem(new GUIContent("Next Dice Roll Bonus"),
                false,
                () => AddEffect<NextDiceRollBonusEffectDefinition>());
            menu.DropDown(buttonRect);
        }

        private void AddEffect<T>() where T : ActionEffectDefinition, new()
        {
            serializedObject.Update();

            int index = _effectsProperty.arraySize;
            _effectsProperty.arraySize++;

            SerializedProperty effect = _effectsProperty.GetArrayElementAtIndex(index);
            effect.managedReferenceValue = new T();
            effect.isExpanded = true;

            serializedObject.ApplyModifiedProperties();
            _effectsList.index = index;
        }

        private void RemoveEffect(ReorderableList list)
        {
            if (list.index < 0 || list.index >= _effectsProperty.arraySize)
                return;

            serializedObject.Update();
            _effectsProperty.DeleteArrayElementAtIndex(list.index);
            serializedObject.ApplyModifiedProperties();

            list.index = Mathf.Clamp(list.index - 1, -1, _effectsProperty.arraySize - 1);
        }

        private static string GetEffectLabel(SerializedProperty effect)
        {
            return effect.managedReferenceValue switch
            {
                DamageEffectDefinition => "Damage Effect",
                HealEffectDefinition => "Heal Effect",
                PushEffectDefinition => "Push Effect",
                AggroEffectDefinition => "Aggro Effect",
                NextDiceRollBonusEffectDefinition => "Next Dice Roll Bonus Effect",
                null => "Missing Effect",
                object value => ObjectNames.NicifyVariableName(
                    value.GetType().Name.Replace("Definition", string.Empty))
            };
        }
    }
}
