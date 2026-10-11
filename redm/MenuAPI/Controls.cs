namespace CitizenFX.Core
{
	/// <summary>RedM's controls. Each value is the hash of the control's name, not an index like in GTA V.</summary>
	public enum Control : uint
	{
		/// <summary>Control hash <c>0x7F8D09B8</c>.</summary>
		NextCamera = 0x7F8D09B8,
		/// <summary>Control hash <c>0xA987235F</c>.</summary>
		LookLr = 0xA987235F,
		/// <summary>Control hash <c>0xD2047988</c>.</summary>
		LookUd = 0xD2047988,
		/// <summary>Control hash <c>0xC0651D40</c>.</summary>
		LookUpOnly = 0xC0651D40,
		/// <summary>Control hash <c>0x8ED92E16</c>.</summary>
		LookDownOnly = 0x8ED92E16,
		/// <summary>Control hash <c>0x08F8BC6D</c>.</summary>
		LookLeftOnly = 0x08F8BC6D,
		/// <summary>Control hash <c>0xA1EB1353</c>.</summary>
		LookRightOnly = 0xA1EB1353,
		/// <summary>Control hash <c>0x7A9093DE</c>.</summary>
		CinematicSlowmo = 0x7A9093DE,
		/// <summary>Control hash <c>0xBA60039F</c>.</summary>
		RadialMenuNavUd = 0xBA60039F,
		/// <summary>Control hash <c>0x390948DC</c>.</summary>
		RadialMenuNavLr = 0x390948DC,
		/// <summary>Control hash <c>0xE71F89B8</c>.</summary>
		RadialMenuSlotNavNext = 0xE71F89B8,
		/// <summary>Control hash <c>0x93D6723F</c>.</summary>
		RadialMenuSlotNavPrev = 0x93D6723F,
		/// <summary>Control hash <c>0x77E56FB3</c>.</summary>
		SelectNextWheel = 0x77E56FB3,
		/// <summary>Control hash <c>0x1F6D95E5</c>.</summary>
		SelectItemWheel = 0x1F6D95E5,
		/// <summary>Control hash <c>0xF09866F3</c>.</summary>
		QuickSelectInspect = 0xF09866F3,
		/// <summary>Control hash <c>0xD45EC04F</c>.</summary>
		QuickSelectSetForSwap = 0xD45EC04F,
		/// <summary>Control hash <c>0x9CC7A1A4</c>.</summary>
		QuickShortcutAbilitiesMenu = 0x9CC7A1A4,
		/// <summary>Control hash <c>0xF1421CF5</c>.</summary>
		QuickSelectSecondaryNavNext = 0xF1421CF5,
		/// <summary>Control hash <c>0xD9F9F017</c>.</summary>
		QuickSelectSecondaryNavPrev = 0xD9F9F017,
		/// <summary>Control hash <c>0xFA0B29CD</c>.</summary>
		QuickSelectToggleShortcutItem = 0xFA0B29CD,
		/// <summary>Control hash <c>0x253FEC09</c>.</summary>
		QuickSelectPutAwayRod = 0x253FEC09,
		/// <summary>Control hash <c>0xA835261B</c>.</summary>
		EmotesFavorite = 0xA835261B,
		/// <summary>Control hash <c>0x7E75F4DC</c>.</summary>
		EmotesManage = 0x7E75F4DC,
		/// <summary>Control hash <c>0xCBB12F87</c>.</summary>
		EmotesSlotNavNext = 0xCBB12F87,
		/// <summary>Control hash <c>0xD0842EDF</c>.</summary>
		SelectNextWeapon = 0xD0842EDF,
		/// <summary>Control hash <c>0xF78D7337</c>.</summary>
		SelectPrevWeapon = 0xF78D7337,
		/// <summary>Control hash <c>0xCDC4E4E9</c>.</summary>
		SkipCutscene = 0xCDC4E4E9,
		/// <summary>Control hash <c>0x972F8D1E</c>.</summary>
		CharacterWheel = 0x972F8D1E,
		/// <summary>Control hash <c>0xE8342FF2</c>.</summary>
		MultiplayerInfo = 0xE8342FF2,
		/// <summary>Control hash <c>0x8FFC75D6</c>.</summary>
		Sprint = 0x8FFC75D6,
		/// <summary>Control hash <c>0xD9D0E1C0</c>.</summary>
		Jump = 0xD9D0E1C0,
		/// <summary>Control hash <c>0xCEFD9220</c>.</summary>
		Enter = 0xCEFD9220,
		/// <summary>Control hash <c>0x07CE1E61</c>.</summary>
		Attack = 0x07CE1E61,
		/// <summary>Control hash <c>0xF84FA74F</c>.</summary>
		Aim = 0xF84FA74F,
		/// <summary>Control hash <c>0x9959A6F0</c>.</summary>
		LookBehind = 0x9959A6F0,
		/// <summary>Control hash <c>0x4CF871D0</c>.</summary>
		Phone = 0x4CF871D0,
		/// <summary>Control hash <c>0xCEE12B50</c>.</summary>
		SpecialAbility = 0xCEE12B50,
		/// <summary>Control hash <c>0x6328239B</c>.</summary>
		SpecialAbilitySecondary = 0x6328239B,
		/// <summary>Control hash <c>0x811F4A1A</c>.</summary>
		SecondarySpecialAbilitySecondary = 0x811F4A1A,
		/// <summary>Control hash <c>0x1ECA87D4</c>.</summary>
		SpecialAbilityAction = 0x1ECA87D4,
		/// <summary>Control hash <c>0x4D8FB4C1</c>.</summary>
		MoveLr = 0x4D8FB4C1,
		/// <summary>Control hash <c>0xFDA83190</c>.</summary>
		MoveUd = 0xFDA83190,
		/// <summary>Control hash <c>0x8FD015D8</c>.</summary>
		MoveUpOnly = 0x8FD015D8,
		/// <summary>Control hash <c>0xD27782E3</c>.</summary>
		MoveDownOnly = 0xD27782E3,
		/// <summary>Control hash <c>0x7065027D</c>.</summary>
		MoveLeftOnly = 0x7065027D,
		/// <summary>Control hash <c>0xB4E465B4</c>.</summary>
		MoveRightOnly = 0xB4E465B4,
		/// <summary>Control hash <c>0xDB096B85</c>.</summary>
		Duck = 0xDB096B85,
		/// <summary>Control hash <c>0x938D4071</c>.</summary>
		TwirlPistol = 0x938D4071,
		/// <summary>Control hash <c>0xB238FE0B</c>.</summary>
		ToggleHolster = 0xB238FE0B,
		/// <summary>Control hash <c>0xAC4BD4F1</c>.</summary>
		OpenWheelMenu = 0xAC4BD4F1,
		/// <summary>Control hash <c>0x4CC0E2FE</c>.</summary>
		OpenSatchelMenu = 0x4CC0E2FE,
		/// <summary>Control hash <c>0x5966D52A</c>.</summary>
		OpenSatchelHorseMenu = 0x5966D52A,
		/// <summary>Control hash <c>0x734C6E39</c>.</summary>
		OpenCraftingMenu = 0x734C6E39,
		/// <summary>Control hash <c>0xF3830D8E</c>.</summary>
		OpenJournal = 0xF3830D8E,
		/// <summary>Control hash <c>0xE6360A8E</c>.</summary>
		Pickup = 0xE6360A8E,
		/// <summary>Control hash <c>0xC75C27B0</c>.</summary>
		Ignite = 0xC75C27B0,
		/// <summary>Control hash <c>0x7ABC6A66</c>.</summary>
		SniperZoom = 0x7ABC6A66,
		/// <summary>Control hash <c>0xA5BDCD3C</c>.</summary>
		SniperZoomInOnly = 0xA5BDCD3C,
		/// <summary>Control hash <c>0x430593AA</c>.</summary>
		SniperZoomOutOnly = 0x430593AA,
		/// <summary>Control hash <c>0x6BE9C207</c>.</summary>
		SniperZoomInSecondary = 0x6BE9C207,
		/// <summary>Control hash <c>0x8A7B8833</c>.</summary>
		SniperZoomOutSecondary = 0x8A7B8833,
		/// <summary>Control hash <c>0x3076E97C</c>.</summary>
		ToggleWeaponScope = 0x3076E97C,
		/// <summary>Control hash <c>0xDE794E3E</c>.</summary>
		Cover = 0xDE794E3E,
		/// <summary>Control hash <c>0x750C8010</c>.</summary>
		CoverTransition = 0x750C8010,
		/// <summary>Control hash <c>0xE30CD707</c>.</summary>
		Reload = 0xE30CD707,
		/// <summary>Control hash <c>0x7DCA9C75</c>.</summary>
		Talk = 0x7DCA9C75,
		/// <summary>Control hash <c>0x73846677</c>.</summary>
		Detonate = 0x73846677,
		/// <summary>Control hash <c>0x580C4473</c>.</summary>
		HudSpecial = 0x580C4473,
		/// <summary>Control hash <c>0xA4F1006B</c>.</summary>
		Arrest = 0xA4F1006B,
		/// <summary>Control hash <c>0x406ADFAE</c>.</summary>
		AccurateAim = 0x406ADFAE,
		/// <summary>Control hash <c>0x827E9EE8</c>.</summary>
		SwitchShoulder = 0x827E9EE8,
		/// <summary>Control hash <c>0x841240A9</c>.</summary>
		IronSight = 0x841240A9,
		/// <summary>Control hash <c>0xD8F73058</c>.</summary>
		AimInAir = 0xD8F73058,
		/// <summary>Control hash <c>0xEED15F18</c>.</summary>
		SwitchFiringMode = 0xEED15F18,
		/// <summary>Control hash <c>0xB73BCA77</c>.</summary>
		Context = 0xB73BCA77,
		/// <summary>Control hash <c>0xF19BE385</c>.</summary>
		ContextSecondary = 0xF19BE385,
		/// <summary>Control hash <c>0x733901F3</c>.</summary>
		WeaponSpecial = 0x733901F3,
		/// <summary>Control hash <c>0x50BA1A77</c>.</summary>
		WeaponSpecialTwo = 0x50BA1A77,
		/// <summary>Control hash <c>0x06052D11</c>.</summary>
		Dive = 0x06052D11,
		/// <summary>Control hash <c>0x7DBCD016</c>.</summary>
		DropWeapon = 0x7DBCD016,
		/// <summary>Control hash <c>0x4E42696E</c>.</summary>
		DropAmmo = 0x4E42696E,
		/// <summary>Control hash <c>0x0AF99998</c>.</summary>
		ThrowGrenade = 0x0AF99998,
		/// <summary>Control hash <c>0xE72B43F4</c>.</summary>
		FocusCam = 0xE72B43F4,
		/// <summary>Control hash <c>0xA61DC630</c>.</summary>
		Inspect = 0xA61DC630,
		/// <summary>Control hash <c>0x53296B75</c>.</summary>
		InspectZoom = 0x53296B75,
		/// <summary>Control hash <c>0x1788C283</c>.</summary>
		InspectLr = 0x1788C283,
		/// <summary>Control hash <c>0xF9781997</c>.</summary>
		InspectUd = 0xF9781997,
		/// <summary>Control hash <c>0x9B1CA8DA</c>.</summary>
		InspectOpenSatchel = 0x9B1CA8DA,
		/// <summary>Control hash <c>0x2EAB0795</c>.</summary>
		DynamicScenario = 0x2EAB0795,
		/// <summary>Control hash <c>0x80F28E95</c>.</summary>
		PlayerMenu = 0x80F28E95,
		/// <summary>Control hash <c>0xE2B557A3</c>.</summary>
		OpenEmoteWheel = 0xE2B557A3,
		/// <summary>Control hash <c>0x8B3FA65E</c>.</summary>
		OpenEmoteWheelHorse = 0x8B3FA65E,
		/// <summary>Control hash <c>0x1C826362</c>.</summary>
		EmoteGroupLink = 0x1C826362,
		/// <summary>Control hash <c>0x4FD1C57B</c>.</summary>
		EmoteGroupLinkHorse = 0x4FD1C57B,
		/// <summary>Control hash <c>0xCF8A4ECA</c>.</summary>
		RevealHud = 0xCF8A4ECA,
		/// <summary>Control hash <c>0x0F39B3D4</c>.</summary>
		SelectRadarMode = 0x0F39B3D4,
		/// <summary>Control hash <c>0x5FEF1B6D</c>.</summary>
		SimpleRadar = 0x5FEF1B6D,
		/// <summary>Control hash <c>0xCF0B11DE</c>.</summary>
		ExpandRadar = 0xCF0B11DE,
		/// <summary>Control hash <c>0x51AA7A35</c>.</summary>
		RegularRadar = 0x51AA7A35,
		/// <summary>Control hash <c>0x70CBD78D</c>.</summary>
		DisableRadar = 0x70CBD78D,
		/// <summary>Control hash <c>0xDB8D69B8</c>.</summary>
		Surrender = 0xDB8D69B8,
		/// <summary>Control hash <c>0x24978A28</c>.</summary>
		Whistle = 0x24978A28,
		/// <summary>Control hash <c>0xE7EB9185</c>.</summary>
		WhistleHorseback = 0xE7EB9185,
		/// <summary>Control hash <c>0x7914A3DD</c>.</summary>
		StopLeadingAnimal = 0x7914A3DD,
		/// <summary>Control hash <c>0x620A6C5E</c>.</summary>
		CinematicCam = 0x620A6C5E,
		/// <summary>Control hash <c>0xD7E7B375</c>.</summary>
		CinematicCamHold = 0xD7E7B375,
		/// <summary>Control hash <c>0xA6C67243</c>.</summary>
		CinematicCamChangeShot = 0xA6C67243,
		/// <summary>Control hash <c>0x84574AE8</c>.</summary>
		CinematicCamUd = 0x84574AE8,
		/// <summary>Control hash <c>0xEFCFE6B7</c>.</summary>
		CinematicCamUpOnly = 0xEFCFE6B7,
		/// <summary>Control hash <c>0x23AE34A2</c>.</summary>
		CinematicCamDownOnly = 0x23AE34A2,
		/// <summary>Control hash <c>0x6BC904FC</c>.</summary>
		CinematicCamLr = 0x6BC904FC,
		/// <summary>Control hash <c>0x5181713D</c>.</summary>
		ContextA = 0x5181713D,
		/// <summary>Control hash <c>0x3B24C470</c>.</summary>
		ContextB = 0x3B24C470,
		/// <summary>Control hash <c>0xE3BF959B</c>.</summary>
		ContextX = 0xE3BF959B,
		/// <summary>Control hash <c>0xD51B784F</c>.</summary>
		ContextY = 0xD51B784F,
		/// <summary>Control hash <c>0xC13A6564</c>.</summary>
		ContextLt = 0xC13A6564,
		/// <summary>Control hash <c>0x07B8BEAF</c>.</summary>
		ContextRt = 0x07B8BEAF,
		/// <summary>Control hash <c>0xB28318C0</c>.</summary>
		ContextAction = 0xB28318C0,
		/// <summary>Control hash <c>0xF1E2852C</c>.</summary>
		VehMoveLr = 0xF1E2852C,
		/// <summary>Control hash <c>0x8A81C00C</c>.</summary>
		VehMoveUd = 0x8A81C00C,
		/// <summary>Control hash <c>0xDEBD7EF6</c>.</summary>
		VehMoveUpOnly = 0xDEBD7EF6,
		/// <summary>Control hash <c>0x16D73E1D</c>.</summary>
		VehMoveDownOnly = 0x16D73E1D,
		/// <summary>Control hash <c>0x9DF54706</c>.</summary>
		VehMoveLeftOnly = 0x9DF54706,
		/// <summary>Control hash <c>0x97A8FD98</c>.</summary>
		VehMoveRightOnly = 0x97A8FD98,
		/// <summary>Control hash <c>0x493919DB</c>.</summary>
		VehSpecial = 0x493919DB,
		/// <summary>Control hash <c>0xB6F3E4FE</c>.</summary>
		VehGunLr = 0xB6F3E4FE,
		/// <summary>Control hash <c>0x482560EE</c>.</summary>
		VehGunUd = 0x482560EE,
		/// <summary>Control hash <c>0xD7CAFCEF</c>.</summary>
		VehAim = 0xD7CAFCEF,
		/// <summary>Control hash <c>0xF4330038</c>.</summary>
		VehAttack = 0xF4330038,
		/// <summary>Control hash <c>0xF1C341BA</c>.</summary>
		VehAttack2 = 0xF1C341BA,
		/// <summary>Control hash <c>0x5B9FD4E2</c>.</summary>
		VehAccelerate = 0x5B9FD4E2,
		/// <summary>Control hash <c>0x6E1F639B</c>.</summary>
		VehBrake = 0x6E1F639B,
		/// <summary>Control hash <c>0x5B3690F2</c>.</summary>
		VehDuck = 0x5B3690F2,
		/// <summary>Control hash <c>0xF1301666</c>.</summary>
		VehHeadlight = 0xF1301666,
		/// <summary>Control hash <c>0xFEFAB9B4</c>.</summary>
		VehExit = 0xFEFAB9B4,
		/// <summary>Control hash <c>0x65D24C98</c>.</summary>
		VehHandbrake = 0x65D24C98,
		/// <summary>Control hash <c>0xCAE9B017</c>.</summary>
		VehLookBehind = 0xCAE9B017,
		/// <summary>Control hash <c>0x22E0F7E7</c>.</summary>
		VehNextRadio = 0x22E0F7E7,
		/// <summary>Control hash <c>0x9785CE13</c>.</summary>
		VehPrevRadio = 0x9785CE13,
		/// <summary>Control hash <c>0xF7FA2DDC</c>.</summary>
		VehNextRadioTrack = 0xF7FA2DDC,
		/// <summary>Control hash <c>0x0A94C4FF</c>.</summary>
		VehPrevRadioTrack = 0x0A94C4FF,
		/// <summary>Control hash <c>0x4915AC0A</c>.</summary>
		VehRadioWheel = 0x4915AC0A,
		/// <summary>Control hash <c>0x63A0D258</c>.</summary>
		VehHorn = 0x63A0D258,
		/// <summary>Control hash <c>0x7232BAB3</c>.</summary>
		VehFlyThrottleUp = 0x7232BAB3,
		/// <summary>Control hash <c>0x084DFF95</c>.</summary>
		VehFlyThrottleDown = 0x084DFF95,
		/// <summary>Control hash <c>0x31589AD1</c>.</summary>
		VehFlyYawLeft = 0x31589AD1,
		/// <summary>Control hash <c>0xBD143FC6</c>.</summary>
		VehFlyYawRight = 0xBD143FC6,
		/// <summary>Control hash <c>0xEE2804D0</c>.</summary>
		VehPassengerAim = 0xEE2804D0,
		/// <summary>Control hash <c>0x27AD4433</c>.</summary>
		VehPassengerAttack = 0x27AD4433,
		/// <summary>Control hash <c>0x5EC33578</c>.</summary>
		VehSpecialAbilityFranklin = 0x5EC33578,
		/// <summary>Control hash <c>0x4AA1560E</c>.</summary>
		VehStuntUd = 0x4AA1560E,
		/// <summary>Control hash <c>0x889A626F</c>.</summary>
		VehSelectNextWeapon = 0x889A626F,
		/// <summary>Control hash <c>0x0C97BAC7</c>.</summary>
		VehSelectPrevWeapon = 0x0C97BAC7,
		/// <summary>Control hash <c>0x3E7CF9A4</c>.</summary>
		VehRoof = 0x3E7CF9A4,
		/// <summary>Control hash <c>0xAA56B926</c>.</summary>
		VehJump = 0xAA56B926,
		/// <summary>Control hash <c>0xB985AA5E</c>.</summary>
		VehGrapplingHook = 0xB985AA5E,
		/// <summary>Control hash <c>0xC7083798</c>.</summary>
		VehShuffle = 0xC7083798,
		/// <summary>Control hash <c>0x739D6261</c>.</summary>
		VehTraversal = 0x739D6261,
		/// <summary>Control hash <c>0xC61611E6</c>.</summary>
		VehDropProjectile = 0xC61611E6,
		/// <summary>Control hash <c>0x39CCABD5</c>.</summary>
		VehMouseControlOverride = 0x39CCABD5,
		/// <summary>Control hash <c>0x3C8AB570</c>.</summary>
		VehFlyRollLr = 0x3C8AB570,
		/// <summary>Control hash <c>0x56F84EA0</c>.</summary>
		VehFlyRollLeftOnly = 0x56F84EA0,
		/// <summary>Control hash <c>0x876B3361</c>.</summary>
		VehFlyRollRightOnly = 0x876B3361,
		/// <summary>Control hash <c>0xE67E1E57</c>.</summary>
		VehFlyPitchUd = 0xE67E1E57,
		/// <summary>Control hash <c>0x6280BA1A</c>.</summary>
		VehFlyPitchUpOnly = 0x6280BA1A,
		/// <summary>Control hash <c>0x0F4E369F</c>.</summary>
		VehFlyPitchDownOnly = 0x0F4E369F,
		/// <summary>Control hash <c>0xFE0FE518</c>.</summary>
		VehFlyUndercarriage = 0xFE0FE518,
		/// <summary>Control hash <c>0x1D71D7AA</c>.</summary>
		VehFlyAttack = 0x1D71D7AA,
		/// <summary>Control hash <c>0x24E94299</c>.</summary>
		VehFlySelectNextWeapon = 0x24E94299,
		/// <summary>Control hash <c>0xC0D874E5</c>.</summary>
		VehFlySelectPrevWeapon = 0xC0D874E5,
		/// <summary>Control hash <c>0x307FC4C1</c>.</summary>
		VehFlySelectTargetLeft = 0x307FC4C1,
		/// <summary>Control hash <c>0x52F25C96</c>.</summary>
		VehFlySelectTargetRight = 0x52F25C96,
		/// <summary>Control hash <c>0xE3238029</c>.</summary>
		VehFlyVerticalFlightMode = 0xE3238029,
		/// <summary>Control hash <c>0x378A10F7</c>.</summary>
		VehFlyDuck = 0x378A10F7,
		/// <summary>Control hash <c>0x2FBA3F0B</c>.</summary>
		VehFlyAttackCamera = 0x2FBA3F0B,
		/// <summary>Control hash <c>0x6C9810A5</c>.</summary>
		VehFlyMouseControlOverride = 0x6C9810A5,
		/// <summary>Control hash <c>0x2CAF327E</c>.</summary>
		VehSubMouseControlOverride = 0x2CAF327E,
		/// <summary>Control hash <c>0x627C4619</c>.</summary>
		VehSubTurnLr = 0x627C4619,
		/// <summary>Control hash <c>0x44E7E093</c>.</summary>
		VehSubTurnLeftOnly = 0x44E7E093,
		/// <summary>Control hash <c>0xE78A5A3C</c>.</summary>
		VehSubTurnRightOnly = 0xE78A5A3C,
		/// <summary>Control hash <c>0x469CE271</c>.</summary>
		VehSubPitchUd = 0x469CE271,
		/// <summary>Control hash <c>0xF9EF072A</c>.</summary>
		VehSubPitchUpOnly = 0xF9EF072A,
		/// <summary>Control hash <c>0xBA2D22AA</c>.</summary>
		VehSubPitchDownOnly = 0xBA2D22AA,
		/// <summary>Control hash <c>0xD28C446F</c>.</summary>
		VehSubThrottleUp = 0xD28C446F,
		/// <summary>Control hash <c>0xF5B2CEFB</c>.</summary>
		VehSubThrottleDown = 0xF5B2CEFB,
		/// <summary>Control hash <c>0xD7991F74</c>.</summary>
		VehSubAscend = 0xD7991F74,
		/// <summary>Control hash <c>0x7D51DE24</c>.</summary>
		VehSubDescend = 0x7D51DE24,
		/// <summary>Control hash <c>0x64214D49</c>.</summary>
		VehSubTurnHardLeft = 0x64214D49,
		/// <summary>Control hash <c>0xA44C0F83</c>.</summary>
		VehSubTurnHardRight = 0xA44C0F83,
		/// <summary>Control hash <c>0xFD8D64A7</c>.</summary>
		VehPushbikePedal = 0xFD8D64A7,
		/// <summary>Control hash <c>0xF03EE151</c>.</summary>
		VehPushbikeSprint = 0xF03EE151,
		/// <summary>Control hash <c>0x585E942D</c>.</summary>
		VehPushbikeFrontBrake = 0x585E942D,
		/// <summary>Control hash <c>0xF8CBAFB5</c>.</summary>
		VehPushbikeRearBrake = 0xF8CBAFB5,
		/// <summary>Control hash <c>0x23595CEA</c>.</summary>
		VehDraftMoveUd = 0x23595CEA,
		/// <summary>Control hash <c>0xA7DFAE8A</c>.</summary>
		VehDraftTurnLr = 0xA7DFAE8A,
		/// <summary>Control hash <c>0x29A5E51E</c>.</summary>
		VehDraftMoveUpOnly = 0x29A5E51E,
		/// <summary>Control hash <c>0x25493EB3</c>.</summary>
		VehDraftMoveDownOnly = 0x25493EB3,
		/// <summary>Control hash <c>0x198AFC64</c>.</summary>
		VehDraftTurnLeftOnly = 0x198AFC64,
		/// <summary>Control hash <c>0x5E371EA7</c>.</summary>
		VehDraftTurnRightOnly = 0x5E371EA7,
		/// <summary>Control hash <c>0xE99D2B05</c>.</summary>
		VehDraftAccelerate = 0xE99D2B05,
		/// <summary>Control hash <c>0xD648E48D</c>.</summary>
		VehDraftBrake = 0xD648E48D,
		/// <summary>Control hash <c>0xBDD5830D</c>.</summary>
		VehDraftAim = 0xBDD5830D,
		/// <summary>Control hash <c>0xF40AB198</c>.</summary>
		VehDraftAttack = 0xF40AB198,
		/// <summary>Control hash <c>0x886F12DD</c>.</summary>
		VehDraftAttack2 = 0x886F12DD,
		/// <summary>Control hash <c>0x70B87844</c>.</summary>
		VehDraftSwitchDrivers = 0x70B87844,
		/// <summary>Control hash <c>0xD8DFCAB3</c>.</summary>
		VehBoatTurnLr = 0xD8DFCAB3,
		/// <summary>Control hash <c>0x5BED7C91</c>.</summary>
		VehBoatTurnLeftOnly = 0x5BED7C91,
		/// <summary>Control hash <c>0xF9780DFB</c>.</summary>
		VehBoatTurnRightOnly = 0xF9780DFB,
		/// <summary>Control hash <c>0xB341E812</c>.</summary>
		VehBoatAccelerate = 0xB341E812,
		/// <summary>Control hash <c>0x428D5F39</c>.</summary>
		VehBoatBrake = 0x428D5F39,
		/// <summary>Control hash <c>0x92F5F01E</c>.</summary>
		VehBoatAim = 0x92F5F01E,
		/// <summary>Control hash <c>0x6866FA3A</c>.</summary>
		VehBoatAttack = 0x6866FA3A,
		/// <summary>Control hash <c>0x876096E9</c>.</summary>
		VehBoatAttack2 = 0x876096E9,
		/// <summary>Control hash <c>0x3BD38D43</c>.</summary>
		VehCarTurnLr = 0x3BD38D43,
		/// <summary>Control hash <c>0x07D1654C</c>.</summary>
		VehCarTurnLeftOnly = 0x07D1654C,
		/// <summary>Control hash <c>0x6E3C3649</c>.</summary>
		VehCarTurnRightOnly = 0x6E3C3649,
		/// <summary>Control hash <c>0xB9F544B0</c>.</summary>
		VehCarAccelerate = 0xB9F544B0,
		/// <summary>Control hash <c>0xD1887B3F</c>.</summary>
		VehCarBrake = 0xD1887B3F,
		/// <summary>Control hash <c>0x6777B840</c>.</summary>
		VehCarAim = 0x6777B840,
		/// <summary>Control hash <c>0x5572F386</c>.</summary>
		VehCarAttack = 0x5572F386,
		/// <summary>Control hash <c>0x5B763AD7</c>.</summary>
		VehCarAttack2 = 0x5B763AD7,
		/// <summary>Control hash <c>0xFF3626FC</c>.</summary>
		VehHandcartAccelerate = 0xFF3626FC,
		/// <summary>Control hash <c>0x2D79D80A</c>.</summary>
		VehHandcartBrake = 0x2D79D80A,
		/// <summary>Control hash <c>0x126796EB</c>.</summary>
		HorseMoveLr = 0x126796EB,
		/// <summary>Control hash <c>0x3BBDEFEF</c>.</summary>
		HorseMoveUd = 0x3BBDEFEF,
		/// <summary>Control hash <c>0x699487BB</c>.</summary>
		HorseMoveUpOnly = 0x699487BB,
		/// <summary>Control hash <c>0x56F82045</c>.</summary>
		HorseMoveDownOnly = 0x56F82045,
		/// <summary>Control hash <c>0x86D773F6</c>.</summary>
		HorseMoveLeftOnly = 0x86D773F6,
		/// <summary>Control hash <c>0x7E6B8612</c>.</summary>
		HorseMoveRightOnly = 0x7E6B8612,
		/// <summary>Control hash <c>0x70089459</c>.</summary>
		HorseSpecial = 0x70089459,
		/// <summary>Control hash <c>0x3D99EEC6</c>.</summary>
		HorseGunLr = 0x3D99EEC6,
		/// <summary>Control hash <c>0xBFF476F9</c>.</summary>
		HorseGunUd = 0xBFF476F9,
		/// <summary>Control hash <c>0x60C81CDE</c>.</summary>
		HorseAttack = 0x60C81CDE,
		/// <summary>Control hash <c>0xC904196D</c>.</summary>
		HorseAttack2 = 0xC904196D,
		/// <summary>Control hash <c>0x5AA007D7</c>.</summary>
		HorseSprint = 0x5AA007D7,
		/// <summary>Control hash <c>0xE16B9AAD</c>.</summary>
		HorseStop = 0xE16B9AAD,
		/// <summary>Control hash <c>0xCBDB82A8</c>.</summary>
		HorseExit = 0xCBDB82A8,
		/// <summary>Control hash <c>0x81280569</c>.</summary>
		HorseLookBehind = 0x81280569,
		/// <summary>Control hash <c>0xE4D2CE1D</c>.</summary>
		HorseJump = 0xE4D2CE1D,
		/// <summary>Control hash <c>0x61470051</c>.</summary>
		HorseAim = 0x61470051,
		/// <summary>Control hash <c>0x7D5B3717</c>.</summary>
		HorseCollect = 0x7D5B3717,
		/// <summary>Control hash <c>0xA95E1468</c>.</summary>
		HitchAnimal = 0xA95E1468,
		/// <summary>Control hash <c>0x4216AF06</c>.</summary>
		HorseCommandFlee = 0x4216AF06,
		/// <summary>Control hash <c>0xAE5DFDED</c>.</summary>
		HorseCommandStay = 0xAE5DFDED,
		/// <summary>Control hash <c>0x763E4D27</c>.</summary>
		HorseCommandFollow = 0x763E4D27,
		/// <summary>Control hash <c>0x1A3EABBB</c>.</summary>
		HorseMelee = 0x1A3EABBB,
		/// <summary>Control hash <c>0x78ED2132</c>.</summary>
		MeleeHorseAttackPrimary = 0x78ED2132,
		/// <summary>Control hash <c>0x162AFEB8</c>.</summary>
		MeleeHorseAttackSecondary = 0x162AFEB8,
		/// <summary>Control hash <c>0x2996DD15</c>.</summary>
		HorseCoverTransition = 0x2996DD15,
		/// <summary>Control hash <c>0xB2F377E8</c>.</summary>
		MeleeAttack = 0xB2F377E8,
		/// <summary>Control hash <c>0x1E7D7275</c>.</summary>
		MeleeModifier = 0x1E7D7275,
		/// <summary>Control hash <c>0xB5EEEFB7</c>.</summary>
		MeleeBlock = 0xB5EEEFB7,
		/// <summary>Control hash <c>0x2277FAE9</c>.</summary>
		MeleeGrapple = 0x2277FAE9,
		/// <summary>Control hash <c>0xADEAF48C</c>.</summary>
		MeleeGrappleAttack = 0xADEAF48C,
		/// <summary>Control hash <c>0x018C47CF</c>.</summary>
		MeleeGrappleChoke = 0x018C47CF,
		/// <summary>Control hash <c>0x91C9A817</c>.</summary>
		MeleeGrappleReversal = 0x91C9A817,
		/// <summary>Control hash <c>0xD0C1FEFF</c>.</summary>
		MeleeGrappleBreakout = 0xD0C1FEFF,
		/// <summary>Control hash <c>0xBE1F4699</c>.</summary>
		MeleeGrappleStandSwitch = 0xBE1F4699,
		/// <summary>Control hash <c>0x67ED272E</c>.</summary>
		MeleeGrappleMountSwitch = 0x67ED272E,
		/// <summary>Control hash <c>0xEBF53058</c>.</summary>
		ParachuteDeploy = 0xEBF53058,
		/// <summary>Control hash <c>0xFFBFF139</c>.</summary>
		ParachuteDetach = 0xFFBFF139,
		/// <summary>Control hash <c>0x8EC920BF</c>.</summary>
		ParachuteTurnLr = 0x8EC920BF,
		/// <summary>Control hash <c>0xC4CF3322</c>.</summary>
		ParachuteTurnLeftOnly = 0xC4CF3322,
		/// <summary>Control hash <c>0x2BDBA378</c>.</summary>
		ParachuteTurnRightOnly = 0x2BDBA378,
		/// <summary>Control hash <c>0xF0526228</c>.</summary>
		ParachutePitchUd = 0xF0526228,
		/// <summary>Control hash <c>0x08BFEA69</c>.</summary>
		ParachutePitchUpOnly = 0x08BFEA69,
		/// <summary>Control hash <c>0x7C3A4352</c>.</summary>
		ParachutePitchDownOnly = 0x7C3A4352,
		/// <summary>Control hash <c>0x272BD8BA</c>.</summary>
		ParachuteBrakeLeft = 0x272BD8BA,
		/// <summary>Control hash <c>0x948B3EA7</c>.</summary>
		ParachuteBrakeRight = 0x948B3EA7,
		/// <summary>Control hash <c>0x2574FAB0</c>.</summary>
		ParachuteSmoke = 0x2574FAB0,
		/// <summary>Control hash <c>0xC675B8BD</c>.</summary>
		ParachutePrecisionLanding = 0xC675B8BD,
		/// <summary>Control hash <c>0xE31C6A41</c>.</summary>
		Map = 0xE31C6A41,
		/// <summary>Control hash <c>0x1F6EEB0F</c>.</summary>
		SelectWeaponUnarmed = 0x1F6EEB0F,
		/// <summary>Control hash <c>0x109E6852</c>.</summary>
		SelectWeaponMelee = 0x109E6852,
		/// <summary>Control hash <c>0x184960E3</c>.</summary>
		SelectWeaponHandgun = 0x184960E3,
		/// <summary>Control hash <c>0x76D3EA05</c>.</summary>
		SelectWeaponShotgun = 0x76D3EA05,
		/// <summary>Control hash <c>0xCEF1BB48</c>.</summary>
		SelectWeaponSmg = 0xCEF1BB48,
		/// <summary>Control hash <c>0x05EEA9D0</c>.</summary>
		SelectWeaponAutoRifle = 0x05EEA9D0,
		/// <summary>Control hash <c>0x96C61FDF</c>.</summary>
		SelectWeaponSniper = 0x96C61FDF,
		/// <summary>Control hash <c>0x3D1675C3</c>.</summary>
		SelectWeaponHeavy = 0x3D1675C3,
		/// <summary>Control hash <c>0xC41ECEF8</c>.</summary>
		SelectWeaponSpecial = 0xC41ECEF8,
		/// <summary>Control hash <c>0xEA9256B8</c>.</summary>
		SelectCharacterMichael = 0xEA9256B8,
		/// <summary>Control hash <c>0x8E8B08CB</c>.</summary>
		SelectCharacterFranklin = 0x8E8B08CB,
		/// <summary>Control hash <c>0xB00CC093</c>.</summary>
		SelectCharacterTrevor = 0xB00CC093,
		/// <summary>Control hash <c>0xDFB2B3B8</c>.</summary>
		SelectCharacterMultiplayer = 0xDFB2B3B8,
		/// <summary>Control hash <c>0x5B3AF9E3</c>.</summary>
		SaveReplayClip = 0x5B3AF9E3,
		/// <summary>Control hash <c>0x52E60A8B</c>.</summary>
		SpecialAbilityPc = 0x52E60A8B,
		/// <summary>Control hash <c>0xE6F612E4</c>.</summary>
		SelectQuickselectSidearmsLeft = 0xE6F612E4,
		/// <summary>Control hash <c>0x1CE6D9EB</c>.</summary>
		SelectQuickselectDualwield = 0x1CE6D9EB,
		/// <summary>Control hash <c>0x4F49CC4C</c>.</summary>
		SelectQuickselectSidearmsRight = 0x4F49CC4C,
		/// <summary>Control hash <c>0x8F9F9E58</c>.</summary>
		SelectQuickselectUnarmed = 0x8F9F9E58,
		/// <summary>Control hash <c>0xAB62E997</c>.</summary>
		SelectQuickselectMeleeNoUnarmed = 0xAB62E997,
		/// <summary>Control hash <c>0xA1FDE2A6</c>.</summary>
		SelectQuickselectSecondaryLongarm = 0xA1FDE2A6,
		/// <summary>Control hash <c>0xB03A913B</c>.</summary>
		SelectQuickselectThrown = 0xB03A913B,
		/// <summary>Control hash <c>0x42385422</c>.</summary>
		SelectQuickselectPrimaryLongarm = 0x42385422,
		/// <summary>Control hash <c>0xD2EE3B1E</c>.</summary>
		CellphoneUp = 0xD2EE3B1E,
		/// <summary>Control hash <c>0x82196002</c>.</summary>
		CellphoneDown = 0x82196002,
		/// <summary>Control hash <c>0x3ABBE990</c>.</summary>
		CellphoneLeft = 0x3ABBE990,
		/// <summary>Control hash <c>0xD25EFDCD</c>.</summary>
		CellphoneRight = 0xD25EFDCD,
		/// <summary>Control hash <c>0xDC264018</c>.</summary>
		CellphoneSelect = 0xDC264018,
		/// <summary>Control hash <c>0xDD833287</c>.</summary>
		CellphoneCancel = 0xDD833287,
		/// <summary>Control hash <c>0xD2C28BB4</c>.</summary>
		CellphoneOption = 0xD2C28BB4,
		/// <summary>Control hash <c>0xBE354011</c>.</summary>
		CellphoneExtraOption = 0xBE354011,
		/// <summary>Control hash <c>0xCB4E1798</c>.</summary>
		CellphoneScrollForward = 0xCB4E1798,
		/// <summary>Control hash <c>0x47CD0F3B</c>.</summary>
		CellphoneScrollBackward = 0x47CD0F3B,
		/// <summary>Control hash <c>0x5AC1805E</c>.</summary>
		CellphoneCameraFocusLock = 0x5AC1805E,
		/// <summary>Control hash <c>0xE18CC57A</c>.</summary>
		CellphoneCameraGrid = 0xE18CC57A,
		/// <summary>Control hash <c>0x6A440BFE</c>.</summary>
		CellphoneCameraSelfie = 0x6A440BFE,
		/// <summary>Control hash <c>0x593DB489</c>.</summary>
		CellphoneCameraDof = 0x593DB489,
		/// <summary>Control hash <c>0xD7E274E7</c>.</summary>
		CellphoneCameraExpression = 0xD7E274E7,
		/// <summary>Control hash <c>0x05CA7C52</c>.</summary>
		FrontendDown = 0x05CA7C52,
		/// <summary>Control hash <c>0x6319DB71</c>.</summary>
		FrontendUp = 0x6319DB71,
		/// <summary>Control hash <c>0xA65EBAB4</c>.</summary>
		FrontendLeft = 0xA65EBAB4,
		/// <summary>Control hash <c>0xDEB34313</c>.</summary>
		FrontendRight = 0xDEB34313,
		/// <summary>Control hash <c>0x5734A944</c>.</summary>
		FrontendRdown = 0x5734A944,
		/// <summary>Control hash <c>0xD7DE6B1E</c>.</summary>
		FrontendRup = 0xD7DE6B1E,
		/// <summary>Control hash <c>0x39336A4F</c>.</summary>
		FrontendRleft = 0x39336A4F,
		/// <summary>Control hash <c>0x5B48F938</c>.</summary>
		FrontendRright = 0x5B48F938,
		/// <summary>Control hash <c>0xFB56DD5B</c>.</summary>
		FrontendAxisX = 0xFB56DD5B,
		/// <summary>Control hash <c>0x091178D0</c>.</summary>
		FrontendAxisY = 0x091178D0,
		/// <summary>Control hash <c>0x3224BC55</c>.</summary>
		FrontendScrollAxisX = 0x3224BC55,
		/// <summary>Control hash <c>0x21651AD6</c>.</summary>
		FrontendScrollAxisY = 0x21651AD6,
		/// <summary>Control hash <c>0x3D23549A</c>.</summary>
		FrontendRightAxisX = 0x3D23549A,
		/// <summary>Control hash <c>0xEB4130DF</c>.</summary>
		FrontendRightAxisY = 0xEB4130DF,
		/// <summary>Control hash <c>0xD82E0BD2</c>.</summary>
		FrontendPause = 0xD82E0BD2,
		/// <summary>Control hash <c>0x4A903C11</c>.</summary>
		FrontendPauseAlternate = 0x4A903C11,
		/// <summary>Control hash <c>0xC7B5340A</c>.</summary>
		FrontendAccept = 0xC7B5340A,
		/// <summary>Control hash <c>0x156F7119</c>.</summary>
		FrontendCancel = 0x156F7119,
		/// <summary>Control hash <c>0x6DB8C62F</c>.</summary>
		FrontendX = 0x6DB8C62F,
		/// <summary>Control hash <c>0x7C0162C0</c>.</summary>
		FrontendY = 0x7C0162C0,
		/// <summary>Control hash <c>0xE885EF16</c>.</summary>
		FrontendLb = 0xE885EF16,
		/// <summary>Control hash <c>0x17BEC168</c>.</summary>
		FrontendRb = 0x17BEC168,
		/// <summary>Control hash <c>0x51104035</c>.</summary>
		FrontendLt = 0x51104035,
		/// <summary>Control hash <c>0x6FED71BC</c>.</summary>
		FrontendRt = 0x6FED71BC,
		/// <summary>Control hash <c>0x43CDA5B0</c>.</summary>
		FrontendLs = 0x43CDA5B0,
		/// <summary>Control hash <c>0x7DA48D2A</c>.</summary>
		FrontendRs = 0x7DA48D2A,
		/// <summary>Control hash <c>0x9EDC8D65</c>.</summary>
		FrontendLeaderboard = 0x9EDC8D65,
		/// <summary>Control hash <c>0x064D1698</c>.</summary>
		FrontendSocialClub = 0x064D1698,
		/// <summary>Control hash <c>0xBDB8D6F3</c>.</summary>
		FrontendSocialClubSecondary = 0xBDB8D6F3,
		/// <summary>Control hash <c>0x4AF4D473</c>.</summary>
		FrontendDelete = 0x4AF4D473,
		/// <summary>Control hash <c>0x3E32FCEE</c>.</summary>
		FrontendEndscreenAccept = 0x3E32FCEE,
		/// <summary>Control hash <c>0xC79BDE9F</c>.</summary>
		FrontendEndscreenExpand = 0xC79BDE9F,
		/// <summary>Control hash <c>0x171910DC</c>.</summary>
		FrontendSelect = 0x171910DC,
		/// <summary>Control hash <c>0x44CD301B</c>.</summary>
		FrontendPhotoMode = 0x44CD301B,
		/// <summary>Control hash <c>0x8CFFE0A1</c>.</summary>
		FrontendNavUp = 0x8CFFE0A1,
		/// <summary>Control hash <c>0x78114AB3</c>.</summary>
		FrontendNavDown = 0x78114AB3,
		/// <summary>Control hash <c>0x877F1027</c>.</summary>
		FrontendNavLeft = 0x877F1027,
		/// <summary>Control hash <c>0x08BD758C</c>.</summary>
		FrontendNavRight = 0x08BD758C,
		/// <summary>Control hash <c>0x125A70E5</c>.</summary>
		FrontendMapNavUp = 0x125A70E5,
		/// <summary>Control hash <c>0xF8480EED</c>.</summary>
		FrontendMapNavDown = 0xF8480EED,
		/// <summary>Control hash <c>0xE0D75B00</c>.</summary>
		FrontendMapNavLeft = 0xE0D75B00,
		/// <summary>Control hash <c>0x28725E5D</c>.</summary>
		FrontendMapNavRight = 0x28725E5D,
		/// <summary>Control hash <c>0x6B359A27</c>.</summary>
		FrontendMapZoom = 0x6B359A27,
		/// <summary>Control hash <c>0x43DBF61F</c>.</summary>
		GameMenuAccept = 0x43DBF61F,
		/// <summary>Control hash <c>0x308588E6</c>.</summary>
		GameMenuCancel = 0x308588E6,
		/// <summary>Control hash <c>0xFBD7B3E6</c>.</summary>
		GameMenuOption = 0xFBD7B3E6,
		/// <summary>Control hash <c>0xD596CFB0</c>.</summary>
		GameMenuExtraOption = 0xD596CFB0,
		/// <summary>Control hash <c>0x911CB09E</c>.</summary>
		GameMenuUp = 0x911CB09E,
		/// <summary>Control hash <c>0x4403F97F</c>.</summary>
		GameMenuDown = 0x4403F97F,
		/// <summary>Control hash <c>0xAD7FCC5B</c>.</summary>
		GameMenuLeft = 0xAD7FCC5B,
		/// <summary>Control hash <c>0x65F9EC5B</c>.</summary>
		GameMenuRight = 0x65F9EC5B,
		/// <summary>Control hash <c>0xCBD5B26E</c>.</summary>
		GameMenuTabLeft = 0xCBD5B26E,
		/// <summary>Control hash <c>0x110AD1D2</c>.</summary>
		GameMenuTabRight = 0x110AD1D2,
		/// <summary>Control hash <c>0x26E9DC00</c>.</summary>
		GameMenuTabLeftSecondary = 0x26E9DC00,
		/// <summary>Control hash <c>0x8CC9CD42</c>.</summary>
		GameMenuTabRightSecondary = 0x8CC9CD42,
		/// <summary>Control hash <c>0x81457A1A</c>.</summary>
		GameMenuScrollForward = 0x81457A1A,
		/// <summary>Control hash <c>0x9DA42644</c>.</summary>
		GameMenuScrollBackward = 0x9DA42644,
		/// <summary>Control hash <c>0x9CA97399</c>.</summary>
		GameMenuStickUp = 0x9CA97399,
		/// <summary>Control hash <c>0x63898D36</c>.</summary>
		GameMenuStickDown = 0x63898D36,
		/// <summary>Control hash <c>0x06C089D4</c>.</summary>
		GameMenuStickLeft = 0x06C089D4,
		/// <summary>Control hash <c>0x5BDBE841</c>.</summary>
		GameMenuStickRight = 0x5BDBE841,
		/// <summary>Control hash <c>0xF0232A03</c>.</summary>
		GameMenuRightStickUp = 0xF0232A03,
		/// <summary>Control hash <c>0xADB78673</c>.</summary>
		GameMenuRightStickDown = 0xADB78673,
		/// <summary>Control hash <c>0x71E38966</c>.</summary>
		GameMenuRightStickLeft = 0x71E38966,
		/// <summary>Control hash <c>0xE1CECE4B</c>.</summary>
		GameMenuRightStickRight = 0xE1CECE4B,
		/// <summary>Control hash <c>0xA8F6DE66</c>.</summary>
		GameMenuLs = 0xA8F6DE66,
		/// <summary>Control hash <c>0x89EA3FA5</c>.</summary>
		GameMenuRs = 0x89EA3FA5,
		/// <summary>Control hash <c>0x4685AA33</c>.</summary>
		GameMenuRightAxisX = 0x4685AA33,
		/// <summary>Control hash <c>0x60C65EB4</c>.</summary>
		GameMenuRightAxisY = 0x60C65EB4,
		/// <summary>Control hash <c>0xF431D57A</c>.</summary>
		GameMenuLeftAxisX = 0xF431D57A,
		/// <summary>Control hash <c>0x226EB1EF</c>.</summary>
		GameMenuLeftAxisY = 0x226EB1EF,
		/// <summary>Control hash <c>0x8E90C7BB</c>.</summary>
		Quit = 0x8E90C7BB,
		/// <summary>Control hash <c>0xC97792B7</c>.</summary>
		DocumentPageNext = 0xC97792B7,
		/// <summary>Control hash <c>0x20190AB4</c>.</summary>
		DocumentPagePrev = 0x20190AB4,
		/// <summary>Control hash <c>0xAC70F311</c>.</summary>
		DocumentScroll = 0xAC70F311,
		/// <summary>Control hash <c>0x3D0C19EC</c>.</summary>
		DocumentScrollUpOnly = 0x3D0C19EC,
		/// <summary>Control hash <c>0xD72F3E29</c>.</summary>
		DocumentScrollDownOnly = 0xD72F3E29,
		/// <summary>Control hash <c>0x0283C582</c>.</summary>
		Attack2 = 0x0283C582,
		/// <summary>Control hash <c>0xCC1075A7</c>.</summary>
		PrevWeapon = 0xCC1075A7,
		/// <summary>Control hash <c>0xFD0F0C2C</c>.</summary>
		NextWeapon = 0xFD0F0C2C,
		/// <summary>Control hash <c>0xE4568AA1</c>.</summary>
		SniperZoomIn = 0xE4568AA1,
		/// <summary>Control hash <c>0xE40CE39E</c>.</summary>
		SniperZoomOut = 0xE40CE39E,
		/// <summary>Control hash <c>0x3A9897C1</c>.</summary>
		SniperZoomInAlternate = 0x3A9897C1,
		/// <summary>Control hash <c>0xBC820489</c>.</summary>
		SniperZoomOutAlternate = 0xBC820489,
		/// <summary>Control hash <c>0xDCA6978E</c>.</summary>
		ReplayStartStopRecording = 0xDCA6978E,
		/// <summary>Control hash <c>0x8991A70B</c>.</summary>
		ReplayStartStopRecordingSecondary = 0x8991A70B,
		/// <summary>Control hash <c>0xC7D2C51B</c>.</summary>
		ReplayMarkerDelete = 0xC7D2C51B,
		/// <summary>Control hash <c>0xF6734E42</c>.</summary>
		ReplayClipDelete = 0xF6734E42,
		/// <summary>Control hash <c>0x083137B2</c>.</summary>
		ReplayPause = 0x083137B2,
		/// <summary>Control hash <c>0xC1339A31</c>.</summary>
		ReplayRewind = 0xC1339A31,
		/// <summary>Control hash <c>0x609A27E8</c>.</summary>
		ReplayFfwd = 0x609A27E8,
		/// <summary>Control hash <c>0xF7C6DA28</c>.</summary>
		ReplayNewmarker = 0xF7C6DA28,
		/// <summary>Control hash <c>0xAD9A9C7C</c>.</summary>
		ReplayRecord = 0xAD9A9C7C,
		/// <summary>Control hash <c>0x567FAF34</c>.</summary>
		ReplayScreenshot = 0x567FAF34,
		/// <summary>Control hash <c>0x7E479C7B</c>.</summary>
		ReplayHidehud = 0x7E479C7B,
		/// <summary>Control hash <c>0x5DAFACCF</c>.</summary>
		ReplayStartpoint = 0x5DAFACCF,
		/// <summary>Control hash <c>0x4EF75BBD</c>.</summary>
		ReplayEndpoint = 0x4EF75BBD,
		/// <summary>Control hash <c>0x323AA450</c>.</summary>
		ReplayAdvance = 0x323AA450,
		/// <summary>Control hash <c>0x088C7CD4</c>.</summary>
		ReplayBack = 0x088C7CD4,
		/// <summary>Control hash <c>0x561A3387</c>.</summary>
		ReplayTools = 0x561A3387,
		/// <summary>Control hash <c>0x81B8BC9D</c>.</summary>
		ReplayRestart = 0x81B8BC9D,
		/// <summary>Control hash <c>0xEBA2A41E</c>.</summary>
		ReplayShowhotkey = 0xEBA2A41E,
		/// <summary>Control hash <c>0x5C220959</c>.</summary>
		ReplayCyclemarkerleft = 0x5C220959,
		/// <summary>Control hash <c>0xC69AE799</c>.</summary>
		ReplayCyclemarkerright = 0xC69AE799,
		/// <summary>Control hash <c>0x5925A10D</c>.</summary>
		ReplayFovincrease = 0x5925A10D,
		/// <summary>Control hash <c>0x2B88D701</c>.</summary>
		ReplayFovdecrease = 0x2B88D701,
		/// <summary>Control hash <c>0x749EFF0C</c>.</summary>
		ReplayCameraup = 0x749EFF0C,
		/// <summary>Control hash <c>0xA1FE9E2A</c>.</summary>
		ReplayCameradown = 0xA1FE9E2A,
		/// <summary>Control hash <c>0xEBC60685</c>.</summary>
		ReplaySave = 0xEBC60685,
		/// <summary>Control hash <c>0xE3FB91B3</c>.</summary>
		ReplayToggletime = 0xE3FB91B3,
		/// <summary>Control hash <c>0xC8A1DE20</c>.</summary>
		ReplayToggletips = 0xC8A1DE20,
		/// <summary>Control hash <c>0x58AC1355</c>.</summary>
		ReplayPreview = 0x58AC1355,
		/// <summary>Control hash <c>0xF8629909</c>.</summary>
		ReplayToggleTimeline = 0xF8629909,
		/// <summary>Control hash <c>0xD2454F90</c>.</summary>
		ReplayTimelinePickupClip = 0xD2454F90,
		/// <summary>Control hash <c>0x4146A033</c>.</summary>
		ReplayTimelineDuplicateClip = 0x4146A033,
		/// <summary>Control hash <c>0x60726F50</c>.</summary>
		ReplayTimelinePlaceClip = 0x60726F50,
		/// <summary>Control hash <c>0xD88B47E7</c>.</summary>
		ReplayCtrl = 0xD88B47E7,
		/// <summary>Control hash <c>0x65D70E9D</c>.</summary>
		ReplayTimelineSave = 0x65D70E9D,
		/// <summary>Control hash <c>0x79022218</c>.</summary>
		ReplayPreviewAudio = 0x79022218,
		/// <summary>Control hash <c>0xD9961107</c>.</summary>
		ReplayActionReplayStart = 0xD9961107,
		/// <summary>Control hash <c>0x93776CAE</c>.</summary>
		ReplayActionReplayCancel = 0x93776CAE,
		/// <summary>Control hash <c>0xFD28D0F4</c>.</summary>
		ReplayRecordingStart = 0xFD28D0F4,
		/// <summary>Control hash <c>0xDB16E702</c>.</summary>
		ReplayRecordingStop = 0xDB16E702,
		/// <summary>Control hash <c>0xEFEC8FDE</c>.</summary>
		ReplaySaveSnapshot = 0xEFEC8FDE,
		/// <summary>Control hash <c>0xA2117C9A</c>.</summary>
		VehDriveLook = 0xA2117C9A,
		/// <summary>Control hash <c>0x55AC04E5</c>.</summary>
		VehDriveLook2 = 0x55AC04E5,
		/// <summary>Control hash <c>0x4D83147C</c>.</summary>
		VehFlyAttack2 = 0x4D83147C,
		/// <summary>Control hash <c>0x14C7291D</c>.</summary>
		RadioWheelUd = 0x14C7291D,
		/// <summary>Control hash <c>0xF9FA6BC8</c>.</summary>
		RadioWheelLr = 0xF9FA6BC8,
		/// <summary>Control hash <c>0xF1F9CD26</c>.</summary>
		VehSlowmoUd = 0xF1F9CD26,
		/// <summary>Control hash <c>0x2B981F4F</c>.</summary>
		VehSlowmoUpOnly = 0x2B981F4F,
		/// <summary>Control hash <c>0x642DE054</c>.</summary>
		VehSlowmoDownOnly = 0x642DE054,
		/// <summary>Control hash <c>0x9BEE9213</c>.</summary>
		MapPoi = 0x9BEE9213,
		/// <summary>Control hash <c>0xF8982F00</c>.</summary>
		InteractLockon = 0xF8982F00,
		/// <summary>Control hash <c>0x26A18F47</c>.</summary>
		InteractLockonNeg = 0x26A18F47,
		/// <summary>Control hash <c>0xF63A17F9</c>.</summary>
		InteractLockonPos = 0xF63A17F9,
		/// <summary>Control hash <c>0x9FA5AD07</c>.</summary>
		InteractLockonRob = 0x9FA5AD07,
		/// <summary>Control hash <c>0x09A92B8B</c>.</summary>
		InteractLockonY = 0x09A92B8B,
		/// <summary>Control hash <c>0xD10A3A36</c>.</summary>
		InteractLockonA = 0xD10A3A36,
		/// <summary>Control hash <c>0x424BD2D2</c>.</summary>
		InteractNeg = 0x424BD2D2,
		/// <summary>Control hash <c>0xF6BB7378</c>.</summary>
		InteractPos = 0xF6BB7378,
		/// <summary>Control hash <c>0x760A9C6F</c>.</summary>
		InteractOption1 = 0x760A9C6F,
		/// <summary>Control hash <c>0x84543902</c>.</summary>
		InteractOption2 = 0x84543902,
		/// <summary>Control hash <c>0xA1ABB953</c>.</summary>
		InteractAnimal = 0xA1ABB953,
		/// <summary>Control hash <c>0x5415BE48</c>.</summary>
		InteractLockonAnimal = 0x5415BE48,
		/// <summary>Control hash <c>0x17D3BFF5</c>.</summary>
		InteractLeadAnimal = 0x17D3BFF5,
		/// <summary>Control hash <c>0xF5C4701B</c>.</summary>
		InteractLockonDetachHorse = 0xF5C4701B,
		/// <summary>Control hash <c>0xB0BCE5D6</c>.</summary>
		InteractHorseCare = 0xB0BCE5D6,
		/// <summary>Control hash <c>0x71F89BBC</c>.</summary>
		InteractLockonCallAnimal = 0x71F89BBC,
		/// <summary>Control hash <c>0xE2473BF0</c>.</summary>
		InteractLockonTrackAnimal = 0xE2473BF0,
		/// <summary>Control hash <c>0x31219490</c>.</summary>
		InteractLockonTargetInfo = 0x31219490,
		/// <summary>Control hash <c>0xB3F388BC</c>.</summary>
		InteractLockonStudyBinoculars = 0xB3F388BC,
		/// <summary>Control hash <c>0x89F3D2E0</c>.</summary>
		InteractWildAnimal = 0x89F3D2E0,
		/// <summary>Control hash <c>0x0D55A0F0</c>.</summary>
		InteractHorseFeed = 0x0D55A0F0,
		/// <summary>Control hash <c>0x63A38F2C</c>.</summary>
		InteractHorseBrush = 0x63A38F2C,
		/// <summary>Control hash <c>0x13C42BB2</c>.</summary>
		EmoteAction = 0x13C42BB2,
		/// <summary>Control hash <c>0x470DC190</c>.</summary>
		EmoteTaunt = 0x470DC190,
		/// <summary>Control hash <c>0x72BAD5AA</c>.</summary>
		EmoteGreet = 0x72BAD5AA,
		/// <summary>Control hash <c>0x661857B3</c>.</summary>
		EmoteComm = 0x661857B3,
		/// <summary>Control hash <c>0xF311100C</c>.</summary>
		EmoteDance = 0xF311100C,
		/// <summary>Control hash <c>0x04FB8191</c>.</summary>
		EmoteTwirlGunHold = 0x04FB8191,
		/// <summary>Control hash <c>0x6990BDDF</c>.</summary>
		EmoteTwirlGunVarA = 0x6990BDDF,
		/// <summary>Control hash <c>0x52D29063</c>.</summary>
		EmoteTwirlGunVarB = 0x52D29063,
		/// <summary>Control hash <c>0xBC2AE312</c>.</summary>
		EmoteTwirlGunVarC = 0xBC2AE312,
		/// <summary>Control hash <c>0xAE69478F</c>.</summary>
		EmoteTwirlGunVarD = 0xAE69478F,
		/// <summary>Control hash <c>0x6070D032</c>.</summary>
		QuickEquipItem = 0x6070D032,
		/// <summary>Control hash <c>0x16B0EEF8</c>.</summary>
		MinigameBuildingCameraNext = 0x16B0EEF8,
		/// <summary>Control hash <c>0x5F97B231</c>.</summary>
		MinigameBuildingCameraPrev = 0x5F97B231,
		/// <summary>Control hash <c>0xFA91AECD</c>.</summary>
		MinigameBuildingHammer = 0xFA91AECD,
		/// <summary>Control hash <c>0x1C559F2E</c>.</summary>
		CursorAcceptDoubleClick = 0x1C559F2E,
		/// <summary>Control hash <c>0xE474F150</c>.</summary>
		CursorAcceptHold = 0xE474F150,
		/// <summary>Control hash <c>0x9D2AEA88</c>.</summary>
		CursorAccept = 0x9D2AEA88,
		/// <summary>Control hash <c>0x27568539</c>.</summary>
		CursorCancel = 0x27568539,
		/// <summary>Control hash <c>0x9CB4ECCE</c>.</summary>
		CursorCancelDoubleClick = 0x9CB4ECCE,
		/// <summary>Control hash <c>0xD7F70F36</c>.</summary>
		CursorCancelHold = 0xD7F70F36,
		/// <summary>Control hash <c>0xD6C4ECDC</c>.</summary>
		CursorX = 0xD6C4ECDC,
		/// <summary>Control hash <c>0xE4130778</c>.</summary>
		CursorY = 0xE4130778,
		/// <summary>Control hash <c>0x62800C92</c>.</summary>
		CursorScrollUp = 0x62800C92,
		/// <summary>Control hash <c>0x8BDE7443</c>.</summary>
		CursorScrollDown = 0x8BDE7443,
		/// <summary>Control hash <c>0x6AA8A71B</c>.</summary>
		CursorScrollClick = 0x6AA8A71B,
		/// <summary>Control hash <c>0xE1B6ED6D</c>.</summary>
		CursorScrollDoubleClick = 0xE1B6ED6D,
		/// <summary>Control hash <c>0x5484DBDD</c>.</summary>
		CursorScrollHold = 0x5484DBDD,
		/// <summary>Control hash <c>0x11DBBAB9</c>.</summary>
		CursorForwardClick = 0x11DBBAB9,
		/// <summary>Control hash <c>0x9805D715</c>.</summary>
		CursorForwardDoubleClick = 0x9805D715,
		/// <summary>Control hash <c>0x7630C9A1</c>.</summary>
		CursorForwardHold = 0x7630C9A1,
		/// <summary>Control hash <c>0x9AF38793</c>.</summary>
		CursorBackwardClick = 0x9AF38793,
		/// <summary>Control hash <c>0xA14BA1FC</c>.</summary>
		CursorBackwardDoubleClick = 0xA14BA1FC,
		/// <summary>Control hash <c>0x01AA9FA1</c>.</summary>
		CursorBackwardHold = 0x01AA9FA1,
		/// <summary>Control hash <c>0x7BF65AC8</c>.</summary>
		EnterCheatCode = 0x7BF65AC8,
		/// <summary>Control hash <c>0xCC510E59</c>.</summary>
		InteractionMenu = 0xCC510E59,
		/// <summary>Control hash <c>0x9720FCEE</c>.</summary>
		MpTextChatAll = 0x9720FCEE,
		/// <summary>Control hash <c>0x9098AD9D</c>.</summary>
		MpTextChatTeam = 0x9098AD9D,
		/// <summary>Control hash <c>0x7098AC73</c>.</summary>
		MpTextChatFriends = 0x7098AC73,
		/// <summary>Control hash <c>0x8142FA92</c>.</summary>
		MpTextChatCrew = 0x8142FA92,
		/// <summary>Control hash <c>0x4BC9DABB</c>.</summary>
		PushToTalk = 0x4BC9DABB,
		/// <summary>Control hash <c>0x339F3730</c>.</summary>
		CreatorLs = 0x339F3730,
		/// <summary>Control hash <c>0xD8CF0C95</c>.</summary>
		CreatorRs = 0xD8CF0C95,
		/// <summary>Control hash <c>0x446258B6</c>.</summary>
		CreatorLt = 0x446258B6,
		/// <summary>Control hash <c>0x3C3DD371</c>.</summary>
		CreatorRt = 0x3C3DD371,
		/// <summary>Control hash <c>0x85D24405</c>.</summary>
		CreatorMenuToggle = 0x85D24405,
		/// <summary>Control hash <c>0x2CD5343E</c>.</summary>
		CreatorAccept = 0x2CD5343E,
		/// <summary>Control hash <c>0xBCD1444B</c>.</summary>
		CreatorMenuUp = 0xBCD1444B,
		/// <summary>Control hash <c>0x97410755</c>.</summary>
		CreatorMenuDown = 0x97410755,
		/// <summary>Control hash <c>0xEC6A30AA</c>.</summary>
		CreatorMenuLeft = 0xEC6A30AA,
		/// <summary>Control hash <c>0x19D8334C</c>.</summary>
		CreatorMenuRight = 0x19D8334C,
		/// <summary>Control hash <c>0xFB9C3231</c>.</summary>
		CreatorMenuAccept = 0xFB9C3231,
		/// <summary>Control hash <c>0xBB3FC460</c>.</summary>
		CreatorMenuCancel = 0xBB3FC460,
		/// <summary>Control hash <c>0x5A03B3F3</c>.</summary>
		CreatorMenuFunction = 0x5A03B3F3,
		/// <summary>Control hash <c>0xE6B8F103</c>.</summary>
		CreatorMenuExtraFunction = 0xE6B8F103,
		/// <summary>Control hash <c>0x0984E40A</c>.</summary>
		CreatorMenuSelect = 0x0984E40A,
		/// <summary>Control hash <c>0xD74CACAD</c>.</summary>
		CreatorPlace = 0xD74CACAD,
		/// <summary>Control hash <c>0x3F4DC0EF</c>.</summary>
		CreatorDelete = 0x3F4DC0EF,
		/// <summary>Control hash <c>0x414034D5</c>.</summary>
		CreatorDrop = 0x414034D5,
		/// <summary>Control hash <c>0xB05FDA25</c>.</summary>
		CreatorFunction = 0xB05FDA25,
		/// <summary>Control hash <c>0x9D75674E</c>.</summary>
		CreatorRotateRight = 0x9D75674E,
		/// <summary>Control hash <c>0xD41E9C2A</c>.</summary>
		CreatorRotateLeft = 0xD41E9C2A,
		/// <summary>Control hash <c>0x338A0D45</c>.</summary>
		CreatorGrab = 0x338A0D45,
		/// <summary>Control hash <c>0x16CCFEC6</c>.</summary>
		CreatorSwitchCam = 0x16CCFEC6,
		/// <summary>Control hash <c>0x335D8D76</c>.</summary>
		CreatorZoomIn = 0x335D8D76,
		/// <summary>Control hash <c>0x24A42F93</c>.</summary>
		CreatorZoomOut = 0x24A42F93,
		/// <summary>Control hash <c>0x0D0FB9B1</c>.</summary>
		CreatorRaise = 0x0D0FB9B1,
		/// <summary>Control hash <c>0x1BDE2EB3</c>.</summary>
		CreatorLower = 0x1BDE2EB3,
		/// <summary>Control hash <c>0xF55864CD</c>.</summary>
		CreatorSearch = 0xF55864CD,
		/// <summary>Control hash <c>0x82428676</c>.</summary>
		CreatorMoveUd = 0x82428676,
		/// <summary>Control hash <c>0x59753EDC</c>.</summary>
		CreatorMoveLr = 0x59753EDC,
		/// <summary>Control hash <c>0x55EA24F3</c>.</summary>
		CreatorLookUd = 0x55EA24F3,
		/// <summary>Control hash <c>0xAEB2A9C7</c>.</summary>
		CreatorLookLr = 0xAEB2A9C7,
		/// <summary>Control hash <c>0xD2CC4644</c>.</summary>
		CutFree = 0xD2CC4644,
		/// <summary>Control hash <c>0xD2928083</c>.</summary>
		Drop = 0xD2928083,
		/// <summary>Control hash <c>0xEB2AC491</c>.</summary>
		PickupCarriable = 0xEB2AC491,
		/// <summary>Control hash <c>0xBE8593AF</c>.</summary>
		PickupCarriable2 = 0xBE8593AF,
		/// <summary>Control hash <c>0x7D326951</c>.</summary>
		PlaceCarriableOntoParent = 0x7D326951,
		/// <summary>Control hash <c>0xA1202C7B</c>.</summary>
		PickupCarriableFromParent = 0xA1202C7B,
		/// <summary>Control hash <c>0x956C2A0E</c>.</summary>
		MercyKill = 0x956C2A0E,
		/// <summary>Control hash <c>0x43F2959C</c>.</summary>
		Revive = 0x43F2959C,
		/// <summary>Control hash <c>0xD9C50532</c>.</summary>
		Hogtie = 0xD9C50532,
		/// <summary>Control hash <c>0x6E9734E8</c>.</summary>
		CarriableSuicide = 0x6E9734E8,
		/// <summary>Control hash <c>0x295175BF</c>.</summary>
		CarriableBreakFree = 0x295175BF,
		/// <summary>Control hash <c>0x0522B243</c>.</summary>
		InteractHitCarriable = 0x0522B243,
		/// <summary>Control hash <c>0x41AC83D1</c>.</summary>
		Loot = 0x41AC83D1,
		/// <summary>Control hash <c>0x399C6619</c>.</summary>
		Loot2 = 0x399C6619,
		/// <summary>Control hash <c>0x27D1C284</c>.</summary>
		Loot3 = 0x27D1C284,
		/// <summary>Control hash <c>0x14DB6C5E</c>.</summary>
		LootVehicle = 0x14DB6C5E,
		/// <summary>Control hash <c>0xC23D7B9E</c>.</summary>
		LootAmmo = 0xC23D7B9E,
		/// <summary>Control hash <c>0x97C71B28</c>.</summary>
		BreakVehicleLock = 0x97C71B28,
		/// <summary>Control hash <c>0xFF8109D8</c>.</summary>
		LootAliveComponent = 0xFF8109D8,
		/// <summary>Control hash <c>0xA8E3F467</c>.</summary>
		FeedInteract = 0xA8E3F467,
		/// <summary>Control hash <c>0x73A8FD83</c>.</summary>
		SaddleTransfer = 0x73A8FD83,
		/// <summary>Control hash <c>0xDFF812F9</c>.</summary>
		ShopBuy = 0xDFF812F9,
		/// <summary>Control hash <c>0x6D1319BE</c>.</summary>
		ShopSell = 0x6D1319BE,
		/// <summary>Control hash <c>0xEA150E72</c>.</summary>
		ShopSpecial = 0xEA150E72,
		/// <summary>Control hash <c>0xD3ECF82F</c>.</summary>
		ShopBounty = 0xD3ECF82F,
		/// <summary>Control hash <c>0x5E723D8C</c>.</summary>
		ShopInspect = 0x5E723D8C,
		/// <summary>Control hash <c>0x90FA19AB</c>.</summary>
		ShopChangeCurrency = 0x90FA19AB,
		/// <summary>Control hash <c>0xC1989F95</c>.</summary>
		QuickUseItem = 0xC1989F95,
		/// <summary>Control hash <c>0x8CF90A9D</c>.</summary>
		PromptPageNext = 0x8CF90A9D,
		/// <summary>Control hash <c>0xE7F89C38</c>.</summary>
		FrontendTouchZoomFactor = 0xE7F89C38,
		/// <summary>Control hash <c>0x16661AD0</c>.</summary>
		FrontendTouchZoomX = 0x16661AD0,
		/// <summary>Control hash <c>0x253DB87F</c>.</summary>
		FrontendTouchZoomY = 0x253DB87F,
		/// <summary>Control hash <c>0xEC93548E</c>.</summary>
		FrontendTouchDragX = 0xEC93548E,
		/// <summary>Control hash <c>0x9AC130EB</c>.</summary>
		FrontendTouchDragY = 0x9AC130EB,
		/// <summary>Control hash <c>0xC10E180A</c>.</summary>
		FrontendTouchTapX = 0xC10E180A,
		/// <summary>Control hash <c>0xCF4B3484</c>.</summary>
		FrontendTouchTapY = 0xCF4B3484,
		/// <summary>Control hash <c>0x1661FAB0</c>.</summary>
		FrontendTouchDoubleTapX = 0x1661FAB0,
		/// <summary>Control hash <c>0x96E87BBF</c>.</summary>
		FrontendTouchDoubleTapY = 0x96E87BBF,
		/// <summary>Control hash <c>0x0FF17F1D</c>.</summary>
		FrontendTouchHoldX = 0x0FF17F1D,
		/// <summary>Control hash <c>0x398ED257</c>.</summary>
		FrontendTouchHoldY = 0x398ED257,
		/// <summary>Control hash <c>0x0B71D439</c>.</summary>
		FrontendTouchSwipeUpX = 0x0B71D439,
		/// <summary>Control hash <c>0x19CA70EA</c>.</summary>
		FrontendTouchSwipeUpY = 0x19CA70EA,
		/// <summary>Control hash <c>0xE3B30955</c>.</summary>
		FrontendTouchSwipeDownX = 0xE3B30955,
		/// <summary>Control hash <c>0xBDFF3DEA</c>.</summary>
		FrontendTouchSwipeDownY = 0xBDFF3DEA,
		/// <summary>Control hash <c>0x2545B0DE</c>.</summary>
		FrontendTouchSwipeLeftX = 0x2545B0DE,
		/// <summary>Control hash <c>0xD43D0ECE</c>.</summary>
		FrontendTouchSwipeLeftY = 0xD43D0ECE,
		/// <summary>Control hash <c>0xEAB68397</c>.</summary>
		FrontendTouchSwipeRightX = 0xEAB68397,
		/// <summary>Control hash <c>0x675B7CE3</c>.</summary>
		FrontendTouchSwipeRightY = 0x675B7CE3,
		/// <summary>Control hash <c>0x9C68CE34</c>.</summary>
		MultiplayerInfoPlayers = 0x9C68CE34,
		/// <summary>Control hash <c>0xB4F298BA</c>.</summary>
		MultiplayerDeadSwitchRespawn = 0xB4F298BA,
		/// <summary>Control hash <c>0x6816A38E</c>.</summary>
		MultiplayerDeadInformLaw = 0x6816A38E,
		/// <summary>Control hash <c>0x18987353</c>.</summary>
		MultiplayerDeadRespawn = 0x18987353,
		/// <summary>Control hash <c>0xF875FC78</c>.</summary>
		MultiplayerDeadDuel = 0xF875FC78,
		/// <summary>Control hash <c>0x4D11FE01</c>.</summary>
		MultiplayerDeadParley = 0x4D11FE01,
		/// <summary>Control hash <c>0xB4A11066</c>.</summary>
		MultiplayerDeadFeud = 0xB4A11066,
		/// <summary>Control hash <c>0xCC18F960</c>.</summary>
		MultiplayerDeadLeaderFeud = 0xCC18F960,
		/// <summary>Control hash <c>0xE50DCA13</c>.</summary>
		MultiplayerDeadPressCharges = 0xE50DCA13,
		/// <summary>Control hash <c>0x014CA044</c>.</summary>
		MultiplayerRaceRespawn = 0x014CA044,
		/// <summary>Control hash <c>0xC5CF41B2</c>.</summary>
		MultiplayerPredatorAbility = 0xC5CF41B2,
		/// <summary>Control hash <c>0xBA065692</c>.</summary>
		MultiplayerSpectatePlayerNext = 0xBA065692,
		/// <summary>Control hash <c>0x5092BF47</c>.</summary>
		MultiplayerSpectatePlayerPrev = 0x5092BF47,
		/// <summary>Control hash <c>0x7DBA5D49</c>.</summary>
		MultiplayerSpectateHideHud = 0x7DBA5D49,
		/// <summary>Control hash <c>0x4E074EE6</c>.</summary>
		MultiplayerSpectatePlayerOptions = 0x4E074EE6,
		/// <summary>Control hash <c>0xA917D24B</c>.</summary>
		MultiplayerLeaderboardScrollUd = 0xA917D24B,
		/// <summary>Control hash <c>0xE9094BA0</c>.</summary>
		MinigameQuit = 0xE9094BA0,
		/// <summary>Control hash <c>0xC7CB8D5F</c>.</summary>
		MinigameIncreaseBet = 0xC7CB8D5F,
		/// <summary>Control hash <c>0xD3EBF425</c>.</summary>
		MinigameDecreaseBet = 0xD3EBF425,
		/// <summary>Control hash <c>0xBDC733EE</c>.</summary>
		MinigameChangeBetAxisY = 0xBDC733EE,
		/// <summary>Control hash <c>0x410B0B2E</c>.</summary>
		MinigamePlaceBet = 0x410B0B2E,
		/// <summary>Control hash <c>0x4A21C66B</c>.</summary>
		MinigameClearBet = 0x4A21C66B,
		/// <summary>Control hash <c>0x9384E0A8</c>.</summary>
		MinigameHelp = 0x9384E0A8,
		/// <summary>Control hash <c>0xC5F53156</c>.</summary>
		MinigameHelpPrev = 0xC5F53156,
		/// <summary>Control hash <c>0x83608AC0</c>.</summary>
		MinigameHelpNext = 0x83608AC0,
		/// <summary>Control hash <c>0x985243B7</c>.</summary>
		MinigameReplay = 0x985243B7,
		/// <summary>Control hash <c>0x5D1788FF</c>.</summary>
		MinigameNewGame = 0x5D1788FF,
		/// <summary>Control hash <c>0x646A7792</c>.</summary>
		MinigamePokerSkip = 0x646A7792,
		/// <summary>Control hash <c>0xDAB9EE72</c>.</summary>
		MinigamePokerCall = 0xDAB9EE72,
		/// <summary>Control hash <c>0x49B4AD1E</c>.</summary>
		MinigamePokerFold = 0x49B4AD1E,
		/// <summary>Control hash <c>0x206B2087</c>.</summary>
		MinigamePokerCheck = 0x206B2087,
		/// <summary>Control hash <c>0x72A9D1F7</c>.</summary>
		MinigamePokerCheckFold = 0x72A9D1F7,
		/// <summary>Control hash <c>0xA9883369</c>.</summary>
		MinigamePokerBet = 0xA9883369,
		/// <summary>Control hash <c>0xC2B1193A</c>.</summary>
		MinigamePokerHoleCards = 0xC2B1193A,
		/// <summary>Control hash <c>0x03753498</c>.</summary>
		MinigamePokerBoardCards = 0x03753498,
		/// <summary>Control hash <c>0xB568BCD0</c>.</summary>
		MinigamePokerSkipTutorial = 0xB568BCD0,
		/// <summary>Control hash <c>0x7765B9D4</c>.</summary>
		MinigamePokerShowPossibleHands = 0x7765B9D4,
		/// <summary>Control hash <c>0xF923B337</c>.</summary>
		MinigamePokerYourCards = 0xF923B337,
		/// <summary>Control hash <c>0xE402B898</c>.</summary>
		MinigamePokerCommunityCards = 0xE402B898,
		/// <summary>Control hash <c>0x2330F517</c>.</summary>
		MinigamePokerCheatLr = 0x2330F517,
		/// <summary>Control hash <c>0xB40A9BDB</c>.</summary>
		MinigameFishingResetCast = 0xB40A9BDB,
		/// <summary>Control hash <c>0xF14FD435</c>.</summary>
		MinigameFishingReleaseFish = 0xF14FD435,
		/// <summary>Control hash <c>0x52C5C34A</c>.</summary>
		MinigameFishingKeepFish = 0x52C5C34A,
		/// <summary>Control hash <c>0xA1CD103A</c>.</summary>
		MinigameFishingHook = 0xA1CD103A,
		/// <summary>Control hash <c>0x69B10623</c>.</summary>
		MinigameFishingLeftAxisX = 0x69B10623,
		/// <summary>Control hash <c>0x09BF4645</c>.</summary>
		MinigameFishingLeftAxisY = 0x09BF4645,
		/// <summary>Control hash <c>0x4FD4E558</c>.</summary>
		MinigameFishingRightAxisX = 0x4FD4E558,
		/// <summary>Control hash <c>0x95F2F193</c>.</summary>
		MinigameFishingRightAxisY = 0x95F2F193,
		/// <summary>Control hash <c>0x0D4C3ABA</c>.</summary>
		MinigameFishingLeanLeft = 0x0D4C3ABA,
		/// <summary>Control hash <c>0x05074A9B</c>.</summary>
		MinigameFishingLeanRight = 0x05074A9B,
		/// <summary>Control hash <c>0x25F525CD</c>.</summary>
		MinigameFishingQuickEquip = 0x25F525CD,
		/// <summary>Control hash <c>0x2FA915F5</c>.</summary>
		MinigameFishingReelSpeedUp = 0x2FA915F5,
		/// <summary>Control hash <c>0xD7AF56A0</c>.</summary>
		MinigameFishingReelSpeedDown = 0xD7AF56A0,
		/// <summary>Control hash <c>0x49C73CB2</c>.</summary>
		MinigameFishingReelSpeedAxis = 0x49C73CB2,
		/// <summary>Control hash <c>0xA303F462</c>.</summary>
		MinigameFishingManualReelIn = 0xA303F462,
		/// <summary>Control hash <c>0x4556642C</c>.</summary>
		MinigameFishingManualReelOutModifier = 0x4556642C,
		/// <summary>Control hash <c>0x524C3787</c>.</summary>
		MinigameCrackpotBoatShowControls = 0x524C3787,
		/// <summary>Control hash <c>0x88F8B6B1</c>.</summary>
		MinigameDominoesViewDominoes = 0x88F8B6B1,
		/// <summary>Control hash <c>0x7733CF2C</c>.</summary>
		MinigameDominoesViewMoves = 0x7733CF2C,
		/// <summary>Control hash <c>0x95F5BB7C</c>.</summary>
		MinigameDominoesPlayTile = 0x95F5BB7C,
		/// <summary>Control hash <c>0xC5E622D7</c>.</summary>
		MinigameDominoesSkipDeal = 0xC5E622D7,
		/// <summary>Control hash <c>0xFDDD89D4</c>.</summary>
		MinigameDominoesMoveLeftOnly = 0xFDDD89D4,
		/// <summary>Control hash <c>0x7D5187C9</c>.</summary>
		MinigameDominoesMoveRightOnly = 0x7D5187C9,
		/// <summary>Control hash <c>0xC6AB8CB3</c>.</summary>
		MinigameDominoesMoveUpOnly = 0xC6AB8CB3,
		/// <summary>Control hash <c>0xFD9FC86D</c>.</summary>
		MinigameDominoesMoveDownOnly = 0xFD9FC86D,
		/// <summary>Control hash <c>0x03F1E7CB</c>.</summary>
		MinigameBlackjackHandView = 0x03F1E7CB,
		/// <summary>Control hash <c>0xADE09435</c>.</summary>
		MinigameBlackjackTableView = 0xADE09435,
		/// <summary>Control hash <c>0x3D2EA092</c>.</summary>
		MinigameBlackjackBetAxisY = 0x3D2EA092,
		/// <summary>Control hash <c>0x661D8A31</c>.</summary>
		MinigameBlackjackBet = 0x661D8A31,
		/// <summary>Control hash <c>0xCD7DDF9B</c>.</summary>
		MinigameBlackjackDecline = 0xCD7DDF9B,
		/// <summary>Control hash <c>0x31260507</c>.</summary>
		MinigameBlackjackStand = 0x31260507,
		/// <summary>Control hash <c>0xA8142713</c>.</summary>
		MinigameBlackjackHit = 0xA8142713,
		/// <summary>Control hash <c>0x74486CA4</c>.</summary>
		MinigameBlackjackDouble = 0x74486CA4,
		/// <summary>Control hash <c>0x432B111F</c>.</summary>
		MinigameBlackjackSplit = 0x432B111F,
		/// <summary>Control hash <c>0x0E717DC6</c>.</summary>
		MinigameFffA = 0x0E717DC6,
		/// <summary>Control hash <c>0x1BC81873</c>.</summary>
		MinigameFffB = 0x1BC81873,
		/// <summary>Control hash <c>0x65F0ACDF</c>.</summary>
		MinigameFffX = 0x65F0ACDF,
		/// <summary>Control hash <c>0x73AD4858</c>.</summary>
		MinigameFffY = 0x73AD4858,
		/// <summary>Control hash <c>0x61E4CACC</c>.</summary>
		MinigameFffZoom = 0x61E4CACC,
		/// <summary>Control hash <c>0x3073681B</c>.</summary>
		MinigameFffSkipTurn = 0x3073681B,
		/// <summary>Control hash <c>0x29A3550E</c>.</summary>
		MinigameFffCycleSequenceLeft = 0x29A3550E,
		/// <summary>Control hash <c>0x7B5B896D</c>.</summary>
		MinigameFffCycleSequenceRight = 0x7B5B896D,
		/// <summary>Control hash <c>0x6FC9DE68</c>.</summary>
		MinigameFffFlourishContinue = 0x6FC9DE68,
		/// <summary>Control hash <c>0xF7750B25</c>.</summary>
		MinigameFffFlourishEnd = 0xF7750B25,
		/// <summary>Control hash <c>0xCA379F82</c>.</summary>
		MinigameFffPractice = 0xCA379F82,
		/// <summary>Control hash <c>0xFF4B2ADA</c>.</summary>
		MinigameMilkingLeftAction = 0xFF4B2ADA,
		/// <summary>Control hash <c>0x30BE7CF2</c>.</summary>
		MinigameMilkingRightAction = 0x30BE7CF2,
		/// <summary>Control hash <c>0x7EC33553</c>.</summary>
		MinigameLeftTrigger = 0x7EC33553,
		/// <summary>Control hash <c>0xBE78B715</c>.</summary>
		MinigameRightTrigger = 0xBE78B715,
		/// <summary>Control hash <c>0x0A1EFC09</c>.</summary>
		MinigameActionLeft = 0x0A1EFC09,
		/// <summary>Control hash <c>0x16D70379</c>.</summary>
		MinigameActionRight = 0x16D70379,
		/// <summary>Control hash <c>0xF5A13A0D</c>.</summary>
		MinigameActionUp = 0xF5A13A0D,
		/// <summary>Control hash <c>0xF601BCFC</c>.</summary>
		MinigameActionDown = 0xF601BCFC,
		/// <summary>Control hash <c>0xF4DD4C67</c>.</summary>
		StickyFeedAccept = 0xF4DD4C67,
		/// <summary>Control hash <c>0x0CFB963F</c>.</summary>
		StickyFeedCancel = 0x0CFB963F,
		/// <summary>Control hash <c>0xBD1D94A1</c>.</summary>
		StickyFeedX = 0xBD1D94A1,
		/// <summary>Control hash <c>0xC85BAB1D</c>.</summary>
		StickyFeedY = 0xC85BAB1D,
		/// <summary>Control hash <c>0x5FC770EA</c>.</summary>
		CameraPutAway = 0x5FC770EA,
		/// <summary>Control hash <c>0xA4BD74A5</c>.</summary>
		CameraBack = 0xA4BD74A5,
		/// <summary>Control hash <c>0x44FA14C2</c>.</summary>
		CameraTakePhoto = 0x44FA14C2,
		/// <summary>Control hash <c>0xE8337356</c>.</summary>
		CameraContextGallery = 0xE8337356,
		/// <summary>Control hash <c>0x776F65E9</c>.</summary>
		CameraHandheldUse = 0x776F65E9,
		/// <summary>Control hash <c>0x3003F9DC</c>.</summary>
		CameraDof = 0x3003F9DC,
		/// <summary>Control hash <c>0xAC5922EA</c>.</summary>
		CameraSelfie = 0xAC5922EA,
		/// <summary>Control hash <c>0x47EC4C22</c>.</summary>
		CameraZoom = 0x47EC4C22,
		/// <summary>Control hash <c>0xF810FB35</c>.</summary>
		CameraPoseNext = 0xF810FB35,
		/// <summary>Control hash <c>0x8D5BE9D1</c>.</summary>
		CameraPosePrev = 0x8D5BE9D1,
		/// <summary>Control hash <c>0xCFA703D3</c>.</summary>
		CameraExpressionNext = 0xCFA703D3,
		/// <summary>Control hash <c>0x07B6435D</c>.</summary>
		CameraExpressionPrev = 0x07B6435D,
		/// <summary>Control hash <c>0x24F37AB5</c>.</summary>
		TithingIncreaseAmount = 0x24F37AB5,
		/// <summary>Control hash <c>0xCEFF5C13</c>.</summary>
		TithingDecreaseAmount = 0xCEFF5C13,
		/// <summary>Control hash <c>0x77110B0A</c>.</summary>
		BreakDoorLock = 0x77110B0A,
		/// <summary>Control hash <c>0xA1AA2D8D</c>.</summary>
		InterrogateQuestion = 0xA1AA2D8D,
		/// <summary>Control hash <c>0x6E1E0D62</c>.</summary>
		InterrogateBeat = 0x6E1E0D62,
		/// <summary>Control hash <c>0x81B2E311</c>.</summary>
		InterrogateKill = 0x81B2E311,
		/// <summary>Control hash <c>0x3C22EF0E</c>.</summary>
		InterrogateRelease = 0x3C22EF0E,
		/// <summary>Control hash <c>0xC67E13BB</c>.</summary>
		CampBedInspect = 0xC67E13BB,
		/// <summary>Control hash <c>0x8AAA0AD4</c>.</summary>
		PcFreeLook = 0x8AAA0AD4,
		/// <summary>Control hash <c>0xA13460F5</c>.</summary>
		MinigameBartenderRaiseGlass = 0xA13460F5,
		/// <summary>Control hash <c>0xF0A25112</c>.</summary>
		MinigameBartenderRaiseBottle = 0xF0A25112,
		/// <summary>Control hash <c>0xCABC2460</c>.</summary>
		MinigameBartenderPour = 0xCABC2460,
		/// <summary>Control hash <c>0xDC03B043</c>.</summary>
		MinigameBartenderServe = 0xDC03B043,
		/// <summary>Control hash <c>0x3C0A40F2</c>.</summary>
		PhotoMode = 0x3C0A40F2,
		/// <summary>Control hash <c>0x35957F6C</c>.</summary>
		PhotoModePc = 0x35957F6C,
		/// <summary>Control hash <c>0x9F06B29C</c>.</summary>
		PhotoModeChangeCamera = 0x9F06B29C,
		/// <summary>Control hash <c>0x4F136512</c>.</summary>
		PhotoModeMoveLr = 0x4F136512,
		/// <summary>Control hash <c>0x311353EB</c>.</summary>
		PhotoModeMoveLeftOnly = 0x311353EB,
		/// <summary>Control hash <c>0x5357A7F5</c>.</summary>
		PhotoModeMoveRightOnly = 0x5357A7F5,
		/// <summary>Control hash <c>0xEC001315</c>.</summary>
		PhotoModeMoveUd = 0xEC001315,
		/// <summary>Control hash <c>0x315D57E6</c>.</summary>
		PhotoModeMoveUpOnly = 0x315D57E6,
		/// <summary>Control hash <c>0x4EBCC409</c>.</summary>
		PhotoModeMoveDownOnly = 0x4EBCC409,
		/// <summary>Control hash <c>0xA209BD57</c>.</summary>
		PhotoModeReset = 0xA209BD57,
		/// <summary>Control hash <c>0xB138D899</c>.</summary>
		PhotoModeLenseNext = 0xB138D899,
		/// <summary>Control hash <c>0x06A057F8</c>.</summary>
		PhotoModeLensePrev = 0x06A057F8,
		/// <summary>Control hash <c>0x2EEA1D2A</c>.</summary>
		PhotoModeRotateLeft = 0x2EEA1D2A,
		/// <summary>Control hash <c>0x96E70854</c>.</summary>
		PhotoModeRotateRight = 0x96E70854,
		/// <summary>Control hash <c>0x7F9055F5</c>.</summary>
		PhotoModeToggleHud = 0x7F9055F5,
		/// <summary>Control hash <c>0xDCE96D67</c>.</summary>
		PhotoModeViewPhotos = 0xDCE96D67,
		/// <summary>Control hash <c>0xA190AAC7</c>.</summary>
		PhotoModeTakePhoto = 0xA190AAC7,
		/// <summary>Control hash <c>0x2F13EC9A</c>.</summary>
		PhotoModeBack = 0x2F13EC9A,
		/// <summary>Control hash <c>0x8F32E2EB</c>.</summary>
		PhotoModeSwitchMode = 0x8F32E2EB,
		/// <summary>Control hash <c>0xFE6DD360</c>.</summary>
		PhotoModeFilterIntensity = 0xFE6DD360,
		/// <summary>Control hash <c>0x2286D46B</c>.</summary>
		PhotoModeFilterIntensityUp = 0x2286D46B,
		/// <summary>Control hash <c>0xB341F407</c>.</summary>
		PhotoModeFilterIntensityDown = 0xB341F407,
		/// <summary>Control hash <c>0x886ABA4E</c>.</summary>
		PhotoModeFocalLength = 0x886ABA4E,
		/// <summary>Control hash <c>0xFAFBD66A</c>.</summary>
		PhotoModeFocalLengthUpOnly = 0xFAFBD66A,
		/// <summary>Control hash <c>0x01EBFABD</c>.</summary>
		PhotoModeFocalLengthDownOnly = 0x01EBFABD,
		/// <summary>Control hash <c>0x699F8D08</c>.</summary>
		PhotoModeFilterNext = 0x699F8D08,
		/// <summary>Control hash <c>0x4F640885</c>.</summary>
		PhotoModeFilterPrev = 0x4F640885,
		/// <summary>Control hash <c>0x5B843BC9</c>.</summary>
		PhotoModeZoomIn = 0x5B843BC9,
		/// <summary>Control hash <c>0x2354D2E6</c>.</summary>
		PhotoModeZoomOut = 0x2354D2E6,
		/// <summary>Control hash <c>0x26B9AE6A</c>.</summary>
		PhotoModeDof = 0x26B9AE6A,
		/// <summary>Control hash <c>0x87B07940</c>.</summary>
		PhotoModeDofUpOnly = 0x87B07940,
		/// <summary>Control hash <c>0x047099F1</c>.</summary>
		PhotoModeDofDownOnly = 0x047099F1,
		/// <summary>Control hash <c>0xC64E2284</c>.</summary>
		PhotoModeExposureUp = 0xC64E2284,
		/// <summary>Control hash <c>0xAD07A5A5</c>.</summary>
		PhotoModeExposureDown = 0xAD07A5A5,
		/// <summary>Control hash <c>0x9DE08D71</c>.</summary>
		PhotoModeExposureLock = 0x9DE08D71,
		/// <summary>Control hash <c>0x483F707F</c>.</summary>
		PhotoModeContrast = 0x483F707F,
		/// <summary>Control hash <c>0x5D2DD717</c>.</summary>
		PhotoModeContrastUpOnly = 0x5D2DD717,
		/// <summary>Control hash <c>0x30811620</c>.</summary>
		PhotoModeContrastDownOnly = 0x30811620,
		/// <summary>Control hash <c>0xB99A9CAD</c>.</summary>
		CraftingEat = 0xB99A9CAD,
		/// <summary>Control hash <c>0x0B1BE2E8</c>.</summary>
		CampSetupTent = 0x0B1BE2E8,
		/// <summary>Control hash <c>0x1D927DF2</c>.</summary>
		MinigameActionX = 0x1D927DF2,
		/// <summary>Control hash <c>0xC1D24F92</c>.</summary>
		DeprecatedAbove = 0xC1D24F92,
		/// <summary>Control hash <c>0x1F8EEF84</c>.</summary>
		ScriptLeftAxisX = 0x1F8EEF84,
		/// <summary>Control hash <c>0x5418D8AB</c>.</summary>
		ScriptLeftAxisY = 0x5418D8AB,
		/// <summary>Control hash <c>0xA6B769E9</c>.</summary>
		ScriptRightAxisX = 0xA6B769E9,
		/// <summary>Control hash <c>0x27A5EBC0</c>.</summary>
		ScriptRightAxisY = 0x27A5EBC0,
		/// <summary>Control hash <c>0x771D6E13</c>.</summary>
		ScriptRup = 0x771D6E13,
		/// <summary>Control hash <c>0x37933367</c>.</summary>
		ScriptRdown = 0x37933367,
		/// <summary>Control hash <c>0xA4DB0458</c>.</summary>
		ScriptRleft = 0xA4DB0458,
		/// <summary>Control hash <c>0x22A3B800</c>.</summary>
		ScriptRright = 0x22A3B800,
		/// <summary>Control hash <c>0xE624C062</c>.</summary>
		ScriptLb = 0xE624C062,
		/// <summary>Control hash <c>0x91E9231C</c>.</summary>
		ScriptRb = 0x91E9231C,
		/// <summary>Control hash <c>0x2B314A1E</c>.</summary>
		ScriptLt = 0x2B314A1E,
		/// <summary>Control hash <c>0x26E9CD17</c>.</summary>
		ScriptRt = 0x26E9CD17,
		/// <summary>Control hash <c>0xAADDC975</c>.</summary>
		ScriptLs = 0xAADDC975,
		/// <summary>Control hash <c>0xD04E9FE2</c>.</summary>
		ScriptRs = 0xD04E9FE2,
		/// <summary>Control hash <c>0x0DC15ADD</c>.</summary>
		ScriptPadUp = 0x0DC15ADD,
		/// <summary>Control hash <c>0xB1DA5574</c>.</summary>
		ScriptPadDown = 0xB1DA5574,
		/// <summary>Control hash <c>0x1AF81D9E</c>.</summary>
		ScriptPadLeft = 0x1AF81D9E,
		/// <summary>Control hash <c>0x82A9B758</c>.</summary>
		ScriptPadRight = 0x82A9B758,
		/// <summary>Control hash <c>0xC8722109</c>.</summary>
		ScriptSelect = 0xC8722109,
		/// <summary>Control hash <c>0xAEB4B1DE</c>.</summary>
		ScriptedFlyUd = 0xAEB4B1DE,
		/// <summary>Control hash <c>0xF1111E4A</c>.</summary>
		ScriptedFlyLr = 0xF1111E4A,
		/// <summary>Control hash <c>0x639B9FC9</c>.</summary>
		ScriptedFlyZup = 0x639B9FC9,
		/// <summary>Control hash <c>0x9C5E030C</c>.</summary>
		ScriptedFlyZdown = 0x9C5E030C,
		/// <summary>Control hash <c>0x8EDFFB30</c>.</summary>
		Count = 0x8EDFFB30,
	}
}