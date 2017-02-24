namespace FleeAndCatch.Commands
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
    }
}
