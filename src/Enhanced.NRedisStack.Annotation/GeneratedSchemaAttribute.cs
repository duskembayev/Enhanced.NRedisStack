﻿namespace Enhanced.NRedisStack.Annotation;

[AttributeUsage(AttributeTargets.Method)]
public sealed class GeneratedSchemaAttribute(Type Type) : Attribute
{
    public PropertyNamingPolicy PropertyNamingPolicy { get; set; }
}