using System.Collections.Generic;
using UnityCable;
using UnityEngine;

[CreateAssetMenu(fileName = "Test_ListData", menuName = "UnityCable/Test List Data")]
public class TestArrayDataSo : DataBaseSo
{
    
    public interface ISlotData
    {
        public bool IconEnable { get; set; }
        public Sprite IconSprite { get; set; }
        public string TMPText { get; set; }
    }
        
    public class TestSlot: ISlotData
    {
        public bool IconEnable { get; set; }
        public Sprite IconSprite { get; set; }
        public string TMPText { get; set; }
    }
    
    public override void InitProxy(VModel model, Dictionary<string, VModel.ModelItem> dict)
    {
        this.vModel = model;
        this.data = DataSetterProxy<ISlotData>.Create(new TestSlot(), dict);
    }

    public override void OnAdd()
    {
        Debug.Log("TestArrayDataSo OnAdd");
    }

    public override void OnRemove()
    {
        Debug.Log("TestArrayDataSo OnRemove");
        if (this.vModel != null)
        {
            Destroy(this.vModel.gameObject);
        }
    }
}
