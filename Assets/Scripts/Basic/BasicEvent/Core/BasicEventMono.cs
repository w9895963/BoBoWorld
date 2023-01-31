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

            private List<(int index, Action action)> actions = new List<(int index, Action action)>();
            private Dictionary<Action, int> actionsDict = new Dictionary<Action, int>();
            protected bool isOnRunAction = false;
            protected Action delayAddOrRemoveAction = null;





            //方法：运行action
            public void RunAction()
            {
                isOnRunAction = true;
                foreach (var item in actions)
                {
                    item.action?.Invoke();
                }
                isOnRunAction = false;
                delayAddOrRemoveAction?.Invoke();
                delayAddOrRemoveAction = null;
            }
            ///<summary> 添加方法,根据index添加到队列中的合适位置 </summary>
            public void AddActionIfNotExist(Action action, int index = 0)
            {
                if (isOnRunAction)
                {
                    delayAddOrRemoveAction += MainAction;
                    return;
                }
                else
                {
                    MainAction();
                }

                void MainAction()
                {
                    //找到队列中的序号,没有则新建
                    List<(int index, Action action)> acs = actions;
                    int i = acs.FindIndex((item) => item.index == index);
                    if (i == -1)
                    {
                        acs.Add((index, null));
                        acs.SortBy((item) => item.index);
                        i = acs.FindIndex((item) => item.index == index);
                    }

                    //存在则不作为,不存在则添加
                    if (!actionsDict.ContainsKey(action))
                    {
                        actionsDict.Add(action, i);
                        acs[i] = (index, acs[i].action + action);
                    }
                }

            }
            ///<summary> 移除方法 </summary>
            public void RemoveAction(Action action)
            {
                if (isOnRunAction)
                {
                    delayAddOrRemoveAction += MainAction;
                    return;
                }
                else
                {
                    MainAction();
                }

                void MainAction()
                {
                    Dictionary<Action, int> actionsD = actionsDict;
                    List<(int index, Action action)> acts = actions;
                    if (actionsD.TryRemove(action, out var item))
                    {
                        int i = acts.FindIndex((x) => x.index == item.value);
                        acts[i] = (item.value, acts[i].action - action);
                    }
                }

            }





        }


        public class BasicEventMono<T> : BasicEventMono
        {
            private List<(int index, Action<T> action)> actions = new List<(int index, Action<T> action)>();
            private Dictionary<Action<T>, int> actionsDict = new Dictionary<Action<T>, int>();


            public void AddActionIfNotExist(Action<T> action, int index = 0)
            {
                if (isOnRunAction)
                {
                    delayAddOrRemoveAction += MainAction;
                    return;
                }
                else
                {
                    MainAction();
                }

                void MainAction()
                {
                    //找到队列中的序号,没有则新建
                    List<(int index, Action<T> action)> acs = actions;
                    int i = acs.FindIndex((item) => item.index == index);
                    if (i == -1)
                    {
                        acs.Add((index, null));
                        acs.SortBy((item) => item.index);
                        i = acs.FindIndex((item) => item.index == index);
                    }

                    //存在则不作为,不存在则添加
                    Dictionary<Action<T>, int> acsD = actionsDict;
                    if (!acsD.ContainsKey(action))
                    {
                        acsD.Add(action, i);
                        acs[i] = (index, acs[i].action + action);
                    }
                }

            }
            ///<summary> 移除方法 </summary>
            public void RemoveAction(Action<T> action)
            {
                if (isOnRunAction)
                {
                    delayAddOrRemoveAction += MainAction;
                    return;
                }
                else
                {
                    MainAction();
                }

                void MainAction()
                {
                    Dictionary<Action<T>, int> actionsD = actionsDict;
                    List<(int index, Action<T> action)> acts = actions;
                    if (actionsD.TryRemove(action, out var item))
                    {
                        int i = acts.FindIndex((x) => x.index == item.value);
                        acts[i] = (item.value, acts[i].action - action);
                    }
                }

            }

            //方法：运行action,覆盖RunAction
            public void RunAction(T date)
            {
                isOnRunAction = true;
                foreach (var item in actions)
                {
                    item.action?.Invoke(date);
                }
                isOnRunAction = false;
                delayAddOrRemoveAction?.Invoke();
                delayAddOrRemoveAction = null;
            }


        }
    }


}
