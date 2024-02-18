using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace UnityCable
{
    public class VModel: MonoBehaviour
    {
        [Serializable]
        public class ModelItem
        {
            /// <summary>
            /// unity component
            /// </summary>
            public UnityEngine.Object component;

            /// <summary>
            /// unity component field name
            /// </summary>
            public string componentField;

            /// <summary>
            /// data interface field name
            /// </summary>
            public string dataField;

            [NonSerialized]
            public Delegate fieldSetter;

            public void Call(object value)
            {
                if (this.fieldSetter == null || this.component == null) return;
                this.fieldSetter.DynamicInvoke(component, value);
            }
        }

        [SerializeField]
        private string key;

        public string Key => this.key;

        [SerializeField]
        private bool auto = true;

        [SerializeField]
        private bool array = false;

        [SerializeField]
        private DataBaseSo bindData;

        [SerializeField]
        private ModelItem[] modelConfig;

        private static Dictionary<string, DataBaseSo> _data = new();
        private static Dictionary<string, List<DataBaseSo>> _listData = new();

        // [NonSerialized]
        // public System.Object proxy;

        private void Awake()
        {
            if (this.auto)
                this.BuildCallCache();
        }

        [ContextMenu("TEST")]
        public void T()
        {
            // var a = DispatchProxy.Create(typeof(TestDataSo.A), typeof(TestDataSo.ITextData));
            // Debug.Log(a);
            Debug.Log(1111);
        }

        public DataBaseSo BuildCallCache()
        {
            if (this.bindData == null) throw new Exception($"bindData is NULL");
            if (string.IsNullOrEmpty(this.key)) throw new Exception($"key is NULL or EMPTY");
            if (this.modelConfig.Length <= 0)
            {
                Debug.LogWarning("The ModelConfig length is 0");
                return null;
            }

            var data = Instantiate(this.bindData);

            Dictionary<string, ModelItem> callDictionary = new();
            for (int i = 0; i < this.modelConfig.Length; i++)
            {
                var cd = this.modelConfig[i];
                this.TryBuildCall(cd, callDictionary);
            }

            data.InitProxy(this, callDictionary);

            if (this.array)
            {
                AddList(this.key, data);
            }
            else
            {
                AddDataSo(this.key, data);
            }

            return data;
        }

        private void TryBuildCall(ModelItem modelItem, Dictionary<string, ModelItem> dict)
        {
            if (modelItem.component == null) throw new Exception($"modelConfig[].component is NULL");
            if (string.IsNullOrEmpty(modelItem.dataField)) throw new Exception("modelConfig[].dataField is NULL");
            if (string.IsNullOrEmpty(modelItem.componentField)) throw new Exception("modelConfig[].componentField is NULL");

            var com = modelItem.component;

            Type handlerType = com.GetType();
            PropertyInfo property = handlerType.GetProperty(modelItem.componentField);
            if (property != null)
            {
                ParameterExpression targetExp = Expression.Parameter(handlerType);
                ParameterExpression valueExp = Expression.Parameter(property.PropertyType);
                MemberExpression propExp = Expression.Property(targetExp, property);
                BinaryExpression assignExp = Expression.Assign(propExp, valueExp);
                Delegate setter = Expression.Lambda(assignExp, targetExp, valueExp).Compile();
                modelItem.fieldSetter = setter;
                dict[$"set_{modelItem.dataField}"] = modelItem;
            }
            else
            {
                throw new Exception($"There is no {handlerType.FullName}.{modelItem.componentField}");
            }
        }

        #region static Data CURD

        public static void AddDataSo(string key, DataBaseSo so)
        {
            if (string.IsNullOrEmpty(key)) return;
            _data.TryAdd(key, so);
            so.OnAdd();
        }

        public static void RemoveDataSo(string key)
        {
            if (string.IsNullOrEmpty(key)) return;
            var so = GetDataSo(key);
            if (so != null)
            {
                so.OnRemove();
            }

            _data.Remove(key);
        }

        public static DataBaseSo GetDataSo(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            DataBaseSo so = null;
            _data.TryGetValue(key, out so);
            return so;
        }

        public static void AddList(string key, DataBaseSo so)
        {
            if (string.IsNullOrEmpty(key)) return;
            List<DataBaseSo> list;
            if (_listData.TryGetValue(key, out list))
            {
                list.Add(so);
            }
            else
            {
                _listData.TryAdd(key, new List<DataBaseSo>() { so });
            }

            so.OnAdd();
        }

        public static void RemoveList(string key, DataBaseSo so)
        {
            if (string.IsNullOrEmpty(key)) return;
            List<DataBaseSo> list;
            if (_listData.TryGetValue(key, out list))
            {
                list.Remove(so);
                so.OnRemove();
            }
        }

        public static void RemoveList(string key, int index)
        {
            if (string.IsNullOrEmpty(key)) return;
            List<DataBaseSo> list;
            if (_listData.TryGetValue(key, out list))
            {
                var so = list[index];
                list.Remove(so);
                so.OnRemove();
            }
        }

        public static void RemoveList(string key)
        {
            if (string.IsNullOrEmpty(key)) return;
            var list = GetList(key);
            if (list != null)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].OnRemove();
                }
            }

            _listData.Remove(key);
        }

        public static List<DataBaseSo> GetList(string key)
        {
            if (string.IsNullOrEmpty(key)) return null;
            List<DataBaseSo> list = null;
            _listData.TryGetValue(key, out list);
            return list;
        }

        #endregion
    }
}