// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// #if UNITY_EDITOR
// using UnityEditor;

// /// <summary> Sets a background color for game objects in the Hierarchy tab</summary>
// [UnityEditor.InitializeOnLoad]
// #endif
// public class HierarchyObjectColor
// {
//     private static Vector2 offset = new Vector2(20, 1);

//     static HierarchyObjectColor()
//     {
//         EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
//     }

//     private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
//     {
//         var obj = EditorUtility.InstanceIDToObject(instanceID);
//         if (obj != null)
//         {
//             Color backgroundColor = Color.white;
//             Color textColor = Color.white;
//             Texture2D texture = null;

//             // Write your object name in the hierarchy.
//             if (obj.name == "----------IMPORTANTS------------------  ")
//             {
//                 backgroundColor = HexToColor("#221228");
//                 textColor = HexToColor("#E5E5E5");
//             }
//             if (obj.name == "----------CAMERAS------------------")
//             {
//                 backgroundColor = HexToColor("#652654");
//                 textColor = HexToColor("#E5E5E5");
//             }if (obj.name == "----------OBJECTS------------------ ")
//             {
//                 backgroundColor = HexToColor("#9e3455");
//                 textColor = HexToColor("#E5E5E5");
//             }if (obj.name == "----------BACKGROUND-----------------")
//             {
//                 backgroundColor = HexToColor("#e56349");
//                 textColor = HexToColor("#E5E5E5");
//             }





//             if (backgroundColor != Color.white)
//             {
//                 Rect offsetRect = new Rect(selectionRect.position + offset, selectionRect.size);
//                 Rect bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);

//                 EditorGUI.DrawRect(bgRect, backgroundColor);
//                 EditorGUI.LabelField(offsetRect, obj.name, new GUIStyle()
//                 {
//                     normal = new GUIStyleState() { textColor = textColor },
//                     fontStyle = FontStyle.Bold
//                 });

//                 if (texture != null)
//                     EditorGUI.DrawPreviewTexture(new Rect(selectionRect.position, new Vector2(selectionRect.height, selectionRect.height)), texture);
//             }
//         }
//     }

//     private static Color HexToColor(string hex)
//     {
//         if (hex.StartsWith("#"))
//         {
//             hex = hex.Substring(1);
//         }

//         if (hex.Length == 6)
//         {
//             hex += "FF"; // Add alpha if not specified
//         }

//         byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
//         byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
//         byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
//         byte a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);

//         return new Color32(r, g, b, a);
//     }
// }
