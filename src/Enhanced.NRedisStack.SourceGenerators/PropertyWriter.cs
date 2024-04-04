using System.Globalization;

namespace Enhanced.NRedisStack.SourceGenerators;

public class PropertyWriter
{
    private readonly string _variable;
    private readonly string _path;
    private readonly string _method;
    private readonly List<(string name, string? value)> _arguments;
    private string? _alias;

    public PropertyWriter(string variable, string path, string method)
    {
        _variable = variable;
        _path = path;
        _method = method;
        _arguments = new List<(string name, string? value)>();
    }

    public void SetAlias(string? alias)
    {
        _alias = alias;
    }

    public void AddArgument(string name, string? value)
    {
        _arguments.Add((name, value));
    }

    public void AddBoolArgument(string name, bool value)
    {
        AddArgument(name, value ? "true" : "false");
    }

    public void AddDoubleArgument(string name, double value)
    {
        AddArgument(name, value.ToString("F", CultureInfo.InvariantCulture));
    }

    public void Write(SchemaWriter writer)
    {
        writer.Write(_variable);
        writer.Write(".");
        writer.Write(_method);

        using (writer.DeclareBracketedBlock())
        {
            writer.Write("new Field");

            using (writer.DeclareBracketedBlock())
            {
                writer.WriteQuoted(_path);

                if (!string.IsNullOrEmpty(_alias))
                {
                    writer.Write(", ");
                    writer.WriteQuoted(_alias!);
                }
            }

            foreach (var (name, value) in _arguments)
            {
                writer.Write(", ");
                writer.Write(name);
                writer.Write(": ");

                if (value is null)
                    writer.WriteNull();
                else
                    writer.WriteQuoted(value);
            }
        }

        writer.Write(";");
    }
}