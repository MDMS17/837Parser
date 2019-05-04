using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

namespace Parser837
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourceFolder = ConfigurationManager.AppSettings["SourceFolder"];
            string archiveFolder = ConfigurationManager.AppSettings["ArchiveFolder"];
            if (string.IsNullOrEmpty(sourceFolder)) return;
            string logFolder = ConfigurationManager.AppSettings["LogFolder"];
            if (!Directory.Exists(archiveFolder)) Directory.CreateDirectory(archiveFolder);
            if (!Directory.Exists(logFolder)) Directory.CreateDirectory(logFolder);
            System.Text.StringBuilder sbLog = new StringBuilder();
            sbLog.AppendLine("Start time:" + DateTime.Now.ToString());
            try
            {
                DirectoryInfo di = new DirectoryInfo(sourceFolder);
                FileInfo[] fis = di.GetFiles();
                Parallel.ForEach(fis, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (fi) => {
                    using (var context = new SHRContext())
                    {
                        SubmissionLog submittedfile = context.SubmissionLogs.Where(x => x.FileName == fi.Name).FirstOrDefault();
                        if (submittedfile != null)
                        {
                            Console.WriteLine("File " + fi.Name + " already processed before");
                            sbLog.AppendLine("File " + fi.Name + " already processed before");
                            return;
                        }
                        string s837 = File.ReadAllText(fi.FullName);
                        if (s837.Contains("~\r\n")) s837 = s837.Replace("\r\n", "");
                        string[] s837Lines = s837.Split('~');
                        s837 = null;
                        int encounterCount = s837Lines.Count(x => x.StartsWith("CLM*"));
                        if (encounterCount == 0)
                        {
                            Console.WriteLine("File " + fi.Name + " not valid");
                            sbLog.AppendLine("File " + fi.Name + " not valid");
                            return;
                        }
                        Console.WriteLine("Processing file " + fi.Name + " total records: " + encounterCount.ToString());
                        sbLog.AppendLine("Processing file " + fi.Name + " total records: " + encounterCount.ToString());

                        submittedfile = new SubmissionLog();
                        submittedfile.FileName = fi.Name;
                        submittedfile.FilePath = sourceFolder;
                        submittedfile.EncounterCount = encounterCount;
                        submittedfile.CreatedDate = DateTime.Now;
                        //isa
                        string[] temp1 = s837Lines[0].Split('*');
                        submittedfile.SubmitterID = temp1[6];
                        submittedfile.ReceiverID = temp1[8];
                        submittedfile.InterchangeControlNumber = temp1[13];
                        submittedfile.ProductionFlag = temp1[15];
                        char elementDelimiter = Char.Parse(temp1[16]);
                        //gs
                        temp1 = s837Lines[1].Split('*');
                        submittedfile.InterchangeDate = temp1[4];
                        submittedfile.InterchangeTime = temp1[5];
                        submittedfile.FileType = temp1[8];
                        //bht
                        temp1 = s837Lines[3].Split('*');
                        submittedfile.BatchControlNumber = temp1[3];
                        submittedfile.ReportType = temp1[6];
                        //nm1*41
                        temp1 = s837Lines[4].Split('*');
                        submittedfile.SubmitterLastName = temp1[3];
                        submittedfile.SubmitterFirstName = temp1[4];
                        submittedfile.SubmitterMiddle = temp1[5];
                        //nm1*40
                        string tempstring = s837Lines.Where(x => x.StartsWith("NM1*40")).FirstOrDefault();
                        temp1 = tempstring.Split('*');
                        submittedfile.ReceiverLastName = temp1[3];
                        //clm
                        tempstring = s837Lines.Where(x => x.StartsWith("CLM*")).FirstOrDefault();
                        string ClaimID = tempstring.Split('*')[1];

                        context.SubmissionLogs.Add(submittedfile);
                        context.SaveChanges();

                        string LoopName = "";

                        Claim claim = new Claim();
                        claim.Header.FileID = submittedfile.FileID;
                        claim.Header.ClaimID = ClaimID;
                        List<Claim> claims = new List<Claim>();
                        bool saveFlag = false;

                        foreach (string s837Line in s837Lines)
                        {
                            Process837.Process837Line(s837Line, ref submittedfile, ref LoopName, ref saveFlag, ref claim, ref claims, ref elementDelimiter);
                        }
                        Process837.InitilizeClaim("Claim", ref claim, ref claims, ref submittedfile);
                        SHRUtil.SaveClaims(ref claims);
                        claims.Clear();
                        claim = null;
                        context.SaveChanges();
                    }
                    if (File.Exists(Path.Combine(archiveFolder, fi.Name))) File.Delete(Path.Combine(archiveFolder, fi.Name));
                    fi.MoveTo(Path.Combine(archiveFolder, fi.Name));
                });
            }
            catch (Exception ex)
            {
                sbLog.AppendLine(ex.Message);
            }
            finally
            {
                sbLog.AppendLine("End time:" + DateTime.Now.ToString());
                File.AppendAllText(Path.Combine(logFolder, "SHRLog.txt"), sbLog.ToString());
            }
        }
    }
}
