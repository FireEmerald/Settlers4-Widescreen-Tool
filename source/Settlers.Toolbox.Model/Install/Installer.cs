using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using Settlers.Toolbox.Model.Addons;
using Settlers.Toolbox.Model.Addons.Interfaces;
using Settlers.Toolbox.Model.Compatibility;
using Settlers.Toolbox.Model.Compatibility.Interfaces;
using Settlers.Toolbox.Model.Interfaces;
using Settlers.Toolbox.Model.Languages;
using Settlers.Toolbox.Model.Languages.Interfaces;
using Settlers.Toolbox.Model.Resolutions;
using Settlers.Toolbox.Model.Resolutions.Interfaces;
using Settlers.Toolbox.Model.Textures;
using Settlers.Toolbox.Model.Textures.Interfaces;
using Settlers.Toolbox.Model.Updates;
using Settlers.Toolbox.Model.Updates.Interfaces;

namespace Settlers.Toolbox.Model.Install
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
            // Addons
            container.Register(Classes.FromAssemblyContaining<IAddon>()
                                      .BasedOn<IAddon>().WithService.FromInterface().LifestyleTransient());

            container.Register(Component.For<IAddonManager>().ImplementedBy<AddonManager>().LifestyleTransient());

            // Compatibility
            container.Register(Component.For<ICompatibilityManager>().ImplementedBy<CompatibilityManager>().LifestyleTransient());

            // Languages
            container.Register(Component.For<ILanguageChanger>().ImplementedBy<LanguageChanger>().LifestyleTransient());

            // Resolutions
            container.Register(
                Component.For<IResolutionChanger>().ImplementedBy<ResolutionChanger>().LifestyleTransient(),
                Component.For<IResolutionFactory>().ImplementedBy<ResolutionFactory>().LifestyleTransient()
            );

            // Textures
            container.Register(Component.For<ITextureSwapper>().ImplementedBy<TextureSwapper>().LifestyleTransient());

            // Updates
            container.Register(Component.For<IUpdateChecker>().ImplementedBy<UpdateChecker>().LifestyleTransient());

            // Root
            container.Register(Component.For<IGame>().ImplementedBy<Game>().LifestyleTransient());
        }
    }
}