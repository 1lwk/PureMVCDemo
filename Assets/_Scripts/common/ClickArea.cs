#region <<文件说明>>

/*--------------------------------------
//文件名称：ClickArea
// 创建者：李满堂
//创建时间：2022年09月12日 星期一 11:12
//文件版本：V1.0.0
//=======================================
//功能描述：
//
//
---------------------------------------*/

#endregion


using UnityEngine.UI;

public class ClickArea : Graphic
{
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
    }
}