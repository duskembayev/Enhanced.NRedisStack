namespace Enhanced.NRedisStack.SourceGenerators;

public partial class SchemaWriter
{
    private class BracketedBlockWriter : IDisposable
    {
        private readonly IndentedTextWriter _indentedTextWriter;
        private readonly char _close;

        public BracketedBlockWriter(IndentedTextWriter indentedTextWriter, char open, char close)
        {
            _indentedTextWriter = indentedTextWriter;
            _close = close;
            _indentedTextWriter.Write(open);
        }

        public void Dispose()
        {
            _indentedTextWriter.Write(_close);
        }
    }
}