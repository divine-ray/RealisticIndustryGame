using TwoPiGrid.Extensions;
using UnityEngine;

namespace TwoPiGrid.Configuration
{
    [CreateAssetMenu(fileName = "GridConfiguration", menuName = "Two Pi Grid/Grid Configuration", order = 1)]
    internal class GridConfiguration : ScriptableObject
    {
        [Tooltip("The namespace for the grid class. For example, \"MyStudio.MyGame\" would yield:" +
                 "\n" +
                 "\nnamespace MyStudio.MyGame" +
                 "\n{" +
                 "\n\tpublic class MyGrid" +
                 "\n\t{" +
                 "\n\t\t/* ... */" +
                 "\n\t}" +
                 "\n}")]
        [SerializeField] internal string namespaceName;
        [Tooltip("The prefix for the grid class. For example, \"My\" would yield \"MyGrid\":" +
                 "\n" +
                 "\nvar grid = new MyGrid(settings);")]
        [SerializeField] internal string gridPrefix;
        [SerializeField] internal CustomCellField[] customCellFields = new CustomCellField[0];

        private void OnValidate()
        {
            namespaceName = namespaceName.RemoveNonAlphanumericOrUnderscoresAndEnsureFirstIsNotNumeric();
            namespaceName = namespaceName.Capitalize();
            gridPrefix = gridPrefix.RemoveNonAlphanumericOrUnderscoresAndEnsureFirstIsNotNumeric();
            gridPrefix = gridPrefix.Capitalize();
            foreach (var customCellField in customCellFields)
                customCellField.ValidateFields();
        }
    }
}
