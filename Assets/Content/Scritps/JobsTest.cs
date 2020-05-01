using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class JobsTest : MonoBehaviour
{
    public GameObject Cube1;
    public GameObject Cube2;

    [BurstCompile]
    public struct MyJob : IJob
    {
        public float3 InputPos;
        public NativeArray<float3> OutputPos;

        public void Execute()
        {
            OutputPos[0] = InputPos + new float3(0.2f, 0.2f, 0.2f);
        }
    }

    private float _lastTime = 0;

    void Update()
    {
        if (Time.unscaledTime - _lastTime > 3)
        {
            _lastTime = Time.unscaledTime;
        }
        else
        {
            return;
        }

        var originalPos = Random.insideUnitSphere * 0.2f;
        float3 originalPosFloat3 = originalPos;

        Cube1.transform.position = originalPos;

        var output = new NativeArray<float3>(1, Allocator.TempJob);
        new MyJob
            {
                InputPos = originalPosFloat3,
                OutputPos = output
            }.Schedule()
            .Complete();

        Cube2.transform.position = output[0];
        output.Dispose();
    }
}