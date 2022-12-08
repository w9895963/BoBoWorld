using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OtStateUtility;
using UnityEngine;



#if(UNITY_EDITOR)
public class OtState_ShowDate : MonoBehaviour
{
    [TextArea(8, 8)]
    public string states;
    public List<string> statesView = new List<string>();

    void Update()
    {
        List<StateBase> stateBases = OtStateUtility.StateBase.allState.TryGetValue(gameObject);
        IEnumerable<string> sts = stateBases?.Select((st) =>
        {
            string en = st.Enabled ? "●" : "○";
            return $"{en}{st.Name}";
        });





        // statesView = sts.ToList();
        states = string.Join(";", sts);

        // statesView = sts.Select((s, i) => (s, i)).GroupBy((s) => s.i / 3, (k) => k.s).Select((s) => string.Join(";", s)).ToList();


    }
}
#endif