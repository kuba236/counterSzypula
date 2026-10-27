namespace counterSzypula.Views;
using System.ComponentModel;
using counterSzypula.Models;

using System;
using System.Xml.Linq;


public partial class NewCounter : ContentPage
{
    private bool isStartingValueUnset = true;


    public NewCounter()
	{
		InitializeComponent();

        Counter counter = new Counter();
        counter.PropertyChanged += Counter_PropertyChanged;

        BindingContext = counter;
        
	}
    private void IncrementCounter(object sender, EventArgs e)
    {
        if (BindingContext is Models.Counter counter)
        {
            counter.Count++;
        }
    }

    private void DecrementCounter(object sender, EventArgs e)
    {
        if (BindingContext is Models.Counter counter)
        {
            counter.Count--;
        }
    }


    private void SetStartingValue(object sender, EventArgs e)
    {
        if (int.TryParse(StartingPoint.Text,out int startingCountState))
        {
            if (isStartingValueUnset)
            {
                if (BindingContext is Models.Counter counter)
                {
                    counter.Count = startingCountState;
                    isStartingValueUnset = false;
                }
            }
        }
       
    }

    private void Counter_PropertyChanged(object? sender,PropertyChangedEventArgs e)
    {
        SaveOrUpdateCounter();
    }


    private void SaveOrUpdateCounter()
    {
        if (BindingContext is not Counter counter)
        {
            return;
        }
        
        if (string.IsNullOrWhiteSpace(counter.Name))
        {
            return;
        }
            

        string path = Path.Combine(
            FileSystem.AppDataDirectory,
            "counters.xml"
        );

        XDocument document;

        if (File.Exists(path))
        {
            document = XDocument.Load(path);
        }
        else
        {
            document = new XDocument(
                new XElement("Counters")
            );
        }

        XElement? savedCounter = document.Root?
            .Elements("Counter")
            .FirstOrDefault(element =>
                element.Element("Id")?.Value == counter.Id
            );

        if (savedCounter is null)
        {
            savedCounter = new XElement(
                "Counter",
                new XElement("Id", counter.Id),
                new XElement("Name", counter.Name),
                new XElement("Count", counter.Count),
                new XElement("SavedAt", DateTime.Now.ToString("O"))
            );

            document.Root!.Add(savedCounter);
        }
        else
        {
            savedCounter.SetElementValue("Name", counter.Name);
            savedCounter.SetElementValue("Count", counter.Count);
            savedCounter.SetElementValue(
                "SavedAt",
                DateTime.Now.ToString("O")
            );
        }

        document.Save(path);
    }

    private async void SaveCounter(object sender, EventArgs e)
    {
        SaveOrUpdateCounter();
        await Shell.Current.GoToAsync("..");
    }
    protected override void OnDisappearing()
    {
        SaveOrUpdateCounter();
        base.OnDisappearing();
    }
}
