#region << 版 本 注 释 >>

/*----------------------------------------------------------------
 * 版权所有 (c) 2022  NJRN 保留所有权利。
 * CLR版本：4.0.30319.42000
 * 机器名称：DESKTOP-DUEGB5U
 * 公司名称：
 * 命名空间：Assets._Scripts
 * 唯一标识：c13b7527-9930-4e14-8580-167e9abfabf0
 * 文件名：MyApp
 * 当前用户域：DESKTOP-DUEGB5U
 * 
 * 创建者：lmt
 * 电子邮箱：3176594@qq.com
 * 创建时间：2022/9/12 10:07:57
 * 版本：V1.0.0
 * 描述：
 *
 * ----------------------------------------------------------------
 * 修改人：
 * 时间：
 * 修改说明：
 *
 * 版本：V1.0.1
 *----------------------------------------------------------------*/

#endregion << 版 本 注 释 >>

using UnityEngine;


public class MyApp : MonoBehaviour
{
    [SerializeField]
    private GameObject ui;
    private void Start()
    {
        //LMT_Facade.GetInstance().Launch(this.ui);
        //LMT_Facade.GetInstance().Launch(this.ui);
        LWK_Facade.GetInstance().Launch(Instantiate(Resources.Load<GameObject>("TestPanel"),this.ui.transform));
    }
}