using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointType
{
    anchor, previous, following
}

[System.Serializable]
public class Path
{
    public List<Vector3> anchor;
    public List<Vector3> previous;
    public List<Vector3> following;

    public Path(Vector3 pos)
    {
        anchor = new List<Vector3>();
        previous = new List<Vector3>();
        following = new List<Vector3>();

        pos.y = 0;

        anchor.Add(pos + Vector3.left);
        previous.Add(pos + Vector3.back + Vector3.left);
        following.Add(pos + Vector3.forward + Vector3.left);

        anchor.Add(pos + Vector3.right);
        previous.Add(pos + Vector3.back + Vector3.right);
        following.Add(pos + Vector3.forward + Vector3.right);
    }

    public void MovePoint(int i, Vector3 newPos, PointType point)
    {
        newPos.y = 0;

        if (point == PointType.anchor)
        {
            Vector3 delta = newPos - anchor[i];

            anchor[i] += delta;
            previous[i] += delta;
            following[i] += delta;
        }
        else if (point == PointType.previous)
        {
            float dist = Vector3.Distance(following[i], anchor[i]);

            previous[i] = newPos;

            Vector3 dir = (anchor[i] - previous[i]).normalized;

            following[i] = anchor[i] + dir * dist;
        }
        else
        {
            float dist = Vector3.Distance(previous[i], anchor[i]);

            following[i] = newPos;

            Vector3 dir = (anchor[i] - following[i]).normalized;

            previous[i] = anchor[i] + dir * dist;
        }
    }

    public void AddPoint(Vector3 pos)
    {
        pos.y = 0;

        anchor.Add(pos);

        Vector3 dir = anchor[LoopIndex(anchor.Count)] - anchor[LoopIndex(anchor.Count - 2)];

        float pDist = Vector3.Distance(anchor[LoopIndex(anchor.Count - 2)], anchor[LoopIndex(anchor.Count - 1)]);
        float fDist = Vector3.Distance(anchor[LoopIndex(anchor.Count)], anchor[LoopIndex(anchor.Count - 1)]);

        previous.Add(pos + -dir.normalized * pDist/2);
        following.Add(pos + dir.normalized * fDist/2);
    }

    public int LoopIndex(int i)
    {
        return ((i % anchor.Count) + anchor.Count) % anchor.Count;
    }
}
