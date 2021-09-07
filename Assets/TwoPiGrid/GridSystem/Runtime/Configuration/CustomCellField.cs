using System;
using TwoPiGrid.Extensions;
using UnityEngine;

namespace TwoPiGrid.Configuration
{
    [Serializable]
    internal class CustomCellField
    {
        internal enum ValueTypes
        {
            _bool,

            _byte,
            _uint,
            _int,
            _long,

            _float,
            _double,

            _string,

            _customEnum,
            _customClass,
            _customStruct
        }

        [Tooltip("You will access this field for each cell like this:" +
                 "\nvar someInfo = grid.Get{Field Name}(32);" +
                 "\n" +
                 "\nFor example, \"TerrainType\" would yield:" +
                 "\nvar terrainType = grid.GetTerrainType(32);" +
                 "\n" +
                 "\n(\"32\" is the cell's index.)")]
        public string FieldName;

        public ValueTypes ValueType;

        [Tooltip("The name of the enum/class/struct you want this field" +
                 " to be. You must have defined it somewhere in your code.")]
        public string Name;

        public CustomCellField(string fieldName, ValueTypes valueType, string name)
        {
            FieldName = fieldName;
            ValueType = valueType;
            Name = name;
        }

        internal void ValidateFields()
        {
            FieldName = FieldName.RemoveNonAlphanumericOrUnderscoresAndEnsureFirstIsNotNumeric();
            FieldName = FieldName.Capitalize();
            Name = Name.RemoveNonAlphanumericOrUnderscoresAndEnsureFirstIsNotNumeric();
            Name = Name.Capitalize();
        }
    }
}
