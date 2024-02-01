using TomatoStates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ColorExample : MonoBehaviour
{
	[SerializeField] Image img;

	InputAction colorSwitchKey, modeSwitchKey, toggleYellowKey;
	Trigger colorSwitch, modeSwitch, toggleYellow;

	StateMachine sm;

	void Start()
	{
		BuildTriggers();
		

		var color = BuildColorMachine();
		var grayscale = BuildGrayscaleMachine();
		var yellow = new State(onEnter: () => img.color = Color.yellow);

		color
			.To(grayscale)
			.On(modeSwitch)
		;

		grayscale
			.To(color)
			.On(modeSwitch)
			.Do(() => Debug.Log("grayscale to color"))
		;

		new IState[] {color, grayscale}
			.To(yellow)
			.On(toggleYellow)
			.Do(() => Debug.Log("either to yellow"))
		;

		yellow
			.To(grayscale)
			.On(toggleYellow)
		;


		sm = new StateMachine(grayscale);
	}

	void BuildTriggers()
	{
		colorSwitchKey = new InputAction("Color Switch", binding: Keyboard.current.spaceKey.path);
		colorSwitchKey.Enable();

		colorSwitch = new Trigger();
		colorSwitch.Subscribe(colorSwitchKey);

		modeSwitchKey = new InputAction("Mode Switch", binding: Keyboard.current.mKey.path);
		modeSwitchKey.Enable();

		modeSwitch = new Trigger();
		modeSwitch.Subscribe(modeSwitchKey);

		toggleYellowKey = new InputAction("Toggle", binding: Keyboard.current.enterKey.path);
		toggleYellowKey.Enable();

		toggleYellow = new Trigger();
		toggleYellow.Subscribe(toggleYellowKey);
	}

	void OnDestroy()
	{
		sm?.Dispose();
		modeSwitch.Unsubscribe(modeSwitchKey);
		colorSwitch.Unsubscribe(colorSwitchKey);
	}

	SubStateMachine BuildColorMachine()
	{
		var red = new State(onEnter: () => img.color = Color.red);
		var green = new State(onEnter: () => img.color = Color.green);
		var blue = new State(onEnter: () => img.color = Color.blue);

		red
			.To(green)
			.On(colorSwitch)
			.Do(() => Debug.Log("red to green"))
		;

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