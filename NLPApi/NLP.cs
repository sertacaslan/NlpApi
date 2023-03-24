

namespace NLPApi
{
    public class NLP
    {
        private string NumberToText(int input)
        {
            Dictionary<int, string> sayilar = new Dictionary<int, string>();

            string[] birler = { "", "bir", "iki", "üç", "dört", "beş", "altı", "yedi", "sekiz", "dokuz" };
            string[] onlar = { "", "on", "yirmi", "otuz", "kırk", "elli", "altmış", "yetmiş", "seksen", "doksan" };
            string[] yuzler = { "", "yüz", "ikiyüz", "üçyüz", "dörtyüz", "beşyüz", "altıyüz", "yediyüz", "sekizyüz", "dokuzyüz" };


            for (int i = 1; i <= 999; i++)
            {
                string okunus = "";
                if (i < 10)
                {
                    sayilar.Add(i, birler[i]);
                }
                else if (i >= 10 && i < 100)
                {
                    if (i % 10 == 0)
                    {
                        sayilar.Add(i, onlar[i / 10]);
                    }
                    else
                    {
                        sayilar.Add(i, onlar[i / 10] + birler[i % 10]);
                    }
                }
                else if (i >= 100 && i < 1000)
                {
                    if (i % 100 == 0)
                    {
                        sayilar.Add(i, yuzler[i / 100]);
                    }
                    else
                    {
                        int onlarBas = i % 100;
                        int yuzlerBas = i / 100;
                        sayilar.Add(i, yuzler[yuzlerBas] + sayilar[onlarBas]);
                    }
                }
            }

            if (input < 1000)
            {
                return sayilar[input];
            }
            else if (input >= 1000 && input <= 999999)
            {
                int binlerBas = input / 1000;
                if (binlerBas == 1)
                {
                    if (input == 1000)
                    {
                        return "bin";
                    }
                    else
                    {
                        return "bin" + sayilar[input % 1000];
                    }
                }
                else
                {
                    if (input % 1000 == 0)
                    {
                        return sayilar[binlerBas] + "bin";
                    }
                    else
                    {
                        return sayilar[binlerBas] + "bin" + sayilar[input % 1000];//7000de hata verdi
                    }



                }
            }
            else
            {
                return "";
            }


        }

        private string TextNumSearch(string input)
        {
            string result = input;

            int numberStartIndex = 0;
            // int numberEndIndex = 0;
            string numText = "";
            int num = 0;



            for (int i = 0; i < result.Length; i++)
            {


                if (Char.IsNumber(result[i]))
                {
                    if (string.IsNullOrEmpty(numText))
                    {
                        numberStartIndex = i;
                        numText += result[i];
                    }
                    else
                    {
                        numText += result[i];
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(numText))
                    {
                        num = Convert.ToInt32(numText);


                        string newNumber = NumberToText(num);
                        result = result.Replace(numText, newNumber);


                        numberStartIndex = 0;
                        numText = "";
                        num = 0;

                    }
                }

                if (i + 1 == result.Length)
                {
                    if (!string.IsNullOrEmpty(numText))
                    {
                        num = Convert.ToInt32(numText);


                        string newNumber = NumberToText(num);
                        result = result.Replace(numText, newNumber);


                        numberStartIndex = 0;
                        numText = "";
                        num = 0;
                    }
                }
            }


            return result;
        }



        public string Converter(string input)
        {
            string result = TextNumSearch(input);
            List<string> sentenceNumbers = new();
            Dictionary<string, int> sayilar = new();
            {
                sayilar.Add("bir", 1);
                sayilar.Add("iki", 2);
                sayilar.Add("üç", 3);
                sayilar.Add("dört", 4);
                sayilar.Add("beş", 5);
                sayilar.Add("altı", 6);
                sayilar.Add("yedi", 7);
                sayilar.Add("sekiz", 8);
                sayilar.Add("dokuz", 9);
                sayilar.Add("on", 10);
                sayilar.Add("yirmi", 20);
                sayilar.Add("otuz", 30);
                sayilar.Add("kırk", 40);
                sayilar.Add("elli", 50);
                sayilar.Add("altmış", 60);
                sayilar.Add("yetmiş", 70);
                sayilar.Add("seksen", 80);
                sayilar.Add("doksan", 90);
                sayilar.Add("yüz", 100);
                sayilar.Add("bin", 1000);
            }


            // sayıları parçalar
            foreach (KeyValuePair<string, int> sayi in sayilar)
            {
                int choosedIndex = result.IndexOf(sayi.Key, StringComparison.OrdinalIgnoreCase);
                int wordEndIndex = choosedIndex + sayi.Key.Length;

                while (choosedIndex > -1)
                {
                    if (wordEndIndex < result.Length)
                    {
                        if (result[wordEndIndex] != '£')
                        {
                            result = result.Insert(wordEndIndex, "£");

                        }
                    }
                    else
                    {
                        result = result + "£";
                    }


                    if (choosedIndex > 0)
                    {

                        if (result[choosedIndex - 1] != '£')
                        {
                            result = result.Insert(choosedIndex, "£");
                        }
                    }
                    else
                    {
                        result = "£" + result;
                    }
                    choosedIndex = result.IndexOf(sayi.Key, choosedIndex + 1, StringComparison.OrdinalIgnoreCase);// while döngüsüiçin
                    wordEndIndex = choosedIndex + sayi.Key.Length;
                }
            }

            List<string> sentenceArr = result.Split("£").ToList<string>();//cümleyi koleksiyon yapar


            int replaceNum = 0;
            int replaceThousandNum = 0;
            bool thousandKey = false;
            List<int> replaceIndexes = new();//değişecek kelimelerin indexleri tutulur



            for (int i = sentenceArr.Count - 1; i >= 0; i--)//cümle dizisini tersten gezer
            {
                if (sayilar.ContainsKey(sentenceArr[i].ToLower()))//eğer cümle dizisindeki seçilen eleman sayı ise
                {

                    if (sentenceArr[i].ToLower() == "bin")
                    {
                        thousandKey = true;
                        replaceIndexes.Add(i);
                        if (i > 0)
                        {
                            if (!sayilar.ContainsKey(sentenceArr[i - 1].ToLower()))
                            {
                                // replaceThousandNum = 2;//todo burada hata var
                            }
                        }
                        else
                        {

                        }

                    }//sayı bin ise

                    else if (sentenceArr[i].ToLower() == "yüz")
                    {
                        replaceIndexes.Add(i);
                        if (i > 0)
                        {
                            if (sentenceArr[i - 1] == " ")//eğer sonraki karakter boşluksa bir sonraki karaktere geç//ok
                            {
                                replaceIndexes.Add(i - 1);
                                if (i - 2 >= 0)
                                {

                                    if (sayilar.ContainsKey(sentenceArr[i - 2].ToLower()))
                                    {
                                        if (sayilar[sentenceArr[i - 2].ToLower()] >= 1 && sayilar[sentenceArr[i - 2].ToLower()] <= 9)
                                        {
                                            if (thousandKey)
                                            {
                                                replaceThousandNum += sayilar[sentenceArr[i - 2].ToLower()] * 100;
                                            }
                                            else
                                            {
                                                replaceNum += sayilar[sentenceArr[i - 2].ToLower()] * 100;
                                            }

                                            replaceIndexes.Add(i - 2);

                                            i = i - 2;
                                        }
                                    }
                                }
                                else
                                {
                                    replaceIndexes.Add(i - 1);
                                }
                            }
                            else if (sayilar.ContainsKey(sentenceArr[i - 1].ToLower()))
                            {
                                if (sayilar[sentenceArr[i - 1].ToLower()] >= 1 && sayilar[sentenceArr[i - 1].ToLower()] <= 9)//yüzün tek basamak sayısını al
                                {
                                    if (thousandKey)
                                    {
                                        replaceThousandNum += sayilar[sentenceArr[i - 1].ToLower()] * 100;
                                    }
                                    else
                                    {
                                        replaceNum += sayilar[sentenceArr[i - 1].ToLower()] * 100;
                                    }

                                    replaceIndexes.Add(i - 1);
                                    i = i - 1;
                                }
                                else
                                {
                                    replaceIndexes.Add(i - 1);
                                }
                            }

                            else// eğer sonraki karakter hiçbiri değiles tekrar 100 ekle
                            {
                                if (thousandKey)
                                {
                                    replaceThousandNum += 100;
                                    if (sentenceArr[i - 1].Length <= 1)
                                    {
                                        replaceIndexes.Add(i - 1);
                                    }
                                }
                                else
                                {
                                    replaceNum += 100;
                                    if (sentenceArr[i - 1].Length <= 1)
                                    {
                                        replaceIndexes.Add(i - 1);
                                    }

                                }


                            }//ok

                        }
                        else// 100 ekle replaceNum
                        {
                            if (thousandKey)
                            {
                                replaceThousandNum += 100;
                            }
                            else
                            {
                                replaceNum += 100;
                            }

                            // replaceIndexes.Add(i);
                        }//ok

                    }//sayı 100 ise

                    else if (sayilar[sentenceArr[i].ToLower()] >= 10 && sayilar[sentenceArr[i].ToLower()] <= 90)//onlar basamağındaysa
                    {
                        if (thousandKey)
                        {
                            replaceThousandNum += sayilar[sentenceArr[i].ToLower()];
                            replaceIndexes.Add(i);
                        }
                        else
                        {
                            replaceNum += sayilar[sentenceArr[i].ToLower()];
                            replaceIndexes.Add(i);
                        }
                    }

                    else if (sayilar[sentenceArr[i].ToLower()] >= 1 && sayilar[sentenceArr[i].ToLower()] <= 9)//sayı tek basamaksa
                    {
                        if (thousandKey)
                        {

                            //todo 1 bin durumunu kontrol et
                            replaceIndexes.Add(i);
                            replaceThousandNum += sayilar[sentenceArr[i].ToLower()];



                        }
                        else
                        {
                            replaceNum += sayilar[sentenceArr[i].ToLower()];
                            replaceIndexes.Add(i);
                        }

                    }



                }
                else if (sentenceArr[i] == " ")//eğer seçilen eleman boşluk ise
                {
                    replaceIndexes.Add(i);
                }
                else
                {
                    if (replaceIndexes.Count > 0)//burada sayıyı dizideki yazıyla değiştir
                    {
                        if (replaceIndexes.Count == 1)//sadece 1 taneyse
                        {
                            if (thousandKey && replaceThousandNum == 0)
                            {
                                replaceThousandNum = 1;
                            }

                            replaceNum = (replaceThousandNum * 1000) + replaceNum;//ok

                            sentenceArr[replaceIndexes[0]] = replaceNum.ToString();

                            replaceIndexes.Clear();
                            replaceThousandNum = 0;
                            thousandKey = false;
                            replaceNum = 0;
                        }
                        else//birdenfazlaysa
                        {
                            if (thousandKey && replaceThousandNum == 0)
                            {
                                replaceThousandNum = 1;
                            }
                            replaceNum = (replaceThousandNum * 1000) + replaceNum;
                            replaceIndexes.Reverse();
                            for (int z = replaceIndexes.Count - 1; z >= 0; z--)
                            {
                                if (z == 0)
                                {
                                    sentenceArr[replaceIndexes[0]] = replaceNum.ToString();
                                }
                                else
                                {
                                    sentenceArr.RemoveAt(replaceIndexes[z]);
                                }

                            }
                            thousandKey = false;
                            replaceIndexes.Clear();
                            replaceThousandNum = 0;
                            replaceNum = 0;


                        }
                    }
                }
            }
            result = "";
            foreach (string word in sentenceArr)
            {
                result += word;
            }

            return result;
        }
    }
}
