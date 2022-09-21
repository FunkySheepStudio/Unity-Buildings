using UnityEditor;
using UnityEngine;

namespace FunkySheep.Buildings
{
    [CustomEditor(typeof(Building))]
    public class Building_Inspector : Editor
    {
        Building building;
        bool creating = false;

        private void OnEnable()
        {
            building = (Building)target;
        }

        private void OnDisable()
        {
             building.Reset();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            creating = GUILayout.Toggle(creating, "Create");
        }

        void OnSceneGUI()
        {
            if (creating)
            {
                //Remove the possibility to select another object
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));

                if (building.basement == null)
                {
                    building.Create();
                }
                else if (!building.basement.GetComponent<Basement>().created)
                {                   
                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        RaycastHit hit = MouseRayCast();
                        if (hit.collider is TerrainCollider)
                        {
                            building.basement.GetComponent<Basement>().AddPoint(hit.point);
                        }
                    }

                    if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
                    {
                        building.basement.GetComponent<Basement>().Create();
                    }

                    if (Event.current.type == EventType.ScrollWheel)
                    {
                        building.basement.GetComponent<Basement>().thickness += Event.current.delta.y;
                    }
                } else
                {
                    if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
                    {
                        building.Reset();
                        creating = false;
                    }
                }
            }
        }

        RaycastHit MouseRayCast()
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);

            RaycastHit hit;
            Physics.Raycast(ray.origin, ray.direction, out hit);

            return hit;
        }
    }
}
