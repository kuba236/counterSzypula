namespace counterSzypula
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.NewCounter), typeof(Views.NewCounter));
        }
    }
}
