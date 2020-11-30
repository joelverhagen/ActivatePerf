using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System.Text;

namespace ActivatePerf
{
    class Program
    {
        static void Main(string[] args)
        {
#if RELEASE
            var baseConfig = DefaultConfig.Instance;
#else
            var baseConfig = new DebugBuildConfig();
#endif
            var config = ManualConfig.Create(baseConfig)
                .WithOption(ConfigOptions.JoinSummary, true);

            BenchmarkRunner.Run(new[]
            {
                BenchmarkConverter.TypeToBenchmarks(typeof(ActivateOfT<object>), config),
                BenchmarkConverter.TypeToBenchmarks(typeof(ActivateType<object>), config),
                BenchmarkConverter.TypeToBenchmarks(typeof(ActivateBaselineObject), config),
            });

#if RELEASE
            BenchmarkRunner.Run(new[]
            {
                BenchmarkConverter.TypeToBenchmarks(typeof(ActivateOfT<PackageAsset>), config),
                BenchmarkConverter.TypeToBenchmarks(typeof(ActivateType<PackageAsset>), config),
                BenchmarkConverter.TypeToBenchmarks(typeof(ActivateBaselinePackageAsset), config),
            });

            BenchmarkRunner.Run(new[]
            {
                BenchmarkConverter.TypeToBenchmarks(typeof(ActivateOfT<StringBuilder>), config),
                BenchmarkConverter.TypeToBenchmarks(typeof(ActivateType<StringBuilder>), config),
                BenchmarkConverter.TypeToBenchmarks(typeof(ActivateBaselineStringBuilder), config),
            });
#endif
        }
    }
}
