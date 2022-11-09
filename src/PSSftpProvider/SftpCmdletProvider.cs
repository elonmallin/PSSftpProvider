using Renci.SshNet;
using Renci.SshNet.Security;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace PSSftpProvider
{
    [CmdletProvider("Sftp", ProviderCapabilities.Credentials)]
    public class SftpCmdletProvider : NavigationCmdletProvider, IContentCmdletProvider
    {
        private SftpDriveInfo DriveInfo => (SftpDriveInfo)PSDriveInfo;
        private SftpClient Client => DriveInfo.Client;

        public void ClearContent(string path)
        {
            throw new NotImplementedException();
        }

        public object ClearContentDynamicParameters(string path)
        {
            throw new NotImplementedException();
        }

        public IContentReader GetContentReader(string path)
        {
            throw new NotImplementedException();
        }

        public object GetContentReaderDynamicParameters(string path)
        {
            throw new NotImplementedException();
        }

        public IContentWriter GetContentWriter(string path)
        {
            throw new NotImplementedException();
        }

        public object GetContentWriterDynamicParameters(string path)
        {
            throw new NotImplementedException();
        }

        public override string GetResourceString(string baseName, string resourceId)
        {
            return base.GetResourceString(baseName, resourceId);
        }

        protected override void ClearItem(string path)
        {
            base.ClearItem(path);
        }

        protected override object ClearItemDynamicParameters(string path)
        {
            return base.ClearItemDynamicParameters(path);
        }

        protected override bool ConvertPath(string path, string filter, ref string updatedPath, ref string updatedFilter)
        {
            return base.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
        }

        // TODO: Has to be same provider, so only sftp!!!!
        protected override void CopyItem(string path, string copyPath, bool recurse)
        {
            if (!IsItemContainer(path))
            {
                var sourceStream = new MemoryStream();

                if (path.Contains(PSDriveInfo.Root))
                {
                    var localPath = GetLocalPath(path);

                    Client.DownloadFile(localPath, sourceStream);
                }
                else
                {
                    using (var stream = File.OpenRead(path))
                    {
                        stream.CopyTo(sourceStream);
                    }
                }

                if (copyPath.Contains(PSDriveInfo.Root))
                {
                    var localCopyPath = GetLocalPath(path);

                    Client.UploadFile(sourceStream, localCopyPath);
                }
                else
                {
                    using (var stream = File.OpenWrite(copyPath))
                    {
                        sourceStream.CopyTo(stream);
                    }
                }

                sourceStream.Dispose();
            }
            else if (recurse)
            {
                var localPath = GetLocalPath(path);

                foreach (var item in Client.ListDirectory(localPath))
                {
                    CopyItem(item.FullName, MakePath(copyPath, item.Name), recurse);
                }
            }
        }

        protected override object CopyItemDynamicParameters(string path, string destination, bool recurse)
        {
            return base.CopyItemDynamicParameters(path, destination, recurse);
        }

        protected override string[] ExpandPath(string path)
        {
            var localPath = GetLocalPath(path);

            return new string[]
            {
                Path.GetDirectoryName(localPath),
                Path.GetFileName(localPath),
            };
        }

        protected override void GetChildItems(string path, bool recurse, uint depth)
        {
            if (depth <= 0) {
                return;
            }

            var localPath = GetLocalPath(path);

            foreach (var item in Client.ListDirectory(localPath))
            {
                WriteItemObject(item.Name, item.FullName, item.IsDirectory);
                if (recurse && item.IsDirectory) {
                    GetChildItems(item.FullName, recurse, depth-1);
                }
            }
        }

        protected override void GetChildItems(string path, bool recurse)
        {
            GetChildItems(path, recurse, uint.MaxValue);
        }

        protected override object GetChildItemsDynamicParameters(string path, bool recurse)
        {
            var list = new List<object>();

            var localPath = GetLocalPath(path);

            foreach (var item in Client.ListDirectory(localPath))
            {
                list.Add(item.FullName);
                // WriteItemObject(item, item.Name, item.IsDirectory);
                if (recurse && item.IsDirectory) {
                    list.AddRange(GetChildItemsDynamicParameters(item.FullName, recurse) as List<object>);
                }
            }

            return list;
            // return base.GetChildItemsDynamicParameters(path, recurse);
        }

        protected override string GetChildName(string path)
        {
            var localPath = GetLocalPath(path);

            var fileName = Path.GetFileName(localPath);

            return string.IsNullOrWhiteSpace(fileName)
                ? "/"
                : fileName;
        }

        protected override void GetChildNames(string path, ReturnContainers returnContainers)
        {
            var localPath = GetLocalPath(path);

            foreach (var item in Client.ListDirectory(localPath))
            {
                WriteItemObject(item.Name, item.FullName, item.IsDirectory);
                if (returnContainers == ReturnContainers.ReturnAllContainers && item.IsDirectory) {
                    GetChildNames(item.FullName, returnContainers);
                }
            }
        }

        protected override object GetChildNamesDynamicParameters(string path)
        {
            return base.GetChildNamesDynamicParameters(path);
        }

        protected override void GetItem(string path)
        {
            var localPath = GetLocalPath(path);

            using (var stream = new MemoryStream())
            {
                Client.DownloadFile(localPath, stream);

                WriteItemObject(stream.ToString(), localPath, IsItemContainer(path));
            }
        }

        protected override object GetItemDynamicParameters(string path)
        {
            return base.GetItemDynamicParameters(path);
        }

        protected override string GetParentPath(string path, string root)
        {
            return base.GetParentPath(path, root)
                .ForwardSlash();
        }

        protected override bool HasChildItems(string path)
        {
            return base.HasChildItems(path);
        }

        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            return base.InitializeDefaultDrives();
        }

        protected override void InvokeDefaultAction(string path)
        {
            base.InvokeDefaultAction(path);
        }

        protected override object InvokeDefaultActionDynamicParameters(string path)
        {
            return base.InvokeDefaultActionDynamicParameters(path);
        }

        protected override bool IsItemContainer(string path)
        {
            var localPath = GetLocalPath(path);
            var item = Client.Get(localPath);
            
            return item.IsDirectory;
        }

        protected override bool IsValidPath(string path)
        {
            var localPath = GetLocalPath(path);

            return !string.IsNullOrWhiteSpace(localPath);
        }

        protected override bool ItemExists(string path)
        {
            var result = true;
            var localPath = GetLocalPath(path);

            if (WildcardPattern.ContainsWildcardCharacters(localPath))
            {
            }
            else
            {
                // result = Client.Exists(localPath);
            }

            return result;
        }

        protected override object ItemExistsDynamicParameters(string path)
        {
            return base.ItemExistsDynamicParameters(path);
        }

        protected override string MakePath(string parent, string child)
        {
            var localPath = Path.Combine(parent.ToLocal(), child)
                .ToLocal();

            return string.IsNullOrWhiteSpace(localPath)
                ? "/"
                : localPath;
        }

        protected override void MoveItem(string path, string destination)
        {
            var localPath = GetLocalPath(path);
            var localDestination = GetLocalPath(destination);

            Client.RenameFile(localPath, localDestination);
        }

        protected override object MoveItemDynamicParameters(string path, string destination)
        {
            return base.MoveItemDynamicParameters(path, destination);
        }

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            var uri = new Uri(drive.Root);
            var username = !string.IsNullOrEmpty(uri.UserInfo)
                ? uri.UserInfo.Split(':')[0]
                : drive.Credential.UserName;
            var password = !string.IsNullOrEmpty(uri.UserInfo)
                ? uri.UserInfo.Split(':')[1]
                : drive.Credential.GetNetworkCredential().Password;
            
            var connectionInfo = new ConnectionInfo(uri.Host, uri.Port, username, new List<AuthenticationMethod>
            {
                new PasswordAuthenticationMethod(username, password),
            }.ToArray());
            var client = new SftpClient(connectionInfo);
            client.Connect();

            drive.CurrentLocation = uri.LocalPath;
            client.ChangeDirectory(uri.LocalPath);

            return new SftpDriveInfo(drive, client);
        }

        protected override object NewDriveDynamicParameters()
        {
            return base.NewDriveDynamicParameters();
        }

        protected override void NewItem(string path, string itemTypeName, object newItemValue)
        {
            var localPath = GetLocalPath(path);

            if (itemTypeName.Equals("File", StringComparison.InvariantCultureIgnoreCase))
            {
                using (var stream = new MemoryStream())
                {
                    var sr = new StreamWriter(stream);
                    sr.Write(newItemValue.ToString());

                    Client.UploadFile(stream, localPath);
                }
            }
            else if (itemTypeName.Equals("Directory", StringComparison.InvariantCultureIgnoreCase))
            {
                Client.CreateDirectory(localPath);
            }
            else if (itemTypeName.Equals("SymbolicLink", StringComparison.InvariantCultureIgnoreCase))
            {

            }
            else if (itemTypeName.Equals("Junction", StringComparison.InvariantCultureIgnoreCase))
            {

            }
            else if (itemTypeName.Equals("HardLink", StringComparison.InvariantCultureIgnoreCase))
            {

            }
        }

        protected override object NewItemDynamicParameters(string path, string itemTypeName, object newItemValue)
        {
            return base.NewItemDynamicParameters(path, itemTypeName, newItemValue);
        }

        protected override string NormalizeRelativePath(string path, string basePath)
        {
            return base.NormalizeRelativePath(path, basePath);
        }

        protected override PSDriveInfo RemoveDrive(PSDriveInfo drive)
        {
            var client = (drive as SftpDriveInfo).Client;
            client.Dispose();

            return base.RemoveDrive(drive);
        }

        protected override void RemoveItem(string path, bool recurse)
        {
            var localPath = GetLocalPath(path);

            var item = Client.Get(localPath);

            if (!item.IsDirectory)
            {
                item.Delete();
            }
            else
            {
                if (recurse)
                {
                    item.Delete();
                }
                else
                {
                    var items = Client.ListDirectory(localPath);
                    foreach (var i in items)
                    {
                        RemoveItem(i.FullName, recurse);
                    }
                    item.Delete();
                }
            }
        }

        protected override object RemoveItemDynamicParameters(string path, bool recurse)
        {
            return base.RemoveItemDynamicParameters(path, recurse);
        }

        protected override void RenameItem(string path, string newName)
        {
            base.RenameItem(path, newName);
        }

        protected override object RenameItemDynamicParameters(string path, string newName)
        {
            return base.RenameItemDynamicParameters(path, newName);
        }

        protected override void SetItem(string path, object value)
        {
            base.SetItem(path, value);
        }

        protected override object SetItemDynamicParameters(string path, object value)
        {
            return base.SetItemDynamicParameters(path, value);
        }

        protected override ProviderInfo Start(ProviderInfo providerInfo)
        {
            return base.Start(providerInfo);
        }

        protected override object StartDynamicParameters()
        {
            return base.StartDynamicParameters();
        }

        protected override void Stop()
        {
            base.Stop();
        }

        protected override void StopProcessing()
        {
            base.StopProcessing();
        }

        private string GetLocalPath(string path)
        {
            return path
                .Replace("\\", "/")
                .Replace(DriveInfo.Root, DriveInfo.Client.WorkingDirectory);
        }
    }
}
