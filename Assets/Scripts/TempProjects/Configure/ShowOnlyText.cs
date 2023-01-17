using System;
using UnityEngine;


//命名空间：配置
namespace Configure
{

    [Serializable]
    public class ShowOnlyText
    {
        [Multiline(5)]
        public string 脚本说明;


        public ShowOnlyText(params string[] texts)
        {
            this.脚本说明 = string.Join("\n", texts);
        }


    }




}

