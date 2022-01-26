using Data;
using Services.DtoModels;
using Services.Implememtations;
using Services.Implementations;
using Services.Interfaces;
using System;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            FileReader fileReader = new FileReader(new TotalErrorDbContext());
            Dictionary<string, List<TransferModel>> transferModelsByFile = fileReader
                                                                            .ReadFileFromDir(@"D:\software engineering\TotalErrorFiles");

            DataObject data = fileReader.Convert(transferModelsByFile);
            List<string> dates = new List<string>();

            foreach (string date in transferModelsByFile.Keys)
            {
                dates.Add(date);
            }
            List<DateTime> dateTimeList = new List<DateTime>();
            foreach (string date in dates)
            {
                DateTime dateTime = DateTime.Parse(date);
                dateTimeList.Add(dateTime);
            }
            data.LastReadFiles = dateTimeList;

            fileReader.Convert(transferModelsByFile);
            fileReader.SaveDataToDatabase(data);
        }
    }
}
