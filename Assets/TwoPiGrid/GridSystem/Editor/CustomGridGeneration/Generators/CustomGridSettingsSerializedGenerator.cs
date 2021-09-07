using TwoPiGrid.Configuration;
using TwoPiGrid.Extensions;

namespace TwoPiGrid.Generation.Generators
{
    internal class CustomGridSettingsSerializedGenerator : CodeGenerator
    {
        private readonly string baseNamespace;
        private readonly string gridName;
        private readonly CustomCellField[] customCellFields;
        
        internal CustomGridSettingsSerializedGenerator(
            string baseNamespace,
            string gridName,
            CustomCellField[] customCellFields
            ) : base(
            "CustomGridSettingsSerialized.cs.txt",
            gridName + "SettingsSerialized.cs")
        {
            this.baseNamespace = baseNamespace;
            this.gridName = gridName;
            this.customCellFields = customCellFields;
        }

        protected override void CustomizeFile(ref string originalText)
        {
            originalText = originalText.Replace("__BASE_NAMESPACE__", baseNamespace);
            originalText = originalText.Replace("__GRID_NAME__", gridName);
            originalText = originalText.Replace("__SERIALIZED_FIELDS_AND_PROPERTIES__", GetSerializedFieldsAndProperties());
            originalText = originalText.Replace("__ENSURE_ARRAY_LENGTHS__", GetEnsureArrayLengths());
            originalText = originalText.Replace("__REDUCE_ARRAYS_TO_CELL_COUNT__", GetReduceArraysToCellCount());
        }

        private string GetSerializedFieldsAndProperties()
        {
            // [SerializeField] private List<Inhabitant> inhabitants = new List<Inhabitant>();
            // [SerializeField] private List<MagicalProperty> magicalProperties = new List<MagicalProperty>();
            //
            // public Inhabitant[] Inhabitants => inhabitants.GetRange(0, (int) CellsAmount).ToArray();
            // public MagicalProperty[] MagicalProperties => magicalProperties.GetRange(0, (int) CellsAmount).ToArray();

            var str = "";

            foreach (var customCellField in customCellFields)
            {
                var type = GetTypeName(customCellField);
                str += $"        [SerializeField] private List<{type}> {customCellField.FieldName.ToLowerFirst()}s" +
                       $" = new List<{type}>();\n";
            }

            foreach (var customCellField in customCellFields)
                str += $"\n        public {GetTypeName(customCellField)}[] {customCellField.FieldName}s =>" +
                       $" {customCellField.FieldName.ToLowerFirst()}s.GetRange(0, (int) CellsAmount).ToArray();";

            return str;
        }

        private string GetEnsureArrayLengths()
        {
            // for (var i=inhabitants.Count; i<cellCount; i++)
            //     inhabitants.Add(default);

            var str = "";

            foreach (var customCellField in customCellFields)
            {
                var fieldName = customCellField.FieldName.ToLowerFirst() + "s";
                str += $"\n            for (var i={fieldName}.Count; i<cellCount; i++)" +
                       $"\n                {fieldName}.Add(default);";
            }

            return str;
        }

        private string GetReduceArraysToCellCount()
        {
            // inhabitants = inhabitants.GetRange(0, cellCount);

            var str = "";

            foreach (var customCellField in customCellFields)
            {
                var fieldName = customCellField.FieldName.ToLowerFirst() + "s";
                str += $"\n            {fieldName} = {fieldName}.GetRange(0, cellCount);";
            }

            return str;
        }
    }
}
