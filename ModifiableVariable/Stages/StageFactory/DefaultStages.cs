namespace ModifiableVariable.Stages.StageFactory
{    
    public enum GateDisjunction
    {
        [StageOp(StageOpKind.Or)] DisjunctionState,
        [StageOp(StageOpKind.Override)] Override,
    }
    public enum GateConjunction
    {
        [StageOp(StageOpKind.And)] ConjunctionState,
        [StageOp(StageOpKind.Override)] Override,
    }
    public enum GateGeneral
    {
        [StageOp(StageOpKind.Or)] DisjunctionState,
        [StageOp(StageOpKind.And)] ConjunctionState,
        [StageOp(StageOpKind.Override)] Override,
    }
    public enum GateComplex
    {
        [StageOp(StageOpKind.Or)] DisjunctionState,
        [StageOp(StageOpKind.And)] ConjunctionState,
        [StageOp(StageOpKind.Or)] LastDisjunctionState,
        [StageOp(StageOpKind.Override)] Override,
    }
    
    public enum Simple
    {
        [StageOp(StageOpKind.Add)] Flat,
    }

    public enum General
    {
        [StageOp(StageOpKind.Add)] Flat,
        [StageOp(StageOpKind.Multiply)] Multiply,
    }

    public enum Complex
    {
        [StageOp(StageOpKind.Add)] Flat,
        [StageOp(StageOpKind.Multiply)] Multiply,
        [StageOp(StageOpKind.Add)] Post,
    }

    public enum Damage
    {
        [StageOp(StageOpKind.Add)] Flat,
        [StageOp(StageOpKind.Multiply)] Multiply,
        [StageOp(StageOpKind.Subtract)] Penetration,
        [StageOp(StageOpKind.Min)] Cap,
    }

    public enum Defense
    {
        [StageOp(StageOpKind.Add)] Flat,
        [StageOp(StageOpKind.Multiply)] Multiply,
        [StageOp(StageOpKind.Min)] Cap,
    }

    public enum Speed
    {
        [StageOp(StageOpKind.Add)] Flat,
        [StageOp(StageOpKind.Multiply)] Multiply,
        [StageOp(StageOpKind.Max)] Min,
        [StageOp(StageOpKind.Min)] Max,
    }

    public enum Resource
    {
        [StageOp(StageOpKind.Add)] Flat,
        [StageOp(StageOpKind.Multiply)] Multiply,
        [StageOp(StageOpKind.Add)] Regen,
        [StageOp(StageOpKind.Min)] Cap,
    }

    public enum Chance
    {
        [StageOp(StageOpKind.Add)] Flat,
        [StageOp(StageOpKind.Multiply)] Multiply,
        [StageOp(StageOpKind.Min)] Cap,
    }

    public enum Cooldown
    {
        [StageOp(StageOpKind.Subtract)] Reduction,
        [StageOp(StageOpKind.Multiply)] Multiply,
        [StageOp(StageOpKind.Max)] Floor,
    }

    public enum ColorModificator
    {
        [StageOp(StageOpKind.Multiply)] Tint,
        [StageOp(StageOpKind.Add)] Overlay,
        [StageOp(StageOpKind.Override)] Override,
    }

    public enum Position
    {
        [StageOp(StageOpKind.Add)] Offset,
        [StageOp(StageOpKind.Multiply)] Scale,
        [StageOp(StageOpKind.Override)] Override,
    }

    public enum Rotation
    {
        [StageOp(StageOpKind.Multiply)] Multiply,
        [StageOp(StageOpKind.Override)] Override,
    }

    public enum Overridable
    {
        [StageOp(StageOpKind.Override)] Override,
    }

    public enum Blend
    {
        [StageOp(StageOpKind.Add)] Offset,
        [StageOp(StageOpKind.Lerp)] Lerp,
        [StageOp(StageOpKind.Override)] Override,
    }
}