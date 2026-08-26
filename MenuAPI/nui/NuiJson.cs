using System.Globalization;
using System.Text;

namespace MenuAPI;

internal sealed class NuiJson
{
    private readonly StringBuilder _builder = new();
    private bool _needsComma;

    internal NuiJson Reset()
    {
        _builder.Clear();
        _needsComma = false;

        return this;
    }

    internal bool Matches(string? value) => value is not null && _builder.Equals(value.AsSpan());

    internal NuiJson Object()
    {
        Separate();
        _builder.Append('{');
        _needsComma = false;

        return this;
    }

    internal NuiJson EndObject()
    {
        _builder.Append('}');
        _needsComma = true;

        return this;
    }

    internal NuiJson Array()
    {
        Separate();
        _builder.Append('[');
        _needsComma = false;

        return this;
    }

    internal NuiJson EndArray()
    {
        _builder.Append(']');
        _needsComma = true;

        return this;
    }

    internal NuiJson Object(string name)
    {
        Key(name);
        _builder.Append('{');
        _needsComma = false;

        return this;
    }

    internal NuiJson Array(string name)
    {
        Key(name);
        _builder.Append('[');
        _needsComma = false;

        return this;
    }

    internal NuiJson Prop(string name, string? value)
    {
        Key(name);

        if (value is null)
        {
            _builder.Append("null");
        }
        else
        {
            Escape(value);
        }

        _needsComma = true;

        return this;
    }

    internal NuiJson Prop(string name, bool value)
    {
        Key(name);
        _builder.Append(value ? "true" : "false");
        _needsComma = true;

        return this;
    }

    internal NuiJson Prop(string name, int value)
    {
        Key(name);
        _builder.Append(value);
        _needsComma = true;

        return this;
    }

    internal NuiJson Prop(string name, float value)
    {
        Key(name);
        _builder.Append(value.ToString("0.####", CultureInfo.InvariantCulture));
        _needsComma = true;

        return this;
    }

    internal NuiJson Null(string name)
    {
        Key(name);
        _builder.Append("null");
        _needsComma = true;

        return this;
    }

    internal NuiJson Value(int value)
    {
        Separate();
        _builder.Append(value);
        _needsComma = true;

        return this;
    }

    internal NuiJson Value(string value)
    {
        Separate();
        Escape(value);
        _needsComma = true;

        return this;
    }

    public override string ToString() => _builder.ToString();

    private void Key(string name)
    {
        Separate();
        Escape(name);
        _builder.Append(':');
    }

    private void Separate()
    {
        if (_needsComma)
        {
            _builder.Append(',');
        }
    }

    private void Escape(string value)
    {
        _builder.Append('"');

        if (!NeedsEscaping(value))
        {
            _builder.Append(value).Append('"');

            return;
        }

        for (var i = 0; i < value.Length; i++)
        {
            var character = value[i];

            switch (character)
            {
                case '"':
                    _builder.Append("\\\"");
                    break;

                case '\\':
                    _builder.Append("\\\\");
                    break;

                case '\n':
                    _builder.Append("\\n");
                    break;

                case '\r':
                    _builder.Append("\\r");
                    break;

                case '\t':
                    _builder.Append("\\t");
                    break;

                default:
                    if (character < ' ')
                    {
                        _builder.Append("\\u").Append(((int)character).ToString("x4"));
                    }
                    else
                    {
                        _builder.Append(character);
                    }

                    break;
            }
        }

        _builder.Append('"');
    }

    private static bool NeedsEscaping(string value)
    {
        for (var i = 0; i < value.Length; i++)
        {
            var character = value[i];

            if (character == '"' || character == '\\' || character < ' ')
            {
                return true;
            }
        }

        return false;
    }
}
