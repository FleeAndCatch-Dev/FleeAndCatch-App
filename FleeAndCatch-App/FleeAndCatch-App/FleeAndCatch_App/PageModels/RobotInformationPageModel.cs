using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands;
using FleeAndCatch.Commands.Models.Devices.Robots;
using FleeAndCatch_App.Communication;
using PropertyChanged;
using Command = Xamarin.Forms.Command;

namespace FleeAndCatch_App.PageModels
{
    [ImplementPropertyChanged]
    public class RobotInformationPageModel : FreshMvvm.FreshBasePageModel
    {
        public Robot Robot { get; set; }

        public RobotInformationPageModel()
        {
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            Robot = initData as Robot;
        }

        public Command BStart_OnCommand
        {
            get
            {
                return new Command(() =>
                {
                    CoreMethods.PushPageModel<ControlPageModel>(Robot);
                    RaisePropertyChanged();
                });
            }
        }
    }
}
