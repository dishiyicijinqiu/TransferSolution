using SuperSocket.ClientEngine.Protocol;

namespace TransferClientLib
{
    public class TransferSocketCommandInfo : ICommandInfo
    {
        public TransferSocketCommandInfo()
        {
        }
        public TransferSocketCommandInfo(string key)
        {
            Key = key;
        }
        public TransferSocketCommandInfo(string key, string text)
        {
            Key = key;
            Text = text;
        }
        public TransferSocketCommandInfo(string key, byte[] data)
        {
            Key = key;
            Data = data;
        }
        public string Key { get; set; }
        public byte[] Data { get; set; }
        public string Text { get; set; }
    }
}
