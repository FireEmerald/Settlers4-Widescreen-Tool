using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using GalaSoft.MvvmLight;

using Settlers.Toolbox.Interfaces;
using Settlers.Toolbox.ViewModels;
using Settlers.Toolbox.ViewModels.Interfaces;
using Settlers.Toolbox.ViewModels.Validator;
using Settlers.Toolbox.ViewModels.Validator.Interfaces;

namespace Settlers.Toolbox.Install
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
            if (ViewModelBase.IsInDesignModeStatic)
            {
                // Create design time view services and models
                container.Register(Component.For<IMainViewModel>().ImplementedBy<DummyMainViewModel>().LifestyleSingleton());
            }
            else
            {
                // Create run time view services and models
                container.Register(Component.For<IMainViewModel>().ImplementedBy<MainViewModel>().LifestyleSingleton());
            }

            // Validator
            container.Register(Component.For<IMainViewModelValidator>().ImplementedBy<MainViewModelValidator>().LifestyleTransient());

            // Root
            container.Register(Component.For<IWindowManager>().ImplementedBy<WindowManager>().LifestyleSingleton());
        }
    }
}