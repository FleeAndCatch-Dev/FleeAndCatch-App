namespace Commands.Components
{
    public class ComponentType
    {
        public enum IdentificationType
        {
            Undefined, App, Robot
        }

        public enum RobotType
        {
            Undefined, ThreeWheelDrive, FourWheelDrive, ChainDrive
        }

        public enum RoleType
        {
            Undefined, Catcher, Fugitive
        }
    }
}
