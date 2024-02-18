using UnityCable;
using UnityEngine;

public class DataBindingExample: MonoBehaviour
{
    private TestDataSo.ITextData data;

    private Vector3 end = new Vector3(8, 0);

    private void Start()
    {
        var so = VModel.GetDataSo("test");
        this.data = so.GetData<TestDataSo.ITextData>();
    }

    private void FixedUpdate()
    {
        if (this.data == null) return;
        if (this.data.Pos.x > end.x - 1) this.data.Pos = new Vector3(-9, 0);
        if (this.data.Color.r > 0.9f) this.data.Color = Color.blue;

        this.data.Text = Time.time.ToString("F2");
        this.data.Pos = Vector3.Lerp(this.data.Pos, end, Time.fixedDeltaTime);
        this.data.Color = Color.Lerp(this.data.Color, Color.yellow, Time.fixedDeltaTime);
    }
}