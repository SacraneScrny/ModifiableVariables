using System;

using ModifiableVariable.Stages;
using ModifiableVariable.Stages.StageFactory;

namespace ModifiableVariable
{
    [Serializable]
    public class GateModifiable<T> : Modifiable<T, GateGeneral>
    {
        public GateModifiable(T baseValue, params (GateGeneral, StageOp<T>)[] stages) :
            base(baseValue, stages) { }
        public GateModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(GateModifiable<T> obj) => obj.GetValue();
        public static implicit operator GateModifiable<T>(T obj) => new(obj);
    }
    [Serializable]
    public class GateConjunctionModifiable<T> : Modifiable<T, GateConjunction>
    {
        public GateConjunctionModifiable(T baseValue, params (GateConjunction, StageOp<T>)[] stages) :
            base(baseValue, stages) { }
        public GateConjunctionModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(GateConjunctionModifiable<T> obj) => obj.GetValue();
        public static implicit operator GateConjunctionModifiable<T>(T obj) => new(obj);
    }
    [Serializable]
    public class GateDisjunctionModifiable<T> : Modifiable<T, GateDisjunction>
    {
        public GateDisjunctionModifiable(T baseValue, params (GateDisjunction, StageOp<T>)[] stages) :
            base(baseValue, stages) { }
        public GateDisjunctionModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(GateDisjunctionModifiable<T> obj) => obj.GetValue();
        public static implicit operator GateDisjunctionModifiable<T>(T obj) => new(obj);
    }
    [Serializable]
    public class GateComplexModifiable<T> : Modifiable<T, GateComplex>
    {
        public GateComplexModifiable(T baseValue, params (GateComplex, StageOp<T>)[] stages) :
            base(baseValue, stages) { }
        public GateComplexModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(GateComplexModifiable<T> obj) => obj.GetValue();
        public static implicit operator GateComplexModifiable<T>(T obj) => new(obj);
    }
    
    [Serializable]
    public class Modifiable<T> : Modifiable<T, General>
    {
        public Modifiable(T baseValue, params (General, StageOp<T>)[] stages) :
            base(baseValue, stages) { }
        public Modifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(Modifiable<T> obj) => obj.GetValue();
        public static implicit operator Modifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class SimpleModifiable<T> : Modifiable<T, Simple>
    {
        public SimpleModifiable(T baseValue, params (Simple, StageOp<T>)[] stages) : base(baseValue,
            stages) { }
        public SimpleModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(SimpleModifiable<T> obj) => obj.GetValue();
        public static implicit operator SimpleModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class ComplexModifiable<T> : Modifiable<T, Complex>
    {
        public ComplexModifiable(T baseValue, params (Complex, StageOp<T>)[] stages) : base(baseValue,
            stages) { }
        public ComplexModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(ComplexModifiable<T> obj) => obj.GetValue();
        public static implicit operator ComplexModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class DamageModifiable<T> : Modifiable<T, Damage>
    {
        public DamageModifiable(T baseValue, params (Damage, StageOp<T>)[] stages) : base(baseValue,
            stages) { }
        public DamageModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(DamageModifiable<T> obj) => obj.GetValue();
        public static implicit operator DamageModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class DefenseModifiable<T> : Modifiable<T, Defense>
    {
        public DefenseModifiable(T baseValue, params (Defense, StageOp<T>)[] stages) : base(baseValue,
            stages) { }
        public DefenseModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(DefenseModifiable<T> obj) => obj.GetValue();
        public static implicit operator DefenseModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class SpeedModifiable<T> : Modifiable<T, Speed>
    {
        public SpeedModifiable(T baseValue, params (Speed, StageOp<T>)[] stages) :
            base(baseValue, stages) { }
        public SpeedModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(SpeedModifiable<T> obj) => obj.GetValue();
        public static implicit operator SpeedModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class ResourceModifiable<T> : Modifiable<T, Resource>
    {
        public ResourceModifiable(T baseValue, params (Resource, StageOp<T>)[] stages) : base(baseValue,
            stages) { }
        public ResourceModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(ResourceModifiable<T> obj) => obj.GetValue();
        public static implicit operator ResourceModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class ChanceModifiable<T> : Modifiable<T, Chance>
    {
        public ChanceModifiable(T baseValue, params (Chance, StageOp<T>)[] stages) : base(baseValue,
            stages) { }
        public ChanceModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(ChanceModifiable<T> obj) => obj.GetValue();
        public static implicit operator ChanceModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class CooldownModifiable<T> : Modifiable<T, Cooldown>
    {
        public CooldownModifiable(T baseValue, params (Cooldown, StageOp<T>)[] stages) : base(baseValue,
            stages) { }
        public CooldownModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(CooldownModifiable<T> obj) => obj.GetValue();
        public static implicit operator CooldownModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class ColorModifiable<T> : Modifiable<T, ColorModificator>
    {
        public ColorModifiable(T baseValue, params (ColorModificator, StageOp<T>)[] stages) : base(
            baseValue, stages) { }
        public ColorModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(ColorModifiable<T> obj) => obj.GetValue();
        public static implicit operator ColorModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class PositionModifiable<T> : Modifiable<T, Position>
    {
        public PositionModifiable(T baseValue, params (Position, StageOp<T>)[] stages) : base(baseValue,
            stages) { }
        public PositionModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(PositionModifiable<T> obj) => obj.GetValue();
        public static implicit operator PositionModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class RotationModifiable<T> : Modifiable<T, Rotation>
    {
        public RotationModifiable(T baseValue, params (Rotation, StageOp<T>)[] stages) : base(baseValue,
            stages) { }
        public RotationModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(RotationModifiable<T> obj) => obj.GetValue();
        public static implicit operator RotationModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class OverridableModifiable<T> : Modifiable<T, Overridable>
    {
        public OverridableModifiable(T baseValue, params (Overridable, StageOp<T>)[] stages) : base(
            baseValue, stages) { }
        public OverridableModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(OverridableModifiable<T> obj) => obj.GetValue();
        public static implicit operator OverridableModifiable<T>(T obj) => new(obj);
    }

    [Serializable]
    public class BlendModifiable<T> : Modifiable<T, Blend>
    {
        public BlendModifiable(T baseValue, params (Blend, StageOp<T>)[] stages) :
            base(baseValue, stages) { }
        public BlendModifiable(T baseValue) : base(baseValue) { }
        public static implicit operator T(BlendModifiable<T> obj) => obj.GetValue();
        public static implicit operator BlendModifiable<T>(T obj) => new(obj);
    }
}