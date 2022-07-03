/**
 * Generates a truncated icosahedron.
 * Belongs to a Chain of Responsibility (pattern).
 *
 * Created by Jazz Emma Prats Camps in October 2020.
 * Math from: http://dmccooey.com/polyhedra/DualGeodesicIcosahedra.html
 */

using UnityEngine;

namespace TwoPiGrid.Geometry.Internal
{
    internal class TruncatedIcosahedron : DualGeodesicIcosahedronGenerator
    {
        public TruncatedIcosahedron(DualGeodesicIcosahedronGenerator nextInChain) : base(nextInChain) { }

        public DualGeodesicIcosahedron GeneratePlease(float radius)
        {
            return DoGenerate(radius);
        }

        protected override bool AmResponsible(int order) => order == 0;

        protected override DualGeodesicIcosahedron DoGenerate(float radius)
        {
            float C0 = (Mathf.Sqrt(5f) - 1f) / 6f;
            float C1 = 1f / 3f;
            float C2 = (Mathf.Sqrt(5f) - 1f) / 3f;
            float C3 = 2f / 3f;
            float C4 = Mathf.Sqrt(5f) / 3f;
            float C5 = (3f + Mathf.Sqrt(5f)) / 6f;

            var vertices = new Vector3[]
            {
                new Vector3(C0, 0, 1).normalized * radius,
                new Vector3(  C0,  0, -1).normalized * radius,
                new Vector3( -C0,  0,  1).normalized * radius,
                new Vector3( -C0,  0, -1).normalized * radius,
                new Vector3( 1,   C0,  0).normalized * radius,
                new Vector3( 1,  -C0,  0).normalized * radius,
                new Vector3(-1,   C0,  0).normalized * radius,
                new Vector3(-1,  -C0,  0).normalized * radius,
                new Vector3( 0,  1,   C0).normalized * radius,
                new Vector3( 0,  1,  -C0).normalized * radius,
                new Vector3( 0, -1,   C0).normalized * radius,
                new Vector3( 0, -1,  -C0).normalized * radius,
                new Vector3(  C2,   C1,   C5).normalized * radius,
                new Vector3(  C2,   C1,  -C5).normalized * radius,
                new Vector3(  C2,  -C1,   C5).normalized * radius,
                new Vector3(  C2,  -C1,  -C5).normalized * radius,
                new Vector3( -C2,   C1,   C5).normalized * radius,
                new Vector3( -C2,   C1,  -C5).normalized * radius,
                new Vector3( -C2,  -C1,   C5).normalized * radius,
                new Vector3( -C2,  -C1,  -C5).normalized * radius,
                new Vector3(  C5,   C2,   C1).normalized * radius,
                new Vector3(  C5,   C2,  -C1).normalized * radius,
                new Vector3(  C5,  -C2,   C1).normalized * radius,
                new Vector3(  C5,  -C2,  -C1).normalized * radius,
                new Vector3( -C5,   C2,   C1).normalized * radius,
                new Vector3( -C5,   C2,  -C1).normalized * radius,
                new Vector3( -C5,  -C2,   C1).normalized * radius,
                new Vector3( -C5,  -C2,  -C1).normalized * radius,
                new Vector3(  C1,   C5,   C2).normalized * radius,
                new Vector3(  C1,   C5,  -C2).normalized * radius,
                new Vector3(  C1,  -C5,   C2).normalized * radius,
                new Vector3(  C1,  -C5,  -C2).normalized * radius,
                new Vector3( -C1,   C5,   C2).normalized * radius,
                new Vector3( -C1,   C5,  -C2).normalized * radius,
                new Vector3( -C1,  -C5,   C2).normalized * radius,
                new Vector3( -C1,  -C5,  -C2).normalized * radius,
                new Vector3(  C0,   C3,   C4).normalized * radius,
                new Vector3(  C0,   C3,  -C4).normalized * radius,
                new Vector3(  C0,  -C3,   C4).normalized * radius,
                new Vector3(  C0,  -C3,  -C4).normalized * radius,
                new Vector3( -C0,   C3,   C4).normalized * radius,
                new Vector3( -C0,   C3,  -C4).normalized * radius,
                new Vector3( -C0,  -C3,   C4).normalized * radius,
                new Vector3( -C0,  -C3,  -C4).normalized * radius,
                new Vector3(  C4,   C0,   C3).normalized * radius,
                new Vector3(  C4,   C0,  -C3).normalized * radius,
                new Vector3(  C4,  -C0,   C3).normalized * radius,
                new Vector3(  C4,  -C0,  -C3).normalized * radius,
                new Vector3( -C4,   C0,   C3).normalized * radius,
                new Vector3( -C4,   C0,  -C3).normalized * radius,
                new Vector3( -C4,  -C0,   C3).normalized * radius,
                new Vector3( -C4,  -C0,  -C3).normalized * radius,
                new Vector3(  C3,   C4,   C0).normalized * radius,
                new Vector3(  C3,   C4,  -C0).normalized * radius,
                new Vector3(  C3,  -C4,   C0).normalized * radius,
                new Vector3(  C3,  -C4,  -C0).normalized * radius,
                new Vector3( -C3,   C4,   C0).normalized * radius,
                new Vector3( -C3,   C4,  -C0).normalized * radius,
                new Vector3( -C3,  -C4,   C0).normalized * radius,
                new Vector3( -C3,  -C4,  -C0).normalized * radius
            };

            var faces = new int[][]
            {
                new int[] { 0, 2, 18, 42, 38, 14 },
                new int[] { 1, 3, 17, 41, 37, 13 },
                new int[] { 2, 0, 12, 36, 40, 16 },
                new int[] { 3, 1, 15, 39, 43, 19 },
                new int[] { 4, 5, 23, 47, 45, 21 },
                new int[] { 5, 4, 20, 44, 46, 22 },
                new int[] { 6, 7, 26, 50, 48, 24 },
                new int[] { 7, 6, 25, 49, 51, 27 },
                new int[] { 8, 9, 33, 57, 56, 32 },
                new int[] { 9, 8, 28, 52, 53, 29 },
                new int[] { 10, 11, 31, 55, 54, 30 },
                new int[] { 11, 10, 34, 58, 59, 35 },
                new int[] { 12, 44, 20, 52, 28, 36 },
                new int[] { 13, 37, 29, 53, 21, 45 },
                new int[] { 14, 38, 30, 54, 22, 46 },
                new int[] { 15, 47, 23, 55, 31, 39 },
                new int[] { 16, 40, 32, 56, 24, 48 },
                new int[] { 17, 49, 25, 57, 33, 41 },
                new int[] { 18, 50, 26, 58, 34, 42 },
                new int[] { 19, 43, 35, 59, 27, 51 },
                new int[] { 0, 14, 46, 44, 12 },
                new int[] { 1, 13, 45, 47, 15 },
                new int[] { 2, 16, 48, 50, 18 },
                new int[] { 3, 19, 51, 49, 17 },
                new int[] { 4, 21, 53, 52, 20 },
                new int[] { 5, 22, 54, 55, 23 },
                new int[] { 6, 24, 56, 57, 25 },
                new int[] { 7, 27, 59, 58, 26 },
                new int[] { 8, 32, 40, 36, 28 },
                new int[] { 9, 29, 37, 41, 33 },
                new int[] { 10, 30, 38, 42, 34 },
                new int[] { 11, 35, 43, 39, 31 }
            };

            var centers = new Vector3[faces.Length];
            for (int i = 0; i < faces.Length; i++)
            {
                Vector3 center = new Vector3();
                for (int j = 0; j < faces[i].Length; j++)
                    center += vertices[faces[i][j]];
                centers[i] = center / faces[i].Length;
            }
            
            return new DualGeodesicIcosahedron(radius, vertices, faces, centers);
        }
    }
}
