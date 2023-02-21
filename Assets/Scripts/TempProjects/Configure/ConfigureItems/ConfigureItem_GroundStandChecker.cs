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
        private List<string> 地面标签 = new List<string>() { BaseData.UnityTag.地表碰撞体.ToString() };




        public override string MenuName => "角色/站立判断";

        public override Type[] RequireComponentsOnGameObject => null;



        public List<string> tags => 地面标签;

        public override ItemRunnerBase CreateRunnerOver(GameObject gameObject)
        {
            return new Runner() { gameObject = gameObject, config = this };
        }

        public class Runner : ItemRunnerBase<ConfigureItem_GroundStandChecker>
        {








            public override void OnInit()
            {
                throw new NotImplementedException();
            }
            public override void OnUnInit()
            {
                throw new NotImplementedException();
            }

            public override void OnDisable()
            {
                throw new NotImplementedException();
            }

            public override void OnEnable()
            {
                throw new NotImplementedException();
            }








        }
    }



}