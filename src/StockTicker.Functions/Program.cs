using Microsoft.Extensions.Hosting;

IHost host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices((h, s) =>
                {

                })
                .Build();

await host.RunAsync();