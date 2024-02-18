using UnityCable;
using UnityEngine;

public class ListBindingExample: MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private Transform prefabParent;

    private string key;

    public void OnAddedButton()
    {
        var go = Instantiate(this.prefab, this.prefabParent);
        this.key = go.GetComponent<VModel>().Key;
    }

    public void OnRemovedButton()
    {
        var list = VModel.GetList(this.key);
        if (list != null && list.Count > 0)
        {
            var index = Random.Range(0, list.Count);
            VModel.RemoveList(this.key, index);
        }
    }

    public void OnClearedButton()
    {
        VModel.RemoveList(this.key);
    }
}