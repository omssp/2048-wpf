using Microsoft.Win32;

namespace Game2048.Common.Models
{
    public class RegValueData
    {
        public string Name { get; set; }

        public RegistryValueKind Kind { get; set; }

        public byte[] Data { get; set; }
    }
}
