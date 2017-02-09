using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLyx.Utilities
{
    public class FileIO
    {

        // Constructor
        protected FileIO() { }


        // Add date to file name
        protected static string AddDateToFileName(DateTime d, string dateFormat, string baseFileName, string extension)
        {
            string dateString = d.ToString(dateFormat);
            return baseFileName + dateString + extension;
        }
    }


    public class MarkitEquityVolatilityFileIO : FileIO
    {

        // Properties
        protected static readonly List<string> MarkitFileNames = new List<string>() {
                            "Lyxor_ExchangeCloseSPUS_EQVol_Index_Exchange_",
                            "Lyxor_ExchangeCloseSTOXX_EQVol_Index_Exchange_",
                            "Lyxor_ExchangeCloseLSE_EQVol_Index_Exchange_",
                            "Lyxor_ExchangeCloseSWIND_EQVol_Index_Exchange_",
                            "Lyxor_ExchangeCloseOSK_EQVol_Index_Exchange_",
                            "Lyxor_ExchangeCloseTK_EQVol_Index_Exchange_",
                            "Lyxor_ExchangeCloseNYSEIND_EQVol_Index_Exchange_" };


        protected static readonly string extension = ".csv";
        protected static readonly string dateFormat_FR = "AAAAMMJJ";
        protected static readonly string dateFormat_EN = "yyyyMMdd";

        
        protected static readonly string sourcePath = @"\\eur.msd.world.socgen\HomeDir\FTPHomeDir\TRANSLyxTechUsrFTP\GEDS-ING-EUR-LYX-DATA\dfo\Markit";
        protected static readonly string targetPath = @"F:\MARK-LYX-QMG\DValo\_Documents\Markit Vol";




        #region Methods to copy from FTP to local directory

        /// <summary>  
        ///  Copy files from FTP directory to local directory  
        /// </summary>  
        public static void CopySingleFile(string fileName)
        {
            CopySingleFile(DateTime.Today, fileName);
        }

        /// <summary>  
        ///  Copy a file from the source to the target directory while renaming it.  
        /// </summary>  
        public static void CopySingleFile(DateTime d, string sourceFileName, string targetFileName, bool overWrite = false)
        {
            String completeSourcefileName = AddDateToFileName(d, dateFormat_EN, sourceFileName, extension);
            String completeTargetFileName = AddDateToFileName(d, dateFormat_EN, targetFileName, extension);

            String sourceFile = System.IO.Path.Combine(sourcePath, completeSourcefileName);
            String destinationFile = System.IO.Path.Combine(targetPath, completeTargetFileName);

            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);
            }

            System.IO.File.Copy(sourceFile, destinationFile, overWrite);
            
        }

        /// <summary>  
        ///   Copy a file from the source to the target directory.
        /// </summary>  
        public static void CopySingleFile(DateTime d, string uniqueFileName)
        {
            CopySingleFile(d, uniqueFileName, uniqueFileName);
        }


        /// <summary>  
        ///  Copy all Markit Equity IV files from the source (SFTP directory) to the target directory.  
        /// </summary>  
        public static void CopyAllFiles(DateTime d)
        {
            foreach (string name in MarkitFileNames)
            {
                try
                {
                    CopySingleFile(d, name);
                }

                catch
                {
                    Console.WriteLine("Unable to copy file : {0}", name);
                }
            }
        }

        
            #endregion

        }


}
