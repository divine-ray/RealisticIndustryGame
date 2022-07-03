using System;
using System.Collections.Generic;
using System.IO;
using TwoPiGrid.Geometry;
using TwoPiGrid.Shape;
using UnityEngine;

namespace TwoPiGrid
{
    /// <summary>
    /// The BaseGrid class.
    /// Has the positions of its cells.
    /// </summary>
    /// <remarks>
    /// This class can tell you the index of the closest cell to a given position.
    /// </remarks>
    [Serializable] //TODO: should it be serializable? CustomGrid is not...
    public partial class BaseGrid
    {
        #region Fields and properties

        /// <summary>
        /// The grid's center in world coordinates. Can be changed dynamically.
        /// </summary>
        public Vector3 Center;

        /// <summary>
        /// The grid's radius. Can be changed dynamically.
        /// </summary>
        public float Radius
        {
            get => mRadius;
            set
            {
                mRadius = value;
                RecalculateVertexPositions();
            }
        }
        private float mRadius;

        /// <summary>
        /// The amount of cells the grid has.
        /// </summary>
        public int CellCount => positions.Length;

        /// <summary>
        /// Query the position of a cell (in world coordinates).
        /// </summary>
        /// <param name="i">Index of the cell.</param>
        /// <returns>The position of the center of the cell.</returns>
        public Vector3 GetCellPosition(int i) => Center + positions[i];
        private Vector3[] positions = new Vector3[0];

        #endregion

        #region Public constructors

        /// <summary>
        /// Construct the grid by passing it its settings object file name.
        /// If the settings can't be found, a default grid will be created
        /// instead (12 cells, radius of 1, center at (0, 0, 0)).
        /// </summary>
        /// <param name="settingsFileName">Name of a <c>GridSettingsSerialized</c>
        /// asset that should be in a Resources folder.</param>
        public BaseGrid(string settingsFileName)
        {
            IGridSettings settings = default;
            try
            {
                settings = GridSettingsSerialized.Load(settingsFileName);
            }
            catch (FileNotFoundException)
            {
                Debug.LogError(
                    "Could not find the settings file. Generating a default" +
                    "grid instead (12 cells, radius of 1, center at (0, 0, 0)).");
                settings = new GridSettings(GridCellsAmount._12);
            }

            var meshSettings = GetMeshSettings(settings.CellsAmount, settings.Radius);

            Initialize(meshSettings, settings.Center);
        }

        /// <summary>
        /// Construct the grid by passing it an <c>IGridSettings</c> object.
        /// </summary>
        /// <param name="settings">Settings object.</param>
        public BaseGrid(IGridSettings settings)
        {
            var meshSettings = GetMeshSettings(settings.CellsAmount, settings.Radius);

            Initialize(meshSettings, settings.Center);
        }

        #endregion

        #region Public factory constructors

        /// <summary>
        /// Creates a grid with 12 cells.
        /// </summary>
        /// <param name="radius">The grid's radius. Can be changed dynamically.</param>
        /// <param name="center">The position of the grid's center (in world coordinates).
        /// Can be changed dynamically.</param>
        /// <returns>A grid with 12 cells</returns>
        public static BaseGrid CreateWith12Cells(float radius = 1, Vector3 center = default)
            => new BaseGrid(SettingsFor12Cells(radius), center);

        /// <summary>
        /// Creates a grid with 20 cells.
        /// </summary>
        /// <param name="radius">The grid's radius. Can be changed dynamically.</param>
        /// <param name="center">The position of the grid's center (in world coordinates).
        /// Can be changed dynamically.</param>
        /// <returns>A grid with 20 cells</returns>
        public static BaseGrid CreateWith20Cells(float radius = 1, Vector3 center = default)
            => new BaseGrid(SettingsFor20Cells(radius), center);

        /// <summary>
        /// Creates a grid with 32 cells.
        /// </summary>
        /// <param name="radius">The grid's radius. Can be changed dynamically.</param>
        /// <param name="center">The position of the grid's center (in world coordinates).
        /// Can be changed dynamically.</param>
        /// <returns>A grid with 32 cells</returns>
        public static BaseGrid CreateWith32Cells(float radius = 1, Vector3 center = default)
            => new BaseGrid(SettingsFor32Cells(radius), center);

        /// <summary>
        /// Creates a grid with 42 cells.
        /// </summary>
        /// <param name="radius">The grid's radius. Can be changed dynamically.</param>
        /// <param name="center">The position of the grid's center (in world coordinates).
        /// Can be changed dynamically.</param>
        /// <returns>A grid with 42 cells</returns>
        public static BaseGrid CreateWith42Cells(float radius = 1, Vector3 center = default)
            => new BaseGrid(SettingsFor42Cells(radius), center);

        /// <summary>
        /// Creates a grid with 80 cells.
        /// </summary>
        /// <param name="radius">The grid's radius. Can be changed dynamically.</param>
        /// <param name="center">The position of the grid's center (in world coordinates).
        /// Can be changed dynamically.</param>
        /// <returns>A grid with 80 cells</returns>
        public static BaseGrid CreateWith80Cells(float radius = 1, Vector3 center = default)
            => new BaseGrid(SettingsFor80Cells(radius), center);

        /// <summary>
        /// Creates a grid with 162 cells.
        /// </summary>
        /// <param name="radius">The grid's radius. Can be changed dynamically.</param>
        /// <param name="center">The position of the grid's center (in world coordinates).
        /// Can be changed dynamically.</param>
        /// <returns>A grid with 162 cells</returns>
        public static BaseGrid CreateWith162Cells(float radius = 1, Vector3 center = default)
            => new BaseGrid(SettingsFor162Cells(radius), center);

        /// <summary>
        /// Creates a grid with 320 cells.
        /// </summary>
        /// <param name="radius">The grid's radius. Can be changed dynamically.</param>
        /// <param name="center">The position of the grid's center (in world coordinates).
        /// Can be changed dynamically.</param>
        /// <returns>A grid with 320 cells</returns>
        public static BaseGrid CreateWith320Cells(float radius = 1, Vector3 center = default)
            => new BaseGrid(SettingsFor320Cells(radius), center);

        /// <summary>
        /// Creates a grid with 642 cells.
        /// </summary>
        /// <param name="radius">The grid's radius. Can be changed dynamically.</param>
        /// <param name="center">The position of the grid's center (in world coordinates).
        /// Can be changed dynamically.</param>
        /// <returns>A grid with 642 cells</returns>
        public static BaseGrid CreateWith642Cells(float radius = 1, Vector3 center = default)
            => new BaseGrid(SettingsFor642Cells(radius), center);

        /// <summary>
        /// Creates a grid with 1280 cells.
        /// </summary>
        /// <param name="radius">The grid's radius. Can be changed dynamically.</param>
        /// <param name="center">The position of the grid's center (in world coordinates).
        /// Can be changed dynamically.</param>
        /// <returns>A grid with 1280 cells</returns>
        public static BaseGrid CreateWith1280Cells(float radius = 1, Vector3 center = default)
            => new BaseGrid(SettingsFor1280Cells(radius), center);

        #endregion

        #region Protected constructors and initializers

        /// <summary>
        /// The point of this constructor is to allow for subclassing and handling
        /// the settings and initialization in the child class. But you MUST call
        /// base.Initialize() from within your constructor.
        /// </summary>
        protected BaseGrid() { }

        protected BaseGrid(GridShapeSettings settings, Vector3 center)
        {
            Initialize(settings, center);
        }

        protected void Initialize(GridShapeSettings settings, Vector3 center)
        {
            Center = center;

            switch (settings.SphereType)
            {
                case SphereTypes.Icosahedron:
                    var icosahedron = new Icosahedron(settings.Radius, settings.Order);
                    positions = settings.CellsAreVerticesUnlessTruncated ? icosahedron.Vertices : icosahedron.Centers;
                    break;

                case SphereTypes.DualGeodesicIcosahedron:
                    var dualGeodesicIcosahedron = DualGeodesicIcosahedron.Generate(settings.Radius, settings.Order);
                    positions = dualGeodesicIcosahedron.Centers;
                    break;

                //case SphereTypes.CubeSphere:
                //    throw new NotImplementedException();

                default:
                    var defaultIcosahedron = new Icosahedron(settings.Radius, settings.Order);
                    positions = settings.CellsAreVerticesUnlessTruncated ? defaultIcosahedron.Vertices : defaultIcosahedron.Centers;
                    break;
            }

            mRadius = settings.Radius;
            RecalculateVertexPositions();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the index of the grid's cell that is closest to the given
        /// position in world coordinates.
        /// </summary>
        /// <param name="point">Position in world coordinates to query.</param>
        /// <returns>The index of the closest cell to the <paramref name="point"/>.</returns>
        public int GetIndexOfClosestVertexTo(Vector3 point)
        {
            int indexOfClosestVertex = 0;
            float dotProductOfClosestVertex = Vector3.Dot(point, positions[0]);
            for (int i = 1; i < positions.Length; i++)
            {
                var dotProduct = Vector3.Dot(point, positions[i]);
                if (dotProduct > dotProductOfClosestVertex)
                {
                    dotProductOfClosestVertex = dotProduct;
                    indexOfClosestVertex = i;
                }
            }
            return indexOfClosestVertex;
        }

        /// <summary>
        /// Get the mesh settings for a grid with the given parameters.
        /// </summary>
        /// <param name="cellsAmount">The amount of cells the grid has.</param>
        /// <param name="radius">The grid's radius.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static GridShapeSettings GetMeshSettings(GridCellsAmount cellsAmount, float radius)
        {
            switch (cellsAmount)
            {
                case GridCellsAmount._12:
                    return SettingsFor12Cells(radius);
                case GridCellsAmount._20:
                    return SettingsFor20Cells(radius);
                case GridCellsAmount._32:
                    return SettingsFor32Cells(radius);
                case GridCellsAmount._42:
                    return SettingsFor42Cells(radius);
                case GridCellsAmount._80:
                    return SettingsFor80Cells(radius);
                case GridCellsAmount._162:
                    return SettingsFor162Cells(radius);
                case GridCellsAmount._320:
                    return SettingsFor320Cells(radius);
                case GridCellsAmount._642:
                    return SettingsFor642Cells(radius);
                case GridCellsAmount._1280:
                    return SettingsFor1280Cells(radius);
                default:
                    Debug.LogError($"Settings for this {nameof(cellsAmount)} is not implemented." +
                                   " Returning settings for a grid of 12 cells instead.");
                    return SettingsFor12Cells(radius);
            }
        }

        #endregion
        
        #region Protected and Internal methods

        protected void SetCellTypes<T>(out T[] cellTypesArray, T[] value)
        {
            if (value == null)
            {
                cellTypesArray = new T[positions.Length];
            }
            else if (value.Length == positions.Length)
            {
                cellTypesArray = value;
            }
            else if (value.Length < positions.Length)
            {
                cellTypesArray = new T[positions.Length];
                for (var i=0; i<value.Length; i++)
                    cellTypesArray[i] = value[i];
            }
            else
            {
                //Get a maximum of positions.length elements from the cellTypes array.
                cellTypesArray = new List<T>(value).GetRange(0, positions.Length).ToArray();
            }

            //TODO: This does not ensure all elements be initialized.
        }

        #endregion

        #region Private methods

        private void RecalculateVertexPositions()
        {
            for (var i = 0; i < positions.Length; i++)
                positions[i] = positions[i].normalized * Radius;
        }

        #endregion
    }
}
