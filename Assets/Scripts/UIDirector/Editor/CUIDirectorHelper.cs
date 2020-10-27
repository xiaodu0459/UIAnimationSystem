using UnityEngine;
using UnityEditor;
using System.Collections;
using UIDirector;

namespace UIDirectorEditor
{

    public static class CUIDirectorHelper
    {
        [MenuItem("GameObject/UI Director/Director", false, 3)]
        public static void CreateUIDirector()
        {
            if (Selection.activeGameObject == null) return;

            CUIDirector director = Selection.activeGameObject.GetComponentInParent<CUIDirector>();
            if (director != null)
            {
                Debug.LogError("父节点包含UIDirector对象, 不能重复创建!");
                return;
            }

            GameObject go = new GameObject("UI Director");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUIDirector>();
            go.AddComponent<CUIDirectorTrigger>();
        }

        [MenuItem("GameObject/UI Director/AgentItem Track", false, 3)]
        public static void CreateUIAgentItemTrack()
        {
            if (Selection.activeGameObject == null) return;

            CAgentTrackGroup agentTrackGroup = Selection.activeGameObject.GetComponent<CAgentTrackGroup>();
            if (agentTrackGroup != null)
            {
                GameObject go = new GameObject("Agent Item Track");
                go.transform.parent = Selection.activeGameObject.transform;
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                go.AddComponent<CAgentItemTrack>();
                return;
            }

            CUIDirector uiDirector = Selection.activeGameObject.GetComponent<CUIDirector>();
            if (uiDirector != null)
            {
                GameObject parent = new GameObject("Agent Track Group");
                parent.transform.parent = Selection.activeGameObject.transform;
                parent.transform.position = Vector3.zero;
                parent.transform.rotation = Quaternion.identity;
                parent.transform.localScale = Vector3.one;
                parent.AddComponent<CAgentTrackGroup>();

                GameObject go = new GameObject("Agent Item Track");
                go.transform.parent = parent.transform;
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                go.AddComponent<CAgentItemTrack>();
                return;
            }

            Debug.LogError("Agent Item Track必须在UI Director或者Agent Track Group下创建");
        }

        [MenuItem("GameObject/UI Director/Multi Agent Item Track", false, 3)]
        public static void CreateUIMultiAgentTrackGroup()
        {
            if (Selection.activeGameObject == null) return;

            CMultiAgentTrackGroup agentTrackGroup = Selection.activeGameObject.GetComponent<CMultiAgentTrackGroup>();
            if (agentTrackGroup != null)
            {
                GameObject go = new GameObject("Agent Item Track");
                go.transform.parent = Selection.activeGameObject.transform;
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                go.AddComponent<CAgentItemTrack>();
                return;
            }

            CUIDirector uiDirector = Selection.activeGameObject.GetComponent<CUIDirector>();
            if (uiDirector != null)
            {
                GameObject parent = new GameObject("MultiAgent Track Group");
                parent.transform.parent = Selection.activeGameObject.transform;
                parent.transform.position = Vector3.zero;
                parent.transform.rotation = Quaternion.identity;
                parent.transform.localScale = Vector3.one;
                parent.AddComponent<CMultiAgentTrackGroup>();

                GameObject go = new GameObject("Agent Item Track");
                go.transform.parent = parent.transform;
                go.transform.position = Vector3.zero;
                go.transform.rotation = Quaternion.identity;
                go.transform.localScale = Vector3.one;
                go.AddComponent<CAgentItemTrack>();
                return;
            }

            Debug.LogError("Agent Item Track必须在UI Director或者Multi Agent Track Group下创建");
        }

        #region Agent Event

        [MenuItem("GameObject/UI Director/Add Event/Widget Position", false, 3)]
        public static void AddEvent_Position()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UIWidgetPosition");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUIWidgetPositionEvent>();
        }

        [MenuItem("GameObject/UI Director/Add Event/Widget Rotation", false, 3)]
        public static void AddEvent_Rotation()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UIWidgetRotation");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUIWidgetPositionEvent>();
        }

        [MenuItem("GameObject/UI Director/Add Event/Widget Alpha", false, 3)]
        public static void AddEvent_Alpha()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UIWidgetAlpha");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUIWidgetAlphaEvent>();
        }

        [MenuItem("GameObject/UI Director/Add Event/Widget Scale", false, 3)]
        public static void AddEvent_Scale()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UIWidgetScale");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUIWidgetScaleEvent>();
        }

        [MenuItem("GameObject/UI Director/Add Event/Enable GameObject", false, 3)]
        public static void AddEvent_EnableGameObject()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UIEnableGameObject");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUIEnableGameObjectEvent>();
        }

        [MenuItem("GameObject/UI Director/Add Event/Enable MonoBehavior", false, 3)]
        public static void AddEvent_EnableMonoBehavior()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UIEnableMonoBehavior");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUIEnableBehaviorEvent>();
        }

        [MenuItem("GameObject/UI Director/Add Event/Set LayoutGroup Spacing", false, 3)]
        public static void AddEvent_SetLayoutSpacing()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UISetLayoutGroupSpacing");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUISetLayoutGroupSpacingEvent>();
        }
        #endregion //Agent Event

        #region Agent Action
        [MenuItem("GameObject/UI Director/Add Action/Widget Move", false, 3)]
        public static void AddAction_Move()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UIWidgetMove");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUIWidgetMoveAction>();
        }

        [MenuItem("GameObject/UI Director/Add Action/Widget Rotate", false, 3)]
        public static void AddAction_Rotate()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UIWidgetRotate");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUIWidgetRotateAction>();
        }

        [MenuItem("GameObject/UI Director/Add Action/Widget Fade", false, 3)]
        public static void AddAction_Fade()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UIWidgetFade");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUIWidgetFadeAction>();
        }

        [MenuItem("GameObject/UI Director/Add Action/Widget Zoom", false, 3)]
        public static void AddAction_Zoom()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UIWidgetZoom");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUIWidgetZoomAction>();
        }

        [MenuItem("GameObject/UI Director/Add Action/LayoutGroup Spacing Anim", false, 3)]
        public static void AddAction_SpacingAnim()
        {
            if (Selection.activeGameObject == null) return;

            CAgentItemTrack agentItemTrack = Selection.activeGameObject.GetComponent<CAgentItemTrack>();
            if (agentItemTrack == null)
            {
                Debug.LogError("Event必须在Agent Item Track下创建");
                return;
            }

            GameObject go = new GameObject("UILayoutGroupSpacingAnim");
            go.transform.parent = Selection.activeGameObject.transform;
            go.transform.position = Vector3.zero;
            go.transform.rotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.AddComponent<CUILayoutGroupSpacingAction>();
        }
        #endregion //Agent Action
    }

}//namespace UIDirectorEditor