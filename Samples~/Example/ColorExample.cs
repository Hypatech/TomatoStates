using TomatoStates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ColorExample : MonoBehaviour
{
	[SerializeField] Image img;

	InputAction colorSwitchKey, modeSwitchKey;
	Trigger colorSwitch, modeSwitch;

	StateMachine sm;

	void Start()
	{
		BuildTriggers();

		var color = BuildColorMachine();
		var grayscale = BuildGrayscaleMachine();

		color
			.To(grayscale)
			.On(modeSwitch);

		grayscale
			.To(color)
			.On(modeSwitch);

		sm = new StateMachine(grayscale);
	}

	void BuildTriggers()
	{
		colorSwitchKey = new InputAction("Color Switch", binding: Keyboard.current.spaceKey.path);
		colorSwitchKey.Enable();

		colorSwitch = new Trigger();
		colorSwitch.Subscribe(ref colorSwitchKey);

		modeSwitchKey = new InputAction("Mode Switch", binding: Keyboard.current.mKey.path);
		modeSwitchKey.Enable();

		modeSwitch = new Trigger();
		modeSwitch.Subscribe(ref modeSwitchKey);
	}

	void OnDestroy()
	{
		sm?.Dispose();
		modeSwitch.Unsubscribe(ref modeSwitchKey);
		colorSwitch.Unsubscribe(ref colorSwitchKey);
	}

	SubStateMachine BuildColorMachine()
	{
		var red = new State(onEnter: () => img.color = Color.red);
		var green = new State(onEnter: () => img.color = Color.green);
		var blue = new State(onEnter: () => img.color = Color.blue);

		red
			.To(green)
			.On(colorSwitch);

		green
			.To(blue)
			.On(colorSwitch);

		blue
			.To(red)
			.On(colorSwitch);

		return new SubStateMachine(red);
	}

	SubStateMachine BuildGrayscaleMachine()
	{
		var black = new State(onEnter: () => img.color = Color.black);
		var white = new State(onEnter: () => img.color = Color.white);

		black
			.To(white)
			.On(colorSwitch);

		white
			.To(black)
			.On(colorSwitch);

		return new SubStateMachine(black);
	}
}