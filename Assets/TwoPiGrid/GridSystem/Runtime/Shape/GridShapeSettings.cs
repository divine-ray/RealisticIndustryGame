/**
 * Settings for the grid mesh.
 * 
 * Created by Jazz Emma Prats Camps in December 2019.
 * Updated in October 2020.
 */

namespace TwoPiGrid.Shape
{
    public enum SphereTypes
    {
        Icosahedron,
        DualGeodesicIcosahedron,
        //CubeSphere
    }

    public struct GridShapeSettings
    {
        public readonly float Radius;
        public readonly SphereTypes SphereType;
        public readonly int Order;
        public readonly int GridWidthPerSide;
        public readonly bool CellsAreVerticesUnlessTruncated;

        public GridShapeSettings(float radius, SphereTypes sphereType, int order = 0, int gridWidthPerSide = 10, bool cellsAreVerticesUnlessTruncated = false)
        {
            Radius = radius;
            SphereType = sphereType;
            Order = order;
            GridWidthPerSide = gridWidthPerSide;
            CellsAreVerticesUnlessTruncated = cellsAreVerticesUnlessTruncated;
        }

        public static GridShapeSettings ForIcosahedron(float radius, int order, bool cellsAreVerticesUnlessTruncated)
        {
            return new GridShapeSettings(radius, SphereTypes.Icosahedron, order,
                cellsAreVerticesUnlessTruncated: cellsAreVerticesUnlessTruncated);
        }

        public static GridShapeSettings ForDualGeodesicIcosahedron(float radius, int order)
        {
            return new GridShapeSettings(radius, SphereTypes.DualGeodesicIcosahedron, order);
        }

        //public static GridShapeSettings ForCubeSphere(float radius, int gridWidthPerSide, bool cellsAreVerticesUnlessTruncated)
        //{
        //    return new GridShapeSettings(radius, SphereTypes.CubeSphere,
        //        gridWidthPerSide: gridWidthPerSide,
        //        cellsAreVerticesUnlessTruncated: cellsAreVerticesUnlessTruncated);
        //}
    }
}
