using CourtRooms.Models.Parsers;
using CourtRoomsDataLayer.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourtRooms.Models.Crawlers
{
    public class ArraignmentCrawler
    {
        private readonly Action<string> Log;
        private readonly Action<string> LogLastProcessed;
        private readonly ArraignmentParser parser;


        public ArraignmentCrawler(Action<string> log, Action<string> logLastProcessed)
        {
            Log = log;
            LogLastProcessed = logLastProcessed;
            parser = new ArraignmentParser();
        }

        public async Task Start(string folderName)
        {
            if (!Directory.Exists(folderName))
            {
                Log($"Directory \"{folderName}\" does not exist");
                return;
            }

            var files = Directory.EnumerateFiles(folderName, "*.html");
            if (!files.Any())
            {
                Log($"Directory \"{folderName}\" does not contain .html files");
                return;
            }

            foreach (var file in files)
            {
                await ProcessFile(file);
            }

        }

        private async Task ProcessFile(string file)
        {
            var shortFileName = Path.GetFileName(file);
            Log($"Processing file \"{shortFileName}\"...");

            var html = File.ReadAllText(file);
            var arraignments = parser.GetArraignments(html);
            if (arraignments == null || !arraignments.Any())
            {
                Log($"No cases found in file");
                return;
            }

            Log($"Found {arraignments.Count} cases in file");

            await ArraignmentHelper.AddArraignmentsAsync(arraignments);

            Log($"Saved {arraignments.Count} arraignments to the database");

            LogLastProcessed(shortFileName);
        }
    }
}
