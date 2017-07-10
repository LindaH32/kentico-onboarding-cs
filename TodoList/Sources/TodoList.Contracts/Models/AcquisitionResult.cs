namespace TodoList.Contracts.Models
{
    public class AcquisitionResult
    {
        public static AcquisitionResult Create(ListItem item) 
            => new AcquisitionResult {AcquiredItem = item};

        private AcquisitionResult() { }

        public ListItem AcquiredItem { get; private set; }

        public bool WasSuccessful => AcquiredItem != null;
    }
}