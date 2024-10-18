using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;

namespace WNC.HYPEPOLY
{
    [CustomEditor(typeof(ProjectilesThrower))]

    public class ProjectilesThrowerEditor : Editor
    {
        private GUIStyle currentStyle = null;
        public override void OnInspectorGUI()
        {
            ProjectilesThrower thrower = (ProjectilesThrower)target;
            currentStyle = new GUIStyle(GUI.skin.box);
            currentStyle.normal.background = MakeTex(2, 2);
            GUILayout.Box(Resources.Load<Texture>("Sprites/WNC_PT_Logo"), currentStyle, GUILayout.Width(Screen.width));

            GUILayout.Box("Parent for spawned projectiles", GUILayout.Width(Screen.width));
            thrower.projectileSpawnParent = (Transform)EditorGUILayout.ObjectField((Object)thrower.projectileSpawnParent, typeof(Transform), GUILayout.Width(Screen.width));

            GUILayout.Box("Projectile prefab", GUILayout.Width(Screen.width));
            thrower.projectile = (GameObject)EditorGUILayout.ObjectField((Object)thrower.projectile, typeof(GameObject), GUILayout.Width(Screen.width));

            GUILayout.Box("Impulse power", GUILayout.Width(Screen.width));
            thrower.impulsePower = EditorGUILayout.FloatField(thrower.impulsePower, GUILayout.Width(Screen.width));
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