using System.Collections.ObjectModel;
using System.Xml.Linq;
using counterSzypula.Models;

namespace counterSzypula.Views;

public partial class CountersList : ContentPage
{
    public ObservableCollection<Counter> Counters { get; } = new();

    public CountersList()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadCounters();

    }

    private void LoadCounters()
    {
        Counters.Clear();

        string path = Path.Combine(
            FileSystem.AppDataDirectory,
            "counters.xml"
        );

        if (!File.Exists(path))
        {
            return;
        }

        XDocument document = XDocument.Load(path);

        IEnumerable<XElement> counterElements =
            document.Root?.Elements("Counter").Reverse()
            ?? Enumerable.Empty<XElement>();

        foreach (XElement element in counterElements)
        {
            string name =
                element.Element("Name")?.Value
                ?? "Unnamed counter";

            int.TryParse(
                element.Element("Count")?.Value,
                out int count
            );

            DateTime.TryParse(
                element.Element("SavedAt")?.Value,
                out DateTime savedAt
            );

            Counter loadedCounter = new Counter
            {
                Name = name,
                Count = count,
                SavedAt = savedAt
            };

            Counters.Add(loadedCounter);
        }
        
    }
    private async void Add_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(NewCounter));
    }
}