using System;
using System.Collections.Generic;
using System.IO;

namespace PrepareDDrive
{
  internal class Program
  {
    static void Main(string[] arguments)
    {
      //Console.SetWindowSize(Console.LargestWindowWidth, Console.LargestWindowHeight);
      Action<string> display = Console.WriteLine;
      string photoPath = @"D:\SPAPPLI\FICHIERS\Photos\2017-1\";
      CreateDirectoryIfNotExists(photoPath);

      int annee = DateTime.Now.Year;
      int mois = DateTime.Now.Month;
      int semestre = mois <= 6 ? 1 : 2;
      string nomAnneeSemestre = $"{annee}-{semestre}";
      string photoPathBaseDirectory = @"D:\SPAPPLI\FICHIERS\Photos\";
      string photoPathAnneeCourante = Path.Combine(photoPathBaseDirectory, nomAnneeSemestre);
      CreateDirectoryIfNotExists(photoPathAnneeCourante);
      string pieceJointePath = $@"D:\SPAPPLI\FICHIERS\Pj\{nomAnneeSemestre}";
      CreateDirectoryIfNotExists(pieceJointePath);
      string titreDirectory = $@"C:\SPAPPLI\FICHIERS\Titres\{nomAnneeSemestre}";
      CreateDirectoryIfNotExists(titreDirectory);

      string samplePicture = @"c:\Users\user1\Pictures\femme.jpg";
      string pictureName = "6611007C.jpg";
      if (File.Exists(samplePicture))
      {
        File.Copy(samplePicture, Path.Combine(photoPath, pictureName), true);
      }

      string xlsTemplates = @"d:\APPLICATION\XSL\";
      CreateDirectoryIfNotExists(xlsTemplates);

      string titreXls = @"d:\APPLICATION\XSL\TitreSTD.xlsx";
      string titreXlsName = "TitreSTD.xlsx";
      string sourceDirectoryTitres = @"C:\APPLICATION\XSL\";
      if (!File.Exists(titreXls))
      {
        CopyFile(Path.Combine(sourceDirectoryTitres, titreXlsName), Path.Combine(xlsTemplates, titreXlsName));
      }

      // copy all titres
      List<string> allXlsTemplate = GetFilesFileteredBySize(new DirectoryInfo(sourceDirectoryTitres), 1);
      foreach (var file in allXlsTemplate)
      {
        CopyFile(file, Path.Combine(xlsTemplates, Path.GetFileName(file)), true);
      }

      string exportDirectory = @"D:\\APPLICATION\\Exports\";
      CreateDirectoryIfNotExists(exportDirectory);

      string batchExportDirectory = @"D:\\APPLICATION\\Batch\\Export\";
      CreateDirectoryIfNotExists(batchExportDirectory);

      // préparation plusieurs sous répertoires dans pièces jointe pour Jira 4385
      for (int annee2 = 2015; annee2 < 2021; annee2++)
      {
        string pieceJointePathTmp = $@"D:\SPAPPLI\FICHIERS\Pj\{annee2}-1";
        CreateDirectoryIfNotExists(pieceJointePathTmp);
        CopyFile(allXlsTemplate[0], Path.Combine(pieceJointePathTmp, "example1.pdf"), true);
        pieceJointePathTmp = $@"D:\SPAPPLI\FICHIERS\Pj\{annee2}-2";
        CreateDirectoryIfNotExists(pieceJointePathTmp);
        CopyFile(allXlsTemplate[0], Path.Combine(pieceJointePathTmp, "example2.pdf"), true);
      }


      display(string.Empty);
      Console.ForegroundColor = ConsoleColor.White;
      display("Press any key to exit:");
      Console.ReadKey();
    }

    private static void CopyFile(string source, string target, bool overwrite = false)
    {
      try
      {
        File.Copy(source, target, overwrite);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"The file: {source} has been copied to: {target}");
      }
      catch (Exception)
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"The file: {source} has NOT been copied to: {target}");
      }
    }

    private static void CreateDirectoryIfNotExists(string directoryPath)
    {
      if (!Directory.Exists(directoryPath))
      {
        Directory.CreateDirectory(directoryPath);
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"The directory: {directoryPath} has been created.");
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine($"The directory: {directoryPath} already exists.");
      }
    }

    public static List<string> GetFilesFileteredBySize(DirectoryInfo directoryInfo, long sizeGreaterOrEqualTo = 1)
    {
      List<string> result = new List<string>();
      foreach (FileInfo fileInfo in directoryInfo.GetFiles())
      {
        if (fileInfo.Length >= sizeGreaterOrEqualTo)
        {
          result.Add(fileInfo.FullName);
        }
      }

      return result;
    }
  }
}
