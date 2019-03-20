namespace Mallos.Networking.ServerSample
{
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.Windows.Threading;

    public class ObservableCollectionDispatcher<T> : ObservableCollection<T>
    {
        private readonly Dispatcher dispatcher;
        private readonly ObservableCollection<T> mirror;

        public ObservableCollectionDispatcher(Dispatcher dispatcher, ObservableCollection<T> mirror)
        {
            this.dispatcher = dispatcher;

            this.mirror = mirror;
            this.mirror.CollectionChanged += Mirror_CollectionChanged;
        }

        private void Mirror_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            dispatcher.InvokeAsync(() =>
            {
                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        foreach (T message in e.NewItems)
                        {
                            this.Add(message);
                        }
                        break;

                    case NotifyCollectionChangedAction.Move:
                        break;

                    case NotifyCollectionChangedAction.Remove:
                        foreach (T message in e.OldItems)
                        {
                            this.Remove(message);
                        }
                        break;

                    case NotifyCollectionChangedAction.Replace:
                        break;

                    case NotifyCollectionChangedAction.Reset:
                        break;

                    default:
                        break;
                }
            });
        }
    }
}
