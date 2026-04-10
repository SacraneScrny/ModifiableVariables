using System;
using System.Collections.Generic;

using ModifiableVariable.Comparers;
using ModifiableVariable.Entities;
using ModifiableVariable.Stages;
using ModifiableVariable.Stages.StageFactory;

using UnityEngine;

namespace ModifiableVariable
{
    [Serializable]
    public class Modifiable<T, TStage>
        where TStage : Enum
    {
        [SerializeField] T _baseValue;
        public T BaseValue
        {
            get => _baseValue;
            set
            {
                if (!_comparer.Equals(_baseValue, value))
                {
                    _baseValue = value;
                    _cachedFrame = -1;
                } 
            }
        }

        readonly Dictionary<int, Stage<T>> _stageMap = new();
        T _cachedValue;
        T _defaultValue;
        bool _hasInited;
        bool _modifiersHasChanged;
        int _cachedCount;
        int _cachedFrame = -1;
        
        IEqualityComparer<T> _comparer;
        readonly List<Stage<T>> _stages = new();
        public IReadOnlyList<Stage<T>> Stages => _stages;
        
        public Modifiable(T baseValue, params (TStage, StageOp<T>)[] stages)
        {
            _comparer = ComparerFactory.Get<T>();
            _baseValue = baseValue;
            for (var i = 0; i < stages.Length; i++)
            {
                AddStage(stages[i]);
            }
        }
        public Modifiable(T baseValue)
        {
            _comparer = ComparerFactory.Get<T>();
            _baseValue = baseValue;
            ModifiableFactory.TryPopulate(this);
        }
        public Modifiable<T, TStage> WithComparer(IEqualityComparer<T> comparer)
        {
            _comparer = comparer;
            return this;
        }
        public Modifiable<T, TStage> AddStage(StageOp<T> op, TStage stage)
        {
            var s = new Stage<T>(op);
            _stages.Add(s);
            _stageMap[ToInt(stage)] = s;
            
            _modifiersHasChanged = true;
            return this;
        }        
        public Modifiable<T, TStage> AddStage((TStage, StageOp<T>) stage)
        {
            AddStage(stage.Item2, stage.Item1);
            return this;
        }
        
        #if UNITY_EDITOR
        public T GetValueEditor()
        {
            var result = Calculate();
            return result;
        }
        #endif
        public T GetValue()
        {
            if (!_hasInited)
            {
                _defaultValue = _baseValue;
                _hasInited = true;
            }

            UpdateCountIfDirty();
            if (!IsFrameDirty())
                return _cachedValue;

            var value = Calculate();
            if (!_comparer.Equals(_cachedValue, value))
            {
                _cachedValue = value;
                ValueChanged?.Invoke(value);
            }
            return value;
        }
         
        public ModifierDelegateHandler<T> Add(ModifierDelegate<T> modifier, TStage stage = default)
        {
            _modifiersHasChanged = true;
            return GetStage(stage)?.Add(modifier) ?? default;
        }       
        
        public bool Remove(ModifierDelegate<T> modifier, TStage stage)
        {
            _modifiersHasChanged = true;
            return GetStage(stage)?.Remove(modifier) ?? false;
        }
        public bool Remove(ModifierDelegateHandler<T> handler, TStage stage)
        {
            _modifiersHasChanged = true;
            return Remove(handler.Modifier, stage);
        }
        
        public bool Remove(ModifierDelegate<T> modifier)
        {
            _modifiersHasChanged = true;
            for (var i = 0; i < _stages.Count; i++)
            {
                if (_stages[i].Remove(modifier))
                {
                    return true;
                }
            }
            return false;
        }        
        public bool Remove(ModifierDelegateHandler<T> modifier)
        {
            _modifiersHasChanged = true;
            for (var i = 0; i < _stages.Count; i++)
            {
                if (_stages[i].Remove(modifier))
                {
                    return true;
                }
            }
            return false;
        }

        public event Action<T> ValueChanged;
        
        public void Clear()
        {
            _cachedFrame = -1;
            for (int i = 0; i < _stages.Count; i++)
                _stages[i].Clear();
            if (_hasInited)
                _baseValue = _defaultValue;
        }

        T Calculate()
        {
            var value = _baseValue;
            for (var i = 0; i < _stages.Count; i++)
            {
                var stage = _stages[i];
                value = stage.Proceed(value);
            }
            return value;
        }
        bool IsFrameDirty()
        {
            var frame = _cachedFrame;
            _cachedFrame = Time.frameCount;
            return frame != _cachedFrame;
        }
        void UpdateCountIfDirty()
        {
            if (_modifiersHasChanged)
            {
                _modifiersHasChanged = false;
                _cachedCount = 0;
                for (var i = 0; i < _stages.Count; i++)
                    _cachedCount += _stages[i].Modifiers.Count;
            }
        }
        Stage<T> GetStage(TStage key) => !_stageMap.TryGetValue(ToInt(key), out var stage) ? _stages[0] : stage;
        static int ToInt(TStage key) => (int)(object)key;
        static TStage ToStage(int key) => (TStage)(object)key;
        
        public static implicit operator T (Modifiable<T, TStage> obj)
        {
            return obj.GetValue();
        }        
        public static implicit operator Modifiable<T, TStage> (T obj)
        {
            return new Modifiable<T, TStage>(obj);
        }

        public int Count
        {
            get
            {
                UpdateCountIfDirty();
                return _cachedCount;
            }
        }
    }
}