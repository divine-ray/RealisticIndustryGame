using TwoPiGrid.Configuration;
using TwoPiGrid.Extensions;

namespace TwoPiGrid.Generation.Generators
{
    internal class CustomGridSettingsGenerator : CodeGenerator
    {
        private readonly string baseNamespace;
        private readonly string gridName;
        private readonly CustomCellField[] customCellFields;
        
        internal CustomGridSettingsGenerator(
            string baseNamespace,
            string gridName,
            CustomCellField[] customCellFields
            ) : base(
            "CustomGridSettings.cs.txt",
            gridName + "Settings.cs")
        {
            this.baseNamespace = baseNamespace;
            this.gridName = gridName;
            this.customCellFields = customCellFields;
        }

        protected override void CustomizeFile(ref string originalText)
        {
            originalText = originalText.Replace("__BASE_NAMESPACE__", baseNamespace);
            originalText = originalText.Replace("__GRID_NAME__", gridName);
            originalText = originalText.Replace("__PUBLIC_PROPERTIES__", GetPublicProperties());
            originalText = originalText.Replace("__CONSTRUCTOR_SIGNATURE_CUSTOM_PARAMS__", GetConstructorSignatureCustomParams());
            originalText = originalText.Replace("__CONSTRUCTOR_BODY__", GetConstructorBody());
        }

        private string GetPublicProperties()
        {
            // public Inhabitant[] Inhabitants { get; }
            // public MagicalProperty[] MagicalProperties { get; }
            // public TerrainTypes[] TerrainTypes { get; }
            // public bool[] Buildable { get; }
            // public float[] MaxSpeeds { get; }

            var str = "";

            foreach (var customCellField in customCellFields)
                str += $"        public {GetTypeName(customCellField)}[] {customCellField.FieldName}s " +
                       "{ get; }\n";

            return str;
        }

        private string GetConstructorSignatureCustomParams()
        {
            // Inhabitant[] inhabitants,
            // MagicalProperty[] magicalProperties,
            // TerrainTypes[] terrainTypes,
            // bool[] buildable,
            // float[] maxSpeeds,

            var str = "";

            foreach (var customCellField in customCellFields)
                str += $"\n            {GetTypeName(customCellField)}[] {customCellField.FieldName.ToLowerFirst()}s,";

            return str;
        }
        
        private string GetConstructorBody()
        {
            // Inhabitants = inhabitants;
            // MagicalProperties = magicalProperties;
            // TerrainTypes = terrainTypes;
            // Buildable = buildable;
            // MaxSpeeds = maxSpeeds;

            var str = "";

            foreach (var customCellField in customCellFields)
                str += $"            {customCellField.FieldName}s = {customCellField.FieldName.ToLowerFirst()}s;\n";

            return str;
        }
    }
}
