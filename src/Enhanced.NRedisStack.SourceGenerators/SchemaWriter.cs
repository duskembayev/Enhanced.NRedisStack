﻿using System.CodeDom.Compiler;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Enhanced.NRedisStack.SourceGenerators;

public partial class SchemaWriter : IDisposable
{
    private readonly IndentedTextWriter _indentedTextWriter;
    private readonly StringWriter _stringWriter;

    public SchemaWriter()
    {
        _stringWriter = new StringWriter();
        _indentedTextWriter = new IndentedTextWriter(_stringWriter);
        _indentedTextWriter.WriteLine("// <auto-generated>");
        _indentedTextWriter.WriteLine("using NRedisStack.Search;");
        _indentedTextWriter.WriteLine();
    }

    public IDisposable DeclarePartialClass(INamedTypeSymbol namedType)
    {
        _indentedTextWriter.WriteLine($"namespace {namedType.ContainingNamespace.ToDisplayString()};");
        _indentedTextWriter.WriteLine();

        if (namedType.IsStatic)
            _indentedTextWriter.Write("static ");

        _indentedTextWriter.WriteLine($"partial class {namedType.Name}");
        return new IndentedSectionWriter(_indentedTextWriter);
    }

    public IDisposable DeclarePartialMethod(IMethodSymbol method)
    {
        _indentedTextWriter.Write("public ");

        if (method.IsStatic)
            _indentedTextWriter.Write("static ");

        _indentedTextWriter.WriteLine($"partial Schema {method.Name}()");
        return new IndentedSectionWriter(_indentedTextWriter);
    }

    public IDisposable DeclareBracketedBlock(char open = '(', char close = ')')
    {
        return new BracketedBlockWriter(_indentedTextWriter, open, close);
    }

    public void Write(string value)
    {
        _indentedTextWriter.Write(value);
    }

    public void WriteLine(string value)
    {
        _indentedTextWriter.WriteLine(value);
    }

    public void WriteQuoted(string value)
    {
        _indentedTextWriter.Write("\"");
        _indentedTextWriter.Write(value);
        _indentedTextWriter.Write("\"");
    }

    public void WriteNull()
    {
        _indentedTextWriter.Write("null");
    }

    public SourceText ToSourceText()
    {
        return SourceText.From(_stringWriter.ToString(), Encoding.UTF8);
    }

    public void Dispose()
    {
        _stringWriter.Dispose();
        _indentedTextWriter.Dispose();
    }

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