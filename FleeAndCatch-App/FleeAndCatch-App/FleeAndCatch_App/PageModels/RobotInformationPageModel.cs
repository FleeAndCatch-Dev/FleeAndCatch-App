using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FleeAndCatch.Commands.Models.Devices.Robots;
using Xamarin.Forms;

namespace FleeAndCatch_App.PageModels
{
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
