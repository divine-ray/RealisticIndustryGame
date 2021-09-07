using TwoPiGrid.Configuration;
using TwoPiGrid.Extensions;

namespace TwoPiGrid.Generation.Generators
{
    internal class ICustomGridSettingsGenerator : CodeGenerator
    {
        private readonly string baseNamespace;
        private readonly string gridName;
        private readonly CustomCellField[] customCellFields;
        
        internal ICustomGridSettingsGenerator(
            string baseNamespace,
            string gridName,
            CustomCellField[] customCellFields
            ) : base(
            "ICustomGridSettings.cs.txt",
            "I" + gridName + "Settings.cs")
        {
            this.baseNamespace = baseNamespace;
            this.gridName = gridName;
            this.customCellFields = customCellFields;
        }

        protected override void CustomizeFile(ref string originalText)
        {
            originalText = originalText.Replace("__BASE_NAMESPACE__", baseNamespace);
            originalText = originalText.Replace("__GRID_NAME__", gridName);
            originalText = originalText.Replace("__PROPERTIES__", GetProperties());
        }

        private string GetProperties()
        {
            // Inhabitant[] Inhabitants { get; }
            // MagicalProperty[] MagicalProperties { get; }
            // TerrainTypes[] TerrainTypes { get; }
            // bool[] Buildable { get; }
            // float[] MaxSpeeds { get; }

            var str = "";

            foreach (var customCellField in customCellFields)
                str += $"        {GetTypeName(customCellField)}[] {customCellField.FieldName}s " +
                       "{ get; }\n";

            return str;
        }
    }
}
