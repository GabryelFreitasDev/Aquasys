namespace Aquasys.Core.Sync
{
    public class PushRequestDto
    {
        public Dictionary<string, List<object>> Entities { get; set; } = new();
    }

    public class PullResponseDto
    {
        public DateTime ServerTimestamp { get; set; }
        public Dictionary<string, List<object>> Entities { get; set; } = new();
    }
}
