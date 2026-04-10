using UnityEngine;

namespace ModifiableVariable.Stages.StageFactory
{
    public static class StageArithmeticUnityBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Init()
        {
            //bool
            StageArithmetic<bool>.Register(StageOpKind.Override, (a, b) => b);
            StageArithmetic<bool>.Register(StageOpKind.Or, (a, b) => a | b);
            StageArithmetic<bool>.Register(StageOpKind.And, (a, b) => a & b);
            
            // Vector2
            StageArithmetic<Vector2>.Register(StageOpKind.Add, (a, b) => a + b);
            StageArithmetic<Vector2>.Register(StageOpKind.Subtract, (a, b) => a - b);
            StageArithmetic<Vector2>.Register(StageOpKind.Multiply, Vector2.Scale);
            StageArithmetic<Vector2>.Register(StageOpKind.Min, Vector2.Min);
            StageArithmetic<Vector2>.Register(StageOpKind.Max, Vector2.Max);
            StageArithmetic<Vector2>.Register(StageOpKind.Lerp, (a, b) => Vector2.Lerp(a, b, 0.5f));
            StageArithmetic<Vector2>.Register(StageOpKind.Override, (a, b) => b);

            // Vector2Int
            StageArithmetic<Vector2Int>.Register(StageOpKind.Add, (a, b) => a + b);
            StageArithmetic<Vector2Int>.Register(StageOpKind.Subtract, (a, b) => a - b);
            StageArithmetic<Vector2Int>.Register(StageOpKind.Multiply, (a, b) => a * b);
            StageArithmetic<Vector2Int>.Register(StageOpKind.Min, Vector2Int.Min);
            StageArithmetic<Vector2Int>.Register(StageOpKind.Max, Vector2Int.Max);
            StageArithmetic<Vector2Int>.Register(StageOpKind.Override, (a, b) => b);

            // Vector3
            StageArithmetic<Vector3>.Register(StageOpKind.Add, (a, b) => a + b);
            StageArithmetic<Vector3>.Register(StageOpKind.Subtract, (a, b) => a - b);
            StageArithmetic<Vector3>.Register(StageOpKind.Multiply, Vector3.Scale);
            StageArithmetic<Vector3>.Register(StageOpKind.Min, Vector3.Min);
            StageArithmetic<Vector3>.Register(StageOpKind.Max, Vector3.Max);
            StageArithmetic<Vector3>.Register(StageOpKind.Lerp, (a, b) => Vector3.Lerp(a, b, 0.5f));
            StageArithmetic<Vector3>.Register(StageOpKind.Override, (a, b) => b);

            // Vector3Int
            StageArithmetic<Vector3Int>.Register(StageOpKind.Add, (a, b) => a + b);
            StageArithmetic<Vector3Int>.Register(StageOpKind.Subtract, (a, b) => a - b);
            StageArithmetic<Vector3Int>.Register(StageOpKind.Multiply, (a, b) => a * b);
            StageArithmetic<Vector3Int>.Register(StageOpKind.Min, Vector3Int.Min);
            StageArithmetic<Vector3Int>.Register(StageOpKind.Max, Vector3Int.Max);
            StageArithmetic<Vector3Int>.Register(StageOpKind.Override, (a, b) => b);

            // Vector4
            StageArithmetic<Vector4>.Register(StageOpKind.Add, (a, b) => a + b);
            StageArithmetic<Vector4>.Register(StageOpKind.Subtract, (a, b) => a - b);
            //StageArithmetic<Vector4>.Register(StageOpKind.Multiply, (a, b) => a * b);
            StageArithmetic<Vector4>.Register(StageOpKind.Lerp, (a, b) => Vector4.Lerp(a, b, 0.5f));
            StageArithmetic<Vector4>.Register(StageOpKind.Min, Vector4.Min);
            StageArithmetic<Vector4>.Register(StageOpKind.Max, Vector4.Max);
            StageArithmetic<Vector4>.Register(StageOpKind.Override, (a, b) => b);

            // Color
            StageArithmetic<Color>.Register(StageOpKind.Add, (a, b) => a + b);
            StageArithmetic<Color>.Register(StageOpKind.Subtract, (a, b) => a - b);
            StageArithmetic<Color>.Register(StageOpKind.Multiply, (a, b) => a * b);
            StageArithmetic<Color>.Register(StageOpKind.Lerp, (a, b) => Color.Lerp(a, b, 0.5f));
            StageArithmetic<Color>.Register(StageOpKind.Override, (a, b) => b);

            // Color32
            StageArithmetic<Color32>.Register(StageOpKind.Lerp, (a, b) => Color32.Lerp(a, b, 0.5f));
            StageArithmetic<Color32>.Register(StageOpKind.Override, (a, b) => b);

            // Quaternion
            StageArithmetic<Quaternion>.Register(StageOpKind.Multiply, (a, b) => a * b);
            StageArithmetic<Quaternion>.Register(StageOpKind.Lerp, (a, b) => Quaternion.Slerp(a, b, 0.5f));
            StageArithmetic<Quaternion>.Register(StageOpKind.Override, (a, b) => b);

            // Rect
            StageArithmetic<Rect>.Register(StageOpKind.Override, (a, b) => b);
            StageArithmetic<Rect>.Register(StageOpKind.Add,
                (a, b) => new Rect(a.x + b.x, a.y + b.y, a.width + b.width, a.height + b.height));
            StageArithmetic<Rect>.Register(StageOpKind.Min,
                (a, b) => new Rect(Mathf.Min(a.x, b.x), Mathf.Min(a.y, b.y), Mathf.Min(a.width, b.width),
                    Mathf.Min(a.height, b.height)));
            StageArithmetic<Rect>.Register(StageOpKind.Max,
                (a, b) => new Rect(Mathf.Max(a.x, b.x), Mathf.Max(a.y, b.y), Mathf.Max(a.width, b.width),
                    Mathf.Max(a.height, b.height)));

            // Bounds
            StageArithmetic<Bounds>.Register(StageOpKind.Override, (a, b) => b);
            StageArithmetic<Bounds>.Register(StageOpKind.Add,
                (a, b) => new Bounds(a.center + b.center, a.size + b.size));
            StageArithmetic<Bounds>.Register(StageOpKind.Max, (a, b) =>
            {
                var r = a;
                r.Encapsulate(b);
                return r;
            });

            // BoundsInt
            StageArithmetic<BoundsInt>.Register(StageOpKind.Override, (a, b) => b);
            StageArithmetic<BoundsInt>.Register(StageOpKind.Add,
                (a, b) => new BoundsInt(a.position + b.position, a.size + b.size));

            // Matrix4x4
            StageArithmetic<Matrix4x4>.Register(StageOpKind.Multiply, (a, b) => a * b);
            StageArithmetic<Matrix4x4>.Register(StageOpKind.Override, (a, b) => b);
        }
    }
}