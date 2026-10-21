using Actors.Actions.ValueProviders;
using UnityEditor;
using UnityEngine;

namespace Actors.Actions.Editor
{
    [CustomPropertyDrawer(typeof(EffectValueProvider), true)]
    public sealed class EffectValueProviderDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            Rect dropdownRect = new(
                position.x,
                position.y,
                position.width,
                EditorGUIUtility.singleLineHeight);
            Rect valueRect = EditorGUI.PrefixLabel(dropdownRect, label);

            if (EditorGUI.DropdownButton(
                    valueRect,
                    new GUIContent(GetProviderLabel(property)),
                    FocusType.Keyboard))
                ShowProviderMenu(valueRect, property.Copy());

            if (property.managedReferenceValue != null)
            {
                float y = dropdownRect.yMax + EditorGUIUtility.standardVerticalSpacing;
                DrawProviderFields(property, position, ref y);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight;
            if (property.managedReferenceValue == null)
                return height;

            SerializedProperty child = property.Copy();
            SerializedProperty end = child.GetEndProperty();
            bool enterChildren = true;

            while (child.NextVisible(enterChildren) &&
                   !SerializedProperty.EqualContents(child, end))
            {
                enterChildren = false;
                if (child.depth != property.depth + 1)
                    continue;

                height += EditorGUIUtility.standardVerticalSpacing;
                height += EditorGUI.GetPropertyHeight(child, includeChildren: true);
            }

            return height;
        }

        private static void DrawProviderFields(
            SerializedProperty property,
            Rect position,
            ref float y)
        {
            SerializedProperty child = property.Copy();
            SerializedProperty end = child.GetEndProperty();
            bool enterChildren = true;

            using (new EditorGUI.IndentLevelScope())
            {
                while (child.NextVisible(enterChildren) &&
                       !SerializedProperty.EqualContents(child, end))
                {
                    enterChildren = false;
                    if (child.depth != property.depth + 1)
                        continue;

                    float height = EditorGUI.GetPropertyHeight(child, includeChildren: true);
                    var childRect = new Rect(position.x, y, position.width, height);
                    EditorGUI.PropertyField(childRect, child, includeChildren: true);
                    y += height + EditorGUIUtility.standardVerticalSpacing;
                }
            }
        }

        private static void ShowProviderMenu(Rect buttonRect, SerializedProperty property)
        {
            object current = property.managedReferenceValue;
            var menu = new GenericMenu();

            menu.AddItem(
                new GUIContent("None"),
                current == null,
                () => SetProvider(property, null));
            menu.AddSeparator(string.Empty);
            menu.AddItem(
                new GUIContent("Adjacent Actor Count"),
                current is AdjacentActorCountValueProvider,
                () => SetProvider(property, new AdjacentActorCountValueProvider()));
            menu.AddItem(
                new GUIContent("Applied Damage"),
                current is AppliedDamageValueProvider,
                () => SetProvider(property, new AppliedDamageValueProvider()));
            menu.AddItem(
                new GUIContent("Healed Target Count"),
                current is HealedTargetCountValueProvider,
                () => SetProvider(property, new HealedTargetCountValueProvider()));
            menu.AddItem(
                new GUIContent("Push Distance"),
                current is PushDistanceValueProvider,
                () => SetProvider(property, new PushDistanceValueProvider()));

            menu.DropDown(buttonRect);
        }

        private static void SetProvider(
            SerializedProperty property,
            EffectValueProvider provider)
        {
            SerializedObject owner = property.serializedObject;
            owner.Update();
            property.managedReferenceValue = provider;
            property.isExpanded = provider != null;
            owner.ApplyModifiedProperties();
        }

        private static string GetProviderLabel(SerializedProperty property)
        {
            return property.managedReferenceValue switch
            {
                AdjacentActorCountValueProvider => "Adjacent Actor Count",
                AppliedDamageValueProvider => "Applied Damage",
                HealedTargetCountValueProvider => "Healed Target Count",
                PushDistanceValueProvider => "Push Distance",
                null => "None",
                object value => ObjectNames.NicifyVariableName(
                    value.GetType().Name.Replace("ValueProvider", string.Empty))
            };
        }
    }
}
