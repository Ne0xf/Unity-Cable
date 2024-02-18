using System.Collections.Generic;
using UnityEngine;

namespace UnityCable
{
    public abstract class DataBaseSo: ScriptableObject
    {
        protected VModel vModel;
        protected System.Object data;
        public abstract void InitProxy(VModel model, Dictionary<string, VModel.ModelItem> dict);

        public virtual void OnAdd()
        {
            
        }
        
        public virtual void OnRemove()
        {
        }
        
        public virtual T GetData<T>()
        {
            if (this.data is T tdata)
            {
                return tdata;
            }
            else
            {
                throw new System.Exception("data is NULL or type mismatch");
            }
        }
    }
}