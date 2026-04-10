# ModifiableVariable

Система модифицируемых переменных для Unity. Позволяет хранить базовое значение любого типа и применять к нему стопку модификаторов, сгруппированных по именованным стадиям — с кешированием результата на кадр и событием изменения значения.

## Концепция

```
BaseValue → [Stage: Flat +10, +5] → [Stage: Multiply ×1.2] → [Stage: Cap min(_, 100)] → FinalValue
```

Каждая `Modifiable<T, TStage>` хранит базовое значение типа `T`. Стадии (stages) определяются перечислением `TStage` — каждый элемент enum соответствует одной стадии с конкретной арифметической операцией. Модификаторы — это просто делегаты `() => T`, добавляемые в нужную стадию. При вызове `GetValue()` значение проходит через все стадии по порядку; результат кешируется на текущий кадр.

## Установка

Скопируй папку `ModifiableVariable` в проект. Assembly Definition `ModifiableVariables.asmdef` включён.

## Быстрый старт

```csharp
// Простая float-переменная с двумя стадиями: Flat (сложение) и Multiply (умножение)
var health = new Modifiable<float>(100f); // использует General по умолчанию

// Добавление модификатора в стадию Flat
var handler = health.Add(() => 50f, General.Flat);

// Чтение значения (кешируется на кадр)
float value = health.GetValue(); // 150f
// или через implicit operator:
float value2 = health;           // 150f

// Удаление через handler (IDisposable)
handler.Dispose();
// или явно:
health.Remove(handler, General.Flat);

// Подписка на изменение
health.ValueChanged += v => Debug.Log($"Health changed: {v}");

// Сброс всех модификаторов к базовому значению
health.Clear();
```

## Встроенные наборы стадий

Все готовые enum-ы находятся в `ModifiableVariable.Stages.StageFactory` и имеют соответствующие типизированные обёртки.

| Тип переменной | Enum | Стадии |
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

**Gate-переменные** (для булевых значений):

| Тип | Enum | Стадии |
|---|---|---|
| `GateModifiable<T>` | `GateGeneral` | `DisjunctionState` (or), `ConjunctionState` (and), `Override` |
| `GateConjunctionModifiable<T>` | `GateConjunction` | `ConjunctionState` (and), `Override` |
| `GateDisjunctionModifiable<T>` | `GateDisjunction` | `DisjunctionState` (or), `Override` |
| `GateComplexModifiable<T>` | `GateComplex` | `DisjunctionState`, `ConjunctionState`, `LastDisjunctionState`, `Override` |

## Добавление и удаление модификаторов

```csharp
var damage = new DamageModifiable<float>(10f);

// Добавить в конкретную стадию
var flatBonus = damage.Add(() => 5f, Damage.Flat);
var multiplier = damage.Add(() => 1.2f, Damage.Multiply); // итого: (10+5) * 1.2 = 18

// ModifierDelegateHandler — IDisposable, безопасно вызывать Dispose повторно
using var tempBonus = damage.Add(() => 100f, Damage.Flat);
// tempBonus автоматически уберётся при выходе из using-блока

// Удаление вручную
damage.Remove(flatBonus, Damage.Flat);

// Удаление без указания стадии (поиск по всем стадиям)
damage.Remove(multiplier);

// Количество активных модификаторов
int count = damage.Count;
```

## Событие ValueChanged

`ValueChanged` срабатывает только при фактическом изменении значения (сравнение через `IEqualityComparer<T>`).

```csharp
var speed = new SpeedModifiable<float>(5f);
speed.ValueChanged += v => Debug.Log($"Speed: {v}");

speed.BaseValue = 8f; // вызовет ValueChanged при следующем GetValue()
```

## Создание своего enum стадий

Достаточно определить enum с атрибутом `[StageOp(...)]` на каждом поле. `ModifiableFactory` автоматически создаст стадии при первом конструировании переменной.

```csharp
public enum AbilityPower
{
    [StageOp(StageOpKind.Add)]      Flat,
    [StageOp(StageOpKind.Multiply)] Amplify,
    [StageOp(StageOpKind.Min)]      Cap,
}

// Использование
var power = new Modifiable<float, AbilityPower>(100f);
power.Add(() => 50f, AbilityPower.Flat);
power.Add(() => 1.5f, AbilityPower.Amplify);
power.Add(() => 200f, AbilityPower.Cap); // не выше 200
```

Или передать стадии вручную через конструктор, минуя атрибуты:

```csharp
var power = new Modifiable<float, AbilityPower>(100f,
    (AbilityPower.Flat,    StageArithmetic<float>.Get(StageOpKind.Add)),
    (AbilityPower.Amplify, StageArithmetic<float>.Get(StageOpKind.Multiply)),
    (AbilityPower.Cap,     StageArithmetic<float>.Get(StageOpKind.Min))
);
```

## Поддержка пользовательских типов

Для типов, у которых нет операторов (`+`, `*` и т.д.) на уровне CLR, нужно зарегистрировать операции вручную через `StageArithmetic<T>.Register` и, при необходимости, компаратор через `ComparerFactory.Register`. Делать это нужно до первого создания переменной — идеальное место `[RuntimeInitializeOnLoadMethod]`.

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

Где `BigNumberNumericComparer` реализует `IEqualityComparer<BigNumber>` — он используется для определения того, изменилось ли значение, прежде чем стрелять `ValueChanged`.

## Встроенные поддерживаемые типы

Числовые типы (`int`, `float`, `double`, `long` и т.д.) поддерживаются автоматически — операции компилируются через Expression Trees при первом обращении.

Unity-типы зарегистрированы в `StageArithmeticUnityBootstrap` (вызывается автоматически `BeforeSceneLoad`):

`bool`, `Vector2`, `Vector2Int`, `Vector3`, `Vector3Int`, `Vector4`, `Color`, `Color32`, `Quaternion`, `Rect`, `Bounds`, `BoundsInt`, `Matrix4x4`

## Компараторы

`ComparerFactory` используется для сравнения значений при проверке изменения `BaseValue` и при вызове `ValueChanged`. Для `float` и векторных типов зарегистрированы epsilon-компараторы. Для произвольного типа можно заменить компаратор глобально или локально:

```csharp
// Глобально (для всех переменных этого типа)
ComparerFactory.Register(new MyTypeComparer());

// Локально (только для конкретного экземпляра)
var v = new Modifiable<MyType>(defaultValue).WithComparer(new MyTypeComparer());
```

## Структура проекта

```
ModifiableVariable/
├── Modifiable.cs                      # Основной класс Modifiable<T, TStage>
├── Modifiables.cs                     # Готовые обёртки (DamageModifiable, SpeedModifiable, ...)
├── Comparers/
│   ├── ComparerFactory.cs             # Реестр IEqualityComparer<T>
│   └── DefaultComparers.cs            # Компараторы для Unity-типов и float/int
├── Entities/
│   ├── ModifierDelegate.cs            # delegate T ModifierDelegate<T>()
│   └── ModifierDelegateHandler.cs     # IDisposable-хэндл модификатора
├── Stages/
│   ├── Stage.cs                       # Коллекция модификаторов + StageOp
│   ├── StageOp.cs                     # delegate T StageOp<T>(T a, T b)
│   └── StageFactory/
│       ├── StageArithmetic.cs         # Реестр операций по StageOpKind
│       ├── StageArithmeticUnityBootstrap.cs  # Регистрация Unity-типов
│       ├── ModifiableFactory.cs       # Авто-создание стадий из [StageOp] атрибутов
│       ├── StageOpAttribute.cs        # [StageOp(StageOpKind)] атрибут
│       └── DefaultStages.cs          # Встроенные enum-ы стадий
└── Editor/
    └── ModifiableDrawer.cs            # PropertyDrawer для инспектора Unity
```
