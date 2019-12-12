using GalaSoft.MvvmLight.Ioc;
using CommonServiceLocator;
using CSharpBNP.Business;

namespace CSharpBNP.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            AddBusiness();

            SimpleIoc.Default.Register<MainViewModel>();
        }

        private static void AddBusiness()
        {
            SimpleIoc.Default.Register<IDialog, Dialog>();
            SimpleIoc.Default.Register<ITransaction, Transaction>();
        }

        public MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
    }
}