using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Commands;
using Communication;
using ComponentType;
using FleeAndCatch_App.pages.content.control;
using Robots;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Robot = Commands.Robot;

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

            var connectionTask = new Task(UpdateRobotList);
            connectionTask.Start();
        }

        private async void UpdateRobotList()
        {
            if (Client.Connected)
            {
                RobotController.Updated = false;
                var command = new Synchronisation(CommandType.Type.Synchronisation.ToString(), SynchronisationType.Type.GetRobots.ToString(), new Identification(Client.Id, Client.Address, Client.Port, Client.Type, Client.Subtype), RobotController.Robots);
                Client.SendCmd(command.GetCommand());

                while (!RobotController.Updated)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(5));
                }       

                var groupedRobots = new ObservableCollection<Group>();
                for (var i = 0; i < Enum.GetNames(typeof(RobotType.Type)).Length; i++)
                {
                    var groupedRobot = new Group(Enum.GetNames(typeof(RobotType.Type))[i]);
                    foreach (var t in RobotController.Robots)
                    {
                        if(t.Identification.Subtype == Enum.GetNames(typeof(RobotType.Type))[i])
                            groupedRobot.Add(new Item(t.Identification.Id.ToString()));
                    }
                    groupedRobots.Insert(groupedRobots.Count, groupedRobot);
                }
                Device.BeginInvokeOnMainThread( () =>
                {
                    LRobots.ItemsSource = null;
                    LRobots.ItemsSource = groupedRobots;
                    LRobots.GroupHeaderTemplate = new DataTemplate(typeof(GroupCell));
                    LRobots.ItemTemplate = new DataTemplate(typeof(ItemCell));
                });
                return;
            }
            Device.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Error", "Ups, there is something wrong, please start the application again", "OK");
            });
        }

        private void LRobots_OnRefreshing(object sender, EventArgs e)
        {
            var connectionTask = new Task(UpdateRobotList);
            connectionTask.Start();
        }

        private async void LRobots_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var robotlist = (ListView)sender;
            var current = (Item)robotlist.SelectedItem;

            Robot robot = null;
            foreach (var t in RobotController.Robots)
            {
                if (Convert.ToInt32(Convert.ToString(current.Id)) == t.Identification.Id)
                    robot = t;
            }

            await Navigation.PushAsync(new pages.content.control.Robot(robot));
        }
    }

    class Group : ObservableCollection<Item>
    {
        public string Name { get; private set; }

        public Group(string pName)
        {
            Name = pName;
        }
    }

    class Item
    {
        public string Id { get; private set; }

        public Item(string pId)
        {
            Id = pId;
        }
    }

    class GroupCell : ViewCell
    {
        private Label label;

        public GroupCell()
        {
            var layout = new StackLayout { Padding = new Thickness(20, 10) };
            label = new Label();

            label.SetBinding(Label.TextProperty, "Name");
            layout.Children.Add(label);
            label.HorizontalTextAlignment = TextAlignment.Start;

            View = layout;
        }

        public string GetText()
        {
            return label.Text;
        }
    }

    class ItemCell : ViewCell
    {
        private Label label;

        public ItemCell()
        {
            var layout = new StackLayout { Padding = new Thickness(20, 10) };
            label = new Label();

            label.SetBinding(Label.TextProperty, "Id");
            layout.Children.Add(label);
            label.HorizontalTextAlignment = TextAlignment.Center;

            View = layout;
        }

        public string GetText()
        {
            return label.Text;
        }
    }
}
