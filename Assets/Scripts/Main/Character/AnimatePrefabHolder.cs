using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "人物动画表", menuName = "配置(旧版本)/人物动画表", order = 0)]
public class AnimatePrefabHolder : ScriptableObject
{
    public List<AnimatePrefab> animatePrefabs = new List<AnimatePrefab>();

    [System.Serializable]
    public class AnimatePrefab
    {
        [HideInInspector] public string name;
        public Conf.AnimationName 动画名;
        public GameObject prefab;
    }





    [ContextMenu("更新列表")]
    private void SetUpList()
    {
        IEnumerable<Conf.AnimationName> names = Enum.GetValues(typeof(Conf.AnimationName)).Cast<Conf.AnimationName>();
        IEnumerable<AnimatePrefab> aniP = names.Except(animatePrefabs.Select((x) => x.动画名)).Select((name) => new AnimatePrefab() { 动画名 = name });
        animatePrefabs.AddRange(aniP);
        OnValidate();
    }

    private void OnValidate()
    {
        animatePrefabs.ForEach((a) => a.name = a.动画名.ToString());
    }
}