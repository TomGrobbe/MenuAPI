namespace MenuAPI;

internal static class FrameState
{
    private static int _ped;
    private static bool _hasPed;
    private static bool _isDead;
    private static bool _hasIsDead;
    private static bool _isInVehicle;
    private static bool _hasIsInVehicle;
    private static bool _isScreenFadedIn;
    private static bool _hasIsScreenFadedIn;
    private static bool _isPauseMenuActive;
    private static bool _hasIsPauseMenuActive;
    private static bool _isPlayerSwitchInProgress;
    private static bool _hasIsPlayerSwitchInProgress;
    private static int _onscreenKeyboard;
    private static bool _hasOnscreenKeyboard;

    internal static void Invalidate()
    {
        _hasPed = false;
        _hasIsDead = false;
        _hasIsInVehicle = false;
        _hasIsScreenFadedIn = false;
        _hasIsPauseMenuActive = false;
        _hasIsPlayerSwitchInProgress = false;
        _hasOnscreenKeyboard = false;
    }

    internal static int Ped
    {
        get
        {
            if (!_hasPed)
            {
                _ped = Native.PlayerPedId();
                _hasPed = true;
            }

            return _ped;
        }
    }

    internal static bool IsDead
    {
        get
        {
            if (!_hasIsDead)
            {
                _isDead = Native.IsPlayerDead(Native.PlayerId());
                _hasIsDead = true;
            }

            return _isDead;
        }
    }

    internal static bool IsInVehicle
    {
        get
        {
            if (!_hasIsInVehicle)
            {
                _isInVehicle = Native.IsPedInAnyVehicle(Ped, false);
                _hasIsInVehicle = true;
            }

            return _isInVehicle;
        }
    }

    internal static bool IsScreenFadedIn
    {
        get
        {
            if (!_hasIsScreenFadedIn)
            {
                _isScreenFadedIn = Native.IsScreenFadedIn();
                _hasIsScreenFadedIn = true;
            }

            return _isScreenFadedIn;
        }
    }

    internal static bool IsPauseMenuActive
    {
        get
        {
            if (!_hasIsPauseMenuActive)
            {
                _isPauseMenuActive = Native.IsPauseMenuActive();
                _hasIsPauseMenuActive = true;
            }

            return _isPauseMenuActive;
        }
    }

    internal static bool IsPlayerSwitchInProgress
    {
        get
        {
            if (!_hasIsPlayerSwitchInProgress)
            {
                _isPlayerSwitchInProgress = Native.IsPlayerSwitchInProgress();
                _hasIsPlayerSwitchInProgress = true;
            }

            return _isPlayerSwitchInProgress;
        }
    }

    internal static int OnscreenKeyboard
    {
        get
        {
            if (!_hasOnscreenKeyboard)
            {
                _onscreenKeyboard = Native.UpdateOnscreenKeyboard();
                _hasOnscreenKeyboard = true;
            }

            return _onscreenKeyboard;
        }
    }
}
