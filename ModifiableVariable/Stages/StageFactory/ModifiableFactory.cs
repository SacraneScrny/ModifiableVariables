using System;
using System.Collections.Generic;
using System.Reflection;

namespace ModifiableVariable.Stages.StageFactory
{
    public static class ModifiableFactory
    {
        static readonly Dictionary<Type, StageOpKind[]> _cache = new();
        
        public static void TryPopulate<T, TStage>(Modifiable<T, TStage> modifiable)
            where TStage : Enum
        {
            var values = (TStage[])Enum.GetValues(typeof(TStage));
            var ops = TryGetOps<TStage>();
            if (ops == null) return;

            for (var i = 0; i < values.Length; i++)
            {
                modifiable.AddStage(StageArithmetic<T>.Get(ops[i]), values[i]);
            }
        }

        static StageOpKind[] TryGetOps<TStage>() where TStage : Enum
        {
            var type = typeof(TStage);
            if (_cache.TryGetValue(type, out var cached)) return cached;

            var values = Enum.GetValues(type);
            var ops = new StageOpKind[values.Length];
            var hasAny = false;

            for (var i = 0; i < values.Length; i++)
            {
                var name = Enum.GetName(type, values.GetValue(i));
                var attr = type.GetField(name).GetCustomAttribute<StageOpAttribute>();
                if (attr != null) hasAny = true;
                ops[i] = attr?.Kind ?? StageOpKind.Add;
            }

            if (!hasAny) return null;

            _cache[type] = ops;
            return ops;
        }
    }
    
    public enum StageOpKind
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Min,
        Max,
        Override,
        Lerp,
        Scale,
        
        Or,
        And
    }
}