using KelsonBall.Utils.ActorModel;
using Roslyn.Lua.Core;
using Roslyn.Lua.Core.LuaSyntaxTree;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System;
using System.Windows.Input;
using System.Windows;

namespace Roslyn.Lua.TreeViewer
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private XmlNodeViewModel _root;
        public XmlNodeViewModel SyntaxTree
        {
            get => _root;
            set
            {
                _root = value;
                OnPropertyChanged();
            }
        }

        private string _lua = "hello world";
        public string Lua
        {
            get => _lua;
            set
            {
                _lua = value;                
                OnPropertyChanged();
            }
        }

        public ICommand ParseCommand { get; set; }

        class ActionCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;

            private Action _action;

            public ActionCommand(Action action) => _action = action;

            public bool CanExecute(object parameter) => true;
                   
            public void Execute(object parameter) => _action?.Invoke();
        }

        public MainViewModel()
        {
            _actor = new TreeActor(stop => App.Current.Exit += (s,e) => stop());
            ParseCommand = new ActionCommand(() =>
            {
                _actor.Invoke(() =>
                {
                    try
                    {
                        var doc = new XmlDocument();
                        doc.LoadXml(Lua.Tokens().ToList().Create<Program>().SerializeToXml(new StringBuilder()).ToString());
                        App.Current.Dispatcher.Invoke(() => SyntaxTree = new XmlNodeViewModel(doc.FirstChild));
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.ToString());
                    }
                });
            });
        }

        private TreeActor _actor;
        public class TreeActor : AbstractActor { public TreeActor(Action<Action> subscribeToStopEvent) : base(subscribeToStopEvent) { } }
    }
}
