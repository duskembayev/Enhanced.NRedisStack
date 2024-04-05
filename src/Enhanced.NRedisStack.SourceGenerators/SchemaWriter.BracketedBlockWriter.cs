namespace Enhanced.NRedisStack.SourceGenerators;

public partial class SchemaWriter
{
    private class BracketedBlockWriter : IDisposable
    {
        private readonly char _close;
        private readonly IndentedTextWriter _indentedTextWriter;

        public BracketedBlockWriter(IndentedTextWriter indentedTextWriter, char open, char close)
        {
            _indentedTextWriter = indentedTextWriter;
            _close = close;
            _indentedTextWriter.Write(open);
        }

        public void Dispose() => _indentedTextWriter.Write(_close);
    }
}
