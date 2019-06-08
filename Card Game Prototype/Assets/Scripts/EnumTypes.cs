﻿using System.Collections;
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
    };

    public enum TargetID
    {
        Enemies,
        Allies,
        All
    };

    public enum DesiredTarget
    {
        Self,
        TriggeringTarget,
        MultiTarget
    };

    public enum PlayableSlot
    {
        None = 0,
        Head = 1 << 0,
        Chest = 1 << 1,
        Hand = 1 << 2,
        Feet = 1 << 3
    };

    public enum CardType
    {
        Weapon,
        Armour,
        Ability,
        Behaviour,
        Environmental
    };

    public enum Rarity
    {
        Common,
        Uncommon,
        Rare
    };
}
