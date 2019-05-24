using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path path;

    void OnEnable()
    {
        creator = (PathCreator)target;
        if (creator.path == null)
        {
            creator.CreatePath();
        }
        path = creator.path;
    }

    void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Draw()
    {
        for (int i = 0; i < path.anchor.Count; i++)
        {
            Handles.color = Color.red;

            Vector3 aPos = Handles.FreeMoveHandle(path.anchor[i], Quaternion.identity, 10, Vector3.zero, Handles.CylinderHandleCap);
            if (aPos != path.anchor[i])
            {
                path.MovePoint(i, aPos, PointType.anchor);
            }

            Handles.color = Color.magenta;

            Vector3 pPos = Handles.FreeMoveHandle(path.previous[i], Quaternion.identity, 10, Vector3.zero, Handles.CylinderHandleCap);
            if (pPos != path.previous[i])
            {
                path.MovePoint(i, pPos, PointType.previous);
            }

            Vector3 fPos = Handles.FreeMoveHandle(path.following[i], Quaternion.identity, 10, Vector3.zero, Handles.CylinderHandleCap);
            if (fPos != path.following[i])
            {
                path.MovePoint(i, fPos, PointType.following);
            }

            Handles.color = Color.black;

            Handles.DrawLine(path.anchor[i], path.previous[i]);
            Handles.DrawLine(path.anchor[i], path.following[i]);

            Handles.DrawBezier(path.anchor[i], path.anchor[path.LoopIndex(i+1)], path.following[i], path.previous[path.LoopIndex(i+1)], Color.green, null, 2);
        }
    }

    void Input()
    {
        Event gui = Event.current;
        Vector3 mouse = HandleUtility.GUIPointToWorldRay(gui.mousePosition).origin;

        if (gui.type == EventType.MouseDown && gui.button == 0 && gui.shift)
        {
            path.AddPoint(mouse);
        }
    }
}
