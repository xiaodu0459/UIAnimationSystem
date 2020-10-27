using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UIDirector;

[CustomEditor(typeof(CUIEnableBehaviorEvent))]
public class CUIEnableBehaviorEventInspector : Editor
{
    #region Properties
    private SerializedObject targetBehavior;
    private SerializedProperty fireTime;
    private SerializedProperty componentsProperty;
    private SerializedProperty isEnable;
    private int componentSelection;
    #endregion Properties

    GUIContent firetimeContent = new GUIContent("Firetime", "The time in seconds at which this event is fired.");

    public void OnEnable()
    {
        targetBehavior = new SerializedObject(this.target);
        this.fireTime = targetBehavior.FindProperty("fireTime");
        this.componentsProperty = targetBehavior.FindProperty("TargetBehavior");
        this.isEnable = targetBehavior.FindProperty("IsEnable");
        Component currentComponent = componentsProperty.objectReferenceValue as Component;        

        CUIEnableBehaviorEvent behavior = (target as CUIEnableBehaviorEvent);
        if (behavior == null || behavior.AgentTrackGroup == null || behavior.AgentTrackGroup.Agent == null)
        {
            return;
        }

        GameObject agent = behavior.AgentTrackGroup.Agent.gameObject;
        Component[] components = agent.GetComponentsInChildren<Component>(true);
        for (int j = 0; j < components.Length; j++)
        {
            if (components[j] == currentComponent)
            {
                componentSelection = j;
            }
        }
    }

    public override void OnInspectorGUI()
    {
        targetBehavior.Update();

        CUIEnableBehaviorEvent behavior = (target as CUIEnableBehaviorEvent);
        GameObject agent = behavior.AgentTrackGroup.Agent.gameObject;
        
        EditorGUILayout.PropertyField(this.fireTime, firetimeContent);

        List<GUIContent> componentSelectionList = new List<GUIContent>();
        Component[] components = agent.GetComponentsInChildren<Component>(true);
        for (int i = 0; i < components.Length; i++)
        {
            Component component = components[i];
            componentSelectionList.Add(new GUIContent(component.GetType().Name));
            if (componentsProperty.objectReferenceValue as Component == component)
            {
                componentSelection = i;
            }
        }

        componentSelection = EditorGUILayout.Popup(new GUIContent("Behavior"), componentSelection, componentSelectionList.ToArray());
        componentsProperty.objectReferenceValue = components[componentSelection];

        EditorGUILayout.PropertyField(this.isEnable);
        
        targetBehavior.ApplyModifiedProperties();
    }
}
