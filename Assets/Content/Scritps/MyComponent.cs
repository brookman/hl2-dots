using Unity.Entities;
using Unity.Profiling;

[GenerateAuthoringComponent]
public struct MyComponent : IComponentData
{
    public float MyValue;
}