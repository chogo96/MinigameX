using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

namespace WNC.HYPEPOLY
{
    [CustomEditor(typeof(Rotator))]

    public class RotatorEditor : Editor
    {
        private GUIStyle currentStyle = null;
        public override void OnInspectorGUI()
        {
            Rotator rotator = (Rotator)target;
            currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.normal.background = MakeTex(2, 2);
            GUILayout.Box(Resources.Load<Texture>("Sprites/WNC_R_Logo"), currentStyle, GUILayout.Width(Screen.width));

            GUILayout.Box("Rotate by local axis", GUILayout.Width(Screen.width));
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            rotator.localAxis = GUILayout.Toggle(rotator.localAxis,"");
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.Box("Rotation direction", GUILayout.Width(Screen.width));
            rotator.direction = EditorGUILayout.Vector3Field("",rotator.direction);


        }
        private Texture2D MakeTex(int width, int height)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = Color.black;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}