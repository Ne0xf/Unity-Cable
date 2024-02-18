using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace UnityCable
{
    public class DataSetterProxy<T>: DispatchProxy
    {
        private const string withKey = "set";
        private static readonly string[] blacklist = { "get_model", "set_model" };
        private T Target { get; set; }
        private Dictionary<string, VModel.ModelItem> callDictionary;

        public static T Create(T decorated, Dictionary<string, VModel.ModelItem> dict)
        {
            Type tType = typeof (T);
            if (!tType.IsInterface)
            {
                throw new Exception($"\"{tType.FullName}\" Must be an interface");
            }

            object gen = Create<T, DataSetterProxy<T>>();
            DataSetterProxy<T> proxy = (DataSetterProxy<T>)gen;
            proxy.SetParameters(decorated, dict);
            T d = (T)gen;
            return d;
        }

        private void SetParameters(T decorated, Dictionary<string, VModel.ModelItem> dict)
        {
            Target = decorated;
            this.callDictionary = dict;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            try
            {
                object result = targetMethod.Invoke(Target, args);

                for (int i = 0; i < blacklist.Length; i++)
                {
                    if (targetMethod.Name == blacklist[i])
                    {
                        return result;
                    }
                }

                if (targetMethod.Name.StartsWith(withKey))
                {
                    if (this.callDictionary.TryGetValue(targetMethod.Name, out VModel.ModelItem modelItem))
                    {
                        modelItem.Call(args[0]);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }

            return null;
        }
    }
}