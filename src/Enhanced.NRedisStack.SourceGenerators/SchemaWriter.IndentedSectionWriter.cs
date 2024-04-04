namespace Enhanced.NRedisStack.SourceGenerators;

public partial class SchemaWriter
{
    private class IndentedSectionWriter : IDisposable
    {
        private readonly IndentedTextWriter _indentedTextWriter;

        public IndentedSectionWriter(IndentedTextWriter indentedTextWriter)
        {
            _indentedTextWriter = indentedTextWriter;
            _indentedTextWriter.WriteLine("{");
            _indentedTextWriter.Indent++;
        }

        public void Dispose()
        {
            _indentedTextWriter.Indent--;
            _indentedTextWriter.WriteLine("}");
        }
    }
}