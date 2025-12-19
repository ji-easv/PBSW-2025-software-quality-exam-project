using BoxFactory.IntegrationTests.Setup;
using Reqnroll;
using Reqnroll.BoDi;

namespace BoxFactory.IntegrationTests.BDD.Setup;

/// <summary>
/// We hook into the Reqnroll lifecycle to set up the database and the web application.
/// </summary>
[Binding]
public static class Hooks
{
    [BeforeScenario(Order = 0)]
    public static async Task StartWebApplication(ScenarioContext scenarioContext)
    {
        var factory = new CustomWebApplicationFactory();
        await factory.InitializeAsync();
        
        // We create a wrapper so we can store responses when doing requests. This is useful for assertions.
        var httpClient = factory.CreateClient();
        scenarioContext.ScenarioContainer.RegisterInstanceAs(httpClient);
        scenarioContext.ScenarioContainer.RegisterInstanceAs(factory);
    }
    
    [AfterScenario(Order = 0)]
    public static async Task AfterTestRun(IObjectContainer container)
    {
        var factory = container.Resolve<CustomWebApplicationFactory>();
        await factory.DisposeAsync();
    }
}