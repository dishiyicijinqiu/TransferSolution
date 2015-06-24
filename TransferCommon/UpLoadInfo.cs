using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransferCommon
{
    [Serializable]
    public class UpLoadInfo
    {
        public string FileName { get; set; }
        public string SaveName { get; set; }
        public long FileSize { get; set; }
        public long StartPos { get; set; }
        public long Length { get; set; }
        public UpLoadInfo(string _FileName, long _FileSize, long _StartPos = 0, long _Length = 0, string _saveName = "")
        {
            if (string.IsNullOrWhiteSpace(_saveName))
                _saveName = System.IO.Path.GetFileName(_FileName);
            if (_Length == 0)
                _Length = FileSize;
            FileName = _FileName;
            SaveName = _saveName;
            FileSize = _FileSize;
            StartPos = _StartPos;
            Length = _Length;
        }
    }
}
