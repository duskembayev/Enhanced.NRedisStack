global using System.CodeDom.Compiler;
global using System.Collections.Immutable;
global using System.Diagnostics;
global using System.Globalization;
global using System.Text;
global using Microsoft.CodeAnalysis;
global using Microsoft.CodeAnalysis.CSharp.Syntax;
global using Microsoft.CodeAnalysis.Text;
global using RedisPropertyInfo = (
    Enhanced.NRedisStack.SourceGenerators.RedisPropertyType type,
    Microsoft.CodeAnalysis.AttributeData? attribute);