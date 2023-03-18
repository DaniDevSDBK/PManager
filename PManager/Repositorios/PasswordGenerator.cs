using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PManager.Repositorios
{
    public class PasswordGenerator
    {
        private string password = "";
        public string Password { get => password; set => password = value; }

        private List<string> _Capitals = new List<string>();
        private List<string> _LowerCase = new List<string>();
        private List<string> _Numbers = new List<string>();
        private List<string> _SpecialCharacters = new List<string>();
        private List<string> _FinalList = new List<string>();

        public PasswordGenerator() {
        
        }
        public PasswordGenerator(List<string>conditions) {
        
            Password = GenPassword(conditions);
        }

        private string GenPassword(List<string>conditions)
        {

            Random random= new Random();

            //  -->Length
            //  -->Quantity Of Numbers
            //  -->Capitals Numbers
            //  -->Lower Case Numbers
            //  -->Special Characteres

            //Length
            StringBuilder pwd = new StringBuilder(Int32.Parse(conditions[0].ToString()));

            //Quantity Of Numbers
            if (Int32.Parse(conditions[1].ToString()) != 0)
            {
                FillList(1);

                FillFinalList(Int32.Parse(conditions[1].ToString()),_Numbers);
            }

            //Capitals Numbers
            if (Int32.Parse(conditions[2].ToString()) != 0)
            {
                FillList(2);

                FillFinalList(Int32.Parse(conditions[0].ToString()) - Int32.Parse(conditions[1].ToString())- Int32.Parse(conditions[3].ToString()), _Capitals);
            }

            //Lower Case Numbers
            if (Int32.Parse(conditions[3].ToString()) != 0)
            {
                FillList(3);

                FillFinalList(Int32.Parse(conditions[0].ToString()) - Int32.Parse(conditions[1].ToString()) - Int32.Parse(conditions[2].ToString()), _LowerCase);

            }

            //Special Characters
            if (Boolean.Parse(conditions[4].ToString()))
            {
                FillList(0);

                FillFinalList(Int32.Parse(conditions[0].ToString()) - Int32.Parse(conditions[1].ToString()) - Int32.Parse(conditions[2].ToString()) - Int32.Parse(conditions[3].ToString()), _SpecialCharacters);
            }

            //Random OrderBy
            foreach (var item in _FinalList.OrderBy(_=>random.Next()).ToList())
            {

                pwd.Append(item.ToString());
            }

            return pwd.ToString();
        }

        private void FillList(int type)
        {

            switch (type)
            {
                case 0:

                    //SpecialCharacteres
                    for (int i = 32; i < 48; i++)
                    {

                       _SpecialCharacters.Add(""+(char)i);
                    }

                    for (int i = 58; i < 65; i++)
                    {

                        _SpecialCharacters.Add("" + (char)i);
                    }

                    for (int i = 91; i < 97; i++)
                    {

                        _SpecialCharacters.Add("" + (char)i);
                    }

                    for (int i = 123; i < 129; i++)
                    {

                        _SpecialCharacters.Add("" + (char)i);
                    }

                    break;

                case 1:

                    //Numbers
                    for (int i = 48; i < 58; i++)
                    {

                        _Numbers.Add("" + (char)i);
                    }

                    break;

                case 2:

                    //Capitals
                    for (int i = 65; i < 91; i++)
                    {

                        _Capitals.Add("" + (char)i);
                    }

                    break;

                case 3:

                    //LowerCase
                    for (int i = 97; i < 123; i++)
                    {

                        _LowerCase.Add("" + (char)i);
                    }

                    break;

                default:
                    break;
            }
        }

        private void FillFinalList(int n, List<string> list)
        {
            int count = 0;

            while (count < n)
            {

                _FinalList.Add(list[GenRandom(0, list.Count)].ToString());

                count++;
            }
        }

        private int GenRandom(int min, int max)
        {

            Random random= new Random();

            return random.Next(min, max);
        }
    }
}
