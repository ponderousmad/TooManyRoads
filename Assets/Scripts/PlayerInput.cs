using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput {

	private string moveAxisX;
	private string moveAxisY;
	private string aimAxisX;
	private string aimAxisY;

	private string fireEmbiggen;
	private string fireDebigulate;

	private float aimAxisDeadzone = 0.5f;
	private float jumpThreshold = 0.5f;

	enum Axis
	{
		MoveX = 0,
		MoveY = 1,
		AimX = 2,
		AimY = 3
	}

	enum Button
	{
		Embiggen = 0,
		Debigulate = 1
	}

	private static int[] WindowsAxisNumbers = { 0, 1, 2, 3 };
	private static int[] WindowsButtonNumbers = { 0, 1 };
	private static int[] OSXAxisNumbers = { 0, 1, 2, 3 };
	private static int[] OSXButtonNumbers = { 0, 1 };

	private static string PlatformName(RuntimePlatform platform)
	{
		switch(platform)
		{
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				return "_OSX";
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
				return "_WIN";
		}
		return "";
	}

	private static int[] PlatformAxis(RuntimePlatform platform)
	{
		switch(platform)
		{
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				return OSXAxisNumbers;
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
				return WindowsAxisNumbers;
		}
		return WindowsAxisNumbers;
	}

	private static int[] PlatformButton(RuntimePlatform platform)
	{
		switch(platform)
		{
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.OSXPlayer:
				return OSXButtonNumbers;
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
				return WindowsButtonNumbers;
		}
		return WindowsButtonNumbers;
	}

	static string Postfix(int player, RuntimePlatform platform)
	{
		return "_P" + player.ToString() + PlatformName(platform);
	}

	static string AxisInput(Axis axis, int player, RuntimePlatform platform, string negativeKey, string positiveKey, string altNegative = "", string altPostive = "")
	{
		string axisName = axis.ToString() + Postfix(player, platform);
		int axisNumber = PlatformAxis(platform)[(int)axis];
		return string.Format(@"
  - serializedVersion: 3
    m_Name: {0}
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: {1}
    positiveButton: {2}
    altNegativeButton: {3}
    altPositiveButton: {4}
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 2
    axis: {5}
    joyNum: {6}", axisName, negativeKey, positiveKey, altNegative, altPostive, axisNumber, player);
	}

	static string ButtonInput(Button button, int player, RuntimePlatform platform, string key)
	{
		string buttonName = button.ToString() + Postfix(player, platform);
		int buttonNumber = PlatformAxis(platform)[(int)button];
		return string.Format(@"
  - serializedVersion: 3
    m_Name: {0}
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton:
    positiveButton: joystick button {1}
    altNegativeButton:
    altPositiveButton: {2}
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: {3}", buttonName, buttonNumber, key, player);
	}

	static IEnumerable<string> ControllerDefinition(int player, RuntimePlatform platform)
	{
		bool useKeys = player == 1;
		yield return AxisInput(Axis.MoveX, player, platform, useKeys ? "a" : "", useKeys ? "d" : "");
		yield return AxisInput(Axis.MoveY, player, platform, useKeys ? "w" : "", useKeys ? "s" : "", useKeys ? "space" : "");
		yield return AxisInput(Axis.AimX, player, platform, useKeys ? "left" : "", useKeys ? "right" : "");
		yield return AxisInput(Axis.AimY, player, platform, useKeys ? "down" : "", useKeys ? "up" : "");
		yield return ButtonInput(Button.Embiggen, player, platform, useKeys ? "e" : "");
		yield return ButtonInput(Button.Embiggen, player, platform, useKeys ? "q" : "");
	}

	public static string AllDefinitions()
	{
		var definitions = new System.Text.StringBuilder();
		foreach (var platform in new RuntimePlatform[] { RuntimePlatform.WindowsPlayer, RuntimePlatform.OSXPlayer})
		{
			for(int player = 1; player <= 4; ++player)
			{
				foreach(string definition in ControllerDefinition(player, platform))
				{
					definitions.Append(definition);
				}
			}
		}
		return definitions.ToString();
	}

	private PlayerInput(int number)
	{
		string postfix = Postfix(number, Application.platform);
		moveAxisX = Axis.MoveX.ToString() + postfix;
		moveAxisY = Axis.MoveY.ToString() + postfix;
		aimAxisX = Axis.AimX.ToString() + postfix;
		aimAxisY = Axis.AimY.ToString() + postfix;

		fireEmbiggen = Button.Embiggen.ToString() + postfix;
		fireDebigulate = Button.Debigulate.ToString() + postfix;
	}

	private float GetAxis(string axis) { return Input.GetAxisRaw(axis); }
	private bool HasAim(float value) { return Mathf.Abs(value) > aimAxisDeadzone; }

	public float AimX { get { return GetAxis(aimAxisX); } }
	public float AimY { get { return GetAxis(aimAxisY); } }
	public bool HasAimX { get { return HasAim(AimX); } }
	public bool HasAimY { get { return HasAim(AimY); } }
	public bool IsGunToHead { get { return(!HasAimX && !HasAimY); } }
	public Vector2 Aim(bool snap)
	{
		Vector2 precise = new Vector2(AimX, AimY);
		if(!snap)
		{
			return precise;
		}
		return new Vector2(
			HasAim(precise.x) ? Mathf.Sign(precise.x) : 0,
			HasAim(precise.y) ? Mathf.Sign(precise.y) : 0
		);
	}

	public float MoveX { get { return GetAxis(moveAxisX); } }
	public float MoveY { get { return GetAxis(moveAxisY); } }

	public bool Jump { get { return MoveY > jumpThreshold; } }

	public bool FireEmbiggen { get { return Input.GetButton(fireEmbiggen); } }
	public bool FireDebigulate { get { return Input.GetButton(fireDebigulate); } }
}
