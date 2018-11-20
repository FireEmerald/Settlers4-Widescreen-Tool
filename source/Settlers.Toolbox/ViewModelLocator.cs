// In App.xaml:
// <Application.Resources>
//     <vm:ViewModelLocator xmlns:vm="clr-namespace:Settlers.Toolbox"
//                          x:Key="Locator" />
// </Application.Resources>
//
// In the View:
// DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"
//
// You can also use Blend to do all this with the tool's support.
// See http://www.galasoft.ch/mvvm
//
// SimpleIoC is included and can be used to resolve components.
// See https://stackoverflow.com/questions/13795596/how-to-use-mvvmlight-simpleioc

using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using Castle.Windsor.Installer;

using Settlers.Toolbox.ViewModels.Interfaces;

namespace Settlers.Toolbox
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        private readonly IWindsorContainer _Container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelLocator"/> class.
        /// </summary>
        public ViewModelLocator()
        {
            _Container = new WindsorContainer();

            // Add support for IEnumerable<T>, ICollection<T>, IList<T> and T[].
            _Container.Kernel.Resolver.AddSubResolver(new CollectionResolver(_Container.Kernel));

            // Call every Installer.
            _Container.Install(FromAssembly.InThisApplication());
        }

        public IMainViewModel MainViewModel => _Container.Resolve<IMainViewModel>();

        public static void Cleanup()
        {
            // Clear the ViewModels here.
        }
    }
}