using System;
using System.Collections.ObjectModel;
using Commands;
using ComponentType;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FleeAndCatch_App.pages.content
{
    public partial class Control : ContentPage
    {

        public Control()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            /*var command = new Synchronisation(CommandType.Type.Synchronisation.ToString(), SynchronisationType.Type.GetRobots.ToString(), new Client(Communication.Client.Id));
            Communication.Client.SendCmd(command.GetCommand());
            Device.BeginInvokeOnMainThread(UpdateRobotList);*/

            var groupedItems = new ObservableCollection<Group>();
            var groupedTemplate = new DataTemplate(typeof(GroupCell));

            for (var i = 0; i < Enum.GetNames(typeof(RobotType.Type)).Length; i++)
            {
                groupedItems.Insert(groupedItems.Count, new Group(Enum.GetNames(typeof(RobotType.Type))[i]));
            }

            LRobots.ItemsSource = groupedItems;
            LRobots.ItemTemplate = groupedTemplate;
        }

        private void UpdateRobotList()
        {
            
        }

        private void BSynchronize_OnClicked(object sender, EventArgs e)
        {
            var command = new Synchronisation(CommandType.Type.Synchronisation.ToString(), SynchronisationType.Type.GetRobots.ToString(), new Client(Communication.Client.Id));
            Communication.Client.SendCmd(command.GetCommand());
            Device.BeginInvokeOnMainThread(UpdateRobotList);
        }
    }

    class Group : ObservableCollection<Item>
    {
        public string Name { get; private set; }

        public Group(string Name)
        {
            this.Name = Name;
        }
    }

    class GroupCell : ViewCell
    {
        private Label label;

        public GroupCell()
        {
            var layout = new StackLayout { Padding = new Thickness(20, 10) };
            label = new Label();

            label.SetBinding(Label.TextProperty, ".");
            layout.Children.Add(label);
            label.HorizontalTextAlignment = TextAlignment.Center;

            View = layout;
        }

        public string GetText()
        {
            return label.Text;
        }
    }

    class Item
    {
        public String Title { get; private set; }
        public String Description { get; private set; }

        public Item(String title, String description)
        {
            Title = title;
            Description = description;
        }

        // Whatever other properties
    }
}
