# TomatoStates
A practical state machine library designed for Unity.

# Usage
Documentation + more detailed usage guide WIP.

First, you must define all the states you wish to use in your state machine. 
```cs
var idleState = new State(onEnter: () => Debug.Log("I am idle!"));
var activeState = new State(onEnter: () => Debug.Log("I am active!"));
```

Then, you must define a Trigger. A Trigger represents an event which causes the state machine to transition.
```cs
var activate = new Trigger();
```

They can be fired manually:
```cs
activate.Fire();
```

Or, they can subscribe to an Action, InputAction, or UnityEvent. ***Make sure to call Unsubscribe when the trigger is no longer needed to avoid memory leaks.***
```cs
Action someAction;
activate.Subscribe(ref someAction);

someAction?.Invoke(); // This causes the trigger to fire.
```

States can be connected via triggers like so:
```cs
idleState
  .To(activeState)
  .On(activate);
```

Conditions can be added to these transitions by using the If function.
```cs
bool canReturnToIdle = false;

activeState
  .To(idleState)
  .On(deactivate)
  .If(() => canReturnToIdle);

// ....
deactivate.Fire(); // this would *not* cause a transition.

canReturnToIdle = true;
deactivate.Fire(); // this *would* cause a transition.
```

Finally, the state machine must be instantiated with the state you want to enter in. ***Remember to call Dispose() on the state machine when it is no longer needed to avoid memory leaks.***
```cs
var sm = new StateMachine(idleState);
```
