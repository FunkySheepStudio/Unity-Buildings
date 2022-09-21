using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.Buildings
{
    [RequireComponent(typeof(ProBuilderMesh))]
    [RequireComponent(typeof(MeshCollider))]
    public class Stage : MonoBehaviour
    {
        public GameObject startSelection = null;
        public GameObject endSelection = null;
        public float height = 2.5f;

        public void Create(List<Vector3> points, int index)
        {
            List<Vector3> calculatedPoints = new List<Vector3>();
            for (int i = 0; i < points.Count; i++)
            {
                calculatedPoints.Add(
                    points[i] + Vector3.up * index * height
                );
            }

            GetComponent<ProBuilderMesh>().CreateShapeFromPolygon(calculatedPoints, 0.5f, false);
        }

        public void AddWall()
        {
            
        }
    }
}
