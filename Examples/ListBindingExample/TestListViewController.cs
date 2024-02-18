using UnityCable;
using UnityEngine;

public class TestListViewController : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;

    private TestArrayDataSo.ISlotData data;
    
    void Start()
    {
        
        var vm = GetComponent<VModel>();
        // If "VModel.auto" is not enabled.
        // Please call "VModel.BuildCallCache()".
        var so = vm.BuildCallCache();
        this.data = so.GetData<TestArrayDataSo.ISlotData>();
        this.data.IconEnable = true;
        this.data.IconSprite = sprites[Random.Range(0, sprites.Length)];
        this.data.TMPText = Random.Range(0, 100).ToString();
    }

    public void OnButton()
    {
        Debug.Log($"TMPText: {this.data.TMPText}, IconSprite: {this.data.IconSprite.name}");
    }

}
