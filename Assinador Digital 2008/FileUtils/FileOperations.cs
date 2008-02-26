using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace FileUtils
{
    public static class FileOperations
    {
        #region PrivateProperties

        static string originalPath;
        private static List<string> allowedFiles = new List<string>();

        #endregion

        #region PublicMethods

        public static List<FileStatus> Copy(List<string> originalFiles, string target, bool overwrite)
        {
            if (!(target.EndsWith("\\")))
                target += "\\";

            //set the root folder that contains the files
            originalPath = Path.GetDirectoryName(originalFiles[0]);
            foreach (string documentPath in originalFiles)
            {
                string newPath = Path.GetDirectoryName(documentPath);
                if (originalPath.Length > newPath.Length)
                    originalPath = newPath;
            }

            List<FileStatus> report = new List<FileStatus>();
            List<string> unauthorizedPath = new List<string>();
            foreach (string fileToCopy in originalFiles)
            {
                string targetPath;
                bool parentPathUnauthorizedAccess = false;
                string subfolderPath = (Path.GetDirectoryName(fileToCopy)).Substring(originalPath.Length);
                if (subfolderPath == "")
                {
                    targetPath = target;
                }
                else
                {
                    targetPath = target + subfolderPath + "\\";
                }

                targetPath += Path.GetFileName(fileToCopy);

                foreach (string parentPath in unauthorizedPath)
                {
                    if ((parentPath + "\\") == targetPath.Substring(0, parentPath.Length + 1))
                        parentPathUnauthorizedAccess = true;
                }

                if (!parentPathUnauthorizedAccess)
                {
                    try
                    {
                        if (File.Exists(targetPath))
                            if (overwrite)
                            {
                                File.Copy(fileToCopy, targetPath, true);
                                report.Add(new FileStatus(fileToCopy, Status.Success));
                            }
                            else
                            {
                                report.Add(new FileStatus(fileToCopy, Status.Unmodified));
                            }
                        else
                        {
                            File.Copy(fileToCopy, targetPath, false);
                            report.Add(new FileStatus(fileToCopy, Status.Success));
                        }
                    }
                    catch (UnauthorizedAccessException)
                    {
                        unauthorizedPath.Add(Path.GetDirectoryName(targetPath));
                        report.Add(new FileStatus(fileToCopy, Status.UnauthorizedAccess));
                    }
                    catch (DirectoryNotFoundException)
                    {
                        try
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(targetPath));
                            File.Copy(fileToCopy, targetPath);
                            report.Add(new FileStatus(fileToCopy, Status.Success));
                        }
                        catch (UnauthorizedAccessException)
                        {
                            unauthorizedPath.Add(Path.GetDirectoryName(targetPath));
                            report.Add(new FileStatus(fileToCopy, Status.UnauthorizedAccess));
                        }
                        catch
                        {
                            report.Add(new FileStatus(fileToCopy, Status.GenericError));
                        }
                    }
                    catch (PathTooLongException)
                    {
                        report.Add(new FileStatus(fileToCopy, Status.PathTooLong));
                    }
                    catch (FileNotFoundException)
                    {
                        report.Add(new FileStatus(fileToCopy, Status.NotFound));
                    }
                    catch
                    {
                        report.Add(new FileStatus(fileToCopy, Status.GenericError));
                    }
                }
                else
                {
                    unauthorizedPath.Add(Path.GetDirectoryName(targetPath));
                    report.Add(new FileStatus(fileToCopy, Status.UnauthorizedAccess));
                }
            }
            return report;
        }

        public static List<string> ListAllowedFilesAndSubfolders(string[] filesAndFolders, bool openFolder, bool openSubfolders)
        {
            List<string> pathsNotAllowed = new List<string>();
            foreach (string path in filesAndFolders)
            {
                if (Path.HasExtension(path))
                {
                    if (!allowedFiles.Contains(path))
                    {
                        string fileExtension = Path.GetExtension(path);
                        if ((fileExtension == ".docx") || (fileExtension == ".docm")
                            || (fileExtension == ".pptx") || (fileExtension == ".pptm")
                            || (fileExtension == ".xlsx") || (fileExtension == ".xlsm")
                            || (fileExtension == ".xps"))
                        {
                            allowedFiles.Add(path);
                        }
                    }
                    else
                    {
                        allowedFiles.Clear();
                        string fileExtension = Path.GetExtension(path);
                        if ((fileExtension == ".docx") || (fileExtension == ".docm")
                            || (fileExtension == ".pptx") || (fileExtension == ".pptm")
                            || (fileExtension == ".xlsx") || (fileExtension == ".xlsm")
                            || (fileExtension == ".xps"))
                        {
                            allowedFiles.Add(path);
                        }
                    }
                }
                else
                {
                    if (openFolder)
                    {
                        bool parentPathAllowedAccess = true;
                        foreach (string parentPathNotAllowed in pathsNotAllowed)
                        {
                            if ((parentPathNotAllowed.Length + 1) <= path.Length)
                                if ((parentPathNotAllowed + "\\") == path.Substring(0, parentPathNotAllowed.Length + 1))
                                    parentPathAllowedAccess = false;
                        }
                        if (parentPathAllowedAccess)
                        {
                            try
                            {
                                string[] filesInFolder = Directory.GetFiles(path);
                                ListAllowedFilesAndSubfolders(filesInFolder, openSubfolders, openSubfolders);
                                string[] foldersInFolder = Directory.GetDirectories(path);
                                ListAllowedFilesAndSubfolders(foldersInFolder, openSubfolders, openSubfolders);
                            }
                            catch
                            {
                                pathsNotAllowed.Add(path);
                            }
                        }
                    }
                }
            }
            return allowedFiles;
        }

        #endregion
    }
}
