using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

namespace WNC.HYPEPOLY
{
    [CustomEditor(typeof(AnimationScenarist))]
    public class AnimationScenaristEditor : Editor
    {
        private GUIStyle currentStyle = null;
        public override void OnInspectorGUI()
        {
            AnimationScenarist scenarist = (AnimationScenarist)target;
            currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.normal.background = MakeTex(2, 2);
            GUILayout.Box(Resources.Load<Texture>("Sprites/WNC_AS_Logo"), currentStyle, GUILayout.Width(Screen.width));

            for(int i = 0; i<scenarist.names.Count; i++)
            {
                if(scenarist.names[i] == "Delay")
                {
                    GUILayout.Box("Delay before start of animator: [" + scenarist.values[i] + "s.]", GUILayout.Width(Screen.width));
                    EditorGUILayout.BeginHorizontal();
                    scenarist.values[i] = LimitDecimalPlace(EditorGUILayout.FloatField(scenarist.values[i], GUILayout.MaxWidth(50f)),2);
                    if (scenarist.values[i] < 0.1f) scenarist.values[i] = 0f;
                    scenarist.values[i] = LimitDecimalPlace(GUILayout.HorizontalSlider(scenarist.values[i], 0.1f, 15f),2);
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                    break;
                }
            }

            for (int i = 0; i < scenarist.names.Count; i++)
            {
                if (scenarist.names[i] == "Timer")
                {
                    GUILayout.Box("Cooldown of animation cycle: [" + scenarist.values[i] + "s.]", GUILayout.Width(Screen.width));
                    EditorGUILayout.BeginHorizontal();
                    scenarist.values[i] = LimitDecimalPlace(EditorGUILayout.FloatField(scenarist.values[i], GUILayout.MaxWidth(50f)), 2);
                    if (scenarist.values[i] < 0.1f) scenarist.values[i] = 0f;
                    scenarist.values[i] = LimitDecimalPlace(GUILayout.HorizontalSlider(scenarist.values[i], 0.1f, 15f), 2);
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                    break;
                }
            }

            for (int i = 0; i < scenarist.names.Count; i++)
            {
                if (scenarist.names[i] != "Timer" && scenarist.names[i] != "Delay")
                {
                    GUILayout.Box("[" + scenarist.names[i] + "] animation speed multiplier: [x" + scenarist.values[i] + "]", GUILayout.Width(Screen.width));
                    EditorGUILayout.BeginHorizontal();
                    scenarist.values[i] = LimitDecimalPlace(EditorGUILayout.FloatField(scenarist.values[i], GUILayout.MaxWidth(50f)), 2);
                    if (scenarist.values[i] < 0.1f) scenarist.values[i] = 0f;
                    scenarist.values[i] = LimitDecimalPlace(GUILayout.HorizontalSlider(scenarist.values[i], 0.1f, 4f), 2);
                    EditorGUILayout.EndHorizontal();
                    GUILayout.Space(5f);
                }
            }
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
        private float LimitDecimalPlace(double number, int limitPlace)
        {
            float result = 0;
            string sNumber = number.ToString("#.##");
            int decimalIndex = sNumber.IndexOf(".");
            if (decimalIndex != -1)
            {
                sNumber = sNumber.Remove(decimalIndex + limitPlace + 1);
            }

            result = float.Parse(sNumber);
            return result;
        }
    }
}