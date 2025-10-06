using UnityEngine;
using System;
using Save;

public static partial class S_SaveSystem
{
    [Serializable]
    private class Save_Player : Save_Transform
    {
        public PlayerManager playerManager;

        public Save_Player(Transform transform, PlayerManager playerManager) : base(transform)
        {
            this.playerManager = playerManager;

        }

        public override void Save()
        {
            base.Save();

        }

        public override void Load()
        {
            base.Load();

        }
    }
}