using System;
using UnityEngine;

[Serializable]
public class Route
{
    [HideInInspector] public string ID;
    [HideInInspector] public RouteName Name;
    [HideInInspector] public RoutePartSettings[] routePartSettings;
}
