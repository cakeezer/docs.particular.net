﻿using System;
using System.Security.Principal;
using System.Threading;
using NServiceBus;
using NServiceBus.Installation.Environments;

class Program
{
    static void Main()
    {
        Console.Title = "Samples.UsernameHeader.Endpoint1";
        Configure.Serialization.Json();
        var configure = Configure.With();
        configure.Log4Net();
        configure.DefineEndpointName("Samples.UsernameHeader.Endpoint1");
        configure.DefaultBuilder();
        configure.UseTransport<Msmq>();
        configure.InMemorySagaPersister();
        configure.UseInMemoryTimeoutPersister();
        configure.InMemorySubscriptionStorage();

        #region ComponentRegistration

        configure.Configurer
            .ConfigureComponent<UsernameMutator>(DependencyLifecycle.InstancePerCall);

        #endregion

        using (var startableBus = configure.UnicastBus().CreateBus())
        {
            var bus = startableBus.Start(() => configure.ForInstallationOn<Windows>().Install());

            #region SendMessage

            var identity = new GenericIdentity("FakeUser");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, new string[0]);
            var message = new MyMessage();
            bus.Send("Samples.UsernameHeader.Endpoint2", message);

            #endregion

            Console.WriteLine("Message sent. Press any key to exit");
            Console.ReadKey();
        }
    }
}