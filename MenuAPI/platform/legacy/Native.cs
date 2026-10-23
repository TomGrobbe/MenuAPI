using CitizenFX.Core;

using Cfx = CitizenFX.Core.Native.API;

namespace MenuAPI;

// Shared code calls Native.X the way FiveM Enhanced names its natives. Being in the MenuAPI namespace,
// this class wins over anything a using pulls in, so those calls land here on Legacy unchanged.
internal static class Native
{
    internal static void DisableControlAction(int padIndex, int control, bool disable) => Cfx.DisableControlAction(padIndex, control, disable);

    internal static string GetControlInstructionalButton(int padIndex, int control, bool unk) => Cfx.GetControlInstructionalButton(padIndex, control, unk ? 1 : 0);

    internal static string GetCurrentResourceName() => Cfx.GetCurrentResourceName();

    internal static Vector3 GetFinalRenderedCamRot(int rotationOrder) => Cfx.GetFinalRenderedCamRot(rotationOrder);

    internal static int GetGameTimer() => Cfx.GetGameTimer();

    internal static void GetHairRgbColor(int colorId, out int r, out int g, out int b)
    {
        r = g = b = 0;
        Cfx.GetHairRgbColor(colorId, ref r, ref g, ref b);
    }

    internal static void GetHudColour(int hudColorIndex, out int r, out int g, out int b, out int a)
    {
        r = g = b = a = 0;
        Cfx.GetHudColour(hudColorIndex, ref r, ref g, ref b, ref a);
    }

    internal static string GetLabelText(string labelName) => Cfx.GetLabelText(labelName);

    internal static void GetMakeupRgbColor(int colorId, out int r, out int g, out int b)
    {
        r = g = b = 0;
        Cfx.GetMakeupRgbColor(colorId, ref r, ref g, ref b);
    }

    internal static float GetSafeZoneSize() => Cfx.GetSafeZoneSize();

    internal static float GetScreenAspectRatio(bool b) => Cfx.GetScreenAspectRatio(b);

    internal static bool HasStreamedTextureDictLoaded(string dict) => Cfx.HasStreamedTextureDictLoaded(dict);

    internal static bool IsControlEnabled(int padIndex, int control) => Cfx.IsControlEnabled(padIndex, control);

    internal static bool IsControlPressed(int padIndex, int control) => Cfx.IsControlPressed(padIndex, control);

    internal static bool IsHudComponentActive(int id) => Cfx.IsHudComponentActive(id);

    internal static bool IsHudHidden() => Cfx.IsHudHidden();

    internal static bool IsPauseMenuActive() => Cfx.IsPauseMenuActive();

    internal static bool IsPauseMenuRestarting() => Cfx.IsPauseMenuRestarting();

    internal static bool IsPedInAnyVehicle(int ped, bool atGetIn) => Cfx.IsPedInAnyVehicle(ped, atGetIn);

    internal static bool IsPlayerDead(int player) => Cfx.IsPlayerDead(player);

    internal static bool IsPlayerSwitchInProgress() => Cfx.IsPlayerSwitchInProgress();

    internal static bool IsScreenFadedIn() => Cfx.IsScreenFadedIn();

    // Same native, the old wrapper still carries its earlier name.
    internal static bool IsUsingKeyboardAndMouse(int padIndex) => Cfx.IsInputDisabled(padIndex);

    internal static bool IsWarningMessageActive() => Cfx.IsWarningMessageActive();

    internal static void PlaySoundFrontend(int soundId, string audioName, string audioRef, bool p3) => Cfx.PlaySoundFrontend(soundId, audioName, audioRef, p3);

    internal static int PlayerId() => Cfx.PlayerId();

    internal static int PlayerPedId() => Cfx.PlayerPedId();

    internal static void ProfilerEnterScope(string scopeName) => Cfx.ProfilerEnterScope(scopeName);

    internal static void ProfilerExitScope() => Cfx.ProfilerExitScope();

    internal static void RegisterKeyMapping(string commandString, string description, string defaultMapper, string defaultParameter) =>
        Cfx.RegisterKeyMapping(commandString, description, defaultMapper, defaultParameter);

    internal static void RegisterNuiCallbackType(string callbackType) => Cfx.RegisterNuiCallbackType(callbackType);

    internal static void RequestStreamedTextureDict(string textureDict, bool p1) => Cfx.RequestStreamedTextureDict(textureDict, p1);

    internal static bool SendNuiMessage(string jsonString) => Cfx.SendNuiMessage(jsonString);

    internal static void SetStreamedTextureDictAsNoLongerNeeded(string textureDict) => Cfx.SetStreamedTextureDictAsNoLongerNeeded(textureDict);

    internal static int UpdateOnscreenKeyboard() => Cfx.UpdateOnscreenKeyboard();
}
