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

    private string pressButton;

	private float aimAxisDeadzone = 0.5f;
	private float jumpThreshold = 0.5f;
    private float fireThreshold = 0.5f;

	enum Axis
	{
		MoveX = 0,  
		MoveY = 1,
		AimX = 2,
		AimY = 3,
		Embiggen = 4,
		Debigulate = 5
	}

	enum Button
	{
		Embiggen = 0,
		Debigulate = 1,
        Press = 2
	}

	private static int[] WindowsAxisNames = { 0, 1, 3, 4, 8, 9 };
	private static int[] WindowsButtonNumbers = { 7, 6, 0 };
	private static int[] OSXAxisNames = { 0, 1, 2, 3, 4, 5 };
	private static int[] OSXButtonNumbers = { 7, 6, 16 };

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
				return OSXAxisNames;
			case RuntimePlatform.WindowsEditor:
			case RuntimePlatform.WindowsPlayer:
				return WindowsAxisNames;
		}
		return WindowsAxisNames;
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

	static string AnalogAxisInput(Axis axis, int player, RuntimePlatform platform, bool invert)
	{
		string axisName = axis.ToString() + Postfix(player, platform);
		int axisNumber = PlatformAxis(platform)[(int)axis];
		return string.Format(@"
  - serializedVersion: 3
    m_Name: {0}
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton:
    positiveButton:
    altNegativeButton:
    altPositiveButton:
    gravity: 0
    dead: 0.02
    sensitivity: 1
    snap: 0
    invert: {1}
    type: 2
    axis: {2}
    joyNum: {3}", axisName, invert ? "1" : "0", axisNumber, player);
	}

    static string KeyboardAxisInput(Axis axis, int player, RuntimePlatform platform, string negativeKey, string positiveKey, string altNegative = "", string altPostive = "" )
    {
		string axisName = axis.ToString() + Postfix(player, platform);  
		return string.Format(@"
  - serializedVersion: 3
    m_Name: {0}
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: {1}
    positiveButton: {2}
    altNegativeButton: {3}
    altPositiveButton: {4}
    gravity: 3
    dead: 0.001
    sensitivity: 3
    snap: 1
    invert: 0
    type: 0
    axis: 0
    joyNum: {5}", axisName, negativeKey, positiveKey, altNegative, altPostive, player);
    }

	static string ButtonAxisInput(Button button, int player, RuntimePlatform platform)
	{
		string buttonName = button.ToString() + Postfix(player, platform);
		int buttonNumber = PlatformButton(platform)[(int)button];
		return string.Format(@"
  - serializedVersion: 3
    m_Name: {0}
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton:
    positiveButton: joystick button {1}
    altNegativeButton:
    altPositiveButton:
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 1
    invert: 0
    type: 0
    axis: 0
    joyNum: {2}", buttonName, buttonNumber, player);
	}

	static string ButtonInput(Button button, int player, RuntimePlatform platform, string key)
	{
		string buttonName = button.ToString() + Postfix(player, platform);
		int buttonNumber = PlatformButton(platform)[(int)button];
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
		if(player==1)
        {
            yield return KeyboardAxisInput(Axis.MoveX, player, platform, "a", "d");
            yield return KeyboardAxisInput(Axis.MoveY, player, platform, "w", "s", "", "space");
            yield return KeyboardAxisInput(Axis.AimX, player, platform, "left", "right");
            yield return KeyboardAxisInput(Axis.AimY, player, platform, "down", "up");
            yield return KeyboardAxisInput(Axis.Embiggen, player, platform, "", "e");
            yield return KeyboardAxisInput(Axis.Debigulate, player, platform, "", "q");
        }
		yield return AnalogAxisInput(Axis.MoveX, player, platform, false);
		yield return AnalogAxisInput(Axis.MoveY, player, platform, true);
		yield return AnalogAxisInput(Axis.AimX, player, platform, false);
		yield return AnalogAxisInput(Axis.AimY, player, platform, true);
		yield return AnalogAxisInput(Axis.Embiggen, player, platform, false);
		yield return AnalogAxisInput(Axis.Debigulate, player, platform, false);
        yield return ButtonAxisInput(Button.Embiggen, player, platform);
        yield return ButtonAxisInput(Button.Debigulate, player, platform);
        yield return ButtonInput(Button.Press, player, platform, "space");
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

	public PlayerInput(int number)
	{
		string postfix = Postfix(number, Application.platform);
		moveAxisX = Axis.MoveX.ToString() + postfix;
		moveAxisY = Axis.MoveY.ToString() + postfix;
		aimAxisX = Axis.AimX.ToString() + postfix;
		aimAxisY = Axis.AimY.ToString() + postfix;

		fireEmbiggen = Button.Embiggen.ToString() + postfix;
		fireDebigulate = Button.Debigulate.ToString() + postfix;

        pressButton = Button.Press.ToString() + postfix;
	}

	private float GetAxis(string axis) {
        float value = Input.GetAxisRaw(axis);
        Debug.Log("Getting " + axis + " = " + value.ToString());
        return value;
    }
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

	public bool FireEmbiggen { get { return GetAxis(fireEmbiggen) > fireThreshold; } }
	public bool FireDebigulate { get { return GetAxis(fireDebigulate) > fireThreshold; } }

    public bool MenuPress { get { return Input.GetButtonDown(pressButton); } }

    public bool MenuUp { get { return MoveY < 0; } }
    public bool MenuDown { get { return MoveY > 0; } }

    public bool MenuLeft { get { return MoveX < 0; } }
    public bool MenuRight { get { return MoveX > 0; } }
}
