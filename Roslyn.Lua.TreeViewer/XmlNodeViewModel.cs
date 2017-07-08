using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Linq;
using System.Collections.Generic;

namespace Roslyn.Lua.TreeViewer
{
    public class XmlNodeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private XmlNode _node;

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        private string _content;
        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<XmlNodeViewModel> _children;
        public ObservableCollection<XmlNodeViewModel> Children
        {
            get => _children;
            set
            {
                _children = value;
                OnPropertyChanged();
            }
        }

        public XmlNodeViewModel(XmlNode node)
        {
            Name = node.Name;
            var collection = new ObservableCollection<XmlNodeViewModel>(node.ChildNodes.AsEnumerable().Select(n => new XmlNodeViewModel(n)));
            if (collection.Count == 1 && collection.First().Name == "#text")
                Content = node.InnerText;
            else
                Children = collection;            
        }        
    }

    public static class XmlNodeListExtensions
    {
        public static IEnumerable<XmlNode> AsEnumerable(this XmlNodeList list)
        {
            var enumerator = list.GetEnumerator();
            while (enumerator.MoveNext())
                yield return (XmlNode)enumerator.Current;
        }
    }
}
