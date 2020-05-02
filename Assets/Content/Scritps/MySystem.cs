using Unity.Collections;
using Unity.Entities;
using Unity.Jobs.LowLevel.Unsafe;
using Unity.Mathematics;
using Unity.Transforms;

public class MySystem : SystemBase
{
    private NativeArray<Random> _randomArray;

    protected override void OnCreate()
    {
        var randomArray = new Random[JobsUtility.MaxJobThreadCount];
        var seed = new System.Random();

        for (var i = 0; i < JobsUtility.MaxJobThreadCount; i++)
        {
            randomArray[i] = new Random((uint) seed.Next());
        }

        _randomArray = new NativeArray<Random>(randomArray, Allocator.Persistent);
    }

    protected override void OnDestroy() => _randomArray.Dispose();

    protected override void OnUpdate()
    {
        var delta = Time.DeltaTime;
        var randomArray = _randomArray;
        Entities
            .WithNativeDisableParallelForRestriction(randomArray)
            .WithoutBurst()
            .ForEach((
                int nativeThreadIndex,
                ref Translation translation,
                ref MyComponent component) =>
            {
                translation.Value = math.lerp(translation.Value, component.Destination, delta * 2);
                if (math.distancesq(translation.Value, component.Destination) < 0.1f)
                {
                    var random = randomArray[nativeThreadIndex];
                    component.Destination = random.NextFloat3Direction();
                    randomArray[nativeThreadIndex] = random; // This is NECESSARY.
                }
            }).ScheduleParallel();
    }
}