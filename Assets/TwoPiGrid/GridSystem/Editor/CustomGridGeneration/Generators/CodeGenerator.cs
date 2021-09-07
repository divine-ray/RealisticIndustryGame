using System.IO;
using TwoPiGrid.Configuration;
using TwoPiGrid.Extensions;

namespace TwoPiGrid.Generation.Generators
{
    internal abstract class CodeGenerator
    {
        protected readonly string sourceFileName;
        public readonly string OutputFileName;

        internal CodeGenerator(string sourceFileName, string outputFileName)
        {
            this.sourceFileName = sourceFileName;
            this.OutputFileName = outputFileName;
        }

        public string GenerateFileContents()
        {
            var str = ReadFile();
            CustomizeFile(ref str);
            return str;
        }

        private string ReadFile()
        {
            var projectDirectory = Directory.GetCurrentDirectory();

            var directories = Directory.GetFiles(
                projectDirectory,
                sourceFileName,
                SearchOption.AllDirectories);

            if (directories.Length <= 0)
                throw new DirectoryNotFoundException(
                    $"Could not find {sourceFileName} in project," +
                    $" which is needed to generate {OutputFileName}.");

            return File.ReadAllText(directories[0]);
        }

        protected abstract void CustomizeFile(ref string originalText);

        protected static string GetTypeName(CustomCellField customCellField)
        {
            switch (customCellField.ValueType)
            {
                case CustomCellField.ValueTypes._customEnum:
                case CustomCellField.ValueTypes._customStruct:
                case CustomCellField.ValueTypes._customClass:
                    return customCellField.Name;
                default:
                    return customCellField.ValueType.ExtractType();
            }
        }
    }
}
