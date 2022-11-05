using Renci.SshNet;
using System.Management.Automation;

namespace PSSftpProvider
{
    public class SftpDriveInfo : PSDriveInfo
    {
        public SftpDriveInfo(PSDriveInfo driveInfo, SftpClient client) : base(driveInfo)
        {
            Client = client;
        }

        public SftpClient Client { get; }
    }
}