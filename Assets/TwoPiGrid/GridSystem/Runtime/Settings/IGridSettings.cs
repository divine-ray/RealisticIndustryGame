using UnityEngine;

namespace TwoPiGrid
{
    public enum GridCellsAmount
    {
        _12 = 12,
        _20 = 20,
        _32 = 32,
        _42 = 42,
        _80 = 80,
        _162 = 162,
        _320 = 320,
        _642 = 642,
        _1280 = 1280
    }

    public interface IGridSettings
    {
        /// <summary>
        /// The amount of cells the grid will have.
        /// </summary>
        GridCellsAmount CellsAmount { get; }

        /// <summary>
        /// The grid's radius.
        /// </summary>
        float Radius { get; }

        /// <summary>
        /// The position of the grid's center.
        /// </summary>
        Vector3 Center { get; }
    }
}
