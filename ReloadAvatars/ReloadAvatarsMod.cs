﻿using System;
using MelonLoader;
using UIExpansionKit.API;
using UnityEngine;
using VRChatUtilityKit.Utilities;

[assembly: MelonInfo(typeof(ReloadAvatars.ReloadAvatarsMod), "ReloadAvatars", "1.1.1", "Sleepers", "https://github.com/SleepyVRC/Mods")]
[assembly: MelonGame("VRChat", "VRChat")]

namespace ReloadAvatars
{
    public class ReloadAvatarsMod : MelonMod
    {
        public static GameObject reloadAvatarButton;
        public static GameObject reloadAllAvatarsButton;

        public static MelonPreferences_Entry<bool> reloadAvatarPref;
        public static MelonPreferences_Entry<bool> reloadAllAvatarsPref;

        public override void OnApplicationStart()
        {
            MelonPreferences_Category category = MelonPreferences.CreateCategory("ReloadAvatars", "ReloadAvatars Settings");
            reloadAvatarPref = category.CreateEntry("ReloadAvatar", true, "Enable/Disable Reload Avatar Button");
            reloadAllAvatarsPref = category.CreateEntry("ReloadAllAvatars", true, "Enable/Disable Reload All Avatars Button");

            foreach (MelonPreferences_Entry entry in category.Entries)
                entry.OnValueChangedUntyped += OnPrefChanged;

            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.UserQuickMenu).AddSimpleButton("Reload Avatar", new Action(() =>
            {
                try
                {
                    VRCUtils.ReloadAvatar(VRCUtils.ActivePlayerInUserSelectMenu);
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error("Error while reloading single avatar:\n" + ex.ToString());
                } // Ignore
            }), new Action<GameObject>((gameObject) => { reloadAvatarButton = gameObject; reloadAvatarButton.SetActive(reloadAllAvatarsPref.Value); OnPrefChanged(); }));

            ExpansionKitApi.GetExpandedMenu(ExpandedMenu.QuickMenu).AddSimpleButton("Reload All Avatars", new Action(() =>
            {
                try
                {
                    VRCUtils.ReloadAllAvatars();
                }
                catch (Exception ex)
                {
                    LoggerInstance.Error("Error while reloading all avatars:\n" + ex.ToString());
                } // Ignore
            }), new Action<GameObject>((gameObject) => { reloadAllAvatarsButton = gameObject; reloadAllAvatarsButton.SetActive(reloadAvatarPref.Value); OnPrefChanged(); }));
            LoggerInstance.Msg("Initialized!");
        }
        public void OnPrefChanged()
        {
            reloadAvatarButton?.SetActive(reloadAvatarPref.Value);
            reloadAllAvatarsButton?.SetActive(reloadAllAvatarsPref.Value);
        }
    }
}
