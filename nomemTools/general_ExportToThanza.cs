// -----------------------------------------------------------------------
// <copyright file="general_ExportToThanza.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace nomemTools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    /// <summary>
    /// Creates payment document for import to Thanza system
    /// </summary>
    public static partial class Tool
    {
        public static void Export(List<AbstractPerson> persons, string mokejimoPaskirtis, string teikimoNumeris, string iban, string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (StreamWriter bankWriter = new StreamWriter(fileName, false, Encoding.GetEncoding(1257)))
            {
                // header
                var random = new Random();
                var metai = DateTime.Now.Date.ToString();
                var data = metai[2].ToString() + metai[3].ToString() + metai[5].ToString() + metai[6].ToString() + metai[8].ToString() + metai[9].ToString();

                var antraste = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                    "MMLT02",
                    "HDR",
                    iban.PadRight(35, ' '),
                    data,
                    "      ",
                    mokejimoPaskirtis.PadRight(140, ' '),
                    teikimoNumeris.PadRight(10, ' '),
                    random.Next(10000000, 99999999).ToString().PadLeft(8, ' '),
                    "LTL") + Environment.NewLine;

                bankWriter.Write(antraste);
                bankWriter.Flush();

                // body
                var Nr = 1;

                foreach (var person in persons)
                {
                    var suma = person.Suma.ToString();

                    if (suma.Contains(",") || suma.Contains("."))
                    {
                        suma = suma.Replace(",", "").Replace(".", "");
                    }
                    else
                    {
                        suma = suma + "00";
                    }

                    var bodyLine = String.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                        "MM  ",
                        Nr.ToString().PadRight(10, ' '),
                        person.IBAN.Substring(4, 5).Trim().PadLeft(12, ' '),
                        person.IBAN.Trim().ToUpper().PadRight(35, ' '),
                        person.AsmensKodas.Trim().PadRight(12, ' '),
                        (person.Vardas + ' ' + person.Pavarde).PadRight(70, ' '),
                        suma.PadLeft(12, ' '),
                        (Nr.ToString() + '/' + teikimoNumeris).PadRight(10, ' '),
                        String.Empty.PadLeft(28, ' ')) + Environment.NewLine;

                    bankWriter.Write(bodyLine);
                    bankWriter.Flush();
                    Nr++;
                }

                // footer
                var totalSum = persons.Sum(x => x.Suma).ToString();

                if (totalSum.Contains(",") || totalSum.Contains("."))
                {
                    totalSum = totalSum.Replace(",", "").Replace(".", "");
                }
                else
                {
                    totalSum = totalSum + "00";
                }

                var footer = String.Format("{0}{1}{2}{3}",
                    "MMLT02",
                    "FTR",
                    (Nr - 1).ToString().PadLeft(8, '0'),
                    totalSum.PadLeft(15, '0'));

                bankWriter.Write(footer);
                bankWriter.Flush();
            }
        }
    }

    public abstract class AbstractPerson
    {
        public string AsmensKodas { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
        public string IBAN { get; set; }
        public double? Suma { get; set; }
        public string JuridinisAMKodas { get; set; }
        public int? TeikimoNr { get; set; }
        abstract public bool IsAsmensKodasGood();
        abstract public bool IsIBANGood();
    }
}
