using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace BasicEvent
{
    namespace Component
    {
        public class BasicEventMono : MonoBehaviour
        {

            private Action action = null;
            private Action runAction;
            protected List<Delegate> actions = new List<Delegate>();
            protected List<(int index, Delegate action)> actionsWithIndex = new List<(int index, Delegate action)>();


            public BasicEventMono()
            {
                runAction = runAction_default;
            }


            //方法：默认的运行方法
            private void runAction_default()
            {
                action?.Invoke();
            }
            //方法：修改后的运行方法
            private void runAction_changed()
            {
                //~重新生成排序后的action
                action = null;
                //遍历
                foreach (var item in actionsWithIndex)
                {
                    action += item.action as Action;
                }
                //运行
                action?.Invoke();

                runAction = runAction_default;
            }

            //方法：运行action
            public void RunAction()
            {
                runAction.Invoke();
            }
            ///<summary> 添加方法,根据index添加到队列中的合适位置 </summary>
            public void AddActionAndSort(Delegate action, int index = 0)
            {
                //找到队列中第一个index大于当前index的位置
                int i = actionsWithIndex.FindIndex((item) => item.index > index);
                //如果没有找到
                if (i == -1)
                {
                    //添加到队列末尾
                    actionsWithIndex.Add((index, action));
                }
                else
                {
                    //插入到队列中
                    actionsWithIndex.Insert(i, (index, action));
                }
                runAction = runAction_changed;
            }
            ///<summary> 移除方法 </summary>
            public void RemoveAction(Delegate action)
            {
                actionsWithIndex.RemoveAll((item) => item.action == action);
                runAction = runAction_changed;
            }
            //方法：存在action
            public bool HasAction(Delegate action)
            {
                return actionsWithIndex.Exists((item) => item.action == action);
            }



            private bool destroyed = false;
            //方法：删除
            public void Destroy()
            {
                Destroy(this);
                destroyed = true;
            }
            //方法：是否删除
            public bool IsDestroyed()
            {
                return destroyed;
            }



        }


        public class BasicEventMono<T> : BasicEventMono
        {
            private Action<T> action = null;
            private Action<T> runAction;

            public BasicEventMono()
            {
                runAction = runAction_default;
            }

            //方法：默认的运行方法
            private void runAction_default(T date)
            {
                action?.Invoke(date);
            }
            //方法：修改后的运行方法
            private void runAction_changed(T date)
            {
                //~重新生成排序后的action
                action = null;
                //遍历
                foreach (var item in actionsWithIndex)
                {
                    action += item.action as Action<T>;
                }
                //运行
                action?.Invoke(date);

                runAction = runAction_default;
            }

            //方法：运行action,覆盖RunAction
            public void RunAction(T date)
            {
                runAction?.Invoke(date);
            }

        }
    }


}
