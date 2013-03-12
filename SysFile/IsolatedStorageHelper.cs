using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.IsolatedStorage;

namespace LCW.Framework.Common.SysFile
{
    //独立存储将文件存储在系统盘-当前用户-本地的-指定文件夹当中。
    public class IsolatedStorageHelper
    {
        public void CreateDir(string dirName)
        {
            IsolatedStorageFile storeFile =
                IsolatedStorageFile.GetUserStoreForApplication();
            storeFile.CreateDirectory(dirName);
        }

        public void SaveFile(string savePath, string content)
        {
            IsolatedStorageFile storeFile =
                IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream sf = storeFile.CreateFile(savePath);
            using (StreamWriter sw = new StreamWriter(sf))
            {
                sw.WriteLine(content);
            }
            sf.Close();
        }

        public void LoadFile(string readPath)
        {
            string content = string.Empty;
            using (IsolatedStorageFile storeFile =
                IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storeFile.FileExists(readPath))
                {
                    StreamReader sr =
                        new StreamReader(storeFile.OpenFile
                            (readPath, FileMode.Open, FileAccess.Read));
                    content = sr.ReadToEnd();
                }
            }
        }

        public void DeleteFile(string path)
        {
            using (IsolatedStorageFile storeFile =
                IsolatedStorageFile.GetUserStoreForApplication())
            {
                storeFile.DeleteFile(path);
            }
        }

        void DeleteDir(string dirPath)
        {
            using (IsolatedStorageFile storeFile =
                IsolatedStorageFile.GetUserStoreForApplication())
            {
                storeFile.DeleteDirectory(dirPath);
            }
        }

        public void LoadDirs()
        {
            using (IsolatedStorageFile storeFile =
                IsolatedStorageFile.GetUserStoreForApplication())
            {
                var itemSource = storeFile.GetDirectoryNames("*");
            }
        }      

        public string Read(string filename)
        {
            string result = string.Empty;
            System.IO.IsolatedStorage.IsolatedStorageFile isoStore =
                System.IO.IsolatedStorage.IsolatedStorageFile.
                GetUserStoreForDomain();
            try
            {
                // Checks to see if the options.txt file exists.
                if (isoStore.GetFileNames(filename).GetLength(0) != 0)
                {
                    // Opens the file because it exists.
                    System.IO.IsolatedStorage.IsolatedStorageFileStream isos =
                        new System.IO.IsolatedStorage.IsolatedStorageFileStream
                            (filename, System.IO.FileMode.Open, isoStore);
                    System.IO.StreamReader reader = null;
                    try
                    {
                        reader = new System.IO.StreamReader(isos);

                        // Reads the values stored.
                        result=reader.ReadLine();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine
                             ("Cannot read options " + ex.ToString());
                    }
                    finally
                    {
                        reader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine
                    ("Cannot read options " + ex.ToString());
            }
            return result;
        }

        // Writes the button options to the isolated storage.
        public void Write(string filename,string content)
        {
            System.IO.IsolatedStorage.IsolatedStorageFile isoStore =
                System.IO.IsolatedStorage.IsolatedStorageFile.
                GetUserStoreForDomain();
            try
            {
                // Checks if the file exists and, if it does, tries to delete it.
                if (isoStore.GetFileNames(filename).GetLength(0) != 0)
                {
                    isoStore.DeleteFile(filename);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine
                    ("Cannot delete file " + ex.ToString());
            }

            // Creates the options file and writes the button options to it.
            System.IO.StreamWriter writer = null;
            try
            {
                System.IO.IsolatedStorage.IsolatedStorageFileStream isos = new
                    System.IO.IsolatedStorage.IsolatedStorageFileStream(filename,
                    System.IO.FileMode.CreateNew, isoStore);

                writer = new System.IO.StreamWriter(isos);
                writer.WriteLine(content);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine
                   ("Cannot write options " + ex.ToString());
            }
            finally
            {
                writer.Close();
            }
        }
    }
}

//使1用?应|用?程ì序ò存?储￠创′建¨对?象ó
            //using (IsolatedStorageFile storeFile =
            //    IsolatedStorageFile.GetUserStoreForApplication())
            //{
            //    //获?取?旧é空?间?大ó小?
            //    long oldSize = storeFile.AvailableFreeSpace;
            //    //定¨义?新?增?空?间?大ó小?
            //    long newSize = 2097152;
            //    if (oldSize < newSize)
            //    {
            //        //分?配?新?的?存?储￠空?间?
            //        storeFile.IncreaseQuotaTo(newSize);
            //    }
            //}
