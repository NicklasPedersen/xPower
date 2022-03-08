namespace xPowerHub;

public interface ISmart
{
    string Name { get; }
    // sets the state to true and returns whether or not it was successful
    public bool TurnOn();
    // sets the state to false and returns whether or not it was successful
    public bool TurnOff();
    // returns the current state or null if error
    public bool? GetCurrentState();
    // sets the state and returns whether or not it was successful
    public bool SetState(bool state);
}
