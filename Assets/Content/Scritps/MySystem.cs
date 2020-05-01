using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class MySystem : SystemBase
{
    protected override void OnUpdate()
    {
        var delta = Time.DeltaTime;
        Entities.ForEach(
            (ref Translation translation,
                in MyComponent component) =>
            {
                translation.Value += new float3(delta * component.MyValue, 0, 0);
            }).ScheduleParallel();
    }
}