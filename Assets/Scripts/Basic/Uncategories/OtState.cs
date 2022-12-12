using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Bolt;
using OtStateUtility;
using UnityEngine;
using static CommonFunction.Static;

public class OtState
{
  
    
  

    

    public abstract class State
    {
        public abstract GameObject GameObject { get; }
        public abstract string Name { get; }
        public abstract bool Enabled { get; set; }

        public abstract void AddFunction(Action<bool> function);
        public abstract void RemoveFunction(Action<bool> function);
        


    }
    public class Condition
    {
        private string[] statesAllOn = new string[0];
        private string[] statesAllOff = new string[0];
        public Func<bool> customCheck;
        private string[] toTrunOn = new string[0];
        private string[] toTrunOff = new string[0];
        public int order = 0;

        public static void Create(GameObject gameObject, Action<Condition> setCondition)
        {
            OtStateUtility.StateBase.CreateCondition(gameObject, setCondition);
        }

        //*Region  ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        #region            Fields Set Method

        public string[] StatesAllOn => statesAllOn;

        public string[] StatesAllOff => statesAllOff;
        public string[] ToTrunOn => toTrunOn;
        public string[] ToTrunOff => toTrunOff;

        public void SetStatesAllOn(params string[] names)
        {
            statesAllOn = names;
        }
        public void SetStatesAllOn(params System.Enum[] names)
        {
            statesAllOn = names.Select((n) => n.ToString()).ToArray();
        }
        public void SetStatesAllOff(params string[] names)
        {
            statesAllOff = names;
        }
        public void SetStatesAllOff(params System.Enum[] names)
        {
            statesAllOff = names.Select((n) => n.ToString()).ToArray();
        }
        public void SetToTrunOn(params string[] names)
        {
            toTrunOn = names;
        }
        public void SetToTrunOn(params System.Enum[] names)
        {
            toTrunOn = names.Select((n) => n.ToString()).ToArray();
        }
        public void SetToTrunOff(params string[] names)
        {
            toTrunOff = names;
        }
        public void SetToTrunOff(params System.Enum[] names)
        {
            toTrunOff = names.Select((n) => n.ToString()).ToArray();
        }

        #endregion
        //*Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
    }



    private State state;
    public OtState(GameObject gameObject, System.Enum name)
    {
        state = StateBase.GetOrCreate(gameObject, name.ToString());
    }
    public static OtState GetOrCreate(GameObject gameObject, System.Enum name) => new OtState(gameObject, name);




    
    public bool Enabled { get => state.Enabled; set => state.Enabled = value; }
    public void AddFunction(Action<bool> function) => state.AddFunction(function);
    public void AddFunction(Action<bool> function, ref Action disableAction)
    {
        AddFunction(function);
        disableAction += () => RemoveFunction(function);
    }
    public void RemoveFunction(Action<bool> function) => state.RemoveFunction(function);



}







namespace OtStateUtility
{
    public class StateBase : OtState.State
    {
        public static Dictionary<GameObject, List<StateBase>> allState = new Dictionary<GameObject, List<StateBase>>();


        private GameObject gameObject;
        private string name;
        private bool enabled = false;
        private Action<bool> function;
        private List<Condition> conditionsToCheck = new List<Condition>();



        private StateBase(GameObject gameObject, string name)
        {
            this.gameObject = gameObject;
            this.name = name;
        }

        public static StateBase GetOrCreate(GameObject gameObject, string name)
        {
            List<StateBase> sts = allState.GetOrCreate(gameObject);
            StateBase st = sts.Find((s) => s.name == name);
            if (st == null)
            {
                st = new StateBase(gameObject, name);
                sts.Add(st);
                sts.Sort((s, d) => s.name.CompareTo(d.name));
            }
            return st;
        }



        //*Region  ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
        #region            Overide      

        public override GameObject GameObject => gameObject;

        public override string Name => name;

        public override bool Enabled
        {
            get => enabled;
            set
            {
                if (enabled == value) return;
                enabled = value;
                ConditionCheck();
                ActionF.QueueAction(() => function?.Invoke(enabled));

            }
        }


        public override void AddFunction(Action<bool> function)
        {
            this.function += function;
        }
        public override void RemoveFunction(Action<bool> function)
        {
            this.function -= function;
        }


        #endregion
        //*Region  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑




        public static Condition CreateCondition(GameObject gameObject, Action<OtState.Condition> setCondition)
        {
            OtState.Condition conStr = new OtState.Condition();
            setCondition(conStr);
            Condition con = Condition.Convert(gameObject, conStr);
            con.statesAllOn.ForEach((st) => st.conditionsToCheck.AddNotHas(con));
            con.statesAllOff.ForEach((st) => st.conditionsToCheck.AddNotHas(con));
            return con;
        }
        public static Condition CreateCondition(GameObject gameObject, Action<OtState.Condition> setCondition, ref Action disableACtion)
        {
            Condition con = CreateCondition(gameObject, setCondition);
            disableACtion += () => con.DestroyCondition();
            return con;
        }




        private void ConditionCheck()
        {
            List<(StateBase, int)> toTurnOn = new List<(StateBase, int)>();
            List<(StateBase, int)> toTurnOff = new List<(StateBase, int)>();
            conditionsToCheck.ForEach((con) =>
            {
                bool allOn = con.statesAllOn.All((st) => st.Enabled == true) | con.statesAllOn.Count() == 0;
                bool allOff = con.statesAllOff.All((st) => st.Enabled == false) | con.statesAllOff.Count() == 0;
                bool customCheck = con.customCheck == null ? true : con.customCheck.Invoke();
                bool test = allOn & allOff & customCheck;
                if (con.statesAllOn.Count() == 0 & con.statesAllOff.Count() == 0 & con.customCheck == null)
                    test = false;
                if (test)
                {
                    toTurnOn.AddRange(con.toTrunOn.Select((st) => (st, con.order)));
                    toTurnOff.AddRange(con.toTrunOff.Select((st) => (st, con.order)));
                }
            });



            GetList(toTurnOn).ForEach((st) => st.Enabled = true);
            GetList(toTurnOff).ForEach((st) => st.Enabled = false);



            static IEnumerable<StateBase> GetList(List<(StateBase, int)> source) //得到每个列表里每种的最后一个单位
            {
                IEnumerable<StateBase> re = source.Select((st) => st.Item1).Distinct();
                re = re.Select((st) => source.Where((sta) => sta.Item1 == st).OrderBy((i) => i.Item2).Last().Item1);
                return re;
            }

        }
        public class Condition
        {
            public StateBase[] statesAllOn;
            public StateBase[] statesAllOff;
            public Func<bool> customCheck;
            public StateBase[] toTrunOn;
            public StateBase[] toTrunOff;
            public int order = 0;


            public void DestroyCondition()
            {
                statesAllOn.ForEach((st) => st.conditionsToCheck.Remove(this));
                statesAllOff.ForEach((st) => st.conditionsToCheck.Remove(this));
            }

            public static Condition Convert(GameObject gameObject, OtState.Condition condition)
            {
                Condition con = new Condition();
                con.statesAllOn = condition.StatesAllOn.Select((n) => GetOrCreate(gameObject, n)).ToArray();
                con.statesAllOff = condition.StatesAllOff.Select((n) => GetOrCreate(gameObject, n)).ToArray();
                con.customCheck = condition.customCheck;
                con.toTrunOn = condition.ToTrunOn.Select((n) => GetOrCreate(gameObject, n)).ToArray();
                con.toTrunOff = condition.ToTrunOff.Select((n) => GetOrCreate(gameObject, n)).ToArray();
                con.order = condition.order;
                return con;
            }


        }



    }




}
