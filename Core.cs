using System;
using System.Xml.Linq;
using HarmonyLib;
using UnityEngine;
using System.IO;
using System.Linq;

public class Core : IModApi
{
    private static Mod ModInstance;
    private static XDocument configDoc;
    private const string ConfigFileName = "versioncheck.xml";
    private static bool versionMismatchShown = false;

     public void InitMod(Mod modInstance)
    {
        ModInstance = modInstance;
        if (LoadConfig())
        {
            Harmony.CreateAndPatchAll(typeof(Core).Assembly);
            ModEvents.GameStartDone.RegisterHandler(OnGameStartDone);

            if (GetConfigBool("DisableNewsScreen"))
            {
                XUiC_MainMenu.shownNewsScreenOnce = true;
            }
        }
        else
        {
            Debug.LogError($"[VersionCheckMod] Failed to load configuration file: {ConfigFileName}");
        }
    }

    private static bool LoadConfig()
    {
        string configPath = FindConfigFile();
        if (string.IsNullOrEmpty(configPath))
        {
            return false;
        }

        try
        {
            configDoc = XDocument.Load(configPath);
            return true;
        }
        catch (Exception ex)
        {
           Debug.LogError($"[VersionCheckMod] Error loading configuration file: {ex.Message}");
            return false;
        }
    }

    private static string FindConfigFile()
    {
        // Search for the config file in all possible mod directories
        foreach (string modPath in ModManager.GetLoadedMods().Select(mod => mod.Path))
        {
            string fullPath = Path.Combine(modPath, ConfigFileName);
            if (File.Exists(fullPath))
            {
                return fullPath;
            }
        }

        Debug.LogWarning($"[VersionCheckMod] Configuration file {ConfigFileName} not found in any mod directory.");
        return null;
    }

    private static string GetConfigString(string key)
    {
        return configDoc.Root.Element("Settings").Element(key)?.Value ?? string.Empty;
    }

    private static bool GetConfigBool(string key)
    {
        return bool.TryParse(GetConfigString(key), out bool result) && result;
    }

    private static void OnGameStartDone()
    {
        CheckVersions();
    }

    [HarmonyPatch(typeof(XUiC_MainMenu))]
    [HarmonyPatch("OnOpen")]
    public class XUiC_MainMenu_OnOpen_Patch
    {
        static void Postfix(XUiC_MainMenu __instance)
        {
            CheckVersions(__instance.xui);
        }
    }

    private static void CheckVersions(XUi xui = null)
    {
        if (versionMismatchShown)
        {
            return;
        }

        string gameVersion = GetGameVersion();
        string modVersion = GetConfigString("ModVersion");

        if (gameVersion != modVersion && xui != null)
        {
            DisplayVersionMismatchMessage(xui, gameVersion, modVersion);
            versionMismatchShown = true;
        }
    }

    private static string GetGameVersion()
    {
        return $"{Constants.cVersionMajor}.{Constants.cVersionMinor}.{Constants.cVersionBuild}";
    }

    private static void DisplayVersionMismatchMessage(XUi xui, string gameVersion, string modVersion)
    {
        string title = configDoc.Root.Element("Settings")?.Element("ErrorMessage")?.Element("Title")?.Value 
            ?? "Version Mismatch Detected";
        string descriptionFormat = configDoc.Root.Element("Settings")?.Element("ErrorMessage")?.Element("Description")?.Value 
            ?? "The game version ({0}) does not match the mod version ({1}). This may cause issues.";

        string message = string.Format(descriptionFormat, gameVersion, modVersion);

        XUiC_MessageBoxWindowGroup.ShowMessageBox(
            xui,
            title,
            message,
            XUiC_MessageBoxWindowGroup.MessageBoxTypes.Ok,
            () => {
                // Return to main menu
                xui.playerUI.windowManager.CloseAllOpenWindows();
                xui.playerUI.windowManager.Open("mainmenu", true);
            },
            null,
            false,
            false
        );
    }
}