using Spire.Xls;
using System.Collections.Generic;
using System.IO;

namespace ItemProcessor.XLS
{
    public class XLS_ReadReport
    {
        private Workbook workbook;
        public Workbook Workbook { get => workbook; set => workbook = value; }
        public string[][][] Read(string Path)
        {
            var fs = File.OpenRead(Path);
            Stream St2 = new MemoryStream();
            fs.CopyTo(St2);
            Workbook = new Workbook();
            using Stream Stream = St2;
            Workbook.LoadFromStream(Stream);
            List<string[][]> X = new List<string[][]>();
            foreach (var item in Workbook.Worksheets)
            {
                string[][] M = new string[100][];
                for (int x = 1; x < 100; x++)
                {
                    M[x - 1] = new string[100];
                    for (int y = 1; y < 50; y++)
                    {
                        M[x - 1][y - 1] = item[x, y].Value;
                    }
                }
                X.Add(M);
            }
            return X.ToArray();
        }
    }
}
