namespace DankMemerEventCatcher
{
    public sealed class Configuration
    {
        public string Token { get; set; }

        public ushort MinimumInterval { get; set; }
        public ushort MaximumInterval { get; set; }

        public bool JoinHeist { get; set; }

        public Configuration()
        {
            Token = "your-token-here";

            MinimumInterval = 2000;
            MaximumInterval = 3000;

            JoinHeist = false;
        }
    }
}