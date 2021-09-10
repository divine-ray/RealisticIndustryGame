using TwoPiGrid.Shape;

namespace TwoPiGrid
{
    public partial class BaseGrid
    {
        protected static GridShapeSettings SettingsFor12Cells(float radius)
        {
            return GridShapeSettings.ForIcosahedron(
                radius: radius,
                order: 0,
                cellsAreVerticesUnlessTruncated: true);
        }

        protected static GridShapeSettings SettingsFor20Cells(float radius)
        {
            return GridShapeSettings.ForIcosahedron(
                radius: radius,
                order: 0,
                cellsAreVerticesUnlessTruncated: false);
        }

        protected static GridShapeSettings SettingsFor32Cells(float radius)
        {
            return GridShapeSettings.ForDualGeodesicIcosahedron(
                radius: radius,
                order: 0);
        }

        protected static GridShapeSettings SettingsFor42Cells(float radius)
        {
            //Either vertices of Icosahedron subdivided once, or faces of Chamfered Dodecahedron.
            return GridShapeSettings.ForDualGeodesicIcosahedron(
                radius: radius,
                order: 1);
        }

        protected static GridShapeSettings SettingsFor80Cells(float radius)
        {
            //Either faces of Icosahedron subdivided once, or vertices of Chamfered Dodecahedron.
            return GridShapeSettings.ForIcosahedron(
                radius: radius,
                order: 1,
                cellsAreVerticesUnlessTruncated: false);
        }

        protected static GridShapeSettings SettingsFor162Cells(float radius)
        {
            return GridShapeSettings.ForIcosahedron(
                radius: radius,
                order: 2,
                cellsAreVerticesUnlessTruncated: true);
        }

        protected static GridShapeSettings SettingsFor320Cells(float radius)
        {
            return GridShapeSettings.ForIcosahedron(
                radius: radius,
                order: 2,
                cellsAreVerticesUnlessTruncated: false);
        }

        protected static GridShapeSettings SettingsFor642Cells(float radius)
        {
            return GridShapeSettings.ForIcosahedron(
                radius: radius,
                order: 3,
                cellsAreVerticesUnlessTruncated: true);
        }

        protected static GridShapeSettings SettingsFor1280Cells(float radius)
        {
            return GridShapeSettings.ForIcosahedron(
                radius: radius,
                order: 3,
                cellsAreVerticesUnlessTruncated: false);
        }
    }
}