using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch.Commands.Models.Szenarios;
using FleeAndCatch_App.Communication;
using Newtonsoft.Json;
using PropertyChanged;
using Command = Xamarin.Forms.Command;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class SzenarioInformationPageModel : FreshMvvm.FreshBasePageModel
    {
        public Szenario Szenario { get; set; }
        public List<Group> GroupedRobots { get; set; }
        private bool accept;

        public override void Init(object initData)
        {
            base.Init(initData);

            Szenario = initData as Szenario;
            accept = false;
            GenerateLists();
        }

        protected override void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            Szenario = Client.Szenario;
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            if (!accept)
            {
                foreach (var t in Szenario.Robots)
                    t.Active = false;

                //Send control end command 
                Szenario.Command = ControlType.End.ToString();
                var cmd = new SzenarioCommand(CommandType.Szenario.ToString(), ControlType.Control.ToString(), Client.Identification, Szenario);
                Client.SendCmd(JsonConvert.SerializeObject(cmd));
            }

            base.ViewIsDisappearing(sender, e);
        }

        public Command BStart_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    accept = true;
                    var type = (SzenarioCommandType) Enum.Parse(typeof(SzenarioCommandType), Szenario.Type);
                    switch (type)
                    {
                        case SzenarioCommandType.Control:
                            //Send control begin command to start the szenario
                            Szenario.Command = ControlType.Begin.ToString();
                            var cmd = new SzenarioCommand(CommandType.Szenario.ToString(), Szenario.Type, Client.Identification, Szenario);
                            Client.SendCmd(cmd.ToJsonString());

                            CoreMethods.PushPageModel<ControlPageModel>(Szenario);
                            break;
                        case SzenarioCommandType.Synchron:
                            break;
                        case SzenarioCommandType.Follow:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }                
                    RaisePropertyChanged();
                });
            }
        }

        /// <summary>
        /// Generates the elements for the informations
        /// </summary>
        private void GenerateLists()
        {
            GroupedRobots = new List<Group>();
            foreach (var t in Szenario.Robots)
            {
                var robot = new Group
                {
                    new Item { Name = "Id:", Text = Convert.ToString(t.Identification.Id)},
                    new Item { Name = "SubType:", Text = Convert.ToString(t.Identification.Subtype)},
                    new Item { Name = "RoleType:", Text = Convert.ToString(t.Identification.Roletype)},
                    new Item { Name = "Active:", Text = Convert.ToString(t.Active)},
                    new Item { Name = "Position:", Text = Convert.ToString(t.Position.X) + " " + Convert.ToString(t.Position.Y) + " " + Convert.ToString(t.Position.Orientation)},
                    new Item { Name = "Speed:", Text = Convert.ToString(t.Speed)}
                };
                robot.Name = "Robot";
                GroupedRobots.Add(robot);
            }
        }
    }

    [ImplementPropertyChanged]
    public class Group : ObservableCollection<Item>
    {
        public string Name { get; set; }
    }

    [ImplementPropertyChanged]
    public class Item
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
