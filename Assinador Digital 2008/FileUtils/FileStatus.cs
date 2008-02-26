using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FileUtils
{
    public class FileStatus
    {
        public string Path;
        public Status Status;

        public FileStatus(string path, Status status)
        {
            Path = path;
            Status = status;
        }
    }

    public enum Status
    {
        Success,
        UnauthorizedAccess,
        DirectoryNotFound,
        PathTooLong,
        NotFound,
        Unmodified,
        ModifiedButNotBackedUp,
        GenericError,
        CorruptedContent,
        NotSigned,
        InUseByAnotherProcess
    }
}
