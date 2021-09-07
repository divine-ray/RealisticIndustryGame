/**
 * Base Polyhedron class.
 *
 * Created by Jazz Emma Prats Camps in December 2019.
 * Updated in October 2020.
 */

using UnityEngine;

namespace TwoPiGrid.Geometry
{
    public abstract class Polyhedron
    {
        public abstract float Radius { get; protected set; }
        public abstract Vector3[] Vertices { get; protected set; }
        public abstract int[][] Faces { get; protected set; }
        public abstract Vector3[] Centers { get; protected set; }
    }
}
