using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitSO))]
public class UnitSOEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Ambil reference target (UnitSO)
        UnitSO unit = (UnitSO)target;

        // Jika icon ada
        if (unit.icon != null)
        {
            GUILayout.Space(10);
            GUILayout.Label("Preview", EditorStyles.boldLabel);

            // Ambil texture dari sprite
            Texture2D texture = unit.icon.texture;
            if (texture != null)
            {
                // Hitung aspect ratio
                float aspect = (float)texture.width / texture.height;

                // Tentukan tinggi preview
                float previewHeight = 100f;
                float previewWidth = previewHeight * aspect;

                // Buat rect untuk gambar
                Rect rect = GUILayoutUtility.GetRect(previewWidth, previewHeight, GUILayout.ExpandWidth(false));

                // Draw sprite sesuai aspect ratio
                GUI.DrawTexture(rect, texture, ScaleMode.ScaleToFit);
            }
        }

        // Default Inspector
        DrawDefaultInspector();
    }
}