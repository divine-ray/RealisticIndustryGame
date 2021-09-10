using UnityEngine;

namespace TwoPiGrid
{
    public class GridSettings : IGridSettings
    {
        /// <summary>
        /// The amount of cells the grid will have.
        /// </summary>
        public GridCellsAmount CellsAmount { get; }

        /// <summary>
        /// The cell's radius. It can be changed dynamically.
        /// </summary>
        public float Radius { get; }

        /// <summary>
        /// The cell's center. It can be changed dynamically.
        /// </summary>
        public Vector3 Center { get; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="cellsAmount">The amount of cells the grid will have.</param>
        /// <param name="radius">The cell's radius. It can be changed dynamically.</param>
        /// <param name="center">The cell's center. It can be changed dynamically.</param>
        public GridSettings(
            GridCellsAmount cellsAmount,
            float radius = 1f,
            Vector3 center = default)
        {
            CellsAmount = cellsAmount;
            Radius = radius;
            Center = center;
        }
    }
}
