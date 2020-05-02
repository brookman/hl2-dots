using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct MyComponent : IComponentData
{
    public float3 Destination;
}