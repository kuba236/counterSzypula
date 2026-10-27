
namespace counterSzypula.Models
{
    public class Counter : BindableObject
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public DateTime SavedAt { get; set; }

        private int _count = 0;
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();

            }
        }
    }
}
