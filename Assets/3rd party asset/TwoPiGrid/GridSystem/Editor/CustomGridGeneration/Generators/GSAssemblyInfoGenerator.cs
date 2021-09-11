namespace TwoPiGrid.Generation.Generators
{
    internal class GSAssemblyInfoGenerator : CodeGenerator
    {
        private readonly string baseNamespace;

        public GSAssemblyInfoGenerator(string baseNamespace)
            : base("GSAssemblyInfo.cs.txt", "AssemblyInfo.cs")
        {
            this.baseNamespace = baseNamespace;
        }

        protected override void CustomizeFile(ref string originalText)
        {
            originalText = originalText.Replace("__BASE_NAMESPACE__", baseNamespace);
        }
    }
}
