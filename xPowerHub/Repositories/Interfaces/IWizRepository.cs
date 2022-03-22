namespace xPowerHub.Repositories.Interfaces;

public interface IWizRepository
{
    bool Run { get; set; }
    void Start(Action<WizDevice> callback);
}