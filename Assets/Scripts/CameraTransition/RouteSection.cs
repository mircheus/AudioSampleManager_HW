using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RouteSection : MonoBehaviour
{
    public static void Draw(List<Route> routes)
    {
        for (int routeNumber = 0; routeNumber < routes.Count; routeNumber++)
        {
            string routeName = routes[routeNumber].Name.ToString();
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField(routeName);

            for (int i = 0; i < routes[routeNumber].routePartSettings.Length; i++)
            {
                RoutePartSettings routePartSettings = routes[routeNumber].routePartSettings[i];

                EditorGUILayout.BeginVertical(GUI.skin.window);
                EditorGUILayout.BeginHorizontal(); 
                DrawDeletePathButton(routes[routeNumber], i);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.LabelField($"Point {i + 1}");

                routePartSettings.Position = EditorGUILayout.Vector3Field("position", routePartSettings.Position);
                routePartSettings.Rotation = EditorGUILayout.Vector3Field("Rotation", routePartSettings.Rotation);
                routePartSettings.MoveDuration = EditorGUILayout.FloatField("Move Duration", routePartSettings.MoveDuration);
                EditorGUILayout.EndVertical();
            }
            
            DrawRoutePathButton(routes[routeNumber]);
            EditorGUILayout.EndVertical();
        }
    }


    private static void DrawRoutePathButton(Route route)
    {
        if (GUILayout.Button("Add", GUILayout.Width(45), GUILayout.Height(30)))
        {
            var routePartSettings = new RoutePartSettings[route.routePartSettings.Length + 1];
            for (int i = 0; i < routePartSettings.Length - 1; i++)
            {
                routePartSettings[i] = route.routePartSettings[i];
            }  

            routePartSettings[routePartSettings.Length - 1] = new RoutePartSettings(routePartSettings[routePartSettings.Length - 2].Position + new Vector3(3, 0, 0));

            route.routePartSettings = routePartSettings;
        }
    }

    private static void DrawDeletePathButton(Route route, int index)
    {
        if (GUILayout.Button("-", GUILayout.Width(17), GUILayout.Height(17)))
        {
            RoutePartSettings[] parts = new RoutePartSettings[route.routePartSettings.Length - 1];
            for (int i = 0; i < index; i++)
            {
                parts[i] = route.routePartSettings[i];
            }

            for (int i = index + 1; i < parts.Length + 1; i++)
            {
                parts[i - 1] = route.routePartSettings[i];
            }

            route.routePartSettings = parts;
        }
    }
}
