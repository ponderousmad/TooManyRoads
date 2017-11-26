using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput {
	private bool useOneStickMode = false;
    private bool useLastMove = false;

	private string moveAxisX;
	private string moveAxisY;
	private string aimAxisX;
	private string aimAxisY;

	private string fireEmbiggen;
	private string fireDebigulate;

	private string selfEmbiggen;
	private string selfDebigulate;

    private string pressButton;
    private string jumpButton;

	private float aimAxisDeadzone = 0.25f;
	private float jumpThreshold = 0.5f;
    private float fireThreshold = 0.5f;
    private float menuThreshold = 0.5f;

    private bool jumpActive = false;
    private bool menuUp = false;
    private bool menuDown = false;
    private bool menuLeft = false;
    private bool menuRight = false;

    private float lastMoveDirection = 1;

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
        Press = 2,
        Jump = 3,
        SelfEmbiggen = 4,
        SelfDebigulate = 5
	}

	private static int[] WindowsAxisNames = { 0, 1, 3, 4, 9, 8 };
	private static int[] WindowsButtonNumbers = { 7, 6, 0, 0, 5, 4 };
	private static int[] OSXAxisNames = { 0, 1, 2, 3, 4, 5 };
	private static int[] OSXButtonNumbers = { 7, 6, 1, 1, 5, 4 };

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
    gravity: 1000
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
    positiveButton: joystick {1} button {2}
    altNegativeButton:
    altPositiveButton:
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 1
    invert: 0
    type: 0
    axis: 0
    joyNum: {1}", buttonName, player, buttonNumber);
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
		//yield return AnalogAxisInput(Axis.Embiggen, player, platform, false);
		//yield return AnalogAxisInput(Axis.Debigulate, player, platform, false);
        yield return ButtonAxisInput(Button.Embiggen, player, platform);
        yield return ButtonAxisInput(Button.Debigulate, player, platform);
        yield return ButtonInput(Button.Press, player, platform, "space");
        yield return ButtonInput(Button.Jump, player, platform, "space");
        yield return ButtonInput(Button.SelfEmbiggen, player, platform, "c");
        yield return ButtonInput(Button.SelfDebigulate, player, platform, "z");
	}

	public static string AllDefinitions()
	{
		var definitions = new System.Text.StringBuilder();
        definitions.Append(DefaultInputs);
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

	public PlayerInput(int number, bool oneStickMode = false, bool lastMoveMode = false)
	{
		string postfix = Postfix(number, Application.platform);
		moveAxisX = Axis.MoveX.ToString() + postfix;
		moveAxisY = Axis.MoveY.ToString() + postfix;
		aimAxisX = Axis.AimX.ToString() + postfix;
		aimAxisY = Axis.AimY.ToString() + postfix;

		fireEmbiggen = Button.Embiggen.ToString() + postfix;
		fireDebigulate = Button.Debigulate.ToString() + postfix;

        selfEmbiggen = Button.SelfEmbiggen.ToString() + postfix;
        selfDebigulate = Button.SelfDebigulate.ToString() + postfix;

		pressButton = Button.Press.ToString() + postfix;
		jumpButton = Button.Jump.ToString() + postfix;


		useOneStickMode = oneStickMode;
        useLastMove = lastMoveMode;
	}

	private float GetAxis(string axis) {
        float value = Input.GetAxis(axis);
        //Debug.Log("Checking axis: " + axis + " = " + value);
        return value;
    }
	private bool HasAim(float value) { return Mathf.Abs(value) > aimAxisDeadzone; }

	public float AimX { get { return GetAxis(useOneStickMode ? moveAxisX : aimAxisX); } }
	public float AimY { get { return GetAxis(useOneStickMode ? moveAxisY : aimAxisY); } }
	public bool HasAimX { get { return HasAim(AimX); } }
	public bool HasAimY { get { return HasAim(AimY); } }
	public bool IsGunToHead { get { return(!HasAimX && !HasAimY); } }
	public Vector2 Aim(bool snap)
	{
        Vector2 precise = new Vector2 (MoveX, MoveY);
        if(useLastMove && !HasAimY)
        {
            precise = new Vector2(lastMoveDirection, precise.y);
        }
        if (!snap)
        {
            return precise;
        }
        return new Vector2 (
            HasAim (precise.x) ? Mathf.Sign (precise.x) : 0,
            HasAim (precise.y) ? Mathf.Sign (precise.y) : 0
        );
	}

	public float MoveX
    {
        get {
            float move = GetAxis(moveAxisX);
            if(Mathf.Abs(move) > aimAxisDeadzone)
            {
                lastMoveDirection = Mathf.Sign(move);
            }
            return move;
        }
    }
	public float MoveY { get { return GetAxis(moveAxisY); } }

    private static bool AnalogToDigital(float input, float threshold, ref bool value)
    {
        bool prevValue = value;
        value = input > threshold;
        return value && !prevValue;
    }

	public bool Jump {
        get
        {
			if (useOneStickMode) {
				return (Input.GetButtonDown (jumpButton));
			} else {
				return (AnalogToDigital(MoveY, jumpThreshold, ref jumpActive) || Input.GetButtonDown(jumpButton));
			}
        }
    }

	public bool FireEmbiggen { get { return GetAxis(fireEmbiggen) > fireThreshold; } }
	public bool FireDebigulate { get { return GetAxis(fireDebigulate) > fireThreshold; } }

    public bool SelfEmbiggen { get { return Input.GetButton(selfEmbiggen); } }
    public bool SelfDebigulate { get { return Input.GetButton(selfDebigulate); } }

    public bool MenuPress { get { return Input.GetButtonDown(pressButton); } }

    public bool MenuUp { get { return AnalogToDigital(MoveY, menuThreshold, ref menuUp); } }
    public bool MenuDown { get { return AnalogToDigital(-MoveY, menuThreshold, ref menuDown); } }

    public bool MenuLeft { get { return AnalogToDigital(-MoveX, menuThreshold, ref menuLeft); } }
    public bool MenuRight { get { return AnalogToDigital(MoveX, menuThreshold, ref menuRight); } }

    private static readonly string DefaultInputs = @"%YAML 1.1
%TAG !u! tag:unity3d.com,2011:
--- !u!13 &1
InputManager:
  m_ObjectHideFlags: 0
  serializedVersion: 2
  m_Axes:
  - serializedVersion: 3
    m_Name: Horizontal
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: left
    positiveButton: right
    altNegativeButton: a
    altPositiveButton: d
    gravity: 3
    dead: 0.001
    sensitivity: 3
    snap: 1
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Vertical
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: down
    positiveButton: up
    altNegativeButton: s
    altPositiveButton: w
    gravity: 3
    dead: 0.001
    sensitivity: 3
    snap: 1
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Fire1
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: left ctrl
    altNegativeButton: 
    altPositiveButton: mouse 0
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Fire2
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: left alt
    altNegativeButton: 
    altPositiveButton: mouse 1
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Fire3
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: left shift
    altNegativeButton: 
    altPositiveButton: mouse 2
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Jump
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: space
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Mouse X
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0
    sensitivity: 0.1
    snap: 0
    invert: 0
    type: 1
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Mouse Y
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0
    sensitivity: 0.1
    snap: 0
    invert: 0
    type: 1
    axis: 1
    joyNum: 0
  - serializedVersion: 3
    m_Name: Mouse ScrollWheel
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0
    sensitivity: 0.1
    snap: 0
    invert: 0
    type: 1
    axis: 2
    joyNum: 0
  - serializedVersion: 3
    m_Name: Horizontal
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0.19
    sensitivity: 1
    snap: 0
    invert: 0
    type: 2
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Vertical
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: 
    altNegativeButton: 
    altPositiveButton: 
    gravity: 0
    dead: 0.19
    sensitivity: 1
    snap: 0
    invert: 1
    type: 2
    axis: 1
    joyNum: 0
  - serializedVersion: 3
    m_Name: Fire1
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 0
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Fire2
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 1
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Fire3
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 2
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Jump
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: joystick button 3
    altNegativeButton: 
    altPositiveButton: 
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Submit
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: return
    altNegativeButton: 
    altPositiveButton: joystick button 0
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Submit
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: enter
    altNegativeButton: 
    altPositiveButton: space
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
  - serializedVersion: 3
    m_Name: Cancel
    descriptiveName: 
    descriptiveNegativeName: 
    negativeButton: 
    positiveButton: escape
    altNegativeButton: 
    altPositiveButton: joystick button 1
    gravity: 1000
    dead: 0.001
    sensitivity: 1000
    snap: 0
    invert: 0
    type: 0
    axis: 0
    joyNum: 0
";
}
