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

    [System.Flags] public enum PlayableSlot
    {
        None = 0,
        Head = 1 << 0,
        Chest = 1 << 1,
        Hand = 1 << 2,
        Feet = 1 << 3
    };

    [System.Flags] public enum Organisation
    {
        // Doesn't work for no apparent reason
        None = 0,
        Pos = 1 << 0,
        Rot = 1 << 1,
        Size = 1 << 2
    };

    public enum CardType
    {
        None,
        Weapon,
        Armour,
        Ability,
        Behaviour,
        Environmental
    };

    public enum Rarity
    {
        None,
        Common,
        Uncommon,
        Rare
    };

    public enum TargetSelection
    {
        Closest,
        Farthest,
        Random
    };

    public enum CC
    {
        None,
        Stun,
        Snare,
        Slow,
        SlowAttack
    };

    public enum StatChange
    {
        None,
        Damage,
        Heal,
        MaxHealth,
        Attack,
        Armour,
        Speed,
        AttackSpeed
    }
}