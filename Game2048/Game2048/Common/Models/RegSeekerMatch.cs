
namespace Game2048.Common.Models
{
    public class RegSeekerMatch
    {
        public string Key { get; set; }

        public RegValueData[] Data { get; set; }

        public bool HasSubKeys { get; set; }

        public override string ToString()
        {
            return $"({Key}:{Data})";
        }
    }
}
