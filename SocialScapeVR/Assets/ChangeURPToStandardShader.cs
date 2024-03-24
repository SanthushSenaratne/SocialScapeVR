using UnityEngine;
using UnityEditor; // Namespace for editor functionality

public class ChangeURPToStandardShader : Editor
{
    [MenuItem("Tools/Change URP Shaders to Standard")] // Creates a menu item
    static void ChangeShaders()
    {
        foreach (var material in Resources.FindObjectsOfTypeAll<Material>())
        {
            if (material.shader.name.StartsWith("Universal Render Pipeline"))
            {
                material.shader = Shader.Find("Standard");
            }
        }

        Debug.Log("Shaders changed successfully!"); // Informative message
    }
}