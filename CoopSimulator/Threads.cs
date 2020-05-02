using CoopSimulator.Data;
using System.Collections.Generic;
using System.Linq;

namespace CoopSimulator
{
    public class Threads
    {
        public Threads()
        {
            Util.ColdStart();
            Helper.ThreadHelper.ExecuteThread(Date);
        }

        public void Date()
        {
            while (true)
            {
                Program.Date.Date = Program.Date.Date.AddDays(1);

                #region Mating
                List<Data.PoultryDto> possibleFemalePoultryList =
                    Program.PoultryList.Where(a => !a.Pregnant &&
                    a.Sex == Enums.PoultrySex.Female &&
                    (int)(Program.Date.Date - a.BirthDate).TotalDays >= 
                    Program.Configuration.PoultryDetail.AdolescenceAge.FemaleAge && 
                    (int)(Program.Date.Date - a.BirthDate).TotalDays < 
                    Program.Configuration.PoultryDetail.OldAge.Value)
                    .ToList();

                if (possibleFemalePoultryList.Any())
                {
                    Data.PoultryDto malePoultry = Program.PoultryList.FirstOrDefault(a => a.Sex == Enums.PoultrySex.Male && 
                    (int)(Program.Date.Date - a.BirthDate).TotalDays >= 
                    Program.Configuration.PoultryDetail.AdolescenceAge.MaleAge && 
                    (int)(Program.Date.Date - a.BirthDate).TotalDays < 
                    Program.Configuration.PoultryDetail.OldAge.Value);

                    if (malePoultry != null)
                    {
                        possibleFemalePoultryList.ForEach(a => 
                        {
                            a.Pregnant = true;
                            a.PregnantDate = Program.Date.Date;
                        });
                    }
                }
                #endregion

                #region Birth
                List<Data.PoultryDto> poultryList = Program.PoultryList
                    .Where(a => 
                                a.Pregnant && a.PregnantDate != null && 
                                (Program.Date.Date - a.PregnantDate.Value).TotalDays >= 
                                Program.Configuration.PoultryDetail.DurationOfPregnancy.Value)
                    .ToList();

                foreach (var poultry in poultryList)
                {
                    int babyCount = Helper.RandomHelper
                        .Number(Program.Configuration.PoultryDetail.NumberOfBabiesForEachPregnancy.Low,
                                Program.Configuration.PoultryDetail.NumberOfBabiesForEachPregnancy.High);
                    for (int i = 1; i <= babyCount; i++)
                    {
                        Data.Enums.PoultrySex poultrySex = Enums.PoultrySex.Female;
                        if (Helper.RandomHelper.Number(1, 100) > 60)
                        {
                            poultrySex = Enums.PoultrySex.Male;
                        }

                        Program.PoultryHandler.Add(new PoultryDto() 
                        {
                            BirthDate = Program.Date.Date,
                            Pregnant = false,
                            PregnantDate = null,
                            Sex = poultrySex
                        });
                    }

                    poultry.Pregnant = false;
                    poultry.PregnantDate = null;
                }
                #endregion

                #region Death
                Program.PoultryHandler.RemoveAll(a =>   (Program.Date.Date - a.BirthDate).TotalDays >=
                                                        Program.Configuration.PoultryDetail.MaxAge.Value);
                #endregion
            }
        }
    }
}