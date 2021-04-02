using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;
using System.Collections;



public class GradientTexGen {

    public static Texture2D Create(Gradient grad, int width = 32, int height = 1, int selectGradientOption = 0)
    {
        var gradTex = new Texture2D(width, height, TextureFormat.ARGB32, false);
        gradTex.filterMode = FilterMode.Bilinear;
        if (selectGradientOption == 0) {
            Debug.Log("Horizontal");
            float inv = 1f / width;
            for (int y = 0; y < height; y++) {
                for (int x = 0; x < width; x++) {
                    var t = x * inv;
                    Color col = grad.Evaluate(t);
                    gradTex.SetPixel(x, y, col);
                }
            }
        } else {
            Debug.Log("Vertical");
            float inv = 1f / height;
            for (int y = height; y >= 0; y--) {
                int inverseY = height - y;
                for (int x = 0; x < width; x++) {
                    var t = inverseY * inv;
                    Color col = grad.Evaluate(t);
                    gradTex.SetPixel(x, y, col);
                    Debug.Log("x: " + x + " y: " + y + " col: " + col);
                }
            }
        }
        gradTex.Apply();
        return gradTex;
    }

}

#if UNITY_EDITOR
public class GradientTexCreator : EditorWindow {

    [SerializeField] Gradient gradient;
    [SerializeField] int width = 128;
    [SerializeField] int height = 16;
    [SerializeField] string fileName = "Gradient";


    string[] options = new string[] {
     "Horizontal", "Vertical"
    };

    [SerializeField] int selectGradientOption = 0;

    [MenuItem("Tools/GradientTex")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(GradientTexCreator));
    }

    void OnGUI()
    {
        EditorGUI.BeginChangeCheck();
        SerializedObject so = new SerializedObject(this);
        EditorGUILayout.PropertyField(so.FindProperty("gradient"), true, null);
        if (EditorGUI.EndChangeCheck()) {
            so.ApplyModifiedProperties();
        }

        using (new GUILayout.HorizontalScope()) {
            GUILayout.Label("Width", GUILayout.Width(80f));
            int.TryParse(GUILayout.TextField(width.ToString(), GUILayout.Width(120f)), out width);
        }

        using (new GUILayout.HorizontalScope()) {
            GUILayout.Label("Height", GUILayout.Width(80f));
            int.TryParse(GUILayout.TextField(height.ToString(), GUILayout.Width(120f)), out height);
        }

        using (new GUILayout.HorizontalScope()) {
            GUILayout.Label("Gradient Opt.", GUILayout.Width(80f));
            selectGradientOption = EditorGUILayout.Popup(selectGradientOption, options, GUILayout.Width(120f));
            //int.TryParse(GUILayout.TextField(selectGradientOption.ToString(), GUILayout.Width(120f)), out selectGradientOption);
        }

            using (new GUILayout.HorizontalScope()) {
            GUILayout.Label("Name", GUILayout.Width(80f));
            fileName = GUILayout.TextField(fileName, GUILayout.Width(120f));
            GUILayout.Label(".png");
        }

        if (GUILayout.Button("Save")) {
            string path = EditorUtility.SaveFolderPanel("Select an output path", "", "");
            if (path.Length > 0) {
                var tex = GradientTexGen.Create(gradient, width, height, selectGradientOption);
                byte[] pngData = tex.EncodeToPNG();
                File.WriteAllBytes(path + "/" + fileName + ".png", pngData);
                AssetDatabase.Refresh();
            }
        }
    }

#endif

}