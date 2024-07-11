using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RoundType / BloodMoon")]
public class RoundData_BloodMoon : RoundData
{
    public override void OnRoundStart()
    {
        base.OnRoundStart();
          LocalHandler.instance.SetBloodMoonBool(true);

    }
    public override void OnRoundEnd()
    {
        base.OnRoundEnd();
      

        LocalHandler.instance.SetBloodMoonBool(true);
    }

    //i just inform them to increase the stats. 
    //increase the speed by flat value
    //health by 25% of the current value.

    //

}
