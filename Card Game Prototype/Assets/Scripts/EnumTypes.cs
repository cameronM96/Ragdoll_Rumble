using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnumTypes
{
    public enum TriggerMethod
    {
        OnStart,
        OnHit,
        OnGetHit,
        OnDamaged,
        OnHealth,
        OnTimer,
        OnButton
    }

    public enum TargetID
    {
        Enemies,
        Allies,
        All
    }

    public enum DesiredTarget
    {
        Self,
        TriggeringTarget,
        MultiTarget
    }
}
