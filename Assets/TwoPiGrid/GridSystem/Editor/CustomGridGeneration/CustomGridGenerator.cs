using System;
using System.Collections.Generic;
using System.IO;
using TwoPiGrid.Configuration;
using TwoPiGrid.Generation.Generators;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TwoPiGrid.Generation
{
    internal class CustomGridGenerator
    {
        private readonly string gridPrefix;
        private readonly CustomCellField[] customCellFields;

        private readonly string baseNamespace;
        private readonly string gridName;

        #region Directory paths

        private string outputsDirectory;
        private string CustomGridDirectory => outputsDirectory + @"/" + gridName;
        private string GridSystemEditorDirectory => CustomGridDirectory + @"/GridSystem/Editor";
        private string GridSystemRuntimeDirectory => CustomGridDirectory + @"/GridSystem/Runtime";
        private string GridSystemSettingsDirectory => GridSystemRuntimeDirectory + @"/Settings";
        private string GridSystemSettingsResourcesDirectory => GridSystemSettingsDirectory + @"/Resources";
        private string GridVisualizationRuntimeDirectory => CustomGridDirectory + @"/GridVisualization/Runtime";
        private string GridVisualizationEditorDirectory => CustomGridDirectory + @"/GridVisualization/Editor";

        #endregion

        internal CustomGridGenerator(string namespaceName, string gridPrefix, CustomCellField[] customCellFields)
        {
            this.gridPrefix = gridPrefix;
            this.customCellFields = customCellFields;

            gridName = gridPrefix + "Grid";
            baseNamespace = namespaceName;
        }

        internal void GenerateCustomGrid()
        {
            if (string.IsNullOrEmpty(baseNamespace))
            {
                Debug.LogError("Could not generate custom grid: You must provide a namespace" +
                               " in the configuration asset.");
                return;
            }

            if (string.IsNullOrEmpty(gridName))
            {
                Debug.LogError("Could not generate custom grid: You must provide a custom" +
                               " grid name in the configuration asset.");
                return;
            }

            if (customCellFields == null || customCellFields.Length == 0)
            {
                Debug.LogError("Could not generate custom grid: You must specify at least" +
                               " one custom cell attribute. If you don't need any custom cell" +
                               " attributes, use the " + nameof(BaseGrid) + " instead.");
                return;
            }

            //Debug.Log("============================\nGenerateCustomGrid() START");

            try
            {
                outputsDirectory = GetDirectory();
            }
            catch (DirectoryNotFoundException e)
            {
                Debug.LogError(e);
                return;
            }

            var shouldGenerate = true;

            if (Directory.Exists(CustomGridDirectory))
                shouldGenerate = EditorUtility.DisplayDialog(
                    "Generate custom grid?",
                    $"There is already a {gridName} in {outputsDirectory}." +
                    " This will overwrite it. Are you sure?",
                    $"Generate {gridName}",
                    "Cancel");

            if (!shouldGenerate)
                return;

            try
            {
                var customGridClassGenerator = new CustomGridClassGenerator(
                    baseNamespace, gridName, customCellFields);
                var generatedText = customGridClassGenerator.GenerateFileContents();
                var outputPath = GridSystemRuntimeDirectory + @"/" + customGridClassGenerator.OutputFileName;
                output.Add((GridSystemRuntimeDirectory, outputPath, generatedText));
                //Debug.Log("Generated " + outputPath);

                var gridSystemAssemblyInfoGenerator = new GSAssemblyInfoGenerator(baseNamespace);
                generatedText = gridSystemAssemblyInfoGenerator.GenerateFileContents();
                outputPath = GridSystemRuntimeDirectory + @"/" + gridSystemAssemblyInfoGenerator.OutputFileName;
                output.Add((GridSystemRuntimeDirectory, outputPath, generatedText));

                var iCustomGridSettingsGenerator = new ICustomGridSettingsGenerator(
                    baseNamespace, gridName, customCellFields);
                generatedText = iCustomGridSettingsGenerator.GenerateFileContents();
                outputPath = GridSystemSettingsDirectory + @"/" + iCustomGridSettingsGenerator.OutputFileName;
                output.Add((GridSystemSettingsDirectory, outputPath, generatedText));

                var customGridSettingsSerializedGenerator = new CustomGridSettingsSerializedGenerator(
                    baseNamespace, gridName, customCellFields);
                generatedText = customGridSettingsSerializedGenerator.GenerateFileContents();
                outputPath = GridSystemSettingsDirectory + @"/" + customGridSettingsSerializedGenerator.OutputFileName;
                output.Add((GridSystemSettingsDirectory, outputPath, generatedText));

                var customGridSettingsGenerator = new CustomGridSettingsGenerator(
                    baseNamespace, gridName, customCellFields);
                generatedText = customGridSettingsGenerator.GenerateFileContents();
                outputPath = GridSystemSettingsDirectory + @"/" + customGridSettingsGenerator.OutputFileName;
                output.Add((GridSystemSettingsDirectory, outputPath, generatedText));

                var customGridSettingsInspectorGenerator = new CustomGridSettingsInspectorGenerator(
                    baseNamespace, gridName, customCellFields);
                generatedText = customGridSettingsInspectorGenerator.GenerateFileContents();
                outputPath = GridSystemEditorDirectory + @"/" + customGridSettingsInspectorGenerator.OutputFileName;
                output.Add((GridSystemEditorDirectory, outputPath, generatedText));

                WriteAllText();
                Debug.Log("Generated custom grid at: " + CustomGridDirectory);
            }
            catch (DirectoryNotFoundException e)
            {
                Debug.LogError("Could not generate custom grid: " + e.Message);
                return;
            }

            WriteAllText();

            //Debug.Log("GenerateCustomGrid() END\n============================");
        }

        private static string GetDirectory()
        {
            var projectDirectory = Directory.GetCurrentDirectory();
            Debug.Log("Project directory: " + projectDirectory);

            var directories = Directory.GetDirectories(
                projectDirectory,
                "TwoPiGrid Outputs",
                SearchOption.AllDirectories);

            if (directories.Length <= 0)
                throw new DirectoryNotFoundException(
                    "In order to generate your custom grid, there must be" +
                    " a folder named \"TwoPiGrid Outputs\" in the project. Please" +
                    " create it wherever you like.");

            return directories[0];
        }

        private static string GetPathToDirectory(string directory)
        {
            var projectDirectory = Directory.GetCurrentDirectory();

            var directories = Directory.GetDirectories(
                projectDirectory,
                directory,
                SearchOption.AllDirectories);

            if (directories.Length <= 0)
                throw new DirectoryNotFoundException($"Did not find \"{directory}\".");

            return directories[0];
        }

        private static string GetFile(string fileName)
        {
            var projectDirectory = Directory.GetCurrentDirectory();
            Debug.Log("Project directory: " + projectDirectory);

            var files = Directory.GetFiles(
                projectDirectory,
                fileName,
                SearchOption.AllDirectories);

            if (files.Length <= 0)
                throw new FileNotFoundException($"Could not find \"{fileName}\".");

            return files[0];
        }

        private readonly List<(string, string, string)> output = new List<(string, string, string)>();

        private void WriteAllText()
        {
            var str = "Generated the following files:";
            foreach (var (directory, filePath, fileContents) in output)
            {
                var safeDirectory = directory.Replace(@"\", "/");
                var safeFilePath = filePath.Replace(@"\", "/");
                if (!Directory.Exists(safeDirectory))
                    Directory.CreateDirectory(safeDirectory);
                File.WriteAllText(safeFilePath, fileContents);
                str += "\n" + safeFilePath;
            }
            Debug.Log(str);
        }
    }
}
