namespace MenuAPI;

/// <summary>The game's controls, by index. The same values as the Control enum in CitizenFX.Core, so code written for Legacy keeps working on Enhanced.</summary>
public enum Control : int
{
    /// <summary>Control index 0.</summary>
    NextCamera,
    /// <summary>Control index 1.</summary>
    LookLeftRight,
    /// <summary>Control index 2.</summary>
    LookUpDown,
    /// <summary>Control index 3.</summary>
    LookUpOnly,
    /// <summary>Control index 4.</summary>
    LookDownOnly,
    /// <summary>Control index 5.</summary>
    LookLeftOnly,
    /// <summary>Control index 6.</summary>
    LookRightOnly,
    /// <summary>Control index 7.</summary>
    CinematicSlowMo,
    /// <summary>Control index 8.</summary>
    FlyUpDown,
    /// <summary>Control index 9.</summary>
    FlyLeftRight,
    /// <summary>Control index 10.</summary>
    ScriptedFlyZUp,
    /// <summary>Control index 11.</summary>
    ScriptedFlyZDown,
    /// <summary>Control index 12.</summary>
    WeaponWheelUpDown,
    /// <summary>Control index 13.</summary>
    WeaponWheelLeftRight,
    /// <summary>Control index 14.</summary>
    WeaponWheelNext,
    /// <summary>Control index 15.</summary>
    WeaponWheelPrev,
    /// <summary>Control index 16.</summary>
    SelectNextWeapon,
    /// <summary>Control index 17.</summary>
    SelectPrevWeapon,
    /// <summary>Control index 18.</summary>
    SkipCutscene,
    /// <summary>Control index 19.</summary>
    CharacterWheel,
    /// <summary>Control index 20.</summary>
    MultiplayerInfo,
    /// <summary>Control index 21.</summary>
    Sprint,
    /// <summary>Control index 22.</summary>
    Jump,
    /// <summary>Control index 23.</summary>
    Enter,
    /// <summary>Control index 24.</summary>
    Attack,
    /// <summary>Control index 25.</summary>
    Aim,
    /// <summary>Control index 26.</summary>
    LookBehind,
    /// <summary>Control index 27.</summary>
    Phone,
    /// <summary>Control index 28.</summary>
    SpecialAbility,
    /// <summary>Control index 29.</summary>
    SpecialAbilitySecondary,
    /// <summary>Control index 30.</summary>
    MoveLeftRight,
    /// <summary>Control index 31.</summary>
    MoveUpDown,
    /// <summary>Control index 32.</summary>
    MoveUpOnly,
    /// <summary>Control index 33.</summary>
    MoveDownOnly,
    /// <summary>Control index 34.</summary>
    MoveLeftOnly,
    /// <summary>Control index 35.</summary>
    MoveRightOnly,
    /// <summary>Control index 36.</summary>
    Duck,
    /// <summary>Control index 37.</summary>
    SelectWeapon,
    /// <summary>Control index 38.</summary>
    Pickup,
    /// <summary>Control index 39.</summary>
    SniperZoom,
    /// <summary>Control index 40.</summary>
    SniperZoomInOnly,
    /// <summary>Control index 41.</summary>
    SniperZoomOutOnly,
    /// <summary>Control index 42.</summary>
    SniperZoomInSecondary,
    /// <summary>Control index 43.</summary>
    SniperZoomOutSecondary,
    /// <summary>Control index 44.</summary>
    Cover,
    /// <summary>Control index 45.</summary>
    Reload,
    /// <summary>Control index 46.</summary>
    Talk,
    /// <summary>Control index 47.</summary>
    Detonate,
    /// <summary>Control index 48.</summary>
    HUDSpecial,
    /// <summary>Control index 49.</summary>
    Arrest,
    /// <summary>Control index 50.</summary>
    AccurateAim,
    /// <summary>Control index 51.</summary>
    Context,
    /// <summary>Control index 52.</summary>
    ContextSecondary,
    /// <summary>Control index 53.</summary>
    WeaponSpecial,
    /// <summary>Control index 54.</summary>
    WeaponSpecial2,
    /// <summary>Control index 55.</summary>
    Dive,
    /// <summary>Control index 56.</summary>
    DropWeapon,
    /// <summary>Control index 57.</summary>
    DropAmmo,
    /// <summary>Control index 58.</summary>
    ThrowGrenade,
    /// <summary>Control index 59.</summary>
    VehicleMoveLeftRight,
    /// <summary>Control index 60.</summary>
    VehicleMoveUpDown,
    /// <summary>Control index 61.</summary>
    VehicleMoveUpOnly,
    /// <summary>Control index 62.</summary>
    VehicleMoveDownOnly,
    /// <summary>Control index 63.</summary>
    VehicleMoveLeftOnly,
    /// <summary>Control index 64.</summary>
    VehicleMoveRightOnly,
    /// <summary>Control index 65.</summary>
    VehicleSpecial,
    /// <summary>Control index 66.</summary>
    VehicleGunLeftRight,
    /// <summary>Control index 67.</summary>
    VehicleGunUpDown,
    /// <summary>Control index 68.</summary>
    VehicleAim,
    /// <summary>Control index 69.</summary>
    VehicleAttack,
    /// <summary>Control index 70.</summary>
    VehicleAttack2,
    /// <summary>Control index 71.</summary>
    VehicleAccelerate,
    /// <summary>Control index 72.</summary>
    VehicleBrake,
    /// <summary>Control index 73.</summary>
    VehicleDuck,
    /// <summary>Control index 74.</summary>
    VehicleHeadlight,
    /// <summary>Control index 75.</summary>
    VehicleExit,
    /// <summary>Control index 76.</summary>
    VehicleHandbrake,
    /// <summary>Control index 77.</summary>
    VehicleHotwireLeft,
    /// <summary>Control index 78.</summary>
    VehicleHotwireRight,
    /// <summary>Control index 79.</summary>
    VehicleLookBehind,
    /// <summary>Control index 80.</summary>
    VehicleCinCam,
    /// <summary>Control index 81.</summary>
    VehicleNextRadio,
    /// <summary>Control index 82.</summary>
    VehiclePrevRadio,
    /// <summary>Control index 83.</summary>
    VehicleNextRadioTrack,
    /// <summary>Control index 84.</summary>
    VehiclePrevRadioTrack,
    /// <summary>Control index 85.</summary>
    VehicleRadioWheel,
    /// <summary>Control index 86.</summary>
    VehicleHorn,
    /// <summary>Control index 87.</summary>
    VehicleFlyThrottleUp,
    /// <summary>Control index 88.</summary>
    VehicleFlyThrottleDown,
    /// <summary>Control index 89.</summary>
    VehicleFlyYawLeft,
    /// <summary>Control index 90.</summary>
    VehicleFlyYawRight,
    /// <summary>Control index 91.</summary>
    VehiclePassengerAim,
    /// <summary>Control index 92.</summary>
    VehiclePassengerAttack,
    /// <summary>Control index 93.</summary>
    VehicleSpecialAbilityFranklin,
    /// <summary>Control index 94.</summary>
    VehicleStuntUpDown,
    /// <summary>Control index 95.</summary>
    VehicleCinematicUpDown,
    /// <summary>Control index 96.</summary>
    VehicleCinematicUpOnly,
    /// <summary>Control index 97.</summary>
    VehicleCinematicDownOnly,
    /// <summary>Control index 98.</summary>
    VehicleCinematicLeftRight,
    /// <summary>Control index 99.</summary>
    VehicleSelectNextWeapon,
    /// <summary>Control index 100.</summary>
    VehicleSelectPrevWeapon,
    /// <summary>Control index 101.</summary>
    VehicleRoof,
    /// <summary>Control index 102.</summary>
    VehicleJump,
    /// <summary>Control index 103.</summary>
    VehicleGrapplingHook,
    /// <summary>Control index 104.</summary>
    VehicleShuffle,
    /// <summary>Control index 105.</summary>
    VehicleDropProjectile,
    /// <summary>Control index 106.</summary>
    VehicleMouseControlOverride,
    /// <summary>Control index 107.</summary>
    VehicleFlyRollLeftRight,
    /// <summary>Control index 108.</summary>
    VehicleFlyRollLeftOnly,
    /// <summary>Control index 109.</summary>
    VehicleFlyRollRightOnly,
    /// <summary>Control index 110.</summary>
    VehicleFlyPitchUpDown,
    /// <summary>Control index 111.</summary>
    VehicleFlyPitchUpOnly,
    /// <summary>Control index 112.</summary>
    VehicleFlyPitchDownOnly,
    /// <summary>Control index 113.</summary>
    VehicleFlyUnderCarriage,
    /// <summary>Control index 114.</summary>
    VehicleFlyAttack,
    /// <summary>Control index 115.</summary>
    VehicleFlySelectNextWeapon,
    /// <summary>Control index 116.</summary>
    VehicleFlySelectPrevWeapon,
    /// <summary>Control index 117.</summary>
    VehicleFlySelectTargetLeft,
    /// <summary>Control index 118.</summary>
    VehicleFlySelectTargetRight,
    /// <summary>Control index 119.</summary>
    VehicleFlyVerticalFlightMode,
    /// <summary>Control index 120.</summary>
    VehicleFlyDuck,
    /// <summary>Control index 121.</summary>
    VehicleFlyAttackCamera,
    /// <summary>Control index 122.</summary>
    VehicleFlyMouseControlOverride,
    /// <summary>Control index 123.</summary>
    VehicleSubTurnLeftRight,
    /// <summary>Control index 124.</summary>
    VehicleSubTurnLeftOnly,
    /// <summary>Control index 125.</summary>
    VehicleSubTurnRightOnly,
    /// <summary>Control index 126.</summary>
    VehicleSubPitchUpDown,
    /// <summary>Control index 127.</summary>
    VehicleSubPitchUpOnly,
    /// <summary>Control index 128.</summary>
    VehicleSubPitchDownOnly,
    /// <summary>Control index 129.</summary>
    VehicleSubThrottleUp,
    /// <summary>Control index 130.</summary>
    VehicleSubThrottleDown,
    /// <summary>Control index 131.</summary>
    VehicleSubAscend,
    /// <summary>Control index 132.</summary>
    VehicleSubDescend,
    /// <summary>Control index 133.</summary>
    VehicleSubTurnHardLeft,
    /// <summary>Control index 134.</summary>
    VehicleSubTurnHardRight,
    /// <summary>Control index 135.</summary>
    VehicleSubMouseControlOverride,
    /// <summary>Control index 136.</summary>
    VehiclePushbikePedal,
    /// <summary>Control index 137.</summary>
    VehiclePushbikeSprint,
    /// <summary>Control index 138.</summary>
    VehiclePushbikeFrontBrake,
    /// <summary>Control index 139.</summary>
    VehiclePushbikeRearBrake,
    /// <summary>Control index 140.</summary>
    MeleeAttackLight,
    /// <summary>Control index 141.</summary>
    MeleeAttackHeavy,
    /// <summary>Control index 142.</summary>
    MeleeAttackAlternate,
    /// <summary>Control index 143.</summary>
    MeleeBlock,
    /// <summary>Control index 144.</summary>
    ParachuteDeploy,
    /// <summary>Control index 145.</summary>
    ParachuteDetach,
    /// <summary>Control index 146.</summary>
    ParachuteTurnLeftRight,
    /// <summary>Control index 147.</summary>
    ParachuteTurnLeftOnly,
    /// <summary>Control index 148.</summary>
    ParachuteTurnRightOnly,
    /// <summary>Control index 149.</summary>
    ParachutePitchUpDown,
    /// <summary>Control index 150.</summary>
    ParachutePitchUpOnly,
    /// <summary>Control index 151.</summary>
    ParachutePitchDownOnly,
    /// <summary>Control index 152.</summary>
    ParachuteBrakeLeft,
    /// <summary>Control index 153.</summary>
    ParachuteBrakeRight,
    /// <summary>Control index 154.</summary>
    ParachuteSmoke,
    /// <summary>Control index 155.</summary>
    ParachutePrecisionLanding,
    /// <summary>Control index 156.</summary>
    Map,
    /// <summary>Control index 157.</summary>
    SelectWeaponUnarmed,
    /// <summary>Control index 158.</summary>
    SelectWeaponMelee,
    /// <summary>Control index 159.</summary>
    SelectWeaponHandgun,
    /// <summary>Control index 160.</summary>
    SelectWeaponShotgun,
    /// <summary>Control index 161.</summary>
    SelectWeaponSmg,
    /// <summary>Control index 162.</summary>
    SelectWeaponAutoRifle,
    /// <summary>Control index 163.</summary>
    SelectWeaponSniper,
    /// <summary>Control index 164.</summary>
    SelectWeaponHeavy,
    /// <summary>Control index 165.</summary>
    SelectWeaponSpecial,
    /// <summary>Control index 166.</summary>
    SelectCharacterMichael,
    /// <summary>Control index 167.</summary>
    SelectCharacterFranklin,
    /// <summary>Control index 168.</summary>
    SelectCharacterTrevor,
    /// <summary>Control index 169.</summary>
    SelectCharacterMultiplayer,
    /// <summary>Control index 170.</summary>
    SaveReplayClip,
    /// <summary>Control index 171.</summary>
    SpecialAbilityPC,
    /// <summary>Control index 172.</summary>
    PhoneUp,
    /// <summary>Control index 173.</summary>
    PhoneDown,
    /// <summary>Control index 174.</summary>
    PhoneLeft,
    /// <summary>Control index 175.</summary>
    PhoneRight,
    /// <summary>Control index 176.</summary>
    PhoneSelect,
    /// <summary>Control index 177.</summary>
    PhoneCancel,
    /// <summary>Control index 178.</summary>
    PhoneOption,
    /// <summary>Control index 179.</summary>
    PhoneExtraOption,
    /// <summary>Control index 180.</summary>
    PhoneScrollForward,
    /// <summary>Control index 181.</summary>
    PhoneScrollBackward,
    /// <summary>Control index 182.</summary>
    PhoneCameraFocusLock,
    /// <summary>Control index 183.</summary>
    PhoneCameraGrid,
    /// <summary>Control index 184.</summary>
    PhoneCameraSelfie,
    /// <summary>Control index 185.</summary>
    PhoneCameraDOF,
    /// <summary>Control index 186.</summary>
    PhoneCameraExpression,
    /// <summary>Control index 187.</summary>
    FrontendDown,
    /// <summary>Control index 188.</summary>
    FrontendUp,
    /// <summary>Control index 189.</summary>
    FrontendLeft,
    /// <summary>Control index 190.</summary>
    FrontendRight,
    /// <summary>Control index 191.</summary>
    FrontendRdown,
    /// <summary>Control index 192.</summary>
    FrontendRup,
    /// <summary>Control index 193.</summary>
    FrontendRleft,
    /// <summary>Control index 194.</summary>
    FrontendRright,
    /// <summary>Control index 195.</summary>
    FrontendAxisX,
    /// <summary>Control index 196.</summary>
    FrontendAxisY,
    /// <summary>Control index 197.</summary>
    FrontendRightAxisX,
    /// <summary>Control index 198.</summary>
    FrontendRightAxisY,
    /// <summary>Control index 199.</summary>
    FrontendPause,
    /// <summary>Control index 200.</summary>
    FrontendPauseAlternate,
    /// <summary>Control index 201.</summary>
    FrontendAccept,
    /// <summary>Control index 202.</summary>
    FrontendCancel,
    /// <summary>Control index 203.</summary>
    FrontendX,
    /// <summary>Control index 204.</summary>
    FrontendY,
    /// <summary>Control index 205.</summary>
    FrontendLb,
    /// <summary>Control index 206.</summary>
    FrontendRb,
    /// <summary>Control index 207.</summary>
    FrontendLt,
    /// <summary>Control index 208.</summary>
    FrontendRt,
    /// <summary>Control index 209.</summary>
    FrontendLs,
    /// <summary>Control index 210.</summary>
    FrontendRs,
    /// <summary>Control index 211.</summary>
    FrontendLeaderboard,
    /// <summary>Control index 212.</summary>
    FrontendSocialClub,
    /// <summary>Control index 213.</summary>
    FrontendSocialClubSecondary,
    /// <summary>Control index 214.</summary>
    FrontendDelete,
    /// <summary>Control index 215.</summary>
    FrontendEndscreenAccept,
    /// <summary>Control index 216.</summary>
    FrontendEndscreenExpand,
    /// <summary>Control index 217.</summary>
    FrontendSelect,
    /// <summary>Control index 218.</summary>
    ScriptLeftAxisX,
    /// <summary>Control index 219.</summary>
    ScriptLeftAxisY,
    /// <summary>Control index 220.</summary>
    ScriptRightAxisX,
    /// <summary>Control index 221.</summary>
    ScriptRightAxisY,
    /// <summary>Control index 222.</summary>
    ScriptRUp,
    /// <summary>Control index 223.</summary>
    ScriptRDown,
    /// <summary>Control index 224.</summary>
    ScriptRLeft,
    /// <summary>Control index 225.</summary>
    ScriptRRight,
    /// <summary>Control index 226.</summary>
    ScriptLB,
    /// <summary>Control index 227.</summary>
    ScriptRB,
    /// <summary>Control index 228.</summary>
    ScriptLT,
    /// <summary>Control index 229.</summary>
    ScriptRT,
    /// <summary>Control index 230.</summary>
    ScriptLS,
    /// <summary>Control index 231.</summary>
    ScriptRS,
    /// <summary>Control index 232.</summary>
    ScriptPadUp,
    /// <summary>Control index 233.</summary>
    ScriptPadDown,
    /// <summary>Control index 234.</summary>
    ScriptPadLeft,
    /// <summary>Control index 235.</summary>
    ScriptPadRight,
    /// <summary>Control index 236.</summary>
    ScriptSelect,
    /// <summary>Control index 237.</summary>
    CursorAccept,
    /// <summary>Control index 238.</summary>
    CursorCancel,
    /// <summary>Control index 239.</summary>
    CursorX,
    /// <summary>Control index 240.</summary>
    CursorY,
    /// <summary>Control index 241.</summary>
    CursorScrollUp,
    /// <summary>Control index 242.</summary>
    CursorScrollDown,
    /// <summary>Control index 243.</summary>
    EnterCheatCode,
    /// <summary>Control index 244.</summary>
    InteractionMenu,
    /// <summary>Control index 245.</summary>
    MpTextChatAll,
    /// <summary>Control index 246.</summary>
    MpTextChatTeam,
    /// <summary>Control index 247.</summary>
    MpTextChatFriends,
    /// <summary>Control index 248.</summary>
    MpTextChatCrew,
    /// <summary>Control index 249.</summary>
    PushToTalk,
    /// <summary>Control index 250.</summary>
    CreatorLS,
    /// <summary>Control index 251.</summary>
    CreatorRS,
    /// <summary>Control index 252.</summary>
    CreatorLT,
    /// <summary>Control index 253.</summary>
    CreatorRT,
    /// <summary>Control index 254.</summary>
    CreatorMenuToggle,
    /// <summary>Control index 255.</summary>
    CreatorAccept,
    /// <summary>Control index 256.</summary>
    CreatorDelete,
    /// <summary>Control index 257.</summary>
    Attack2,
    /// <summary>Control index 258.</summary>
    RappelJump,
    /// <summary>Control index 259.</summary>
    RappelLongJump,
    /// <summary>Control index 260.</summary>
    RappelSmashWindow,
    /// <summary>Control index 261.</summary>
    PrevWeapon,
    /// <summary>Control index 262.</summary>
    NextWeapon,
    /// <summary>Control index 263.</summary>
    MeleeAttack1,
    /// <summary>Control index 264.</summary>
    MeleeAttack2,
    /// <summary>Control index 265.</summary>
    Whistle,
    /// <summary>Control index 266.</summary>
    MoveLeft,
    /// <summary>Control index 267.</summary>
    MoveRight,
    /// <summary>Control index 268.</summary>
    MoveUp,
    /// <summary>Control index 269.</summary>
    MoveDown,
    /// <summary>Control index 270.</summary>
    LookLeft,
    /// <summary>Control index 271.</summary>
    LookRight,
    /// <summary>Control index 272.</summary>
    LookUp,
    /// <summary>Control index 273.</summary>
    LookDown,
    /// <summary>Control index 274.</summary>
    SniperZoomIn,
    /// <summary>Control index 275.</summary>
    SniperZoomOut,
    /// <summary>Control index 276.</summary>
    SniperZoomInAlternate,
    /// <summary>Control index 277.</summary>
    SniperZoomOutAlternate,
    /// <summary>Control index 278.</summary>
    VehicleMoveLeft,
    /// <summary>Control index 279.</summary>
    VehicleMoveRight,
    /// <summary>Control index 280.</summary>
    VehicleMoveUp,
    /// <summary>Control index 281.</summary>
    VehicleMoveDown,
    /// <summary>Control index 282.</summary>
    VehicleGunLeft,
    /// <summary>Control index 283.</summary>
    VehicleGunRight,
    /// <summary>Control index 284.</summary>
    VehicleGunUp,
    /// <summary>Control index 285.</summary>
    VehicleGunDown,
    /// <summary>Control index 286.</summary>
    VehicleLookLeft,
    /// <summary>Control index 287.</summary>
    VehicleLookRight,
    /// <summary>Control index 288.</summary>
    ReplayStartStopRecording,
    /// <summary>Control index 289.</summary>
    ReplayStartStopRecordingSecondary,
    /// <summary>Control index 290.</summary>
    ScaledLookLeftRight,
    /// <summary>Control index 291.</summary>
    ScaledLookUpDown,
    /// <summary>Control index 292.</summary>
    ScaledLookUpOnly,
    /// <summary>Control index 293.</summary>
    ScaledLookDownOnly,
    /// <summary>Control index 294.</summary>
    ScaledLookLeftOnly,
    /// <summary>Control index 295.</summary>
    ScaledLookRightOnly,
    /// <summary>Control index 296.</summary>
    ReplayMarkerDelete,
    /// <summary>Control index 297.</summary>
    ReplayClipDelete,
    /// <summary>Control index 298.</summary>
    ReplayPause,
    /// <summary>Control index 299.</summary>
    ReplayRewind,
    /// <summary>Control index 300.</summary>
    ReplayFfwd,
    /// <summary>Control index 301.</summary>
    ReplayNewmarker,
    /// <summary>Control index 302.</summary>
    ReplayRecord,
    /// <summary>Control index 303.</summary>
    ReplayScreenshot,
    /// <summary>Control index 304.</summary>
    ReplayHidehud,
    /// <summary>Control index 305.</summary>
    ReplayStartpoint,
    /// <summary>Control index 306.</summary>
    ReplayEndpoint,
    /// <summary>Control index 307.</summary>
    ReplayAdvance,
    /// <summary>Control index 308.</summary>
    ReplayBack,
    /// <summary>Control index 309.</summary>
    ReplayTools,
    /// <summary>Control index 310.</summary>
    ReplayRestart,
    /// <summary>Control index 311.</summary>
    ReplayShowhotkey,
    /// <summary>Control index 312.</summary>
    ReplayCycleMarkerLeft,
    /// <summary>Control index 313.</summary>
    ReplayCycleMarkerRight,
    /// <summary>Control index 314.</summary>
    ReplayFOVIncrease,
    /// <summary>Control index 315.</summary>
    ReplayFOVDecrease,
    /// <summary>Control index 316.</summary>
    ReplayCameraUp,
    /// <summary>Control index 317.</summary>
    ReplayCameraDown,
    /// <summary>Control index 318.</summary>
    ReplaySave,
    /// <summary>Control index 319.</summary>
    ReplayToggletime,
    /// <summary>Control index 320.</summary>
    ReplayToggletips,
    /// <summary>Control index 321.</summary>
    ReplayPreview,
    /// <summary>Control index 322.</summary>
    ReplayToggleTimeline,
    /// <summary>Control index 323.</summary>
    ReplayTimelinePickupClip,
    /// <summary>Control index 324.</summary>
    ReplayTimelineDuplicateClip,
    /// <summary>Control index 325.</summary>
    ReplayTimelinePlaceClip,
    /// <summary>Control index 326.</summary>
    ReplayCtrl,
    /// <summary>Control index 327.</summary>
    ReplayTimelineSave,
    /// <summary>Control index 328.</summary>
    ReplayPreviewAudio,
    /// <summary>Control index 329.</summary>
    VehicleDriveLook,
    /// <summary>Control index 330.</summary>
    VehicleDriveLook2,
    /// <summary>Control index 331.</summary>
    VehicleFlyAttack2,
    /// <summary>Control index 332.</summary>
    RadioWheelUpDown,
    /// <summary>Control index 333.</summary>
    RadioWheelLeftRight,
    /// <summary>Control index 334.</summary>
    VehicleSlowMoUpDown,
    /// <summary>Control index 335.</summary>
    VehicleSlowMoUpOnly,
    /// <summary>Control index 336.</summary>
    VehicleSlowMoDownOnly,
    /// <summary>Control index 337.</summary>
    VehicleHydraulicsControlToggle,
    /// <summary>Control index 338.</summary>
    VehicleHydraulicsControlLeft,
    /// <summary>Control index 339.</summary>
    VehicleHydraulicsControlRight,
    /// <summary>Control index 340.</summary>
    VehicleHydraulicsControlUp,
    /// <summary>Control index 341.</summary>
    VehicleHydraulicsControlDown,
    /// <summary>Control index 342.</summary>
    VehicleHydraulicsControlUpDown,
    /// <summary>Control index 343.</summary>
    VehicleHydraulicsControlLeftRight,
    /// <summary>Control index 344.</summary>
    SwitchVisor,
    /// <summary>Control index 345.</summary>
    VehicleMeleeHold,
    /// <summary>Control index 346.</summary>
    VehicleMeleeLeft,
    /// <summary>Control index 347.</summary>
    VehicleMeleeRight,
    /// <summary>Control index 348.</summary>
    MapPointOfInterest,
    /// <summary>Control index 349.</summary>
    ReplaySnapmaticPhoto,
    /// <summary>Control index 350.</summary>
    VehicleCarJump,
    /// <summary>Control index 351.</summary>
    VehicleRocketBoost,
    /// <summary>Control index 352.</summary>
    VehicleFlyBoost,
    /// <summary>Control index 353.</summary>
    VehicleParachute,
    /// <summary>Control index 354.</summary>
    VehicleBikeWings,
    /// <summary>Control index 355.</summary>
    VehicleFlyBombBay,
    /// <summary>Control index 356.</summary>
    VehicleFlyCounter,
    /// <summary>Control index 357.</summary>
    VehicleFlyTransform
}