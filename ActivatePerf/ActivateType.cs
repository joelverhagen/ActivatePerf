using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Reflection.Emit;
using System.Text;

namespace ActivatePerf
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
#if RELEASE
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [SimpleJob(RuntimeMoniker.Net48)]
#endif
    public class ActivateType<T> where T : new()
    {
        private Type _type;
        private MyActivate _emitActivate;
        private MyActivate _reflectionActivate;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _type = typeof(T);
            _emitActivate = GetEmitActivate();
            _reflectionActivate = GetReflectionActivate();
        }

        [Benchmark] public object Reflection() => GetReflectionActivate()();
#if RELEASE
        [Benchmark] public object ReflectionCached() => _reflectionActivate();
        [Benchmark] public object Emit() => GetEmitActivate()();
        [Benchmark] public object EmitCached() => _emitActivate();
        [Benchmark] public object Activator() => System.Activator.CreateInstance(_type);
#endif

        private MyActivate GetReflectionActivate()
        {
            var constructor = _type.GetConstructor(Type.EmptyTypes);
            return () => constructor.Invoke(null);
        }

        private MyActivate GetEmitActivate()
        {
            var method = new DynamicMethod("EmitActivate", _type, null, true);
            var generator = method.GetILGenerator();
            generator.Emit(OpCodes.Newobj, _type.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Ret);
            var emitActivate = (MyActivate)method.CreateDelegate(typeof(MyActivate));
            return emitActivate;
        }
    }
}
