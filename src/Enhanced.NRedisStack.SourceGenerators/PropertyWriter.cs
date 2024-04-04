namespace Enhanced.NRedisStack.SourceGenerators;

public class PropertyWriter
{
    private readonly string _variable;
    private readonly string _path;
    private readonly string _method;
    private readonly List<(string name, string value)> _arguments;
    private string? _alias;

    public PropertyWriter(string variable, string path, string method)
    {
        _variable = variable;
        _path = path;
        _method = method;
        _arguments = new List<(string name, string value)>();
    }

    public void SetAlias(string prefix, string? alias)
    {
        if (string.IsNullOrEmpty(alias))
        {
            _alias = null;
            return;
        }

        _alias = prefix + alias;
    }

    public void AddBoolArgument(string name, bool value)
    {
        _arguments.Add((name, value ? "true" : "false"));
    }

    public void AddDoubleArgument(string name, double value)
    {
        _arguments.Add((name, value.ToString("F", CultureInfo.InvariantCulture)));
    }

    public void AddStringArgument(string name, string? value)
    {
        _arguments.Add((name, value is null ? "null" : $"\"{value}\""));
    }

    public void Write(SchemaWriter writer)
    {
        writer.Write(_variable);
        writer.Write(".");
        writer.Write(_method);

        using (writer.DeclareBracketedBlock())
        {
            writer.Write("new FieldName");

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
                writer.Write(value);
            }
        }

        writer.WriteLine(";");
    }
}