using TwoPiGrid.Configuration;
using TwoPiGrid.Extensions;
namespace TwoPiGrid.Generation.Generators
{
    internal class CustomGridSettingsInspectorGenerator : CodeGenerator
    {
        private readonly string baseNamespace;
        private readonly string gridName;
        private readonly CustomCellField[] customCellFields;

        internal CustomGridSettingsInspectorGenerator(
            string baseNamespace,
            string gridName,
            CustomCellField[] customCellFields
        ) : base(
            "CustomGridSettingsInspector.cs.txt",
            gridName + "SettingsInspector.cs")
        {
            this.baseNamespace = baseNamespace;
            this.gridName = gridName;
            this.customCellFields = customCellFields;
        }

        protected override void CustomizeFile(ref string originalText)
        {
            LoadInfoForTheList();

            originalText = originalText.Replace("__BASE_NAMESPACE__", baseNamespace);
            originalText = originalText.Replace("__GRID_NAME__", gridName);
            originalText = originalText.Replace("__INFO_FOR_THE_LIST__", GetInfoForTheList());
            originalText = originalText.Replace("__LOAD_CHILD_NAMES__", GetLoadChildNames());
        }

        private string unitsCount;
        private string headers;
        private string propertyNames;
        private string shouldDrawLabel;

        private void LoadInfoForTheList()
        {
            unitsCount = "";
            headers = "{";
            propertyNames = "{";
            shouldDrawLabel = "{";

            switch (customCellFields[0].ValueType)
            {
                case CustomCellField.ValueTypes._customClass:
                case CustomCellField.ValueTypes._customStruct:
                    unitsCount += "2";
                    shouldDrawLabel += "true";
                    break;

                default:
                    unitsCount += "1";
                    shouldDrawLabel += "false";
                    break;
            }
            headers += "\"" + customCellFields[0].FieldName.Capitalize() + "\"";
            propertyNames += $"\"{customCellFields[0].FieldName.ToLowerFirst()}s\"";

            for (var i=1; i<customCellFields.Length; i++)
            {
                var customCellField = customCellFields[i];
                switch (customCellField.ValueType)
                {
                    case CustomCellField.ValueTypes._customClass:
                    case CustomCellField.ValueTypes._customStruct:
                        unitsCount += " + 2";
                        shouldDrawLabel += ", true";
                        break;

                    default:
                        unitsCount += " + 1";
                        shouldDrawLabel += ", false";
                        break;
                }
                headers += ", \"" + customCellFields[i].FieldName.Capitalize() + "\"";
                propertyNames += $", \"{customCellFields[i].FieldName.ToLowerFirst()}s\"";
            }

            headers += "}";
            propertyNames += "}";
            shouldDrawLabel += "}";
        }

        private string GetInfoForTheList()
        {
            var customCellFieldsCount = customCellFields.Length;

            var str = $"        private readonly int columnCount = 1 + {customCellFieldsCount};" +
                      $"\n        private readonly int unitsCount = 1 + {unitsCount}; //for performance" +
                      $"\n        private readonly int propertyCount = {customCellFieldsCount}; //for performance" +
                      $"\n" +
                      $"\n        private readonly string[] headers = {headers};" +
                      $"\n        private readonly string[] propertyNames = {propertyNames};" +
                      $"\n        private string[][] propertyChildNames;" +
                      $"\n        private readonly bool[] shouldDrawLabel = {shouldDrawLabel};";

            return str;
        }

        private string GetLoadChildNames()
        {
            var str = "";

            for (var i=0; i<customCellFields.Length; i++)
            {
                var customCellField = customCellFields[i];
                switch (customCellField.ValueType)
                {
                    case CustomCellField.ValueTypes._customClass:
                    case CustomCellField.ValueTypes._customStruct:
                        str += $"\n\n            var privateFields{i} = typeof({GetTypeName(customCellField)}).GetFields(" +
                               "BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);" +
                               $"\n            propertyChildNames[{i}] = privateFields{i}.Select(field => field.Name).ToArray();";
                        break;

                    default:
                        str += $"\n\n            propertyChildNames[{i}] = new string[0];";
                        break;
                }
            }

            return str;
        }
     }
 }
 