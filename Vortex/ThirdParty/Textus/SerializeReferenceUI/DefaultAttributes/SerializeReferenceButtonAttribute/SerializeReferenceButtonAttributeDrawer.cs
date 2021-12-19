#if UNITY_EDITOR

using UnityEditor; 
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializeReferenceButtonAttribute))]
public class SerializeReferenceButtonAttributeDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        try
        {
            EditorGUI.BeginProperty(position, label, property);

            var labelPosition = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(labelPosition, label);

            var typeRestrictions = SerializedReferenceUIDefaultTypeRestrictions.GetAllBuiltInTypeRestrictions(fieldInfo);
            property.DrawSelectionButtonForManagedReference(position, typeRestrictions);

            if (property.isArray)
            {
                if (property.arraySize < 0) { property.arraySize = 0; }
            }
            try
            {
                EditorGUI.PropertyField(position, property, GUIContent.none, true);
            }
            catch (System.Exception ex)
            {
                Debug.Log("ex 1: " + ex.Message + " display name: " + property.displayName + " and name: " + property.name+" is array type: "+property.isArray);
            }
            

            EditorGUI.EndProperty();
        }
        catch (System.Exception ex)
        {
            Debug.Log("ex 2: " + ex.Message + " display name: " + property.displayName + " and name: " + property.name + " is array type: " + property.isArray);
        }

    } 
}
#endif