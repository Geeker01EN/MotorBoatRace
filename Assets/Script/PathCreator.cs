using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [HideInInspector] public Path path;

    public void CreatePath()
    {
        path = new Path(transform.position);
    }

    GameObject[] limiters;
    int limitersNum;

    [Range(0f, 1f)] [SerializeField] float timeBetweenLimiters;
    [SerializeField] GameObject circuitLimiter;
    [SerializeField] float width;

    void Start()
    {
        limitersNum = (int)Mathf.Floor(1 / timeBetweenLimiters);
        limiters = new GameObject[path.anchor.Count * limitersNum * 2];

        for (int a = 0; a < path.anchor.Count; a++)
        {
            float t = 0;

            for (int i = 0; i < limitersNum; i++)
            {

                Vector3 pos = Bezier.Cubic(path.anchor[a], path.following[a], path.previous[path.LoopIndex(a + 1)], path.anchor[path.LoopIndex(a+1)], t);
                Vector3 dir = (Bezier.Cubic(path.anchor[a], path.following[a], path.previous[path.LoopIndex(a + 1)], path.anchor[path.LoopIndex(a + 1)], t + .01f) - pos).normalized;

                limiters[a * limitersNum + i] = Instantiate(circuitLimiter, pos + new Vector3(-dir.z, 0, dir.x) * width, Quaternion.identity);
                limiters[path.anchor.Count * limitersNum + a * limitersNum + i] = Instantiate(circuitLimiter, pos + new Vector3(dir.z, 0, -dir.x) * width, Quaternion.identity);

                limiters[a * limitersNum + i].GetComponent<KeepPosition>().PositionToMaintain = pos + new Vector3(-dir.z, 0, dir.x) * width;
                limiters[path.anchor.Count * limitersNum + a * limitersNum + i].GetComponent<KeepPosition>().PositionToMaintain = pos + new Vector3(dir.z, 0, -dir.x) * width;

                t += timeBetweenLimiters;
            }
        
        }
    }
}
