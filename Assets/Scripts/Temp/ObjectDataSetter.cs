using System;
using System.Collections;
using System.Collections.Generic;
using EventDataS;
using UnityEngine;







public class ObjectDataSetter : MonoBehaviour
{
    //物体标签组
    public EventDataName.ObjectTag[] objectTags = new EventDataName.ObjectTag[0];





    void Enable()
    {   //如果物体标签组不为空
        if (objectTags.Length > 0)
        {   //将物体标签组添加到事件数据
            EventDataF.SetData_local(gameObject, EventDataName.ObjectData.物体标签组, objectTags);
        }


    }








}


namespace ObjectTag
{
    //抽象类
    public class Abstract
    {
        public Action enable;
        public Action disable;
    }


    //玩家
    public class Player : Abstract
    {
       
    }




}
