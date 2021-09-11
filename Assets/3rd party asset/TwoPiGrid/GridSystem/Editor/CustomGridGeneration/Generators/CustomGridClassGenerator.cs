using TwoPiGrid.Configuration;
using TwoPiGrid.Extensions;

namespace TwoPiGrid.Generation.Generators
{
    internal class CustomGridClassGenerator : CodeGenerator
    {
        private readonly string baseNamespace;
        private readonly string gridName;
        private readonly CustomCellField[] customCellFields;
        
        internal CustomGridClassGenerator(
            string baseNamespace,
            string gridName,
            CustomCellField[] customCellFields
            ) : base("CustomGrid.cs.txt", gridName + ".cs")
        {
            this.baseNamespace = baseNamespace;
            this.gridName = gridName;
            this.customCellFields = customCellFields;
        }

        protected override void CustomizeFile(ref string originalText)
        {
            originalText = originalText.Replace("__NAMESPACE__", baseNamespace);
            originalText = originalText.Replace("__GRID_NAME__", gridName);
            originalText = originalText.Replace("__FIELDS_AND_PROPERTIES__", GetFieldsAndProperties());
            originalText = originalText.Replace("__INITIALIZE_PARAMS_FROM_SETTINGS__", GetSettingsPropertiesSeparatedByCommasStartingWithComma());
            originalText = originalText.Replace("__FACTORY_CONSTRUCTOR_PARAMS_SIGNATURE__", GetFactoryConstructorCustomParamsSignature());
            originalText = originalText.Replace("__CONSTRUCTOR_INITIALIZE_PARAMS_SIGNATURE__", GetProtectedConstructorAndInitializeCustomParamsSignature());
            originalText = originalText.Replace("__INITIALIZE_PARAMS__", GetInitializeParameters());
            originalText = originalText.Replace("__ENSURE_INITIALIZE_PARAMS__", GetEnsureInitializeParams());
            originalText = originalText.Replace("__CLASS_COMMENT_FIELDS_LIST__", GetClassCommentFieldsList());
            originalText = originalText.Replace("__DEFAULT_PARAMETERS__", GetDefaultParameters());
        }


        private string GetFieldsAndProperties()
        {
            // /// <summary>
            // /// Get the inhabitant property of a cell.
            // /// </summary>
            // /// <param name="i">Index of the cell.</param>
            // /// <returns>The <c>Inhabitant</c> property of the cell.</returns>
            // public Inhabitant GetInhabitant(int i) => inhabitants[i];
            // private Inhabitant[] inhabitants = new Inhabitant[0];
            //
            // public {NAME} Get{FIELD_NAME}(int i) => {FIELD_NAME.ToLowerFirst()}s[i];
            // public {NAME}[] {FIELD_NAME.ToLowerFirst()}s = new {NAME}[0];

            // public bool GetBuildable(int i) => buildable[i];
            // private bool[] buildable = new bool[0];
            //
            // public {VALUE_TYPE.ToString()} Get{FIELD_NAME}(int i) => {FIELD_NAME.ToLowerFirst()}s[i];
            // private {VALUE_TYPE.ToString()}[] {FIELD_NAME.ToLowerFirst()}s = new {VALUE_TYPE.ToString()}[0];

            // public float GetMaxSpeed(int i) => maxSpeeds[i];
            // private float[] maxSpeeds = new float[0];
            //
            // public {VALUE_TYPE.ToString()} Get{FIELD_NAME}(int i) => {FIELD_NAME.ToLowerFirst()}s[i];
            // private {VALUE_TYPE.ToString()}[] {FIELD_NAME.ToLowerFirst()}s = new {VALUE_TYPE.ToString()}[0];

            var str = "";

            foreach (var customCellField in customCellFields)
            {
                var type = GetTypeName(customCellField);

                str += "\n        /// <summary>" +
                      $"\n        /// Get the {customCellField.FieldName.ToLowerFirst()} property of a cell." +
                       "\n        /// </summary>" +
                       "\n        /// <param name=\"i\">Index of the cell.</param>" +
                      $"\n        /// <returns>The <c>{customCellField.FieldName}</c> property of the cell.</returns>";
                str += "\n";
                str += $"        public {type} Get{customCellField.FieldName}(int i) => {customCellField.FieldName.ToLowerFirst()}s[i];";
                str += "\n";
                str += $"        private {type}[] {customCellField.FieldName.ToLowerFirst()}s = new {type}[0];";
                str += "\n";
            }

            return str;
        }

        private string GetSettingsPropertiesSeparatedByCommasStartingWithComma()
        {
            // , settings.Buildable, settings.MaxSpeeds, settings.TerrainTypes, settings.MagicalProperties, settings.Inhabitants
            //TODO: ensure that this is correct. We might want to add an "s" after the field name.
            var str = "";

            foreach (var customCellField in customCellFields)
                str += $", settings.{customCellField.FieldName}s";

            return str;
        }

        private string GetFactoryConstructorCustomParamsSignature()
        {
            // bool[] buildable, float[] maxSpeeds, TerrainTypes[] terrainTypes, MagicalProperty[] magicalProperties, Inhabitant[] inhabitants, 

            if (customCellFields.Length > 0)
                return GetCustomPropertiesWithTheirTypeSeparatedByCommas() + ", ";

            return "";
        }

        private string GetProtectedConstructorAndInitializeCustomParamsSignature()
        {
            // , bool[] buildable, float[] maxSpeeds, TerrainTypes[] terrainTypes, MagicalProperty[] magicalProperties, Inhabitant[] inhabitants

            if (customCellFields.Length > 0)
                return ", " + GetCustomPropertiesWithTheirTypeSeparatedByCommas();

            return "";
        }

        private string GetCustomPropertiesWithTheirTypeSeparatedByCommas()
        {
            // bool[] buildable, float[] maxSpeeds, TerrainTypes[] terrainTypes, MagicalProperty[] magicalProperties, Inhabitant[] inhabitants

            var str = "";

            if (customCellFields.Length > 0)
                str += $"{GetTypeName(customCellFields[0])}[] {customCellFields[0].FieldName.ToLowerFirst()}s";

            for (var i=1; i<customCellFields.Length; i++)
                str += $", {GetTypeName(customCellFields[i])}[] {customCellFields[i].FieldName.ToLowerFirst()}s";

            return str;
        }

        private string GetInitializeParameters()
        {
            // , buildable, maxSpeeds, terrainTypes, magicalProperties, inhabitants
            
            var str = "";

            foreach (var customCellField in customCellFields)
                str += $", {customCellField.FieldName.ToLowerFirst()}s";

            return str;
        }
        
        private string GetEnsureInitializeParams()
        {
            // if (buildable == null)
            //     throw new ArgumentNullException(nameof(buildable));
            // if (buildable.Length != CellCount)
            //     throw new ArgumentException(nameof(buildable) + " array length must match " + nameof(CellCount));
            // this.buildable = buildable;

            //string GetBlockOld(string fieldName)
            //{
            //    return $"if ({fieldName} == null)" +
            //        $"\n                throw new ArgumentNullException(nameof({fieldName}));" +
            //        $"\n            if ({fieldName}.Length != CellCount)" +
            //        $"\n                throw new ArgumentException(nameof({fieldName}) +" +
            //        $" \" array length must match \" + nameof(CellCount));" +
            //        $"\n            this.{fieldName} = {fieldName};";
            //}

            // var wasArgumentCorrect = false;
            // if (buildable == null)
            //     this.buildable = new bool[CellCount];
            // else if (buildable.Length > CellCount)
            //     this.buildable = buildable.ToList().GetRange(0, CellCount).ToArray();
            // else if (buildable.Length < CellCount)
            // {
            //     this.buildable = new bool[CellCount];
            //     for (var i=0; i<buildable.Length; i++)
            //         this.buildable[i] = buildable[i];
            // }
            // else
            // {
            //     wasArgumentCorrect = true;
            //     this.buildable = buildable;
            // }
            // if (!wasArgumentCorrect)
            //     Debug.LogWarning("Buildable parameter was not correct, so it is possible" +
            //                      " that the grid might not be properly initialized.");
            
            string GetBlock(string fieldName, string type)
            {
                return $"wasArgumentCorrect = false;" +
                    $"\n            if ({fieldName} == null)" +
                    $"\n                this.{fieldName} = new {type}[CellCount];" +
                    $"\n            else if ({fieldName}.Length > CellCount)" +
                    $"\n                this.{fieldName} = {fieldName}.ToList().GetRange(0, CellCount).ToArray();" +
                    $"\n            else if ({fieldName}.Length < CellCount)" +
                     "\n            {" +
                    $"\n                this.{fieldName} = new {type}[CellCount];" +
                    $"\n                for (var i=0; i<{fieldName}.Length; i++)" +
                    $"\n                    this.{fieldName}[i] = {fieldName}[i];" +
                     "\n            }" + 
                     "\n            else" +
                     "\n            {" +
                     "\n                wasArgumentCorrect = true;" +
                    $"\n                this.{fieldName} = {fieldName};" +
                     "\n            }" +
                     "\n            if (!wasArgumentCorrect)" +
                    $"\n                Debug.LogWarning(\"{fieldName.Capitalize()} parameter was not correct, so" +
                    " it is possible\" +\n                                 \" that the grid might not be properly" +
                    " initialized.\");";
            }

            var str = "";

            if (customCellFields.Length > 0)
                str += "            var " +
                       GetBlock(
                           customCellFields[0].FieldName.ToLowerFirst() + "s",
                           GetTypeName(customCellFields[0]));

            for (var i=1; i<customCellFields.Length; i++)
                str += "\n\n            " +
                       GetBlock(
                           customCellFields[i].FieldName.ToLowerFirst() + "s",
                           GetTypeName(customCellFields[i]));

            return str;
        }

        private string GetClassCommentFieldsList()
        {
            // , inhabitants, magicalProperties, terrainTypes, buildables, and maxSpeeds

            var str = "";

            var count = customCellFields.Length;

            for (var i=0; i<count-1; i++)
                str += $", {customCellFields[count - 1].FieldName.ToLowerFirst()}s";

            if (count > 1)
                str += ",";

            if (count > 0)
                str += $" and {customCellFields[count-1].FieldName.ToLowerFirst()}s";

            return str;
        }

        private string GetDefaultParameters()
        {
            //
            // default,
            // default,
            // default,
            //

            var str = "";

            if (customCellFields.Length > 0)
                str += "\n                    ";

            for (var i=0; i<customCellFields.Length; i++)
                str += "default,\n                    ";

            return str;
        }
    }
}
