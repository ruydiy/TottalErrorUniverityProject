using Services.DtoModels;
using Services.Implememtations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IFileReader
    {
        public Dictionary<string, List<TransferModel>> ReadFileFromDir(string dir);
        public DataObject Convert(Dictionary<string, List<TransferModel>> transferModelsByFile);
        public void SaveDataToDatabase(DataObject data);

    }
}
