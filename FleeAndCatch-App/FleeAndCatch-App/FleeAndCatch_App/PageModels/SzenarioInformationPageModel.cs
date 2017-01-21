using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models.Szenarios;
using PropertyChanged;
using Xamarin.Forms;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class SzenarioInformationPageModel : FreshMvvm.FreshBasePageModel
    {
        public Szenario Szenario { get; set; }
        public List<Group> GroupedApps { get; set; }
        public List<Group> GroupedRobots { get; set; }

        public override void Init(object initData)
        {
            base.Init(initData);

            Szenario = initData as Szenario;
            GenerateLists();
        }

        public Command BStart_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    CoreMethods.PushPageModel<ControlPageModel>(Szenario);
                    RaisePropertyChanged();
                });
            }
        }

        private void GenerateLists()
        {
            GroupedApps = new List<Group>();
            foreach (var t in Szenario.Apps)
            {
                var app = new Group
                {
                    new Item { Name = "Id:", Text = Convert.ToString(t.Identification.Id)},
                    new Item { Name = "RoleType:", Text = Convert.ToString(t.Identification.Roletype)},
                    new Item { Name = "Active:", Text = Convert.ToString(t.Active)}
                };
                GroupedApps.Add(app);
            }
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
                GroupedApps.Add(robot);
            }
        }
    }

    [ImplementPropertyChanged]
    public class Group : ObservableCollection<Item>
    {
    }

    [ImplementPropertyChanged]
    public class Item
    {
        public string Name { get; set; }
        public string Text { get; set; }
    }
}
