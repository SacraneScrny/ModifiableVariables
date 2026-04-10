# ModifiableVariable

A modifiable variable system for Unity. Stores a base value of any type and applies a stack of modifiers grouped into named stages — with per-frame result caching and a value-changed event.

## Concept

```
BaseValue → [Stage: Flat +10, +5] → [Stage: Multiply ×1.2] → [Stage: Cap min(_, 100)] → FinalValue
```

Each `Modifiable<T, TStage>` holds a base value of type `T`. Stages are defined by a `TStage` enum — each enum member corresponds to one stage with a specific arithmetic operation. Modifiers are plain delegates `() => T` added to the desired stage. When `GetValue()` is called, the value passes through all stages in order; the result is cached for the current frame.

## Installation

Copy the `ModifiableVariable` folder into your project. An Assembly Definition `ModifiableVariables.asmdef` is included.

## Quick Start

```csharp
// A float variable with two stages: Flat (addition) and Multiply (multiplication)
var health = new Modifiable<float>(100f); // uses General by default

// Add a modifier to the Flat stage
var handler = health.Add(() => 50f, General.Flat);

// Read the value (cached per frame)
float value = health.GetValue(); // 150f
// or via implicit operator:
float value2 = health;           // 150f

// Remove via handler (IDisposable)
handler.Dispose();
// or explicitly:
health.Remove(handler, General.Flat);

// Subscribe to value changes
health.ValueChanged += v => Debug.Log($"Health changed: {v}");

// Reset all modifiers and restore base value
health.Clear();
```

## Built-in Stage Sets

All predefined enums live in `ModifiableVariable.Stages.StageFactory` and have corresponding typed wrapper classes.

| Type | Enum | Stages |
|---|---|---|
| `Modifiable<T>` | `General` | `Flat` (+), `Multiply` (×) |
| `SimpleModifiable<T>` | `Simple` | `Flat` (+) |
| `ComplexModifiable<T>` | `Complex` | `Flat` (+), `Multiply` (×), `Post` (+) |
| `DamageModifiable<T>` | `Damage` | `Flat` (+), `Multiply` (×), `Penetration` (−), `Cap` (min) |
| `DefenseModifiable<T>` | `Defense` | `Flat` (+), `Multiply` (×), `Cap` (min) |
| `SpeedModifiable<T>` | `Speed` | `Flat` (+), `Multiply` (×), `Min` (max), `Max` (min) |
| `ResourceModifiable<T>` | `Resource` | `Flat` (+), `Multiply` (×), `Regen` (+), `Cap` (min) |
| `ChanceModifiable<T>` | `Chance` | `Flat` (+), `Multiply` (×), `Cap` (min) |
| `CooldownModifiable<T>` | `Cooldown` | `Reduction` (−), `Multiply` (×), `Floor` (max) |
| `ColorModifiable<T>` | `ColorModificator` | `Tint` (×), `Overlay` (+), `Override` |
| `PositionModifiable<T>` | `Position` | `Offset` (+), `Scale` (×), `Override` |
| `RotationModifiable<T>` | `Rotation` | `Multiply` (×), `Override` |
| `OverridableModifiable<T>` | `Overridable` | `Override` |
| `BlendModifiable<T>` | `Blend` | `Offset` (+), `Lerp`, `Override` |

**Gate variables** (for boolean values):

| Type | Enum | Stages |
|---|---|---|
| `GateModifiable<T>` | `GateGeneral` | `DisjunctionState` (or), `ConjunctionState` (and), `Override` |
| `GateConjunctionModifiable<T>` | `GateConjunction` | `ConjunctionState` (and), `Override` |
| `GateDisjunctionModifiable<T>` | `GateDisjunction` | `DisjunctionState` (or), `Override` |
| `GateComplexModifiable<T>` | `GateComplex` | `DisjunctionState`, `ConjunctionState`, `LastDisjunctionState`, `Override` |

## Adding and Removing Modifiers

```csharp
var damage = new DamageModifiable<float>(10f);

// Add to a specific stage
var flatBonus = damage.Add(() => 5f, Damage.Flat);
var multiplier = damage.Add(() => 1.2f, Damage.Multiply); // result: (10+5) * 1.2 = 18

// ModifierDelegateHandler is IDisposable — safe to call Dispose multiple times
using var tempBonus = damage.Add(() => 100f, Damage.Flat);
// tempBonus is automatically removed when the using block exits

// Remove manually
damage.Remove(flatBonus, Damage.Flat);

// Remove without specifying a stage (searches all stages)
damage.Remove(multiplier);

// Number of active modifiers
int count = damage.Count;
```

## ValueChanged Event

`ValueChanged` fires only when the computed value actually changes (compared via `IEqualityComparer<T>`).

```csharp
var speed = new SpeedModifiable<float>(5f);
speed.ValueChanged += v => Debug.Log($"Speed: {v}");

speed.BaseValue = 8f; // triggers ValueChanged on the next GetValue() call
```

## Defining a Custom Stage Enum

Define an enum with a `[StageOp(...)]` attribute on each field. `ModifiableFactory` will automatically create the stages the first time a variable of that type is constructed.

```csharp
public enum AbilityPower
{
    [StageOp(StageOpKind.Add)]      Flat,
    [StageOp(StageOpKind.Multiply)] Amplify,
    [StageOp(StageOpKind.Min)]      Cap,
}

// Usage
var power = new Modifiable<float, AbilityPower>(100f);
power.Add(() => 50f, AbilityPower.Flat);
power.Add(() => 1.5f, AbilityPower.Amplify);
power.Add(() => 200f, AbilityPower.Cap); // capped at 200
```

Alternatively, pass stages manually through the constructor, bypassing attributes:

```csharp
var power = new Modifiable<float, AbilityPower>(100f,
    (AbilityPower.Flat,    StageArithmetic<float>.Get(StageOpKind.Add)),
    (AbilityPower.Amplify, StageArithmetic<float>.Get(StageOpKind.Multiply)),
    (AbilityPower.Cap,     StageArithmetic<float>.Get(StageOpKind.Min))
);
```

## Supporting Custom Types

For types that have no CLR-level operators (`+`, `*`, etc.), register operations manually via `StageArithmetic<T>.Register` and, if needed, a comparer via `ComparerFactory.Register`. This must happen before any variable of that type is created — `[RuntimeInitializeOnLoadMethod]` is the ideal place.

```csharp
public static class BigNumberStageArithmeticsUnityBoostrap
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        StageArithmetic<BigNumber>.Register(StageOpKind.Add,      (a, b) => a + b);
        StageArithmetic<BigNumber>.Register(StageOpKind.Subtract, (a, b) => a - b);
        StageArithmetic<BigNumber>.Register(StageOpKind.Multiply, (a, b) => a * b);
        StageArithmetic<BigNumber>.Register(StageOpKind.Divide,   (a, b) => a / b);
        StageArithmetic<BigNumber>.Register(StageOpKind.Min,      (a, b) => BigNumber.Min(a, b));
        StageArithmetic<BigNumber>.Register(StageOpKind.Max,      (a, b) => BigNumber.Max(a, b));

        ComparerFactory.Register(new BigNumberNumericComparer());
    }
}
```

`BigNumberNumericComparer` implements `IEqualityComparer<BigNumber>` and is used to determine whether the value has actually changed before firing `ValueChanged`.

## Built-in Supported Types

Numeric types (`int`, `float`, `double`, `long`, etc.) are supported automatically — operations are compiled via Expression Trees on first access.

Unity types are registered in `StageArithmeticUnityBootstrap` (called automatically `BeforeSceneLoad`):

`bool`, `Vector2`, `Vector2Int`, `Vector3`, `Vector3Int`, `Vector4`, `Color`, `Color32`, `Quaternion`, `Rect`, `Bounds`, `BoundsInt`, `Matrix4x4`

## Comparers

`ComparerFactory` is used to compare values when checking whether `BaseValue` has changed and when deciding whether to fire `ValueChanged`. Epsilon-based comparers are registered for `float` and all vector types. For any other type you can replace the comparer globally or per-instance:

```csharp
// Globally (affects all variables of this type)
ComparerFactory.Register(new MyTypeComparer());

// Per-instance (only affects this variable)
var v = new Modifiable<MyType>(defaultValue).WithComparer(new MyTypeComparer());
```

## Project Structure

```
ModifiableVariable/
├── Modifiable.cs                      # Core Modifiable<T, TStage> class
├── Modifiables.cs                     # Typed wrappers (DamageModifiable, SpeedModifiable, ...)
├── Comparers/
│   ├── ComparerFactory.cs             # IEqualityComparer<T> registry
│   └── DefaultComparers.cs            # Comparers for Unity types and float/int
├── Entities/
│   ├── ModifierDelegate.cs            # delegate T ModifierDelegate<T>()
│   └── ModifierDelegateHandler.cs     # IDisposable modifier handle
├── Stages/
│   ├── Stage.cs                       # Modifier collection + StageOp
│   ├── StageOp.cs                     # delegate T StageOp<T>(T a, T b)
│   └── StageFactory/
│       ├── StageArithmetic.cs         # Per-type operation registry keyed by StageOpKind
│       ├── StageArithmeticUnityBootstrap.cs  # Unity type registrations
│       ├── ModifiableFactory.cs       # Auto-creates stages from [StageOp] attributes
│       ├── StageOpAttribute.cs        # [StageOp(StageOpKind)] attribute
│       └── DefaultStages.cs          # Built-in stage enums
└── Editor/
    └── ModifiableDrawer.cs            # Unity inspector PropertyDrawer
```
