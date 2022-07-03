using System.IO;
using System.Linq;
using UnityEngine;

namespace TwoPiGrid
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "Two Pi Grid/Grid Settings", order = 1)]
    public class GridSettingsSerialized : ScriptableObject, IGridSettings
    {
        [Tooltip("The amount of cells the grid will have.")]
        [SerializeField] private GridCellsAmount cellsAmount = GridCellsAmount._12;

        [Tooltip("The grid's radius. It can be changed dynamically.")]
        [SerializeField] private float radius = 1f;

        [Tooltip("The position of the grid's center. It can be changed dynamically.")]
        [SerializeField] private Vector3 center = Vector3.zero;

        public GridCellsAmount CellsAmount => cellsAmount;

        public float Radius => radius;
        public Vector3 Center => center;

        public delegate void Refresh();
        public Refresh refreshDelegate;

        protected virtual void OnValidate()
        {
            refreshDelegate?.Invoke();
        }

        /// <summary>
        /// Tries to load a [GridSettingsSerialized] object from a Resources folder.
        /// Throws [FileNotFoundException] if asset not found.
        /// </summary>
        /// <param name="fileName">The name of the asset.</param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException">If asset not found.</exception>
        internal static GridSettingsSerialized Load(string fileName)
        {
            var resources = Resources.FindObjectsOfTypeAll<GridSettingsSerialized>();

            var settings = resources.FirstOrDefault(resource => resource.name == fileName);

            if (settings == null)
            {
                resources = Resources.LoadAll<GridSettingsSerialized>("GridSettings"); //TODO: Should this be fileName?

                settings = resources.FirstOrDefault(resource => resource.name == fileName);
            }

            if (settings == null)
                settings = Resources.Load<GridSettingsSerialized>(fileName);

            if (settings == null)
                throw new FileNotFoundException($"Could not find the settings asset named \"{fileName}\".", fileName);

            return settings;
        }
    }
}
