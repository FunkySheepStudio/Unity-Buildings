using System.Collections.Generic;
using UnityEngine;

namespace FunkySheep.Buildings
{
    [AddComponentMenu("FunkySheep/Buildings/Building")]
    public class Building : MonoBehaviour
    {
        public GameObject basement = null;
        public GameObject basementPrefab;
        public GameObject stagePrefab;

        public void Create()
        {
            if (basement == null)
            {
                basement = GameObject.Instantiate(basementPrefab, transform);
            }
        }

        public void Reset()
        {
            foreach (Point point in basement.transform.GetComponentsInChildren<Point>())
            {
                DestroyImmediate(point.gameObject);
            }
            
            basement = null;
        }
    }
}
