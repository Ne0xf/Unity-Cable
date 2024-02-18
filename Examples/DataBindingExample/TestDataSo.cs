using System.Collections.Generic;
using UnityEngine;

namespace UnityCable
{
    [CreateAssetMenu(fileName = "Test_Data", menuName = "UnityCable/Test Data")]
    public class TestDataSo : DataBaseSo
    {
        public interface ITextData
        {
            public Color Color { get; set; }
            public string Text { get; set; }
            public Vector3 Pos { get; set; }
        }
        
        public class A: ITextData
        {
            public Color Color { get; set; }
            public string Text { get; set; }
            public Vector3 Pos { get; set; }
        }

        public override void InitProxy(VModel model, Dictionary<string, VModel.ModelItem> dict)
        {
            this.vModel = model;
            this.data = DataSetterProxy<ITextData>.Create(new A(), dict);
        }
        
        public override void OnAdd()
        {
            Debug.Log("TestDataSo OnAdd");
        }

        public override void OnRemove()
        {
            Debug.Log("TestDataSo OnRemove");
            if (this.vModel != null)
            {
                Destroy(this.vModel.gameObject);
            }
        }
    }
}
