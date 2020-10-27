using UnityEngine;
using UnityEditor;
using System.Collections;

namespace UIDirector
{

    public class CUIDirectorWindow : EditorWindow
    {
        [MenuItem("Katana/UI Director Window")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(CUIDirectorWindow));
        }

        public const float TOOLBAR_HEIGHT = 17f;
        private int elapsedTime = 0;
        private Vector2 scrollPos;

        void OnGUI()
        {
            Rect toolbarArea = new Rect(0, 0, base.position.width, TOOLBAR_HEIGHT);
            Rect controlArea = new Rect(0, TOOLBAR_HEIGHT, base.position.width, base.position.height - TOOLBAR_HEIGHT);

            /// Toolbar
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUILayout.IntField("Elapsed Time", elapsedTime, EditorStyles.textField);
            EditorGUILayout.EndHorizontal();

            /// Control Area
            EditorGUILayout.BeginHorizontal();
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            ///Timeline toolbar
            float lengthOfSeconds = 60;
            float lengthOfInterval = lengthOfSeconds / 2;
            float TextHeight = 10;
            float markHeight = 7;            
            for (int i = 0; i < 10; i++)
            {
                Rect timeline = new Rect(i * lengthOfSeconds, 0, lengthOfSeconds, TOOLBAR_HEIGHT);
                GUILayout.BeginArea(timeline, EditorStyles.toolbar);
                GUILayout.Label("|");
                GUILayout.EndArea();

                GUILayout.BeginArea(new Rect(i * lengthOfSeconds + 1, 0, lengthOfSeconds - 1, TextHeight), EditorStyles.label);
                GUILayout.TextArea("0:00");
                GUILayout.EndArea();
                
                //GUILayout.BeginArea(new Rect(i * lengthOfSeconds + lengthOfInterval, TextHeight, lengthOfInterval-1, markHeight), EditorStyles.toolbar);
                //GUILayout.Label("|");
                //GUILayout.EndArea();                
            }

            

            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }
    }

}//