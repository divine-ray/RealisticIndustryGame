/**
 * Generates a dual geodesic icosahedron with the given
 * radius and type.
 * 
 * Created by Jazz Emma Prats Camps in November 2019.
 * Updated in October 2020.
 * Math from: http://dmccooey.com/polyhedra/DualGeodesicIcosahedra.html
 */

using TwoPiGrid.Geometry.Internal;
using UnityEngine;

namespace TwoPiGrid.Geometry
{
    public class DualGeodesicIcosahedron : Polyhedron
    {
        public sealed override float Radius { get; protected set; }

        public sealed override Vector3[] Vertices { get; protected set; }
        public sealed override int[][] Faces { get; protected set; }
    
        public sealed override Vector3[] Centers { get; protected set; }

        public static DualGeodesicIcosahedron Generate(float radius, int order)
        {
            var secondInChain = new ChamferedDodecahedron(null);
            var firstInChain = new TruncatedIcosahedron(secondInChain);

            var dualGeodesicIcosahedron = firstInChain.Generate(radius, order);
            if (dualGeodesicIcosahedron == null)
            {
                LogNotImplementedWarningForOrder(order);
                return firstInChain.GeneratePlease(radius);
            }
            else
                return dualGeodesicIcosahedron;
        }

        //TODO: Turn this into a factory method
        internal DualGeodesicIcosahedron(float radius, Vector3[] vertices, int[][] faces, Vector3[] centers)
        {
            Radius = radius;
            Vertices = vertices;
            Faces = faces;
            Centers = centers;
        }

        private static void LogNotImplementedWarningForOrder(int order)
        {
            string text;
            switch (order)
            {
                case 1:
                    text = "Chamfered Dodecahedron (order 1) not implemented.";
                    break;
                case 2:
                    text = "Hexpropello Dodecahedron (order 2) not implemented.";
                    break;
                case 3:
                    text = "Pentakis Dodecahedron (order 3) not implemented.";
                    break;
                default:
                    text = "Dual Geodesic Icosahedron of order " + order + " not implemented.";
                    break;
            }
            text += " Generating Truncated Icosahedron (order 0) instead.";
            Debug.LogWarning(text);
        }
    }
}
