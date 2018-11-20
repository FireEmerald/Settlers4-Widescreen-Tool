using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using Serilog.Core;

using Settlers.Toolbox.Infrastructure.Cabinet;
using Settlers.Toolbox.Infrastructure.Cabinet.Interfaces;
using Settlers.Toolbox.Infrastructure.IO;
using Settlers.Toolbox.Infrastructure.IO.Interfaces;
using Settlers.Toolbox.Infrastructure.Json;
using Settlers.Toolbox.Infrastructure.Json.Interfaces;
using Settlers.Toolbox.Infrastructure.Logging;
using Settlers.Toolbox.Infrastructure.Logging.Interfaces;
using Settlers.Toolbox.Infrastructure.Logging.Logger;
using Settlers.Toolbox.Infrastructure.Logging.Logger.Interfaces;
using Settlers.Toolbox.Infrastructure.Logging.Sinks;
using Settlers.Toolbox.Infrastructure.Network;
using Settlers.Toolbox.Infrastructure.Network.Interfaces;
using Settlers.Toolbox.Infrastructure.Registry;
using Settlers.Toolbox.Infrastructure.Registry.Interfaces;
using Settlers.Toolbox.Infrastructure.Reporting;
using Settlers.Toolbox.Infrastructure.Reporting.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Install
{
    public class Installer : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            // Cabinet
            container.Register(
                Component.For<ICabContainerFactory>().ImplementedBy<CabContainerFactory>().LifestyleTransient()
            );

            // IO
            container.Register(
                Component.For<IIniFileAdapter>().ImplementedBy<IniFileAdapter>().LifestyleTransient(),
                Component.For<IZipFileAdapter>().ImplementedBy<ZipFileAdapter>().LifestyleTransient()
            );

            // Json
            container.Register(Component.For<IJsonAdapter>().ImplementedBy<JsonAdapter>().LifestyleTransient());

            // Logging
            container.Register(
                Component.For<ILogLevelConverter>().ImplementedBy<LogLevelConverter>().LifestyleTransient(),
                Component.For<IInternalLogger>().ImplementedBy<SerilogLogger>().LifestyleTransient()
            );

            // Sinks
            container.Register(Component.For<ILogEventSink>().ImplementedBy<ReportingSink>().LifestyleTransient());

            // Network
            container.Register(
                Component.For<IFileDownloader>().ImplementedBy<FileDownloader>().LifestyleTransient(),
                Component.For<IStringDownloader>().ImplementedBy<StringDownloader>().LifestyleTransient()
            );

            // Registry
            container.Register(
                Component.For<IRegistryAdapter>().ImplementedBy<RegistryAdapter>().LifestyleTransient(),
                Component.For<IRegistryKeyAdapter>().ImplementedBy<RegistryKeyAdapter>().LifestyleTransient()
            );

            // Reporting
            container.Register(Component.For<IReportManager>().ImplementedBy<ReportManager>().LifestyleSingleton());
        }
    }
}