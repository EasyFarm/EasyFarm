using EasyFarm.Classes;

/// <summary>
/// Used by the ModuleInit. All code inside the Initialize method is ran as soon as the assembly is loaded.
/// </summary>
public static class ModuleInitializer
{
    /// <summary>
    /// Initializes the module.
    /// </summary>
    public static void Initialize()
    {
        DisablePausePeriods();
    }

    /// <summary>
    /// Disable all pauses so tests will run faster. 
    /// </summary>
    private static void DisablePausePeriods()
    {
        TimeWaiter.IsEnabled = false;
    }
}