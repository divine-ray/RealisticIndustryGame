/**
 * Generates a chamfered dodecahedron.
 * Belongs to a Chain of Responsibility (pattern).
 *
 * Created by Jazz Emma Prats Camps in October 2020.
 * Math from: http://dmccooey.com/polyhedra/DualGeodesicIcosahedra.html
 */

using UnityEngine;

namespace TwoPiGrid.Geometry.Internal
{
    internal class ChamferedDodecahedron : DualGeodesicIcosahedronGenerator
    {
        public ChamferedDodecahedron(DualGeodesicIcosahedronGenerator nextInChain) : base(nextInChain) { }

        protected override bool AmResponsible(int order) => order == 1;

        protected override DualGeodesicIcosahedron DoGenerate(float radius)
        {
            float C0 = (Mathf.Sqrt(5f) - 3f + 2f * Mathf.Sqrt(5f - 2f * Mathf.Sqrt(5f))) / 4f;
            float C1 = (1f - Mathf.Sqrt(5f) + Mathf.Sqrt(2f * (5f - Mathf.Sqrt(5f)))) / 4f;
            float C2 = (Mathf.Sqrt(5f) - 1f) / 4f;
            float C3 = Mathf.Sqrt(5f - 2f * Mathf.Sqrt(5f)) / 2f;
            float C4 = Mathf.Sqrt(2f * (5f - Mathf.Sqrt(5f))) / 4f;
            float C5 = (Mathf.Sqrt(5f) - 1f + 2f * Mathf.Sqrt(5f - 2f * Mathf.Sqrt(5f))) / 4f;
            float C6 = (Mathf.Sqrt(5f) - 3f + Mathf.Sqrt(2f * (5f + Mathf.Sqrt(5f)))) / 4f;
            float C7 = (3 - Mathf.Sqrt(5f) + Mathf.Sqrt(2f * (5f - Mathf.Sqrt(5f)))) / 4f;
            float C8 = Mathf.Sqrt(2f * (5f + Mathf.Sqrt(5f))) / 4f;

            var vertices = new Vector3[]
            {
                new Vector3(0,  C3,  C8).normalized * radius,
                new Vector3(0,  C3, -C8).normalized * radius,
                new Vector3(0, -C3,  C8).normalized * radius,
                new Vector3(0, -C3, -C8).normalized * radius,
                new Vector3( C8, 0,  C3).normalized * radius,
                new Vector3( C8, 0, -C3).normalized * radius,
                new Vector3(-C8, 0,  C3).normalized * radius,
                new Vector3(-C8, 0, -C3).normalized * radius,
                new Vector3( C3,  C8,   0).normalized * radius,
                new Vector3( C3, -C8,   0).normalized * radius,
                new Vector3(-C3,  C8,   0).normalized * radius,
                new Vector3(-C3, -C8,   0).normalized * radius,
                new Vector3( C2,  C0,  C8).normalized * radius,
                new Vector3( C2,  C0, -C8).normalized * radius,
                new Vector3( C2, -C0,  C8).normalized * radius,
                new Vector3( C2, -C0, -C8).normalized * radius,
                new Vector3(-C2,  C0,  C8).normalized * radius,
                new Vector3(-C2,  C0, -C8).normalized * radius,
                new Vector3(-C2, -C0,  C8).normalized * radius,
                new Vector3(-C2, -C0, -C8).normalized * radius,
                new Vector3( C8,  C2,  C0).normalized * radius,
                new Vector3( C8,  C2, -C0).normalized * radius,
                new Vector3( C8, -C2,  C0).normalized * radius,
                new Vector3( C8, -C2, -C0).normalized * radius,
                new Vector3(-C8,  C2,  C0).normalized * radius,
                new Vector3(-C8,  C2, -C0).normalized * radius,
                new Vector3(-C8, -C2,  C0).normalized * radius,
                new Vector3(-C8, -C2, -C0).normalized * radius,
                new Vector3( C0,  C8,  C2).normalized * radius,
                new Vector3( C0,  C8, -C2).normalized * radius,
                new Vector3( C0, -C8,  C2).normalized * radius,
                new Vector3( C0, -C8, -C2).normalized * radius,
                new Vector3(-C0,  C8,  C2).normalized * radius,
                new Vector3(-C0,  C8, -C2).normalized * radius,
                new Vector3(-C0, -C8,  C2).normalized * radius,
                new Vector3(-C0, -C8, -C2).normalized * radius,
                new Vector3( C4,  C1,  C7).normalized * radius,
                new Vector3( C4,  C1, -C7).normalized * radius,
                new Vector3( C4, -C1,  C7).normalized * radius,
                new Vector3( C4, -C1, -C7).normalized * radius,
                new Vector3(-C4,  C1,  C7).normalized * radius,
                new Vector3(-C4,  C1, -C7).normalized * radius,
                new Vector3(-C4, -C1,  C7).normalized * radius,
                new Vector3(-C4, -C1, -C7).normalized * radius,
                new Vector3( C7,  C4,  C1).normalized * radius,
                new Vector3( C7,  C4, -C1).normalized * radius,
                new Vector3( C7, -C4,  C1).normalized * radius,
                new Vector3( C7, -C4, -C1).normalized * radius,
                new Vector3(-C7,  C4,  C1).normalized * radius,
                new Vector3(-C7,  C4, -C1).normalized * radius,
                new Vector3(-C7, -C4,  C1).normalized * radius,
                new Vector3(-C7, -C4, -C1).normalized * radius,
                new Vector3( C1,  C7,  C4).normalized * radius,
                new Vector3( C1,  C7, -C4).normalized * radius,
                new Vector3( C1, -C7,  C4).normalized * radius,
                new Vector3( C1, -C7, -C4).normalized * radius,
                new Vector3(-C1,  C7,  C4).normalized * radius,
                new Vector3(-C1,  C7, -C4).normalized * radius,
                new Vector3(-C1, -C7,  C4).normalized * radius,
                new Vector3(-C1, -C7, -C4).normalized * radius,
                new Vector3(0,  C5,  C6).normalized * radius,
                new Vector3(0,  C5, -C6).normalized * radius,
                new Vector3(0, -C5,  C6).normalized * radius,
                new Vector3(0, -C5, -C6).normalized * radius,
                new Vector3( C6, 0,  C5).normalized * radius,
                new Vector3( C6, 0, -C5).normalized * radius,
                new Vector3(-C6, 0,  C5).normalized * radius,
                new Vector3(-C6, 0, -C5).normalized * radius,
                new Vector3( C5,  C6, 0).normalized * radius,
                new Vector3( C5, -C6, 0).normalized * radius,
                new Vector3(-C5,  C6, 0).normalized * radius,
                new Vector3(-C5, -C6, 0).normalized * radius,
                new Vector3( C4,  C4,  C4).normalized * radius,
                new Vector3( C4,  C4, -C4).normalized * radius,
                new Vector3( C4, -C4,  C4).normalized * radius,
                new Vector3( C4, -C4, -C4).normalized * radius,
                new Vector3(-C4,  C4,  C4).normalized * radius,
                new Vector3(-C4,  C4, -C4).normalized * radius,
                new Vector3(-C4, -C4,  C4).normalized * radius,
                new Vector3(-C4, -C4, -C4).normalized * radius,
            };

            var faces = new int[][]
            {
                new int[] { 72, 36, 64,  4, 20, 44 },
                new int[] { 72, 44, 68,  8, 28, 52 },
                new int[] { 72, 52, 60,  0, 12, 36 },
                new int[] { 73, 37, 13,  1, 61, 53 },
                new int[] { 73, 53, 29,  8, 68, 45 },
                new int[] { 73, 45, 21,  5, 65, 37 },
                new int[] { 74, 38, 14,  2, 62, 54 },
                new int[] { 74, 54, 30,  9, 69, 46 },
                new int[] { 74, 46, 22,  4, 64, 38 },
                new int[] { 75, 39, 65,  5, 23, 47 },
                new int[] { 75, 47, 69,  9, 31, 55 },
                new int[] { 75, 55, 63,  3, 15, 39 },
                new int[] { 76, 40, 16,  0, 60, 56 },
                new int[] { 76, 56, 32, 10, 70, 48 },
                new int[] { 76, 48, 24,  6, 66, 40 },
                new int[] { 77, 41, 67,  7, 25, 49 },
                new int[] { 77, 49, 70, 10, 33, 57 },
                new int[] { 77, 57, 61,  1, 17, 41 },
                new int[] { 78, 42, 66,  6, 26, 50 },
                new int[] { 78, 50, 71, 11, 34, 58 },
                new int[] { 78, 58, 62,  2, 18, 42 },
                new int[] { 79, 43, 19,  3, 63, 59 },
                new int[] { 79, 59, 35, 11, 71, 51 },
                new int[] { 79, 51, 27,  7, 67, 43 },
                new int[] { 2, 14, 12,  0, 16, 18 },
                new int[] { 3, 19, 17,  1, 13, 15 },
                new int[] { 4, 22, 23,  5, 21, 20 },
                new int[] { 7, 27, 26,  6, 24, 25 },
                new int[] { 8, 29, 33, 10, 32, 28 },
                new int[] { 9, 30, 34, 11, 35, 31 },
                new int[] { 60, 52, 28, 32, 56 },
                new int[] { 61, 57, 33, 29, 53 },
                new int[] { 62, 58, 34, 30, 54 },
                new int[] { 63, 55, 31, 35, 59 },
                new int[] { 64, 36, 12, 14, 38 },
                new int[] { 65, 39, 15, 13, 37 },
                new int[] { 66, 42, 18, 16, 40 },
                new int[] { 67, 41, 17, 19, 43 },
                new int[] { 68, 44, 20, 21, 45 },
                new int[] { 69, 47, 23, 22, 46 },
                new int[] { 70, 49, 25, 24, 48 },
                new int[] { 71, 50, 26, 27, 51 }
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