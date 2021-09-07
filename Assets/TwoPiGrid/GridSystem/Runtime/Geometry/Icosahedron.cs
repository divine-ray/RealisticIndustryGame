/**
 * Generates an Icosahedron subdivided as many
 * times as specified.
 * 
 * Created by Jazz Emma Prats Camps in November 2019.
 * Updated in October 2020.
 */

using System;
using System.Collections.Generic;
using UnityEngine;

namespace TwoPiGrid.Geometry
{
    public class Icosahedron : Polyhedron
    {
        public sealed override float Radius { get; protected set; }

        public override Vector3[] Vertices
        {
            get => vertices.ToArray();
            protected set => vertices = new List<Vector3>(value);
        }
        private List<Vector3> vertices;
        public sealed override int[][] Faces { get; protected set; }
        public override Vector3[] Centers { get; protected set; }

        public Icosahedron(float radius, int subdivisions = 0)
        {
            Radius = radius;

            float t = (1f + Mathf.Sqrt(5f)) / 2f;
            vertices = new List<Vector3>
            {
                new Vector3(-1, t, 0).normalized * radius,
                new Vector3(1, t, 0).normalized * radius,
                new Vector3(-1, -t, 0).normalized * radius,
                new Vector3(1, -t, 0).normalized * radius,
        
                new Vector3(0, -1, t).normalized * radius,
                new Vector3(0, 1, t).normalized * radius,
                new Vector3(0, -1, -t).normalized * radius,
                new Vector3(0, 1, -t).normalized * radius,
        
                new Vector3(t, 0, -1).normalized * radius,
                new Vector3(t, 0, 1).normalized * radius,
                new Vector3(-t, 0, -1).normalized * radius,
                new Vector3(-t, 0, 1).normalized * radius
            };

            var triangles = new int[60]
            {
                0, 11, 5,
                0, 5, 1,
                0, 1, 7,
                0, 7, 10,
                0, 10, 11,
                1, 5, 9,
                5, 11, 4,
                11, 10, 2,
                10, 7, 6,
                7, 1, 8,
                3, 9, 4,
                3, 4, 2,
                3, 2, 6,
                3, 6, 8,
                3, 8, 9,
                4, 9, 5,
                2, 4, 11,
                6, 2, 10,
                8, 6, 7,
                9, 8, 1
            };

            for (int i = 0; i < subdivisions; i++)
                Subdivide(ref triangles);

            Faces = new int[triangles.Length/3][];
            for (int i = 0; i < triangles.Length/3; i++)
                Faces[i] = new int[3] { triangles[3*i], triangles[3*i+1], triangles[3*i+2] };

            CalculateIncenters(triangles);
        }

        private void Subdivide(ref int[] triangles)
        {
            int[] newTriangles = new int[triangles.Length * 4]; //4 new faces for each triangle.

            int j = 0;
            for (int i = 0; i < triangles.Length; i+=3)
            {
                int a = GetMiddleVertex(triangles[i], triangles[i+1]);
                int b = GetMiddleVertex(triangles[i+1], triangles[i+2]);
                int c = GetMiddleVertex(triangles[i+2], triangles[i]);

                newTriangles[j] = triangles[i];
                newTriangles[j+1] = a;
                newTriangles[j+2] = c;

                newTriangles[j+3] = triangles[i+1];
                newTriangles[j+4] = b;
                newTriangles[j+5] = a;

                newTriangles[j+6] = triangles[i+2];
                newTriangles[j+7] = c;
                newTriangles[j+8] = b;

                newTriangles[j+9] = a;
                newTriangles[j+10] = b;
                newTriangles[j+11] = c;

                j += 12;
            }

            triangles = newTriangles;
        }

        private readonly Dictionary<Int64, int> middlePointIndexCache = new Dictionary<Int64, int>();

        private int GetMiddleVertex(int vertexIndex1, int vertexIndex2)
        {
            bool firstIsSmaller = vertexIndex1 < vertexIndex2;
            Int64 smallerIndex = firstIsSmaller ? vertexIndex1 : vertexIndex2;
            Int64 greaterIndex = firstIsSmaller ? vertexIndex2 : vertexIndex1;
            Int64 key = (smallerIndex << 32) + greaterIndex;

            //Check if it already exists
            int middleVertexIndex;
            if (middlePointIndexCache.TryGetValue(key, out middleVertexIndex))
                return middleVertexIndex;

            //If it doesn't, create it
            middleVertexIndex = vertices.Count;
            vertices.Add(((Vertices[vertexIndex1] + Vertices[vertexIndex2]) / 2f).normalized * Radius);

            //And store it in the dictionary
            middlePointIndexCache.Add(key, middleVertexIndex);

            return middleVertexIndex;
        }

        private void CalculateIncenters(IReadOnlyList<int> triangles)
        {
            Centers = new Vector3[triangles.Count / 3];

            int j = 0;
            for (int i = 0; i < triangles.Count; i += 3)
            {
                float a = Vector3.Distance(vertices[triangles[i+1]], vertices[triangles[i+2]]);
                float b = Vector3.Distance(vertices[triangles[i+2]], vertices[triangles[i]]);
                float c = Vector3.Distance(vertices[triangles[i]], vertices[triangles[i+1]]);
                float perimeter = a + b + c;
                Centers[j] = new Vector3
                {
                    x = (a * vertices[triangles[i]].x + b * vertices[triangles[i+1]].x + c * vertices[triangles[i+2]].x) / perimeter,
                    y = (a * vertices[triangles[i]].y + b * vertices[triangles[i+1]].y + c * vertices[triangles[i+2]].y) / perimeter,
                    z = (a * vertices[triangles[i]].z + b * vertices[triangles[i+1]].z + c * vertices[triangles[i+2]].z) / perimeter
                };
                j++;
            }
        }
    }
}
