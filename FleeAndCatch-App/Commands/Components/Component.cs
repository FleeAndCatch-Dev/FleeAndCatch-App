namespace Commands.Components
{
    public class ComponentType
    {
        public enum IdentificationType
        {
            App, Robot
        }

        public enum RobotType
        {
            ThreeWheelDrive, FourWheelDrive, ChainDrive
        }

        public enum RoleType
        {
            Undefined, Catcher, Fugitive
        }
    }
}
