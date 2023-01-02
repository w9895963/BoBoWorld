using System.Collections;
using System.Collections.Generic;
using Configure;
using EventData;
using UnityEngine;

[CreateAssetMenu(fileName = "标签", menuName = "动态配置/标签", order = 1)]
public class ConfigureItem_Tag : ConfigureBase
{
    public List<Tag> 标签 = new List<Tag>();
}
