using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Reflection.Emit;

namespace ActivatePerf
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
#if RELEASE
    [SimpleJob(RuntimeMoniker.NetCoreApp50)]
    [SimpleJob(RuntimeMoniker.Net48)]
#endif
    public abstract class ActivateOfT<T> where T : new()
    {
        private MyActivate _emitActivate;
        private MyActivate _reflectionActivate;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _emitActivate = GetEmitActivate();
            _reflectionActivate = GetReflectionActivate();
        }

        [Benchmark] public T Reflection() => (T)GetReflectionActivate()();
#if RELEASE
        [Benchmark] public T ReflectionCached() => (T)_reflectionActivate();
        [Benchmark] public T Emit() => (T)GetEmitActivate()();
        [Benchmark] public T EmitCached() => (T)_emitActivate();
        [Benchmark] public T Activator() => System.Activator.CreateInstance<T>();
        [Benchmark] public T NewT() => new T();
#endif

        private MyActivate GetReflectionActivate()
        {
            Type type = typeof(T);
            var constructor = type.GetConstructor(Type.EmptyTypes);
            return () => constructor.Invoke(null);
        }

        private MyActivate GetEmitActivate()
        {
            Type type = typeof(T);
            var method = new DynamicMethod("EmitActivate", type, null, true);
            var generator = method.GetILGenerator();
            generator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
            generator.Emit(OpCodes.Ret);
            var emitActivate = (MyActivate)method.CreateDelegate(typeof(MyActivate));
            return emitActivate;
        }
    }
}
