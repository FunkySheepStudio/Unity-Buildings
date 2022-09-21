using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;
using UnityEngine.ProBuilder.MeshOperations;

namespace FunkySheep.Buildings
{
    [RequireComponent(typeof(ProBuilderMesh))]
    [RequireComponent(typeof(MeshCollider))]
    public class Basement : MonoBehaviour
    {
        public GameObject pointPrefab;
        public List<Vector3> points = new List<Vector3>();
        public float thickness = 0;
        public bool created = false;
        public float maxHeight = 0;
        public float minHeight = 0;
        List<GameObject> pointsGo = new List<GameObject>();

        public void AddPoint(Vector3 point)
        {
            if (!created)
            {
                if (points.Count == 0)
                {
                    maxHeight = point.y;
                    minHeight = point.y;
                } else if (point.y > maxHeight)
                {
                    maxHeight = point.y;
                } else if (point.y < minHeight)
                {
                    minHeight = point.y;
                }

                GameObject pointGo = GameObject.Instantiate(pointPrefab);
                pointGo.transform.position = point;
                pointGo.transform.parent = transform;
                pointsGo.Add(pointGo);
                points.Add(point);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if (!created && points.Count >= 2)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    //Connected lines
                    Gizmos.DrawLine(points[i], points[i + 1]);

                    //Height lines
                    Vector3 lowPoint = new Vector3(
                            points[i].x,
                            minHeight,
                            points[i].z
                        );
                    Gizmos.DrawLine(lowPoint, lowPoint + Vector3.up * (maxHeight - minHeight + thickness));

                    lowPoint = new Vector3(
                            points[i + 1].x,
                            minHeight,
                            points[i + 1].z
                        );
                    Gizmos.DrawLine(lowPoint, lowPoint + Vector3.up * (maxHeight - minHeight + thickness));
                }
            }
        }

        public void Create()
        {
            List<Vector3> calculatedPoints = new List<Vector3>();
            ProceduralToolkit.Samples.Buildings.PolygonAsset polygonAsset = ScriptableObject.CreateInstance<ProceduralToolkit.Samples.Buildings.PolygonAsset>();

            for (int i = 0; i < points.Count; i++)
            {
                polygonAsset.vertices.Add(new Vector2(points[i].x, points[i].z));
                calculatedPoints.Add(
                    new Vector3(points[i].x, minHeight, points[i].z)
                );

                pointsGo[i].transform.position = calculatedPoints[i] + Vector3.up * (maxHeight - minHeight + thickness);
            }

            // Basement
            GetComponent<ProBuilderMesh>().CreateShapeFromPolygon(calculatedPoints, maxHeight - minHeight + thickness, false);
            GetComponent<ProceduralToolkit.Samples.Buildings.BuildingGeneratorComponent>().foundationPolygon = polygonAsset;

            GameObject Go = new GameObject("Building");
            Go.transform.parent = transform;
            Go.transform.localPosition = Vector3.up * maxHeight;

            GetComponent<ProceduralToolkit.Samples.Buildings.BuildingGeneratorComponent>().Generate(Go.transform);

            points = calculatedPoints;

            created = true;
        }
    }
}
