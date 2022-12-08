using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Conf;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;




public interface ICharecterAction
{
    List<CharacterAction> CharacterActions { get; }

}


public class CharacterAction
{

    #region Field
    private Conf.CharacterActionName actionName;
    private GameObject gameObject;



    private Func<TriggerData, bool?> condition;
    private Action<bool> actionContent;

    private List<Conf.CharacterActionName> actionEndAllow;




    private bool actionState = false;
    private int timeAtTriggerFr;
    private float timeAtStart;


    #endregion
    // * Region  End---------------------------------- 




    #region Construction

    public class ConstructParm
    {
        public Func<TriggerData, bool> conditionForActionOn = (d) => true;

        public Func<TriggerData, bool> conditionForActionOff = (d) => true;
        public Action actionEnable;
        public Action actionDisable;


        public List<Conf.CharacterActionName> actionEndAllow = new List<CharacterActionName>();


    }

    private void Construction(CharacterActionName actionName, GameObject gameObject, ConstructParm parm)
    {
        this.actionName = actionName;
        this.gameObject = gameObject;


        this.actionContent += (t) =>
        {
            if (t) parm.actionEnable?.Invoke();
        };
        this.actionContent += (t) =>
        {
            if (!t) parm.actionDisable?.Invoke();
        };

        this.condition = (d) =>
        {
            bool? re = null;
            if (actionState == true)
            {
                bool? v = parm.conditionForActionOff?.Invoke(d);
                if (v != false)
                {
                    re = false;
                }
            }
            else
            {
                bool? v = parm.conditionForActionOn?.Invoke(d);
                if (v != false)
                {
                    re = true;
                }
            }
            return re;
        };

        this.actionEndAllow = parm.actionEndAllow;

    }
    public CharacterAction(CharacterActionName actionName, GameObject gameObject, ConstructParm parm)
    {
        Construction(actionName, gameObject, parm);
    }
    public CharacterAction(CharacterActionName actionName, GameObject gameObject, ConstructParm parm, ref Action disableAction)
    {
        Construction(actionName, gameObject, parm);
        disableAction += () => DisableAll();
    }




    #endregion
    // * Region  End---------------------------------- 






    public class TriggerData
    {
        public System.Object sourceData;

        public TriggerData(System.Object obj)
        {
            sourceData = obj;
        }

        public bool? IsKeyPressed(string inputName)
        {
            InputAction.CallbackContext inputD = default;
            bool success = sourceData.TryConvert<InputAction.CallbackContext>(out inputD);
            if (success)
            {
                if (inputD.action.name == inputName)
                {
                    return inputD.IsKeyOn();
                }

            }

            return null;

        }
        public bool? IsKeyPressed(Conf.InputName inputName)
        {
            return IsKeyPressed(inputName.ToString());
        }
    }



    private void MainTrigger(TriggerData d)
    {
        bool _如果同一时间启动了两次则停止 = timeAtTriggerFr == Time.frameCount;
        if (_如果同一时间启动了两次则停止) return;
        timeAtTriggerFr = Time.frameCount;



        bool? conditionResult = condition?.Invoke(d);

        bool changeActionState = actionState != conditionResult & conditionResult != null;
        bool changeActionStateTo = conditionResult.GetValueOrDefault();
        if (changeActionState)
        {
            actionState = changeActionStateTo;
            timeAtStart = Time.time;
            actionContent?.Invoke(changeActionStateTo);
        }


        bool triggerActionDisable = changeActionState & changeActionStateTo == false;
        if (triggerActionDisable)
        {
            OnActionDisable(d);
        }

    }
    private void OnActionDisable(TriggerData d)
    {
        CharacterActionF.OnActionEndInvoke(gameObject);
    }

    public float LastTime => Time.time - timeAtStart;

    public bool ActionState
    {
        get => actionState;
    }
    public Conf.CharacterActionName Name => actionName;




    public void OnTrigger(InputAction.CallbackContext d)
    {
        MainTrigger(new TriggerData(d));
    }
    public void OnTriggerNull()
    {
        MainTrigger(null);
    }


    public void SetInitial(bool v)
    {
        if (v)
        {
            actionState = true;
            actionContent?.Invoke(true);
        }
        else
        {
            actionState = false;
            actionContent?.Invoke(false);
        }
    }
    public void EnableTrigger()
    {
    }
    public void DisableAll(bool TriggerEndEvent = true)
    {
        actionState = false;
        actionContent?.Invoke(false);


        if (TriggerEndEvent)
        {
            OnActionDisable(new TriggerData(this));
        }

    }





}


public static class CharacterActionF
{
    private static Dictionary<GameObject, UnityEvent> Dic_CharacterActionEndEvent = new Dictionary<GameObject, UnityEvent>();
    public static void OnActionEndAdd(GameObject gameObject, UnityAction action)
    {
        var dic = Dic_CharacterActionEndEvent;
        UnityEvent unityEvent = dic.GetOrCreate(gameObject);
        unityEvent.AddListener(action);

        BasicEvent.OnDestroyEvent.Add(gameObject, () => dic.Remove(gameObject));
    }
    public static void OnActionEndAdd(GameObject gameObject, UnityAction action, ref Action disableAction)
    {
        OnActionEndAdd(gameObject, action);
        disableAction += () => OnActionEndRemove(gameObject, action);
    }
    public static void OnActionEndRemove(GameObject gameObject, UnityAction action)
    {
        var dic = Dic_CharacterActionEndEvent;
        UnityEvent OnActionEnd = dic.TryGetValue(gameObject);
        OnActionEnd?.RemoveListener(action);
    }
    public static void OnActionEndInvoke(GameObject gameObject)
    {
        if (!gameObject.activeInHierarchy) return;

        var dic = Dic_CharacterActionEndEvent;
        UnityEvent OnActionEnd = dic.TryGetValue(gameObject);
        OnActionEnd?.Invoke();
    }




    public static bool IsAllActionOff(GameObject gameObject, params Conf.CharacterActionName[] actions)
    {
        IEnumerable<CharacterActionName> allOff = gameObject.GetComponents<ICharecterAction>()
                .SelectMany((a) => a.CharacterActions)
                .Where((ac) => ac.ActionState == false)
                .Select((ac) => ac.Name);

        return actions.Intersect(allOff).Any();
    }



    private static Dictionary<GameObject, Dictionary<string, bool>> Dic_CharacterState = new Dictionary<GameObject, Dictionary<string, bool>>();

    public static void SetState(GameObject gameObject, CharacterActionName actionName, bool state)
    {
        Dictionary<string, bool> stateDic = Dic_CharacterState.GetOrCreate(gameObject);
        stateDic[actionName.ToString()] = state;
    }
    public static bool? GetState(GameObject gameObject, CharacterActionName actionName)
    {
        Dictionary<string, bool> stateDic;
        bool hasDate = Dic_CharacterState.TryGetValue(gameObject, out stateDic);
        if (hasDate == false) return null;

        bool state;
        hasDate = stateDic.TryGetValue(actionName.ToString(), out state);
        if (hasDate == false) return null;

        return state;
    }
}






