using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;
using Commands.Components;
using Commands.Devices.Robots;
using Communication;
using Controller;
using FleeAndCatch_App.pages.content.home.szenario.selection;
using Xamarin.Forms;

namespace FleeAndCatch_App.pages.content.home.szenario
{
    public partial class Robots : ContentPage
    {
        public Robots()
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
                var command = new Synchronization(CommandType.Synchronization.ToString(), SynchronizationType.Robots.ToString(), Client.Identification, RobotController.Robots);
                Client.SendCmd(command.GetCommand());

                while (!RobotController.Updated)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(10));
                }

                var robotCollection = new ObservableCollection<RobotViewModel>();

                for (int i = 0; i < Enum.GetNames(typeof(ComponentType.RobotType)).Length; i++)
                {
                    var counter = 0;
                    foreach (var t in RobotController.Robots)
                    {
                        if (t.Identification.Subtype == Enum.GetNames(typeof(ComponentType.RobotType))[i] && !t.Active)
                            counter++;
                    }
                    robotCollection.Insert(robotCollection.Count, new RobotViewModel {Name = Enum.GetNames(typeof(ComponentType.RobotType))[i], Number = counter});
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    LRobots.ItemsSource = null;
                    LRobots.ItemsSource = robotCollection;
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
            var current = (RobotViewModel)robotlist.SelectedItem;

            Robot robot = null;
            foreach (var t in RobotController.Robots)
            {
                if (current.Name == t.Identification.Subtype)
                    robot = t;
            }

            var cmd = new Control(CommandType.Control.ToString(), ControlType.Begin.ToString(), Client.Identification, robot, new Position.Steering(0, 0));
            Client.SendCmd(cmd.GetCommand());

            await Navigation.PushAsync(new RobotInformation(robot));
        }
    }

    class RobotViewModel
    {
        public string Name { get; set; }
        public int Number { get; set; }
    }
}
