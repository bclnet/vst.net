namespace Jacobi.Vst3.Core
{
    /// <summary>
    /// Predefined StateType used for Key kStateType
    /// </summary>
    public static class StateType
    {
        public const string Project = "Project";        // the state is restored from a project loading or it is saved in a project
        public const string Default = "Default";        // the state is restored from a preset (marked as default) or the host wants to store a default state of the plug-in
    }
}
