using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;

namespace Avalonia.ReactiveProgressBar.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        const int MaxCout = 100;
        private int _progress;
        private int _count = 0;

        public int Progress
        {
            get => _progress;
            set => this.RaiseAndSetIfChanged(ref _progress, value);
        }

        public MainWindowViewModel()
        {
            this.SubscribeCollection()
                .Subscribe(x => 
                {
                    this.Progress = (x.Index * 100) / MaxCout;
                });
        }

        private IObservable<Item> SubscribeCollection()
        {
            var scheduleInstance = ThreadPoolScheduler.Instance;

            return Observable.Create<Item>(element => 
            {
                var disposable = Observable
                    .Interval(TimeSpan.FromMilliseconds(250), scheduleInstance)
                    .Subscribe(_ => 
                    {
                        this._count ++;
                        if (this._count <= MaxCout)
                        {
                            element.OnNext(new Item { Index = this._count });
                        }
                    });

                return Disposable.Empty;
            });
        }
    }

    public class Item 
    {
        public int Index { get; set; }
    }
}
