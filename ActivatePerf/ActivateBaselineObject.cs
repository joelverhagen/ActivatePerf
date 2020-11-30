using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System.Text;

namespace ActivatePerf
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
#if RELEASE
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [SimpleJob(RuntimeMoniker.Net48)]
#endif
    public class ActivateBaselineObject
    {
        [Benchmark(Baseline = true)] public object NewObject() => new object();
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
#if RELEASE
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [SimpleJob(RuntimeMoniker.Net48)]
#endif
    public class ActivateBaselinePackageAsset
    {
        [Benchmark(Baseline = true)] public PackageAsset NewPackageAsset() => new PackageAsset();
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
#if RELEASE
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [SimpleJob(RuntimeMoniker.Net48)]
#endif
    public class ActivateBaselineStringBuilder
    {
        [Benchmark(Baseline = true)] public StringBuilder NewStringBuilder() => new StringBuilder();
    }
}

