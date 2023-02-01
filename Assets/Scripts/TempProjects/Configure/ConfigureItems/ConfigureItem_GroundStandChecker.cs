using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Configure;
using UnityEngine;

namespace Configure.ConfigureItems
{
    //类定义:一个用于判断角色是否站在地面上的判断组件
    public class ConfigureItem_GroundStandChecker : ConfigureItemBase
    {
        [SerializeField]
        private List<string> 地面标签 = new List<string>() { UnityTag.地表碰撞体.ToString() };




        public override string MenuName => "角色/站立判断";

        public override Type[] RequireComponents => null;



        public List<string> tags => 地面标签;








        public class Runner : ItemRunnerBase
        {

            // private List<string> tags => config.tags;







            public override void Init()
            {
                throw new NotImplementedException();
            }
            public override void Destroy()
            {
                throw new NotImplementedException();
            }

            public override void Disable()
            {
                throw new NotImplementedException();
            }

            public override void Enable()
            {
                throw new NotImplementedException();
            }








        }
    }



}