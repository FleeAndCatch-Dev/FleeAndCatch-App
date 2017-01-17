using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleeAndCatch.Components
{
    public class ComponentType
    {
        /// <summary>
        /// Enumeration for the different identification types of the components
        /// </summary>
        public enum IdentificationType
        {
            Undefined, App, Robot
        }

        /// <summary>
        /// Enumeration for the different robot types
        /// </summary>
        public enum RobotType
        {
            Undefined, ThreeWheelDrive, FourWheelDrive, ChainDrive
        }

        /// <summary>
        /// Enumeration for the different role types
        /// </summary>
        public enum RoleType
        {
            Undefined, Catcher, Fugitive
        }

        /// <summary>
        /// Enumeration for the different szenario types of
        /// </summary>
        public enum SzenarioType
        {
            Control, Synchron, Follow
        }
    }
}
